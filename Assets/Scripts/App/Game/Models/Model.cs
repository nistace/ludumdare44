using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Model<Entity> : MonoBehaviour
{
	public Entity entity { get; set; }
	public GameObject prefab { get; set; }
}
