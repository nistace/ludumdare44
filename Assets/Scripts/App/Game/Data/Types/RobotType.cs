using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class RobotType : AbstractType
{
	public enum SpecialAbility { platform, shoot }

	public GameObject prefab;
	public Sprite icon;
	public SpecialAbility specialAbility;

	public string helpDisplayAbility
	{
		get
		{
			switch (this.specialAbility)
			{
				case SpecialAbility.platform: return "Extend the arm to bring the platform up";
				case SpecialAbility.shoot: return "Shoot (on the right)";
			}
			return null;
		}
	}
}
