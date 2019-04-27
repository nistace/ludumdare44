using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelManager : MonoBehaviour
{
	private static ModelManager instance { get; set; }

	private static readonly Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

	public void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this.gameObject);
		else
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public static WorldTileModel CreateModel(WorldTile worldTile)
	{
		return GetPooledOrNew<WorldTileModel, WorldTile>(worldTile.type.prefab, worldTile);
	}

	public static void DestroyModel<E>(Model<E> model)
	{
		if (!model) return;
		if (!pool.ContainsKey(model.prefab)) pool.Add(model.prefab, new Queue<GameObject>());
		pool[model.prefab].Enqueue(model.gameObject);
		model.gameObject.SetActive(false);
		model.entity = default;
	}

	public static void DestroyAllModels<E, F>() where E : Model<F>
	{
		foreach (Model<F> model in instance.transform.GetComponentsInChildren<E>())
		{
			DestroyModel(model);
		}
	}

	private static E GetPooledOrNew<E, F>(GameObject prefab, F entity) where E : Model<F>
	{
		GameObject gameObject = null;
		if (pool.ContainsKey(prefab) && pool[prefab].Count > 0)
		{
			gameObject = pool[prefab].Dequeue();
			gameObject.SetActive(true);
		}
		else
		{
			gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, instance.transform);
		}
		E model = gameObject.GetComponent<E>();
		if (model == null) model = gameObject.AddComponent<E>();
		model.prefab = prefab;
		model.entity = entity;

		return model;
	}
}
