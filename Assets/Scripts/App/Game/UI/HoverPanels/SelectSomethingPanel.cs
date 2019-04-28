using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSomethingPanel<E> : MonoBehaviour
{
	public int width;
	public int height;
	public Vector3 distanceWithPointer;

	private RectTransform _rectTransform { get; set; }
	public RectTransform rectTransform => this._rectTransform ?? (this._rectTransform = this.GetComponent<RectTransform>());

	private Action<E> callback;

	public virtual void Open(Action<E> callback)
	{
		this.callback = callback;
		this.gameObject.SetActive(true);

		this.rectTransform.anchorMin = Vector2.zero;
		this.rectTransform.anchorMax = Vector2.zero;
		Vector3 mousePosition = Input.mousePosition;
		this.rectTransform.offsetMax = mousePosition + distanceWithPointer;
		this.rectTransform.offsetMin = mousePosition + distanceWithPointer - new Vector3(width, height, 0);
	}

	public void SelectOption(E option)
	{
		this.callback(option);
		this.gameObject.SetActive(false);
	}

	public void Update()
	{
		if (Input.GetAxis("Fire2") != 0 || Input.GetAxis("Cancel") != 0)
		{
			this.gameObject.SetActive(false);
		}
		else if (Input.GetAxis("Fire1") != 0)
		{
			Vector3 mousePosition = Input.mousePosition;
			if (mousePosition.x < this.rectTransform.offsetMin.x || mousePosition.x > this.rectTransform.offsetMax.x || mousePosition.y < this.rectTransform.offsetMin.y || mousePosition.y > this.rectTransform.offsetMax.y)
				this.gameObject.SetActive(false);
		}
	}
}
