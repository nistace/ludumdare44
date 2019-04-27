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
}
