using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTile
{
	public WorldTileType type { get; set; }
	public bool active { get; set; }

	public Vector2Int worldPosition { get; set; }
}
