using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameFactory
{
	private static Dictionary<Color, WorldTileType> tilesPerColor;

	public static void Init()
	{
		tilesPerColor = ResourcesManager.LoadAllTypes<WorldTileType>().ToDictionary(t => t.parserColor, t => t);
	}

	public static World ParseWorld(Texture2D mapTexture)
	{
		World world = new World
		{
			tiles = new WorldTile[mapTexture.width, mapTexture.height]
		};
		for (int i = 0; i < mapTexture.width; ++i)
		{
			for (int j = 0; j < mapTexture.height; ++j)
			{
				Color mapPixelColor = mapTexture.GetPixel(i, j);
				if (mapPixelColor.a > 0)
				{
					WorldTileType type = tilesPerColor.ContainsKey(mapPixelColor) ? tilesPerColor[mapPixelColor] : null;
					if (type == null) Debug.LogWarning("Found a pixel " + mapPixelColor + " but that color was not linked to a tile");
					else
					{
						world.tiles[i, j] = new WorldTile { type = type };
					}
				}
			}
		}
		return world;
	}
}
