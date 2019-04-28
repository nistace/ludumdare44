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
}
