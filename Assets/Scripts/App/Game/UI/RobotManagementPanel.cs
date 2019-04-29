using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotManagementPanel : AbstractUIMonoBehaviour
{
	public GameObject tabsPanel;
	public Sprite defaultTabSprite;
	public Sprite selectedTabSprite;

	public MyRobotsPanel myRobotsPanel;
	public PurchasePanel purchasePanel;

	public DebriefPanel debriefPanel;
	public ExecutionButtonsPanel executionButtonsPanel;

	private GameObject selectedPanel;

	public GameObject levelHelpPanel;
	public TMPro.TMP_Text levelHelpText;

	private void Start()
	{
		this.selectedPanel = this.myRobotsPanel.gameObject;
		this.Refresh();
		GameController.instance.OnGameStatusChanged += Refresh;
		GameController.instance.OnLevelHelp += this.ShowHelp;
	}


	public void ShowHelp(string helpText)
	{
		this.myRobotsPanel.gameObject.SetActive(false);
		this.purchasePanel.gameObject.SetActive(false);
		this.tabsPanel.gameObject.SetActive(false);
		this.executionButtonsPanel.gameObject.SetActive(false);
		this.debriefPanel.gameObject.SetActive(false);
		this.levelHelpPanel.SetActive(true);
		this.levelHelpText.text = helpText;
	}

	public void CloseHelp()
	{
		this.Refresh();
		AudioManager.instance.PlayButtonSfx();
	}


	private void Refresh()
	{
		this.levelHelpPanel.SetActive(false);
		this.tabsPanel.SetActive(Game.current.status == Game.Status.Preparing);
		this.myRobotsPanel.gameObject.SetActive(Game.current.status == Game.Status.Preparing && this.selectedPanel == this.myRobotsPanel.gameObject);
		this.purchasePanel.gameObject.SetActive(Game.current.status == Game.Status.Preparing && this.selectedPanel == this.purchasePanel.gameObject);
		this.myRobotsPanel.tabImage.sprite = this.selectedPanel == this.myRobotsPanel.gameObject ? App.instance.tabActiveSprite : App.instance.defaultTabSprite;
		this.myRobotsPanel.tabButton.interactable = this.selectedPanel != this.myRobotsPanel.gameObject;
		this.purchasePanel.tabImage.sprite = this.selectedPanel == this.purchasePanel.gameObject ? App.instance.tabActiveSprite : App.instance.defaultTabSprite;
		this.purchasePanel.tabButton.interactable = this.selectedPanel != this.purchasePanel.gameObject;
		this.executionButtonsPanel.gameObject.SetActive(Game.current.status != Game.Status.Preparing || this.selectedPanel != this.purchasePanel.gameObject);
		this.debriefPanel.gameObject.SetActive(Game.current.status != Game.Status.Preparing);
		if (Game.current.status != Game.Status.Preparing)
		{
			this.myRobotsPanel.HideAllProgrammation();
		}
	}

	public void SelectMyRobotsTab()
	{
		this.selectedPanel = this.myRobotsPanel.gameObject;
		this.Refresh();
		AudioManager.instance.PlayButtonSfx();
	}

	public void SelectPurchaseTab()
	{
		this.selectedPanel = this.purchasePanel.gameObject;
		this.Refresh();
		AudioManager.instance.PlayButtonSfx();
	}


}
