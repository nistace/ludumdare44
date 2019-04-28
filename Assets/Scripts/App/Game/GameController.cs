﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController instance { get; private set; }

	public int currentLevel = -1;

	public bool hoverTile { get; private set; }
	public Vector2Int hoverTileCoordinates { get; private set; }
	public Robot selectedRobot { get; private set; }
	public string helpMessage { get; private set; }

	public Dictionary<Robot, RobotModel> robotModels { get; } = new Dictionary<Robot, RobotModel>();
	public Dictionary<WorldTile, WorldTileModel> tileObjectModels { get; } = new Dictionary<WorldTile, WorldTileModel>();

	public event Action OnHoverTileChanged = delegate { };
	public event Action OnSelectedRobotChanged = delegate { };
	public event Action OnHelpMessageChanged = delegate { };
	public event Action OnRobotSpawnChanged = delegate { };
	public event Action OnWorldChanged = delegate { };

	public event Action OnGameStatusChanged = delegate { };

	public void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else
		{
			this.SetHelpMessage("Hello!");
			GameFactory.Init();
			Game.current = GameFactory.CreateGame();
			Game.current.AddPurchasables(GameFactory.CreateRandomRobots(Game.current.keepCountItemsInPurchaseList));
			Game.current.OnFundsChanged += this.PlayFundsChangedSound;
		}
	}

	private void PlayFundsChangedSound(float diff)
	{
		if (diff > 0)
		{
			AudioManager.instance.PlaySfx(ResourcesManager.LoadAudioClip("Positive"));
		}
		else
		{
			AudioManager.instance.PlaySfx(ResourcesManager.LoadAudioClip("Pay"));
		}
	}

	internal void DeleteRobotProgrammationOption(Robot robot, Programmation.Instruction instruction)
	{
		robot.instructions.Remove(instruction);
		this.OnRobotSpawnChanged();
	}

	public void Start()
	{
		this.LoadCurrentLevel();
	}

	internal void SetRobotProgrammationDirection(Programmation.Instruction instruction, Programmation.ConditionDirection direction)
	{
		instruction.conditionDirection = direction;
		this.OnRobotSpawnChanged();
	}

	internal void SetRobotProgrammationType(Programmation.Instruction instruction, Programmation.ConditionType type)
	{
		instruction.conditionType = type;
		this.OnRobotSpawnChanged();
	}

	internal void Purchase(Robot robot)
	{
		Game.current.PurchaseRobot(robot);
		Game.current.AddPurchasables(GameFactory.CreateRandomRobots(Game.current.keepCountItemsInPurchaseList - Game.current.purchasableRobots.Count));
		this.SetHelpMessage("You are now the happy owner of " + robot.name + "!");
	}

	internal void RerollPurchassableRobots()
	{
		Game.current.ClearPurchaseList();
		Game.current.AddPurchasables(GameFactory.CreateRandomRobots(Game.current.keepCountItemsInPurchaseList - Game.current.purchasableRobots.Count));
		this.SetHelpMessage("New robots for sale!");
	}


	internal void SetRobotProgrammationOperation(Programmation.Instruction instruction, Programmation.Operation type)
	{
		instruction.operation = type;
		this.OnRobotSpawnChanged();
	}

	public void LoadNextLevel()
	{
		this.currentLevel++;
		this.LoadCurrentLevel();
	}


	public void LoadCurrentLevel()
	{
		this.StartCoroutine(this.LoadLevel());
	}

	private IEnumerator LoadLevel()
	{
		this.SetHelpMessage("Loading level " + this.currentLevel);
		yield return null;
		this.ClearLevel();
		yield return null;
		Game.current.world = GameFactory.ParseWorld(ResourcesManager.LoadLevelTexture(this.currentLevel));
		for (int i = 0; i < Game.current.world.tiles.GetLength(0); ++i)
		{
			for (int j = 0; j < Game.current.world.tiles.GetLength(1); ++j)
			{
				foreach (WorldTile tile in Game.current.world.tiles[i, j])
				{
					if (tile != null)
					{
						WorldTileModel model = ModelManager.CreateModel(tile);
						this.tileObjectModels.Add(tile, model);
						model.transform.position = new Vector2(i, j);
						if (tile.type.robotSpawn) tile.active = true;
					}
				}
			}
		}
		if (CameraController.instance) CameraController.instance.Refresh();
		Game.current.status = Game.Status.Preparing;
		this.OnWorldChanged();
		this.OnGameStatusChanged();
		this.SetHelpMessage(null);
	}

	internal void SetRobotProgrammationOrder(Robot robot, int i)
	{
		Game.current.world.robotsInWorld.Remove(robot);
		Game.current.world.robotsInWorld.Insert(i, robot);
		this.OnRobotSpawnChanged();
	}

	internal void PlaceRobot()
	{
		if (this.selectedRobot != null && this.hoverTile)
		{
			WorldTile tile = Game.current.world.tiles[this.hoverTileCoordinates.x, this.hoverTileCoordinates.y].SingleOrDefault(t => t.type.robotSpawn);
			if (tile != null && tile.type.robotSpawn)
			{
				Robot previousRobot = Game.current.world.robotsInWorld.SingleOrDefault(t => t.positionInLevel == tile.worldPosition);
				if (previousRobot != null) this.RemoveRobotSpawn(previousRobot);
				this.selectedRobot.inLevel = true;
				this.selectedRobot.positionInLevel = tile.worldPosition;
				Game.current.world.robotsInWorld.Add(this.selectedRobot);
				RobotModel model = ModelManager.CreateModel(this.selectedRobot);
				model.transform.position = new Vector2(tile.worldPosition.x, tile.worldPosition.y);
				this.robotModels.Add(this.selectedRobot, model);
				Game.current.world.tiles[this.selectedRobot.xInLevel, this.selectedRobot.yInLevel].Single(t => t.type.robotSpawn).active = true;
				tile.active = false;
				this.SelectRobot(null);
				this.OnRobotSpawnChanged();
			}
		}
	}

	public void RemoveRobotSpawn(Robot robot)
	{
		ModelManager.DestroyModel(this.robotModels[robot]);
		Game.current.world.tiles[robot.xInLevel, robot.yInLevel].Single(t => t.type.robotSpawn).active = true;
		robot.inLevel = false;
		Game.current.world.robotsInWorld.Remove(robot);
		this.robotModels.Remove(robot);
		this.OnRobotSpawnChanged();
	}

	public void ClearLevel()
	{
		foreach (Robot robot in Game.current.ownedRobots)
		{
			robot.inLevel = false;
		}
		this.robotModels.Clear();
		this.tileObjectModels.Clear();
		ModelManager.DestroyAllModels<WorldTileModel, WorldTile>();
		ModelManager.DestroyAllModels<RobotModel, Robot>();
		this.OnRobotSpawnChanged();
	}

	internal void HoverTile(Vector2Int tileCoordinates)
	{
		if (tileCoordinates != this.hoverTileCoordinates || !this.hoverTile)
		{
			this.hoverTile = true;
			this.hoverTileCoordinates = tileCoordinates;
			this.OnHoverTileChanged();

			if (Game.current != null && Game.current.world != null && this.hoverTileCoordinates.x >= 0 && this.hoverTileCoordinates.x < Game.current.world.width
				&& this.hoverTileCoordinates.y >= 0 && this.hoverTileCoordinates.y < Game.current.world.height)
			{
				this.SetHelpMessage(Game.current.world.tiles[this.hoverTileCoordinates.x, this.hoverTileCoordinates.y].FirstOrDefault()?.type.help);
			}
			else
			{
				this.SetHelpMessage(null);
			}
		}
	}

	internal void UnhoverTile()
	{
		if (this.hoverTile)
		{
			this.hoverTile = false;
			this.OnHoverTileChanged();
		}
	}

	public void SelectRobot(Robot robot)
	{
		this.selectedRobot = robot;
		this.OnSelectedRobotChanged();
	}

	public void UnselectedRobot()
	{
		this.selectedRobot = null;
		this.OnSelectedRobotChanged();
	}

	internal void AddRobotProgrammationOption(Robot robot)
	{
		robot.instructions.Add(new Programmation.Instruction());
		this.OnRobotSpawnChanged();
	}

	internal void SetRobotOtherwiseOperation(Robot robot, Programmation.Operation operation)
	{
		robot.elseOperation = operation;
		this.OnRobotSpawnChanged();
	}


	public void SetHelpMessage(string helpMessage)
	{
		if (this.helpMessage != helpMessage)
		{
			this.helpMessage = helpMessage;
			this.OnHelpMessageChanged();
		}
	}

	public void StartExecution()
	{
		this.SelectRobot(null);
		GameTime.instance.SetSpeed(1);
		StartCoroutine(this.Play());
	}

	#region level execution

	public IEnumerator Play()
	{
		Game.current.status = Game.Status.Playing;
		Game.current.initialObjectives = Game.current.world.rewardCount;
		Game.current.remainingObjectives = Game.current.world.rewardCount;
		int turnsWithNoEffect = 0;
		Game.current.executionResult = new ExecutionResult { earnedAmount = 0, lostRobots = 0, success = false, done = false };
		this.OnGameStatusChanged();
		yield return null;
		while (Game.current.status == Game.Status.Playing && turnsWithNoEffect < 3 && Game.current.remainingObjectives > 0)
		{
			Game.current.somethingHappenedThisTurn = false;
			for (int i = 0; Game.current.status == Game.Status.Playing && Game.current.remainingObjectives > 0 && i < Game.current.world.robotsInWorld.Count; ++i)
			{
				Robot robot = Game.current.world.robotsInWorld[i];
				if (!Game.current.turnDestroyedRobots.Contains(robot))
				{
					Programmation.Operation robotOperation = this.EvalRobotOperation(robot);
					if (robotOperation == Programmation.Operation.nothing)
					{

					}
					else if (robotOperation == Programmation.Operation.special)
					{

					}
					else
					{
						Vector2Int direction = Vector2Int.zero;
						switch (robotOperation)
						{
							case Programmation.Operation.moveLeft: direction = Vector2Int.left; break;
							case Programmation.Operation.moveRight: direction = Vector2Int.right; break;
							case Programmation.Operation.moveBottom: direction = Vector2Int.down; break;
							case Programmation.Operation.moveTop: direction = Vector2Int.up; break;
						}
						if (this.CheckMovement(robot, direction, out List<Robot> movingRobots, out List<WorldTile> movingTileObjects))
						{
							Game.current.somethingHappenedThisTurn = true;
							bool movementEnded = false;
							Vector3 movement = Vector2.zero;
							Vector3 movementSpeedVector = new Vector3(direction.x * Game.current.movementSpeed, direction.y * Game.current.movementSpeed, 0);
							while (!movementEnded)
							{
								yield return null;
								Vector3 frameMovement = GameTime.deltaTime * movementSpeedVector;
								if ((movement + frameMovement).sqrMagnitude > 1)
								{
									movementEnded = true;
								}
								else
								{
									movement += frameMovement;
									foreach (Robot r in movingRobots) this.robotModels[r].transform.position += frameMovement;
									foreach (WorldTile t in movingTileObjects) this.tileObjectModels[t].transform.position += frameMovement;
								}
							}
							this.EndMovement(direction, movingRobots, movingTileObjects);
						}

					}

					// Gravity
					List<WorldTile> fallingItems = new List<WorldTile>();
					List<Robot> fallingRobots = new List<Robot>();
					do
					{
						fallingItems.Clear();
						fallingRobots.Clear();
						for (int j = 0; j < Game.current.world.width; ++j)
						{
							for (int k = 0; k < Game.current.world.height; ++k)
							{
								if (Game.current.world.tiles[j, k].Count > 0 || Game.current.world.robotsInWorld.Any(t => t.xInLevel == j && t.yInLevel == k))
								{
									if (k == 0 ||
										(!Game.current.world.tiles[j, k - 1].Any(t => t.type.obstacleType == WorldTileType.ObstacleType.Fixed || t.type.obstacleType == WorldTileType.ObstacleType.Push && !fallingItems.Contains(t))
										&& !Game.current.world.robotsInWorld.Any(t => t.xInLevel == j && t.yInLevel == k - 1 && !fallingRobots.Contains(t))))
									{
										fallingItems.AddRange(Game.current.world.tiles[j, k].Where(t => t.type.gravity));
										fallingRobots.AddRange(Game.current.world.robotsInWorld.Where(t => t.xInLevel == j && t.yInLevel == k));
									}
								}
							}
						}
						if (fallingItems.Count > 0 || fallingRobots.Count > 0)
						{
							Game.current.somethingHappenedThisTurn = true;
							bool movementEnded = false;
							Vector3 movement = Vector2.zero;
							Vector3 movementSpeedVector = Vector3.down * Game.current.fallSpeed;
							while (!movementEnded)
							{
								yield return null;
								Vector3 frameMovement = GameTime.deltaTime * movementSpeedVector;
								if ((movement + frameMovement).sqrMagnitude > 1)
								{
									movementEnded = true;
								}
								else
								{
									movement += frameMovement;
									foreach (Robot r in fallingRobots) this.robotModels[r].transform.position += frameMovement;
									foreach (WorldTile t in fallingItems) this.tileObjectModels[t].transform.position += frameMovement;
								}
							}
							this.EndMovement(Vector2Int.down, fallingRobots, fallingItems);
						}

					} while (fallingItems.Count + fallingRobots.Count > 0);
				}
			}
			foreach (Robot robot in Game.current.turnDestroyedRobots)
			{
				Game.current.ownedRobots.Remove(robot);
				Game.current.world.robotsInWorld.Remove(robot);
				ModelManager.DestroyModel(this.robotModels[robot]);
				this.robotModels.Remove(robot);
				Game.current.executionResult.lostRobots++;
			}
			if (Game.current.turnDestroyedRobots.Count > 0)
			{
				Game.current.turnDestroyedRobots.Clear();
				AudioManager.instance.PlaySfx(ResourcesManager.LoadAudioClip("Explosion"));
			}
			foreach (WorldTile tile in Game.current.turnDestroyedItems)
			{
				if (tile.worldPosition.x >= 0 && tile.worldPosition.x < Game.current.world.width && tile.worldPosition.y >= 0 && tile.worldPosition.y < Game.current.world.height)
					Game.current.world.tiles[tile.worldPosition.x, tile.worldPosition.y].Remove(tile);
				ModelManager.DestroyModel(this.tileObjectModels[tile]);
				this.tileObjectModels.Remove(tile);
			}
			Game.current.turnDestroyedItems.Clear();
			turnsWithNoEffect = Game.current.somethingHappenedThisTurn ? 0 : turnsWithNoEffect + 1;
		}

		yield return new WaitForSeconds(.4f);
		Game.current.status = Game.Status.Played;
		Game.current.executionResult.done = true;
		Game.current.executionResult.success = Game.current.remainingObjectives == 0;
		if (Game.current.executionResult.success)
		{
			AudioManager.instance.PlaySfx(ResourcesManager.LoadAudioClip("Success"));
		}
		else
		{
			AudioManager.instance.PlaySfx(ResourcesManager.LoadAudioClip("Lose"));
		}
		yield return new WaitForSeconds(.4f);
		foreach (Robot r in Game.current.world.robotsInWorld)
		{
			r.maintenanceCost += r.useCost;
		}
		Game.current.AddFunds(-Game.current.ownedRobots.Sum(t => t.maintenanceCost));
		this.OnGameStatusChanged();
	}

	private void EndMovement(Vector2Int direction, IEnumerable<Robot> movingRobots, IEnumerable<WorldTile> movingTileObjects)
	{

		foreach (Robot r in movingRobots)
		{
			r.positionInLevel = r.positionInLevel + direction;
		}
		foreach (Robot r in movingRobots)
		{
			List<WorldTile> itemsOnTile = this.GetTileContent(r.positionInLevel).Item1;
			foreach (WorldTile tile in itemsOnTile.Where(t => t.type.reward > 0 && t.type.objective == WorldTileType.Objective.Reach).ToList())
			{
				Game.current.AddFunds(tile.type.reward);
				Game.current.executionResult.earnedAmount += tile.type.reward;
				Game.current.remainingObjectives--;
				Game.current.world.tiles[tile.worldPosition.x, tile.worldPosition.y].Remove(tile);
				ModelManager.DestroyModel(this.tileObjectModels[tile]);
				this.tileObjectModels.Remove(tile);
			}
			this.robotModels[r].transform.position = new Vector3(r.positionInLevel.x, r.positionInLevel.y, 0);
		}
		foreach (WorldTile t in movingTileObjects)
		{
			Game.current.world.tiles[t.worldPosition.x, t.worldPosition.y].Remove(t);
			t.worldPosition += direction;
			if (t.worldPosition.x >= 0 && t.worldPosition.x < Game.current.world.width && t.worldPosition.y >= 0 && t.worldPosition.y < Game.current.world.height)
			{
				Game.current.world.tiles[t.worldPosition.x, t.worldPosition.y].Add(t);
			}
			this.tileObjectModels[t].transform.position = new Vector3(t.worldPosition.x, t.worldPosition.y, 0);
		}
		foreach (Robot r in movingRobots)
		{
			if (r.xInLevel < 0 || r.xInLevel >= Game.current.world.width || r.yInLevel < 0 || r.yInLevel >= Game.current.world.height)
			{
				Game.current.turnDestroyedRobots.Add(r);
			}
		}
		foreach (WorldTile t in movingTileObjects)
		{
			if (t.worldPosition.x < 0 || t.worldPosition.x >= Game.current.world.width || t.worldPosition.y < 0 || t.worldPosition.y >= Game.current.world.height)
			{
				Game.current.turnDestroyedItems.Add(t);
			}
		}
	}

	private bool CheckMovement(Robot robot, Vector2Int direction, out List<Robot> movingRobots, out List<WorldTile> movingTileObjects)
	{
		movingRobots = new List<Robot> { robot };
		movingTileObjects = new List<WorldTile>();
		bool movable = true;
		bool keepCheckingMovable = true;
		Vector2Int lastTileMoving = new Vector2Int(robot.xInLevel, robot.yInLevel);
		while (keepCheckingMovable)
		{
			lastTileMoving += direction;
			Tuple<List<WorldTile>, Robot> nextTileContent = this.GetTileContent(lastTileMoving);
			if (nextTileContent.Item2 != null)
			{
				movingRobots.Add(nextTileContent.Item2);
			}
			else if (nextTileContent.Item1.Any(t => t.type.obstacleType != WorldTileType.ObstacleType.WalkThrough))
			{
				if (nextTileContent.Item1.Any(t => t.type.obstacleType == WorldTileType.ObstacleType.Fixed))
				{
					movable = false;
					keepCheckingMovable = false;
				}
				else
				{
					movingTileObjects.AddRange(nextTileContent.Item1.Where(t => t.type.obstacleType == WorldTileType.ObstacleType.Push));
				}
			}
			else
			{
				movable = true;
				keepCheckingMovable = false;
			}
		}
		return movable;
	}

	private Programmation.Operation EvalRobotOperation(Robot robot)
	{
		foreach (Programmation.Instruction instruction in robot.instructions)
		{
			Tuple<List<WorldTile>, Robot> tileContent = this.GetDirectionTileContent(robot.xInLevel, robot.yInLevel, instruction.conditionDirection);
			bool conditionMet = false;
			switch (instruction.conditionType)
			{
				case Programmation.ConditionType.empty: conditionMet = tileContent.Item1.Count == 0 && tileContent.Item2 == null; break;
				case Programmation.ConditionType.anything: conditionMet = tileContent.Item1.Count > 0 || tileContent.Item2 != null; break;
				case Programmation.ConditionType.obstacle: conditionMet = tileContent.Item1.Any(t => t.type.obstacleType != WorldTileType.ObstacleType.WalkThrough) || tileContent.Item2 != null; break;
				case Programmation.ConditionType.robot: conditionMet = tileContent.Item2 != null; break;
			}
			if (conditionMet) return instruction.operation;
		}
		return robot.elseOperation;

	}

	private Tuple<List<WorldTile>, Robot> GetDirectionTileContent(int srcX, int srcY, Programmation.ConditionDirection dir)
	{
		switch (dir)
		{
			case Programmation.ConditionDirection.bottom: return GetTileContent(new Vector2Int(srcX, srcY - 1));
			case Programmation.ConditionDirection.bottomleft: return GetTileContent(new Vector2Int(srcX - 1, srcY - 1));
			case Programmation.ConditionDirection.bottomright: return GetTileContent(new Vector2Int(srcX + 1, srcY - 1));
			case Programmation.ConditionDirection.top: return GetTileContent(new Vector2Int(srcX, srcY + 1));
			case Programmation.ConditionDirection.topleft: return GetTileContent(new Vector2Int(srcX - 1, srcY + 1));
			case Programmation.ConditionDirection.topright: return GetTileContent(new Vector2Int(srcX + 1, srcY + 1));
			case Programmation.ConditionDirection.left: return GetTileContent(new Vector2Int(srcX - 1, srcY));
			case Programmation.ConditionDirection.right: return GetTileContent(new Vector2Int(srcX + 1, srcY));
			default: Debug.LogError("GetDirectionTileContent Not implemented: " + dir.ToString()); break;
		}
		return new Tuple<List<WorldTile>, Robot>(new List<WorldTile>(), null);
	}

	private Tuple<List<WorldTile>, Robot> GetTileContent(Vector2Int coordinates)
	{
		if (coordinates.x < 0 || coordinates.x >= Game.current.world.width || coordinates.y < 0 || coordinates.y >= Game.current.world.height)
			return new Tuple<List<WorldTile>, Robot>(new List<WorldTile>(), null);
		return new Tuple<List<WorldTile>, Robot>(Game.current.world.tiles[coordinates.x, coordinates.y], Game.current.world.robotsInWorld.SingleOrDefault(t => t.xInLevel == coordinates.x && t.yInLevel == coordinates.y));
	}

	#endregion


}
