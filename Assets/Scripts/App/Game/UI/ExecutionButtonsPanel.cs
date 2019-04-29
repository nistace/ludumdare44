using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecutionButtonsPanel : AbstractUIMonoBehaviour
{
	public float pauseSpeed = 0;
	public float normalSpeed = 1;
	public float fastSpeed = 3;

	public Button executeButton;
	public Button pauseButton;
	private Image _pauseButtonImage { get; set; }
	public Image pauseButtonImage => this._pauseButtonImage ?? (this._pauseButtonImage = this.pauseButton.GetComponent<Image>());
	public Button normalSpeedButton;
	private Image _normalSpeedButtonImage { get; set; }
	public Image normalSpeedButtonImage => this._normalSpeedButtonImage ?? (this._normalSpeedButtonImage = this.normalSpeedButton.GetComponent<Image>());
	public Button fastSpeedButton;
	private Image _fastSpeedButtonImage { get; set; }
	public Image fastSpeedButtonImage => this._fastSpeedButtonImage ?? (this._fastSpeedButtonImage = this.fastSpeedButton.GetComponent<Image>());
	public Button abortButton;

	public Button retryButton;
	public Button nextLevelButton;
	public Button gameOverButton;
	public Button gameFinishedButton;
	public TMPro.TMP_Text selectOneRobotText;

	private void Start()
	{
		this.Refresh();
		GameTime.OnSpeedChanged += this.Refresh;
		GameController.instance.OnWorldChanged += this.Refresh;
		GameController.instance.OnRobotSpawnChanged += this.Refresh;
		GameController.instance.OnGameStatusChanged += this.Refresh;
	}

	private void Refresh()
	{
		if (Game.current != null && Game.current.world != null)
		{
			this.selectOneRobotText.gameObject.SetActive(Game.current.status == Game.Status.Preparing && Game.current.world.robotsInWorld.Count == 0);
			this.executeButton.gameObject.SetActive(Game.current.status == Game.Status.Preparing && Game.current.world.robotsInWorld.Count > 0);

			this.pauseButton.gameObject.SetActive(Game.current.status == Game.Status.Playing);
			this.pauseButtonImage.sprite = GameTime.speed == this.pauseSpeed ? App.instance.tabActiveSprite : App.instance.defaultTabSprite;
			this.pauseButton.interactable = GameTime.speed != this.pauseSpeed;

			this.normalSpeedButton.gameObject.SetActive(Game.current.status == Game.Status.Playing);
			this.normalSpeedButtonImage.sprite = GameTime.speed == this.normalSpeed ? App.instance.tabActiveSprite : App.instance.defaultTabSprite;
			this.normalSpeedButton.interactable = GameTime.speed != this.normalSpeed;

			this.fastSpeedButton.gameObject.SetActive(Game.current.status == Game.Status.Playing);
			this.fastSpeedButtonImage.sprite = GameTime.speed == this.fastSpeed ? App.instance.tabActiveSprite : App.instance.defaultTabSprite;
			this.fastSpeedButton.interactable = GameTime.speed != this.fastSpeed;

			this.abortButton.gameObject.SetActive(Game.current.status == Game.Status.Playing);

			this.retryButton.gameObject.SetActive(Game.current.status == Game.Status.Played && !Game.current.executionResult.success && Game.current.funds >= 0);
			this.nextLevelButton.gameObject.SetActive(Game.current.status == Game.Status.Played && Game.current.executionResult.success && Game.current.funds >= 0 && GameController.instance.levelIndex + 1 < GameController.instance.levels.Length);
			this.gameOverButton.gameObject.SetActive(Game.current.status == Game.Status.Played && Game.current.funds < 0);
			this.gameFinishedButton.gameObject.SetActive(Game.current.status == Game.Status.Played && Game.current.funds >= 0 && Game.current.executionResult.success && GameController.instance.levelIndex + 1 >= GameController.instance.levels.Length);
		}
	}

	public void HelpExecute()
	{
		GameController.instance.SetHelpMessage("Click this button to start the level resolution (you need to assign at least one robot). Careful : no matter the outcome, you will have to pay the maintenance cost for every robot you own.");
	}

	public void HelpAbort()
	{
		GameController.instance.SetHelpMessage("Click this button to abort the level resolution after the current action is done. If you didn't gather all the rewards, you may not progress to the next level.");
	}

	public void HelpPause()
	{
		GameController.instance.SetHelpMessage("Click this button to pause the level resolution.");
	}

	public void HelpNormalSpeed()
	{
		GameController.instance.SetHelpMessage("Click this button to progress at normal speed.");
	}

	public void HelpFastSpeed()
	{
		GameController.instance.SetHelpMessage("Click this button to progress at higher speed.");
	}

	public void HelpRetry()
	{
		GameController.instance.SetHelpMessage("Click this button to restart this level. If you want to finish the game, it is actually not an option.");
	}

	public void HelpNextLevel()
	{
		GameController.instance.SetHelpMessage("Congratulations on this one! Click this button to start the next level.");
	}

	public void HelpGameOver()
	{
		if (Game.current.funds >= 0)
		{
			GameController.instance.SetHelpMessage("That's it! You made it though all the levels! And you managed to have a positive balance in the end. Impressive! Thanks for playing!");
		}
		else
		{
			GameController.instance.SetHelpMessage("You are in the red. You couldn't manage your money, and all your machines are now broken. World War III is inevitable.");
		}
	}

	public void NextLevel()
	{
		GameController.instance.LoadNextLevel();
		AudioManager.instance.PlayButtonSfx();
	}

	public void Retry()
	{
		GameController.instance.LoadCurrentLevel();
		AudioManager.instance.PlayButtonSfx();
	}

	public void GameOver()
	{
		App.instance.LoadThxScene();
		AudioManager.instance.PlayButtonSfx();
	}

	public void SetPauseSpeed()
	{
		GameTime.instance.SetSpeed(this.pauseSpeed);
		AudioManager.instance.PlayButtonSfx();
	}

	public void SetNormalSpeed()
	{
		GameTime.instance.SetSpeed(this.normalSpeed);
		AudioManager.instance.PlayButtonSfx();
	}

	public void SetFastSpeed()
	{
		GameTime.instance.SetSpeed(this.fastSpeed);
		AudioManager.instance.PlayButtonSfx();
	}

	public void Execute()
	{
		GameController.instance.StartExecution();
		AudioManager.instance.PlayButtonSfx();
	}

	public void StopAsSoonAsPossible()
	{
		Game.current.status = Game.Status.Finishing;
		GameTime.instance.SetSpeed(this.fastSpeed);
		AudioManager.instance.PlayButtonSfx();
	}
}
