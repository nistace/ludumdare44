using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World
{

	public List<WorldTile>[,] tiles { get; set; }
	public List<Robot> robotsInWorld { get; } = new List<Robot>();

	public int width => this.tiles.GetLength(0);
	public int height => this.tiles.GetLength(1);
	public int rewardCount => this.tiles.Count(t => t.Any(u => u.type.reward > 0));
}
