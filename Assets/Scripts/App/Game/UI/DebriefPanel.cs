using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriefPanel : MonoBehaviour
{
	public Color defaultStatusColor = Color.black;
	public Color negativeStatusColor = Color.red;
	public Color positiveStatusColor = Color.green;


	public TMPro.TMP_Text statusValue;
	public TMPro.TMP_Text objectivesValue;
	public TMPro.TMP_Text robotsLostValue;
	public TMPro.TMP_Text rewardValue;
	public TMPro.TMP_Text maintenantValue;


	public void Update()
	{
		if (Game.current != null)
		{
			if (!Game.current.executionResult.done)
			{
				this.statusValue.text = "Running";
				this.statusValue.color = defaultStatusColor;
			}
			else if (Game.current.executionResult.success)
			{
				this.statusValue.text = "Succeeded";
				this.statusValue.color = positiveStatusColor;
			}
			else
			{
				this.statusValue.text = "Failed";
				this.statusValue.color = negativeStatusColor;
			}
			this.objectivesValue.text = Game.current.executionResult.objectivesReached + "/" + Game.current.executionResult.objectives;
			this.robotsLostValue.text = Game.current.executionResult.lostRobots.ToString();
			this.rewardValue.text = "+$" + Game.current.executionResult.earnedAmount.ToString("0.00");
			this.maintenantValue.text = Game.current.status == Game.Status.Played ? "-$" + Game.current.maintenanceCost.ToString("0.00") : "?";
		}
	}
}
