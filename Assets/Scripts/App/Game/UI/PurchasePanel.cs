using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanel : AbstractUIMonoBehaviour
{
	public Button tabButton;
	private Image _tabImage { get; set; }
	public Image tabImage => this._tabImage ?? (this._tabImage = this.tabButton.GetComponent<Image>());
}
