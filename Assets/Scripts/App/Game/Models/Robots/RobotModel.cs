using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class RobotModel : Model<Robot>
{
	private SpriteRenderer _spriteRenderer { get; set; }
	public SpriteRenderer spriteRenderer => this._spriteRenderer ?? (this._spriteRenderer = this.GetComponent<SpriteRenderer>());
}