using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
	public static App instance { get; private set; }

	public string theme;

	public event Action<AsyncOperation> OnSceneChange = delegate { };

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else DontDestroyOnLoad(this.gameObject);
	}

	public void LoadGameScene()
	{
		this.OnSceneChange(SceneManager.LoadSceneAsync("Game"));
	}

}