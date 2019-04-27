using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnModel : WorldTileModel
{
	private bool activeSpawn;

	private void Start()
	{
		this.SetActiveSpawn(this.entity != null && this.entity.active);
	}

	public void SetActiveSpawn(bool active)
	{
		this.activeSpawn = active;
		this.animator.SetBool("Active", this.activeSpawn);
	}

	public void Update()
	{
		if (this.activeSpawn != this.entity.active) this.SetActiveSpawn(this.entity.active);
	}
}
