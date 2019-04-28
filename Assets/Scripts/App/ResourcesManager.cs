using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
	public static ResourcesManager instance { get; set; }

	public static Dictionary<Type, AbstractType[]> loadedTypes = new Dictionary<Type, AbstractType[]>();

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
	public static readonly Dictionary<string, Texture2D> levelTextures = new Dictionary<string, Texture2D>();

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

	public static Texture2D LoadLevelTexture(string levelName)
	{
		if (!levelTextures.ContainsKey(levelName)) levelTextures.Add(levelName, Resources.Load<Texture2D>("Levels/" + levelName));
		return levelTextures[levelName];
	}

	public static AudioClip LoadAudioClip(string name)
	{
		if (!audioClips.ContainsKey(name))
			audioClips.Add(name, Resources.Load<AudioClip>("Audio/" + name));
		return audioClips[name];
	}

	public static void LoadLevelsData(out string[] levelNames, out string[] helpMessages)
	{
		TextAsset descriptor = Resources.Load<TextAsset>("Levels/levels");
		List<string> levelNamesList = new List<string>();
		Dictionary<string, List<string>> levelHelp = new Dictionary<string, List<string>>();
		string level = default;
		foreach (string line in descriptor.text.Split(Environment.NewLine[0]))
		{
			string cleanLine = line.Trim();
			if (cleanLine.StartsWith("lvl "))
			{
				level = cleanLine.Substring("lvl ".Length).Trim();
				levelHelp.Add(level, new List<string>());
				levelNamesList.Add(level);
			}
			else if (cleanLine.StartsWith("help "))
			{
				levelHelp[level].Add(cleanLine.Substring("help ".Length).Trim());
			}
			else
			{
				Debug.LogWarning("Couldn't parse the line " + cleanLine);
			}
		}
		levelNames = levelNamesList.ToArray();
		helpMessages = new string[levelNames.Length];
		for (int i = 0; i < levelNames.Length; ++i)
		{
			helpMessages[i] = String.Join(Environment.NewLine + Environment.NewLine, levelHelp[levelNames[i]]);
		}
	}
}
