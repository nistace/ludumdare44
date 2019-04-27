using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Cursor : MonoBehaviour
{
	private SpriteRenderer _cursorSprite { get; set; }
	public SpriteRenderer cursorSprite => this._cursorSprite ?? (this._cursorSprite = this.GetComponent<SpriteRenderer>());
	public SpriteRenderer placementSprite;

	public void Start()
	{

		this.RefreshPlacementImage();
		this.RefreshVisibility();
		GameController.instance.OnHoverTileChanged += this.RefreshVisibility;
		GameController.instance.OnSelectedRobotChanged += this.RefreshPlacementImage;
	}

	private void RefreshPlacementImage()
	{
		this.placementSprite.sprite = GameController.instance.selectedRobot?.type.icon;
		this.RefreshVisibility();
	}

	private void RefreshVisibility()
	{
		if (GameController.instance.hoverTile)
		{
			this.cursorSprite.enabled = true;
			this.placementSprite.enabled = this.placementSprite.sprite != null;
			this.transform.position = new Vector3(GameController.instance.hoverTileCoordinates.x, GameController.instance.hoverTileCoordinates.y, 0);
		}
		else
		{
			this.cursorSprite.enabled = false;
			this.placementSprite.enabled = false;
		}
	}
}
