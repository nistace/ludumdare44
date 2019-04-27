using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
	public static App instance { get; private set; }

	public string theme;

	public Texture2D cursorSprite;

	public Sprite defaultButtonSprite;
	public Sprite buttonPressedSprite;
	public Sprite defaultTabSprite;
	public Sprite tabActiveSprite;


	public event Action<AsyncOperation> OnSceneChange = delegate { };

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		UnityEngine.Cursor.SetCursor(this.cursorSprite, Vector2.up, CursorMode.Auto);
	}

	public void LoadGameScene()
	{
		this.OnSceneChange(SceneManager.LoadSceneAsync("Game"));
	}

	public void LoadMainScene()
	{
		ModelManager.DestroyAllModels<WorldTileModel, WorldTile>();
		ModelManager.DestroyAllModels<RobotModel, Robot>();
		this.OnSceneChange(SceneManager.LoadSceneAsync("Main"));
	}

}