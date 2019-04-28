using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyRobotoProgrammationBox : AbstractUIMonoBehaviour
{
	public Robot robot;
	public Programmation.Instruction instruction;
	public int instructionIndex;

	public TMPro.TMP_Text optionName;
	public Image conditionPositionImage;
	public Image conditionContentImage;
	public Image operationImage;

	public void Refresh()
	{
		this.optionName.text = "Option " + instructionIndex;
		this.conditionPositionImage.sprite = ResourcesManager.directionSprites[this.instruction.conditionDirection];
		this.conditionContentImage.sprite = ResourcesManager.conditionTypeSprites[this.instruction.conditionType];
		this.operationImage.sprite = ResourcesManager.operationSprites[this.instruction.operation];
	}


	public void Help()
	{
		GameController.instance.SetHelpMessage("Defines the AI of this robot. If the " + this.instructionIndex + " previous options conditions weren't met, this one will be tested. \"" + this.instruction.ToString() + "\".");
	}

	public void Remove()
	{
		GameController.instance.DeleteRobotProgrammationOption(this.robot, this.instruction);
		AudioManager.instance.PlayButtonSfx();
	}

	public void SelectDirection()
	{
		SelectDirectionPanel.instance.Open(this.SelectDirection);
		AudioManager.instance.PlayButtonSfx();
	}

	private void SelectDirection(Programmation.ConditionDirection direction)
	{
		GameController.instance.SetRobotProgrammationDirection(this.instruction, direction);
	}


	public void SelectType()
	{
		SelectTypePanel.instance.Open(this.SelectType);
		AudioManager.instance.PlayButtonSfx();
	}

	private void SelectType(Programmation.ConditionType type)
	{
		GameController.instance.SetRobotProgrammationType(this.instruction, type);
	}


	public void SelectOperation()
	{
		SelectOperationPanel.instance.Open(this.SelectOperation, this.robot.type.helpDisplayAbility);
		AudioManager.instance.PlayButtonSfx();
	}

	private void SelectOperation(Programmation.Operation type)
	{
		GameController.instance.SetRobotProgrammationOperation(this.instruction, type);
	}
}
