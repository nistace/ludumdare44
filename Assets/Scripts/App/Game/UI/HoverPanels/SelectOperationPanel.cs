using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectOperationPanel : SelectSomethingPanel<Programmation.Operation>
{
	public static SelectOperationPanel instance { get; private set; }

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
}
