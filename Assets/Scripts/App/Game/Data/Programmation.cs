using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Programmation
{
	public enum ConditionDirection
	{
		left, bottomleft, bottom, bottomright, right, topright, top, topleft
	}

	public enum ConditionType
	{
		empty, obstacle, robot
	}

	public enum Operation
	{
		moveTop, moveLeft, moveRight, moveBottom, special, nothing
	}

	public struct Instruction
	{
		public ConditionDirection conditionDirection { get; set; }
		public ConditionType conditionType { get; set; }
		public Operation operation { get; set; }
	}
}