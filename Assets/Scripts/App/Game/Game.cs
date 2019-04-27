using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game
{
	public static Game current { get; set; }

	public World world { get; set; }
	public float currentCurrency { get; set; } = 1000;
}
