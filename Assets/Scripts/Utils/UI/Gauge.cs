using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
	public RectTransform filler;

	internal void SetProgress(float progress)
	{
		this.filler.anchorMin = Vector2.zero;
		this.filler.anchorMax = new Vector2(progress, 1);
		this.filler.offsetMin = Vector2.zero;
		this.filler.offsetMax = Vector2.zero;
	}
}
