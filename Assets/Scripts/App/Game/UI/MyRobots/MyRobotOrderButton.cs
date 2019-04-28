using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyRobotOrderButton : AbstractUIMonoBehaviour
{
	public Button button;
	public Image image;
	public TMPro.TMP_Text text;
	public Robot robot;
	public int orderIndex;

	public void SetOrder()
	{
		GameController.instance.SetRobotProgrammationOrder(this.robot, this.orderIndex);
		AudioManager.instance.PlayButtonSfx();
	}


	public void Help()
	{
		GameController.instance.SetHelpMessage("Select the execution order here. A lower value makes the robot act earlier. (" + this.orderIndex + ")");
	}
}
