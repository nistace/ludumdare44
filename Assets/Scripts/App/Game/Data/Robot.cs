using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot
{
	public string name { get; set; }
	public Color color { get; set; }
	public RobotType type { get; set; }
	public float maintenanceCost { get; set; } = 50;
	public float useCost => this.maintenanceCost * .12f;

	public bool inLevel { get; set; } = false;
	public Vector2Int positionInLevel { get; set; }
	public int xInLevel => this.positionInLevel.x;
	public int yInLevel => this.positionInLevel.y;


	public List<Programmation.Instruction> instructions { get; } = new List<Programmation.Instruction>();
	public Programmation.Operation elseOperation { get; set; } = Programmation.Operation.moveRight;

}
