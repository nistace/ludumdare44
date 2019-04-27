using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class StringExtension
{

	public static string Random(this string chars, int size = 1, bool allowDoubles = true)
	{
		List<char> allowedChars = new List<char>(chars);
		StringBuilder str = new StringBuilder();
		for (int i = 0; i < size; ++i)
		{
			int index = UnityEngine.Random.Range(0, allowedChars.Count);
			str.Append(allowedChars[index]);
			if (!allowDoubles) allowedChars.RemoveAt(index);
		}
		return str.ToString();
	}
}
