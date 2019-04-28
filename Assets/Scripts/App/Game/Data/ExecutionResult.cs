using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecutionResult
{
	public bool success { get; set; }
	public int lostRobots { get; set; }
	public float earnedAmount { get; set; }
	public int objectives { get; set; }
	public int objectivesReached { get; set; }
	public bool allObjectivesReached => this.objectivesReached >= this.objectives;
	public bool done { get; set; }
}
