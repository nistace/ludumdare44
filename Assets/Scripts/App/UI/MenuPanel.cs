using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
	public Image easyBackground;
	public Image mediumBackground;
	public Image hardBackground;

	public void Start()
	{
		this.RefreshDifficultyButtons();
		App.instance.OnDifficultyChanged += this.RefreshDifficultyButtons;
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

	public void RefreshDifficultyButtons()
	{
		this.easyBackground.sprite = App.instance.difficultyLevel == 0 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		this.mediumBackground.sprite = App.instance.difficultyLevel == 1 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
		this.hardBackground.sprite = App.instance.difficultyLevel == 2 ? App.instance.buttonPressedSprite : App.instance.defaultButtonSprite;
	}
}
