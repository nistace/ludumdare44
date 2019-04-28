using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MyRobotsPanel : AbstractUIMonoBehaviour
{

	public Transform boxesContainer;
	public MyRobotsBox boxPrefab;
	public Button tabButton;
	private Image _tabImage { get; set; }
	public Image tabImage => this._tabImage ?? (this._tabImage = this.tabButton.GetComponent<Image>());

	private readonly Dictionary<Robot, MyRobotsBox> boxes = new Dictionary<Robot, MyRobotsBox>();
	private readonly Queue<MyRobotsBox> emptyBoxes = new Queue<MyRobotsBox>();

	private void Awake()
	{
		this.RefreshList();
		Game.current.OnOwnedRobotListChanged += this.RefreshList;
		GameController.instance.OnSelectedRobotChanged += this.RefreshSelected;
		GameController.instance.OnWorldChanged += this.RefreshList;
		GameController.instance.OnGameStatusChanged += this.RefreshList;
		GameController.instance.OnRobotSpawnChanged += this.RefreshList;
	}

	public void HideAllProgrammation()
	{
		foreach (MyRobotsBox box in this.boxes.Values)
		{
			box.HideProgrammation();
		}
	}

	private void RefreshList()
	{
		foreach (Robot toRemove in this.boxes.Keys.Where(t => !Game.current.ownedRobots.Contains(t)).ToList())
		{
			this.RemoveBox(toRemove);
		}
		foreach (Robot newVisibleRobot in Game.current.ownedRobots.Where(t => !this.boxes.ContainsKey(t)))
		{
			this.AddBox(newVisibleRobot);
		}
		foreach (MyRobotsBox box in this.boxes.Values)
		{
			box.Refresh();
		}
		if (Game.current?.world?.robotsInWorld != null)
		{
			List<Robot> orderedRobots = Game.current.ownedRobots.OrderBy(t => t.inLevel ? Game.current.world.robotsInWorld.IndexOf(t) : Game.current.world.robotsInWorld.Count).ToList();
			for (int i = 0; i < orderedRobots.Count; ++i)
			{
				this.boxes[orderedRobots[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	private void RefreshSelected()
	{
		foreach (KeyValuePair<Robot, MyRobotsBox> robotBoxKv in this.boxes)
		{
			robotBoxKv.Value.SetSelected(GameController.instance.selectedRobot == robotBoxKv.Key);
		}
	}

	private void AddBox(Robot robot)
	{
		if (!this.boxes.ContainsKey(robot))
		{
			MyRobotsBox box = this.emptyBoxes.Count > 0 ? this.emptyBoxes.Dequeue() : Instantiate(this.boxPrefab, this.boxesContainer);
			box.SetRobot(robot);
			box.gameObject.SetActive(true);
			this.boxes.Add(robot, box);
			box.transform.SetParent(this.boxesContainer);
			box.transform.SetSiblingIndex(this.boxes.Count);
		}
	}

	private void RemoveBox(Robot robot)
	{
		if (this.boxes.ContainsKey(robot))
		{
			MyRobotsBox box = this.boxes[robot];
			box.SetRobot(null);
			box.gameObject.SetActive(false);
			this.boxes.Remove(robot);
			box.transform.SetSiblingIndex(this.boxes.Count);
		}
	}


}

