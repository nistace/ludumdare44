using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelManager : MonoBehaviour
{
	private static ModelManager instance { get; set; }

	public GameObject bulletPrefab;

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

	public static GameObject CreateBullet()
	{
		if (!pool.ContainsKey(instance.bulletPrefab)) pool.Add(instance.bulletPrefab, new Queue<GameObject>());
		GameObject bullet;
		if (pool[instance.bulletPrefab].Count > 0) bullet = pool[instance.bulletPrefab].Dequeue();
		else bullet = Instantiate(instance.bulletPrefab, Vector3.zero, Quaternion.identity, instance.transform);
		bullet.SetActive(true);
		return bullet;
	}

	public static void DestroyBullet(GameObject model)
	{
		if (!pool.ContainsKey(instance.bulletPrefab)) pool.Add(instance.bulletPrefab, new Queue<GameObject>());
		pool[instance.bulletPrefab].Enqueue(model);
		model.SetActive(false);
	}

	public static RobotModel CreateModel(Robot robot)
	{
		RobotModel model = GetPooledOrNew<RobotModel, Robot>(robot.type.prefab, robot);
		model.spriteRenderer.color = robot.color;
		return model;
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
