using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class WorldTileModel : Model<WorldTile>
{
	private Animator _animator { get; set; }
	protected Animator animator => this._animator ?? (this._animator = this.GetComponent<Animator>());
}
