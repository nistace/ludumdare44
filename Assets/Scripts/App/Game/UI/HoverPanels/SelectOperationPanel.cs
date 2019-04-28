using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectOperationPanel : SelectSomethingPanel<Programmation.Operation>
{
	public static SelectOperationPanel instance { get; private set; }

	private string specialHelp;

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this.gameObject);
		this.gameObject.SetActive(false);
	}


	public void SelectMoveLeft() { this.SelectOption(Programmation.Operation.moveLeft); }
	public void SelectMoveTop() { this.SelectOption(Programmation.Operation.moveTop); }
	public void SelectMoveDown() { this.SelectOption(Programmation.Operation.moveBottom); }
	public void SelectMoveRight() { this.SelectOption(Programmation.Operation.moveRight); }
	public void SelectSpecial() { this.SelectOption(Programmation.Operation.special); }
	public void SelectNothing() { this.SelectOption(Programmation.Operation.nothing); }


	public override void Open(Action<Programmation.Operation> callback) { this.Open(callback, null); }
	public void Open(Action<Programmation.Operation> callback, string specialHelp)
	{
		this.specialHelp = specialHelp;
		base.Open(callback);
	}


	public void HelpMoveLeft() { GameController.instance.SetHelpMessage("Instruction: move left"); }
	public void HelpMoveRight() { GameController.instance.SetHelpMessage("Instruction: move right"); }
	public void HelpMoveTop() { GameController.instance.SetHelpMessage("Instruction: move up"); }
	public void HelpMoveDown() { GameController.instance.SetHelpMessage("Instruction: move down"); }
	public void HelpSpecial() { if (this.specialHelp != null) GameController.instance.SetHelpMessage("Instruction: " + specialHelp); }
	public void HelpWait() { GameController.instance.SetHelpMessage("Instruction: do nothing"); }
}
