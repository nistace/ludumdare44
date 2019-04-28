using System;
using System.Collections.Generic;
using System.Text;
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

	public class Instruction
	{
		public ConditionDirection conditionDirection { get; set; }
		public ConditionType conditionType { get; set; }
		public Operation operation { get; set; }

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append("When the tile ");
			switch (this.conditionDirection)
			{
				case ConditionDirection.bottom: str.Append("under"); break;
				case ConditionDirection.bottomleft: str.Append("on the left, under"); break;
				case ConditionDirection.bottomright: str.Append("on the left, under"); break;
				case ConditionDirection.right: str.Append("on the right of"); break;
				case ConditionDirection.left: str.Append("on the left of"); break;
				case ConditionDirection.top: str.Append("above"); break;
				case ConditionDirection.topleft: str.Append("on the left, above"); break;
				case ConditionDirection.topright: str.Append("on the right, above"); break;
			}
			str.Append(" the robot is ");
			switch (this.conditionType)
			{
				case ConditionType.empty: str.Append("empty"); break;
				case ConditionType.obstacle: str.Append("an obstacle"); break;
				case ConditionType.robot: str.Append("a robot"); break;
			}
			str.Append(", the robot ");
			switch (this.operation)
			{
				case Operation.moveTop: str.Append("moves up"); break;
				case Operation.moveLeft: str.Append("moves left"); break;
				case Operation.moveRight: str.Append("moves right"); break;
				case Operation.moveBottom: str.Append("moves down"); break;
				case Operation.special: str.Append("uses its special ability"); break;
				case Operation.nothing: str.Append("waits"); break;
			}
			return str.ToString();
		}
	}
}