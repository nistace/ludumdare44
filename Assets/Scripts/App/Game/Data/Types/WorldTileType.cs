using UnityEngine;

[CreateAssetMenu]
public class WorldTileType : AbstractType
{
	public Color parserColor;
	public GameObject prefab;
	public bool obstacle;
	public bool robotSpawn;
	public int health;
}
