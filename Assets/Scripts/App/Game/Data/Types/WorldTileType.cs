using UnityEngine;

[CreateAssetMenu]
public class WorldTileType : AbstractType
{
	public enum Objective
	{
		Reach, Destroy
	}

	public enum ObstacleType
	{
		Fixed, WalkThrough, Push
	}

	public Color parserColor;
	public GameObject prefab;
	public string help;
	public ObstacleType obstacleType;
	public bool robotSpawn;

	public int health;
	public float reward;
	public Objective objective;
}
