using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HelpLine : AbstractUIMonoBehaviour
{
	private Image _background { get; set; }
	private Image background => this._background ?? (this._background = this.GetComponent<Image>());
	public TMPro.TMP_Text text;

	private void Awake()
	{
		this.Refresh();
		GameController.instance.OnHelpMessageChanged += this.Refresh;
	}

	private void Refresh()
	{
		if (string.IsNullOrEmpty(GameController.instance.helpMessage))
		{
			this.background.enabled = false;
			this.text.enabled = false;
		}
		else
		{
			this.background.enabled = true;
			this.text.enabled = true;
			this.text.text = GameController.instance.helpMessage;
		}
	}
}
