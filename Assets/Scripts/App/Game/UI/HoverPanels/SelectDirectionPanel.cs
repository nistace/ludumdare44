using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectDirectionPanel : SelectSomethingPanel<Programmation.ConditionDirection>
{
	public static SelectDirectionPanel instance { get; private set; }

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this.gameObject);
		this.gameObject.SetActive(false);
	}

	public void SelectTopLeft() { this.SelectOption(Programmation.ConditionDirection.topleft); }
	public void SelectTopRight() { this.SelectOption(Programmation.ConditionDirection.topright); }
	public void SelectTop() { this.SelectOption(Programmation.ConditionDirection.top); }
	public void SelectLeft() { this.SelectOption(Programmation.ConditionDirection.left); }
	public void SelectDownLeft() { this.SelectOption(Programmation.ConditionDirection.bottomleft); }
	public void SelectDownRight() { this.SelectOption(Programmation.ConditionDirection.bottomright); }
	public void SelectDown() { this.SelectOption(Programmation.ConditionDirection.bottom); }
	public void SelectRight() { this.SelectOption(Programmation.ConditionDirection.right); }

	public void HelpTopLeft() { GameController.instance.SetHelpMessage("Check the tile above the robot, on the left"); }
	public void HelpTopRight() { GameController.instance.SetHelpMessage("Check the tile above the robot, on the right"); }
	public void HelpTop() { GameController.instance.SetHelpMessage("Check the tile above the robot"); }
	public void HelpLeft() { GameController.instance.SetHelpMessage("Check the tile on the left of the robot"); }
	public void HelpDown() { GameController.instance.SetHelpMessage("Check the tile under the robot"); }
	public void HelpDownLeft() { GameController.instance.SetHelpMessage("Check the tile under the robot, on the left"); }
	public void HelpDownRight() { GameController.instance.SetHelpMessage("Check the tile under the robot, on the right"); }
	public void HelpRight() { GameController.instance.SetHelpMessage("Check the tile on the right the robot"); }
}
