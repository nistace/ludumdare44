using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
	public static GameTime instance { get; private set; }
	public static float speed { get; private set; }
	public static float deltaTime { get; private set; }

	public static event Action OnSpeedChanged = delegate { };

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
	}

	private void Update()
	{
		deltaTime = Time.deltaTime * speed;
	}

	public void SetSpeed(float speed)
	{
		GameTime.speed = speed;
		OnSpeedChanged();
	}
}
