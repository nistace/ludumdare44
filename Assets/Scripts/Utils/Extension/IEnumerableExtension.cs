using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class IEnumerableExtension
{
	public static E Random<E>(this E[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static int Count<E>(this E[,] table, Func<E, bool> func)
	{
		int count = 0;
		for (int i = 0; i < table.GetLength(0); ++i)
			for (int j = 0; j < table.GetLength(1); ++j)
				if (func(table[i, j]))
					count++;
		return count;
	}
}
