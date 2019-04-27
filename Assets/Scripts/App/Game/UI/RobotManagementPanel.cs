using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotManagementPanel : AbstractUIMonoBehaviour
{
	public Sprite defaultTabSprite;
	public Sprite selectedTabSprite;

	public MyRobotsPanel myRobotsPanel;
	public PurchasePanel purchasePanel;

	private void Start()
	{
		this.SelectMyRobotsTab();
	}

	public void SelectMyRobotsTab()
	{
		this.myRobotsPanel.tabImage.sprite = this.selectedTabSprite;
		this.myRobotsPanel.tabButton.interactable = false;
		this.purchasePanel.tabImage.sprite = this.defaultTabSprite;
		this.purchasePanel.tabButton.interactable = true;

		this.myRobotsPanel.gameObject.SetActive(true);
		this.purchasePanel.gameObject.SetActive(false);
	}

	public void SelectPurchaseTab()
	{
		this.purchasePanel.tabImage.sprite = this.selectedTabSprite;
		this.purchasePanel.tabButton.interactable = false;
		this.myRobotsPanel.tabImage.sprite = this.defaultTabSprite;
		this.myRobotsPanel.tabButton.interactable = true;

		this.purchasePanel.gameObject.SetActive(true);
		this.myRobotsPanel.gameObject.SetActive(false);
	}


}
