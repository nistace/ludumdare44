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
	public float price;
	public float initialMaintenanceCost;
	public float useCostPercentage;


	public string specialAbilityName
	{
		get
		{
			switch (this.specialAbility)
			{
				case SpecialAbility.platform: return "Platform";
				case SpecialAbility.shoot: return "Shoot";
			}
			return null;
		}
	}

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
