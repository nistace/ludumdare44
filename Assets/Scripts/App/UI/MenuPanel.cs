using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
	public Image easyBackground;
	public Image mediumBackground;
	public Image hardBackground;

	public Image helpEnabledBackground;
	public Image helpDisabledBackground;

	public void Start()
	{
		this.Refresh();
		App.instance.OnParametersChanged += this.Refresh;
	}

	public void StartGame()
	{
		App.instance.LoadGameScene();
		AudioManager.instance.PlayButtonSfx();
	}

	public void SetDifficulty(int level)
	{
		App.instance.SetDifficulty(level);
		AudioManager.instance.PlayButtonSfx();
	}

	public void SetHelp(bool enabled)
	{
		App.instance.SetHelpEnabled(enabled);
		AudioManager.instance.PlayButtonSfx();
	}

	public void Refresh()
	{
		this.easyBackground.sprite = App.instance.difficultyLevel == 0 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		this.mediumBackground.sprite = App.instance.difficultyLevel == 1 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		this.hardBackground.sprite = App.instance.difficultyLevel == 2 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		this.easyBackground.sprite = App.instance.difficultyLevel == 0 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;

		this.helpEnabledBackground.sprite = App.instance.helpEnabled ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		this.helpDisabledBackground.sprite = !App.instance.helpEnabled ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
	}
}
