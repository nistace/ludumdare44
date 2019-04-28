using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanel : AbstractUIMonoBehaviour
{
	public Button tabButton;
	private Image _tabImage { get; set; }
	public Image tabImage => this._tabImage ?? (this._tabImage = this.tabButton.GetComponent<Image>());

	public Transform purchaseBoxContainer;
	public PurchaseBox boxPrefab;
	public readonly List<PurchaseBox> purchaseBoxes = new List<PurchaseBox>();

	private void Awake()
	{
		this.Refresh();
		Game.current.OnPurchasableListChanged += this.Refresh;
	}

	private void Refresh()
	{
		for (int i = 0; i < Game.current.purchasableRobots.Count; ++i)
		{
			PurchaseBox box = null;
			if (i < this.purchaseBoxes.Count) box = this.purchaseBoxes[i];
			else
			{
				box = Instantiate(this.boxPrefab, this.purchaseBoxContainer);
				box.transform.SetSiblingIndex(i);
				this.purchaseBoxes.Add(box);
			}
			box.gameObject.SetActive(true);
			box.robot = Game.current.purchasableRobots[i];
			box.Refresh();
		}
		for (int i = Game.current.purchasableRobots.Count; i < this.purchaseBoxes.Count; ++i)
		{
			this.purchaseBoxes[i].gameObject.SetActive(false);
		}
	}

	public void MoreChoices()
	{
		GameController.instance.RerollPurchassableRobots();
	}

	public void HelpMoreChoices()
	{
		GameController.instance.SetHelpMessage("Click this button to re-roll the purchasable robots list and get brand new ones. Don't ever feel like you MUST buy a robot you don't instantly fall in love with. We will find what you desire.");
	}
}
