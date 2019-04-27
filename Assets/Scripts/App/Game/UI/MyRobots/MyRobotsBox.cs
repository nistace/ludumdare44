using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MyRobotsBox : AbstractUIMonoBehaviour
{
	private Robot robot { get; set; }

	public Sprite defaultSelectSprite;
	public Sprite selectedSprite;

	public Image selectImage;
	public Image iconImage;
	public TMPro.TMP_Text nameText;

	public TMPro.TMP_Text maintenanceCostText;
	public TMPro.TMP_Text useCost;

	public Button editProgrammationButton;
	public Button placeRobotButton;
	private Image _placeRobotButtonImage { get; set; }
	public Image placeRobotButtonImage => this._placeRobotButtonImage ?? (this._placeRobotButtonImage = this.placeRobotButton.GetComponent<Image>());

	public void Start()
	{
		this.editProgrammationButton.onClick.AddListener(this.ToggleProgrammation);
		this.placeRobotButton.onClick.AddListener(this.SelectRobotToPlace);
	}

	public void SetRobot(Robot robot)
	{
		this.robot = robot;
		this.Refresh();
	}

	internal void Refresh()
	{
		if (this.robot != null)
		{
			this.iconImage.sprite = this.robot.type.icon;
			this.iconImage.color = this.robot.color;
			this.nameText.text = this.robot.name;
			this.maintenanceCostText.text = "- $" + this.robot.maintenanceCost.ToString("0.00");
			this.useCost.text = "+ $" + this.robot.useCost.ToString("0.00");
			this.placeRobotButtonImage.sprite = this.robot.inLevel ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		}
	}

	public void SetPlacementEnabled(bool enabled)
	{
		this.placeRobotButton.gameObject.SetActive(enabled);
	}

	internal void SetSelected(bool selected)
	{
		this.selectImage.sprite = selected ? this.selectedSprite : this.defaultSelectSprite;
	}

	private void SelectRobotToPlace()
	{
		if (this.robot.inLevel)
		{
			GameController.instance.RemoveRobotSpawn(this.robot);
		}
		else
		{
			GameController.instance.SelectRobot(this.robot);
		}
	}

	private void ToggleProgrammation()
	{
		throw new NotImplementedException();
	}

	public void HelpUseCost()
	{
		GameController.instance.SetHelpMessage("Maintenance will be increased by $" + this.robot.useCost.ToString("0.00") + " if you use this robot in the next execution");
	}

	public void HelpMaintenanceCost()
	{
		GameController.instance.SetHelpMessage("After any level execution, $" + this.robot.maintenanceCost.ToString("0.00") + " will be debited to maintain this robot (but don't forget that a destroyed robot does not require maintenance)");
	}

	public void HelpPlacement()
	{
		if (this.robot.inLevel)
		{
			GameController.instance.SetHelpMessage("This robot will act in during the next execution. Click again to remove it from the next execution");
		}
		else
		{
			GameController.instance.SetHelpMessage("Click on this button then on a spawn position to assign this robot to the next execution");
		}
	}

	public void HelpProgrammation()
	{
		GameController.instance.SetHelpMessage("Click on this button to display/hide this robot AI code and edit instructions");
	}
}
