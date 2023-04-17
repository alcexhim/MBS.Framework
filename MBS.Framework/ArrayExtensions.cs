//
//  ArrayExtensions.cs
//
//  Author:
//       Mike Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2019 Mike Becker
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;

namespace MBS.Framework
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// Splits the <paramref name="array" /> into two arrays at the specified
		/// <paramref name="index" />, where <paramref name="left" /> contains the
		/// elements before <paramref name="index" /> and <paramref name="right" />
		/// contains the elements after <paramref name="index" />.
		/// </summary>
		/// <param name="array">The array to split.</param>
		/// <param name="index">The index at which to split the array.</param>
		/// <param name="left">The array containing elements before the split point.</param>
		/// <param name="right">The array containing elements after the split point.</param>
		/// <typeparam name="T">The <see cref="Type" /> of elements in the array.</typeparam>
		public static void Bisect<T>(this T[] array, int index, out T[] left, out T[] right)
		{
			left = new T[index];
			right = new T[array.Length - index];

			Array.Copy(array, 0, left, 0, left.Length);
			Array.Copy(array, index, right, 0, right.Length);
		}
		public static void Array_RemoveAt<T>(ref T[] array, int index, int count = 1)
		{
			T[] old = (T[])array.Clone();

			int start = index;
			int length = count;
			if (count < 0)
			{
				start = index + count;
				length = Math.Abs(count);
			}
			Array.Resize<T>(ref array, old.Length - length);

			Array.Copy(old, 0, array, 0, start);
			if (array.Length - (start + length) > -1)
				Array.Copy(old, start + length, array, start, array.Length - (start + length));
		}
		public static void Array_Append<T>(ref T[] destinationArray, T[] sourceArray)
		{
			int start = destinationArray.Length;
			Array.Resize<T>(ref destinationArray, destinationArray.Length + sourceArray.Length);
			Array.Copy(sourceArray, 0, destinationArray, start, sourceArray.Length);
		}

		/// <summary>
		/// Determines if <paramref name="array1" /> and <paramref name="array2" />
		/// contain the same elements.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if both arrays contain the same elements,
		/// <see langword="false" /> otherwise.</returns>
		/// <param name="array1">The first array to search.</param>
		/// <param name="array2">The second array to search.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Matches<T>(this IList<T> array1, IList<T> array2)
		{
			if (array1.Count != array2.Count)
			{
				// short-circuit if arrays have different lengths
				return false;
			}

			for (int i = 0; i < array1.Count; i++)
			{
				if (!array1[i].Equals(array2[i]))
					return false;
			}
			return true;
		}

		public static T[] Concat<T>(T[] array1, T[] array2)
		{
			T[] array3 = new T[array1.Length + array2.Length];
			Array.Copy(array1, 0, array3, 0, array1.Length);
			Array.Copy(array2, 0, array3, array1.Length, array2.Length);
			return array3;
		}
	}
}
