using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnModel : WorldTileModel
{

	private void Start()
	{
		this.SetActiveSpawn(true);
		this.animator.SetBool("Active", this.entity.active);
	}

	public void SetActiveSpawn(bool active)
	{
		this.entity.active = active;
		this.animator.SetBool("Active", this.entity.active);
	}
}
