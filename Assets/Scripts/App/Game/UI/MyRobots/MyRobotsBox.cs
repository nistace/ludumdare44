using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MyRobotsBox : AbstractUIMonoBehaviour
{


	private RectTransform _rectTransform { get; set; }
	public RectTransform rectTransform => this._rectTransform ?? (this._rectTransform = this.GetComponent<RectTransform>());

	private Robot robot { get; set; }

	public int heightCollapsed = 140;
	public int heightExtended = 460;

	private bool showProgrammation = false;

	public Sprite defaultSelectSprite;
	public Sprite selectedSprite;

	public Image selectImage;
	public Image iconImage;
	public TMPro.TMP_Text nameText;

	public TMPro.TMP_Text maintenanceCostText;
	public TMPro.TMP_Text useCost;

	public Button editProgrammationButton;
	private Image _editProgrammationButtonImage { get; set; }
	public Image editProgrammationButtonImage => this._editProgrammationButtonImage ?? (this._editProgrammationButtonImage = this.editProgrammationButton.GetComponent<Image>());
	public Button placeRobotButton;
	private Image _placeRobotButtonImage { get; set; }
	public Image placeRobotButtonImage => this._placeRobotButtonImage ?? (this._placeRobotButtonImage = this.placeRobotButton.GetComponent<Image>());

	public GameObject programmationPartGameObject;
	public Transform orderContainerTransform;
	public MyRobotOrderButton orderButtonPrefab;
	public readonly List<MyRobotOrderButton> orderButtons = new List<MyRobotOrderButton>();

	public Transform programmationBoxsContainer;
	public MyRobotoProgrammationBox programmationBoxPrefab;
	public readonly List<MyRobotoProgrammationBox> programmationBoxes = new List<MyRobotoProgrammationBox>();
	public Image otherwiseOperationSprite;

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
		if (this.robot != null && Game.current != null && Game.current.world != null)
		{
			this.iconImage.sprite = this.robot.type.icon;
			this.iconImage.color = this.robot.color;
			this.nameText.text = this.robot.name;
			this.maintenanceCostText.text = "- $" + this.robot.maintenanceCost.ToString("0.00");
			this.useCost.text = "+ $" + this.robot.useCost.ToString("0.00");
			this.placeRobotButtonImage.sprite = this.robot.inLevel ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;


			this.programmationPartGameObject.SetActive(this.showProgrammation);
			this.rectTransform.anchorMin = Vector2.zero;
			this.rectTransform.anchorMax = Vector2.zero;
			this.rectTransform.offsetMin = Vector2.zero;
			this.rectTransform.offsetMax = new Vector2(0, this.showProgrammation ? this.heightExtended : this.heightCollapsed);
			this.editProgrammationButtonImage.sprite = this.showProgrammation ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;

			if (this.showProgrammation)
			{
				this.RefreshProgrammationOrder();
				this.RefreshProgrammationInstructions();
			}

		}
	}

	private void RefreshProgrammationOrder()
	{
		int order = Game.current.world.robotsInWorld.IndexOf(this.robot);
		for (int i = 0; i < Game.current.world.robotsInWorld.Count; ++i)
		{
			MyRobotOrderButton button = null;
			if (i < this.orderButtons.Count) button = this.orderButtons[i];
			else
			{
				button = Instantiate(this.orderButtonPrefab, this.orderContainerTransform);
				button.button.onClick.AddListener(delegate { this.SetOrder(i); });
				button.text.text = i.ToString();
				this.orderButtons.Add(button);
			}
			button.gameObject.SetActive(true);
			button.image.sprite = order == i ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
			button.button.interactable = order != i;
		}
		for (int i = Game.current.world.robotsInWorld.Count; i < this.orderButtons.Count; ++i)
		{
			this.orderButtons[i].gameObject.SetActive(false);
		}
	}

	private void RefreshProgrammationInstructions()
	{
		for (int i = 0; i < this.robot.instructions.Count; ++i)
		{
			MyRobotoProgrammationBox box = null;
			if (i < this.programmationBoxes.Count) box = this.programmationBoxes[i];
			else
			{
				box = Instantiate(this.programmationBoxPrefab, this.programmationBoxsContainer);
				box.transform.SetSiblingIndex(i);
				this.programmationBoxes.Add(box);
			}
			box.gameObject.SetActive(true);
			box.robot = this.robot;
			box.instruction = this.robot.instructions[i];
			box.instructionIndex = i;
			box.Refresh();
		}
		for (int i = this.robot.instructions.Count; i < this.programmationBoxes.Count; ++i)
		{
			this.programmationBoxes[i].gameObject.SetActive(false);
		}
		this.otherwiseOperationSprite.sprite = ResourcesManager.operationSprites[this.robot.elseOperation];
	}


	private void SetOrder(int i)
	{
		GameController.instance.SetRobotProgrammationOrder(this.robot, i);
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

	public void AddProgrammationOption()
	{
		GameController.instance.AddRobotProgrammationOption(this.robot);
	}

	private void ToggleProgrammation()
	{
		this.showProgrammation = !this.showProgrammation;
		this.Refresh();
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

	public void HelpOtherwiseProgrammationOption()
	{
		GameController.instance.SetHelpMessage("This option is the default one. If no other option is set or none is matched, this is the operation that will be done: " + this.robot.elseOperation);
	}


	public void HelpProgrammation()
	{
		GameController.instance.SetHelpMessage("Click on this button to display/hide this robot AI code and edit instructions");
	}

	public void SelectOtherwiseOperation()
	{
		SelectOperationPanel.instance.Open(this.SelectOtherwiseOperation, this.robot.type.helpDisplayAbility);
	}

	private void SelectOtherwiseOperation(Programmation.Operation operation)
	{
		GameController.instance.SetRobotOtherwiseOperation(this.robot, operation);
	}
}
