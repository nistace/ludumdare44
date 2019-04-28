using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
	public static ResourcesManager instance { get; set; }

	public static Dictionary<Type, AbstractType[]> loadedTypes = new Dictionary<Type, AbstractType[]>();
	public static Texture2D[] levelTextures;
	//public static Dictionary<string, IList<>


	public Sprite directionLeft;
	public Sprite directionRight;
	public Sprite directionUp;
	public Sprite directionDown;
	public Sprite directionUpLeft;
	public Sprite directionUpRight;
	public Sprite directionDownLeft;
	public Sprite directionDownRight;

	public Sprite contentObstacle;
	public Sprite contentEmpty;
	public Sprite contentRobot;
	public Sprite contentAnything;

	public Sprite actionLeft;
	public Sprite actionRight;
	public Sprite actionUp;
	public Sprite actionDown;
	public Sprite actionSpecial;
	public Sprite actionNothing;


	public static readonly Dictionary<Programmation.ConditionDirection, Sprite> directionSprites = new Dictionary<Programmation.ConditionDirection, Sprite>();
	public static readonly Dictionary<Programmation.ConditionType, Sprite> conditionTypeSprites = new Dictionary<Programmation.ConditionType, Sprite>();
	public static readonly Dictionary<Programmation.Operation, Sprite> operationSprites = new Dictionary<Programmation.Operation, Sprite>();

	public static readonly Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	private void Awake()
	{
		instance = this;
		directionSprites.Add(Programmation.ConditionDirection.left, this.directionLeft);
		directionSprites.Add(Programmation.ConditionDirection.right, this.directionRight);
		directionSprites.Add(Programmation.ConditionDirection.top, this.directionUp);
		directionSprites.Add(Programmation.ConditionDirection.bottom, this.directionDown);
		directionSprites.Add(Programmation.ConditionDirection.topleft, this.directionUpLeft);
		directionSprites.Add(Programmation.ConditionDirection.topright, this.directionUpRight);
		directionSprites.Add(Programmation.ConditionDirection.bottomleft, this.directionDownLeft);
		directionSprites.Add(Programmation.ConditionDirection.bottomright, this.directionDownRight);

		conditionTypeSprites.Add(Programmation.ConditionType.empty, this.contentEmpty);
		conditionTypeSprites.Add(Programmation.ConditionType.obstacle, this.contentObstacle);
		conditionTypeSprites.Add(Programmation.ConditionType.robot, this.contentRobot);
		conditionTypeSprites.Add(Programmation.ConditionType.anything, this.contentAnything);

		operationSprites.Add(Programmation.Operation.moveBottom, this.actionDown);
		operationSprites.Add(Programmation.Operation.moveLeft, this.actionLeft);
		operationSprites.Add(Programmation.Operation.moveRight, this.actionRight);
		operationSprites.Add(Programmation.Operation.moveTop, this.actionUp);
		operationSprites.Add(Programmation.Operation.nothing, this.actionNothing);
		operationSprites.Add(Programmation.Operation.special, this.actionSpecial);
	}

	public static E[] LoadAllTypes<E>() where E : AbstractType
	{
		if (!loadedTypes.ContainsKey(typeof(E))) loadedTypes.Add(typeof(E), Resources.LoadAll<E>("Types/" + typeof(E).Name.Replace("Type", "") + "s"));
		return loadedTypes[typeof(E)] as E[];
	}

	private static Texture2D[] LoadLevelTextures()
	{
		if (levelTextures == null) levelTextures = Resources.LoadAll<Texture2D>("Levels");
		return levelTextures;
	}

	public static int CountLevels()
	{
		return LoadLevelTextures().Length;
	}

	public static Texture2D LoadLevelTexture(int index)
	{
		return LoadLevelTextures()[index];
	}

	public static AudioClip LoadAudioClip(string name)
	{
		if (!audioClips.ContainsKey(name))
			audioClips.Add(name, Resources.Load<AudioClip>("Audio/" + name));
		return audioClips[name];
	}
}
