using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
	public static App instance { get; private set; }

	public string theme;

	public float[] difficultyFunds = new float[] { 1000, 500, 200 };
	public int difficultyLevel = 1;
	public bool helpEnabled = true;

	public Texture2D cursorSprite;

	public Sprite defaultButtonSprite;
	public Sprite buttonPressedSprite;
	public Sprite defaultTabSprite;
	public Sprite tabActiveSprite;

	public event Action<AsyncOperation> OnSceneChange = delegate { };
	public event Action OnParametersChanged = delegate { };

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

	public void SetDifficulty(int level)
	{
		this.difficultyLevel = level;
		this.OnParametersChanged();
	}

	public void SetHelpEnabled(bool enabled)
	{
		this.helpEnabled = enabled;
		this.OnParametersChanged();
	}
}