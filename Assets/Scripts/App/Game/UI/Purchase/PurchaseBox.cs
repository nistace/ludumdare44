using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseBox : AbstractUIMonoBehaviour
{
	public Robot robot { get; set; }

	public Image robotIcon;
	public TMPro.TMP_Text robotName;
	public TMPro.TMP_Text price;
	public TMPro.TMP_Text maintenance;
	public TMPro.TMP_Text useCost;
	public TMPro.TMP_Text specialAbility;

	internal void Refresh()
	{
		if (this.robot != null)
		{
			this.robotIcon.sprite = this.robot.type.icon;
			this.robotIcon.color = this.robot.color;
			this.robotName.text = this.robot.name;
			this.price.text = "$" + this.robot.type.price.ToString("0.00");
			this.maintenance.text = "$" + this.robot.type.initialMaintenanceCost.ToString("0.00");
			this.useCost.text = "+" + (this.robot.type.useCostPercentage * 100).ToString("0.00") + "%";
			this.specialAbility.text = this.robot.type.specialAbilityName;
		}
	}


	public void Purchase()
	{
		GameController.instance.Purchase(this.robot);
		AudioManager.instance.PlayButtonSfx();
	}

	public void HelpPrice()
	{
		GameController.instance.SetHelpMessage("This is the price you have to pay once to acquire the robot ($" + this.robot.type.price.ToString("0.00") + ").");
	}

	public void HelpMaintenance()
	{
		GameController.instance.SetHelpMessage("As long as this robot exists, you will have to pay its maintenance cost ($" + this.robot.type.initialMaintenanceCost.ToString("0.00") + ").");
	}

	public void HelpUseCost()
	{
		GameController.instance.SetHelpMessage("Each time you will use this robot in a level, its maintenance cost will be indefinitely increased (+" + (this.robot.type.useCostPercentage * 100).ToString("0.00") + "% each time)");
	}

	public void HelpSpecialAbility()
	{
		GameController.instance.SetHelpMessage("Special ability: " + this.robot.type.helpDisplayAbility);
	}
}
