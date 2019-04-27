using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController instance { get; private set; }

	public int currentLevel = 0;

	public void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else
		{
			GameFactory.Init();
			Game.current = new Game { currentCurrency = 1000, world = null };
		}
	}

	public void Start()
	{
		this.LoadLevel(0);
	}

	public void LoadLevel(int level)
	{
		this.ClearLevel();
		this.currentLevel = level;
		Game.current.world = GameFactory.ParseWorld(ResourcesManager.LoadLevelTexture(this.currentLevel));
		for (int i = 0; i < Game.current.world.tiles.GetLength(0); ++i)
		{
			for (int j = 0; j < Game.current.world.tiles.GetLength(1); ++j)
			{
				if (Game.current.world.tiles[i, j] != null) ModelManager.CreateModel(Game.current.world.tiles[i, j]).transform.position = new Vector2(i, j);
			}
		}
		CameraController.instance.Refresh();
	}

	public void ClearLevel()
	{
		ModelManager.DestroyAllModels<WorldTileModel, WorldTile>();
	}

}
