using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World
{
	public WorldTile[,] tiles { get; set; }

	public int width => this.tiles.GetLength(0);
	public int height => this.tiles.GetLength(1);
}
