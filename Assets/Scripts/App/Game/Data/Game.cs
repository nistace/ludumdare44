using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game
{

	public enum Status { Preparing, Playing, Finishing, Played }

	public static Game current { get; set; }

	public World world { get; set; }
	public float funds { get; set; } = 1000;
	public int keepCountItemsInPurchaseList { get; set; } = 4;
	public List<Robot> ownedRobots { get; } = new List<Robot>();
	public List<Robot> purchasableRobots { get; } = new List<Robot>();
	public float movementSpeed { get; } = 2;// tiles/second
	public float fallSpeed { get; } = 4;// tiles/second

	/// <summary>Currennt or latest exection result</summary>
	public ExecutionResult executionResult { get; set; }
	public bool somethingHappenedThisTurn { get; set; }
	public HashSet<Robot> turnDestroyedRobots { get; } = new HashSet<Robot>();
	public HashSet<WorldTile> turnDestroyedItems { get; } = new HashSet<WorldTile>();

	public Status status { get; set; }
	public int initialObjectives { get; set; }
	public int remainingObjectives { get; set; }

	/// <summary>Funds changed, gives the difference</summary>
	public event Action<float> OnFundsChanged = delegate { };
	public event Action OnOwnedRobotListChanged = delegate { };
	public event Action OnPurchasableListChanged = delegate { };


	public void AddFunds(float amount)
	{
		if (amount == 0) return;
		this.funds += amount;
		this.OnFundsChanged(amount);
	}

	internal void PurchaseRobot(Robot robot)
	{
		this.purchasableRobots.Remove(robot);
		this.ownedRobots.Add(robot);
		this.AddFunds(-robot.type.price);
		this.OnOwnedRobotListChanged();
		this.OnPurchasableListChanged();
	}

	public void ClearPurchaseList()
	{
		this.purchasableRobots.Clear();
		this.OnPurchasableListChanged();
	}

	public void AddPurchasables(Robot[] robots)
	{
		this.purchasableRobots.AddRange(robots);
		this.OnPurchasableListChanged();
	}
}
