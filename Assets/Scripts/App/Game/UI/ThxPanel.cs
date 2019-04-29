using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThxPanel : MonoBehaviour
{
	public void GoToMenu()
	{
		AudioManager.instance.PlayButtonSfx();
		App.instance.LoadMainScene();
	}
}
