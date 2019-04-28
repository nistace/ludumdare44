using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectTypePanel : SelectSomethingPanel<Programmation.ConditionType>
{
	public static SelectTypePanel instance { get; private set; }

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this.gameObject);
		this.gameObject.SetActive(false);
	}

	public void SelectEmpty() { this.SelectOption(Programmation.ConditionType.empty); }
	public void SelectObstacle() { this.SelectOption(Programmation.ConditionType.obstacle); }
	public void SelectRobot() { this.SelectOption(Programmation.ConditionType.robot); }
}
