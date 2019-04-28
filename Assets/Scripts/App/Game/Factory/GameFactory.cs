using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameFactory
{
	private static Dictionary<Color, WorldTileType> tilesPerColor;

	private const string char1 = "AZERTYUIOPQSDFGHJKLMWXCVBN";
	private const string char2 = "0123456789AZER";
	private const string char3 = "AZERTYUIOPQSDFGHJKLMWXCVBN";
	private const string char4 = "-------------0123456789";
	private const string char5 = "01234567890123456789azerqsdfwx;cvbnghjkltyuiop";
	private static readonly HashSet<string> existingNames = new HashSet<string>();

	private static string CreateNewRobotName()
	{
		do
		{
			string name = char1.Random() + char2.Random() + char3.Random() + char4.Random() + char5.Random();
			if (!existingNames.Contains(name))
			{
				existingNames.Add(name);
				return name;
			}
		} while (true);
	}


	public static void Init()
	{
		tilesPerColor = ResourcesManager.LoadAllTypes<WorldTileType>().ToDictionary(t => t.parserColor, t => t);
	}


	public static Game CreateGame()
	{
		Game game = new Game
		{
			funds = 1000
		};
		game.ownedRobots.Add(CreateRobot(ResourcesManager.LoadAllTypes<RobotType>().Single(t => t.name == "Default")));
		return game;
	}



	public static Robot CreateRobot(RobotType type)
	{
		Robot robot = new Robot
		{
			type = type,
			maintenanceCost = type.initialMaintenanceCost,

			color = new Color(UnityEngine.Random.Range(.3f, 1), UnityEngine.Random.Range(.3f, 1), UnityEngine.Random.Range(.3f, 1)),
			name = CreateNewRobotName()
		};
		return robot;
	}

	public static Robot[] CreateRandomRobots(int amount)
	{
		Robot[] robots = new Robot[amount];
		for (int i = 0; i < amount; ++i)
		{
			robots[i] = CreateRobot(ResourcesManager.LoadAllTypes<RobotType>().Random());
		};
		return robots;
	}

	public static World ParseWorld(Texture2D mapTexture)
	{
		World world = new World
		{
			tiles = new List<WorldTile>[mapTexture.width, mapTexture.height]
		};
		for (int i = 0; i < mapTexture.width; ++i)
		{
			for (int j = 0; j < mapTexture.height; ++j)
			{
				world.tiles[i, j] = new List<WorldTile>();
				Color mapPixelColor = mapTexture.GetPixel(i, j);
				if (mapPixelColor.a > 0)
				{
					WorldTileType type = tilesPerColor.ContainsKey(mapPixelColor) ? tilesPerColor[mapPixelColor] : null;
					if (type == null) Debug.LogWarning("Found a pixel " + mapPixelColor + " but that color was not linked to a tile");
					else
					{
						world.tiles[i, j].Add(new WorldTile { type = type, worldPosition = new Vector2Int(i, j) });
					}
				}
			}
		}

		return world;
	}
}
