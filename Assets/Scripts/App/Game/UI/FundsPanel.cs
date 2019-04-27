using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundsPanel : AbstractUIMonoBehaviour
{
	public Color positiveColor = Color.white;
	public Color negativeColor = Color.red;

	public TMPro.TMP_Text fundsText;

	private void Start()
	{
		this.Refresh(0);
		Game.current.OnFundsChanged += this.Refresh;
	}

	public void Refresh(float diff)
	{
		this.fundsText.text = Game.current.funds.ToString("0.00");
		this.fundsText.color = Game.current.funds > 0 ? this.positiveColor : this.negativeColor;
	}

	public void Help()
	{
		GameController.instance.SetHelpMessage("You currently have $" + Game.current.funds.ToString("0.00") + ". If after the next execution, you are in the red, game will be over.");
	}
}
