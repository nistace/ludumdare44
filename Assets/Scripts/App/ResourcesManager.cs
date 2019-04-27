using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ResourcesManager
{

	public static Dictionary<Type, AbstractType[]> loadedTypes = new Dictionary<Type, AbstractType[]>();
	public static Texture2D[] levelTextures;
	//public static Dictionary<string, IList<>


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
}
