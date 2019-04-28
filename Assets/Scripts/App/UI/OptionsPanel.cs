using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
	public Slider musicSlider;
	public Slider sfxSlider;

	public void Start()
	{
		this.musicSlider.value = AudioManager.instance.musicVolume;
		this.sfxSlider.value = AudioManager.instance.sfxVolume;
	}

	public void SetSfxVolume()
	{
		AudioManager.instance.SetSfxVolume(this.sfxSlider.value);
	}

	public void SetMusicVolume()
	{
		AudioManager.instance.SetMusicVolume(this.musicSlider.value);
	}
}
