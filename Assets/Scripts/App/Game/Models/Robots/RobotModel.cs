using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class RobotModel : Model<Robot>
{
	private SpriteRenderer _spriteRenderer { get; set; }
	public SpriteRenderer spriteRenderer => this._spriteRenderer ?? (this._spriteRenderer = this.GetComponent<SpriteRenderer>());
	private Animator _animator { get; set; }
	public Animator animator => this._animator ?? (this._animator = this.GetComponent<Animator>());

	private void OnEnable()
	{
		this.SetSpecial(false);
	}

	internal void SetSpecial(bool enabled)
	{
		this.animator.SetBool("Special", enabled);
	}
}