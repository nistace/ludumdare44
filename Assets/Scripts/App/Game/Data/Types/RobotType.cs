using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class RobotType : AbstractType
{
	public enum SpecialAbility { none, shoot, stick, gravity }

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
				case SpecialAbility.none: return "None";
				case SpecialAbility.shoot: return "Shoot";
				case SpecialAbility.gravity: return "Antigrav";
				case SpecialAbility.stick: return "Sticky";
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
				case SpecialAbility.none: return "This robot was released before it was finished. You can program it to do a special action, but... Well... Nothing will happen.";
				case SpecialAbility.shoot: return "Shoots bullets that can destroy stuff. Please don't use to destroy other robots. Note: the turret is stuck on the right. Sorry for inconvenience.";
				case SpecialAbility.gravity: return "When this robot focuses, gravity is disabled. Gravity is enabled again as soon as an other action is performed";
				case SpecialAbility.stick: return "This robot comes with its suction pad on the head. When enabled, it becomes one with anything over him. Toggle (activate to enable, reactive to disable). ";
			}
			return null;
		}
	}
}
