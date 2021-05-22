//
//  ExtensionMethods.cs - implements generic-typed extension methods for System.Collections classes
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
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

namespace MBS.Framework.Collections.Generic
{
	public static class ExtensionMethods
	{
		private static Dictionary<System.Collections.IList, Dictionary<Type, System.Collections.IList>> cacheOfT = new Dictionary<System.Collections.IList, Dictionary<Type, System.Collections.IList>>();
		/// <summary>
		/// Returns an array of type <typeparamref name="T" /> which consists of all elements in the specified <see cref="System.Collections.IList" /> that are of type
		/// <typeparamref name="T" />.
		/// </summary>
		/// <returns>An array of type <typeparamref name="T" /> which consists of all elements in the specified <see cref="System.Collections.IList" /> that are of type
		/// <typeparamref name="T" />.</returns>
		/// <param name="obj">A <see cref="System.Collections.IList" /> containing the elements to search.</param>
		/// <typeparam name="T">A type parameter indicating the type of elements to retrieve.</typeparam>
		public static T[] OfType<T>(this System.Collections.IList obj)
		{
			if (!cacheOfT.ContainsKey(obj))
			{
				cacheOfT[obj] = new Dictionary<Type, System.Collections.IList>();
			}
			if (!cacheOfT[obj].ContainsKey(typeof(T)))
			{
				List<T> list = new List<T>();
				for (int i = 0; i < obj.Count; i++)
				{
					if (obj[i] is T)
					{
						list.Add((T)obj[i]);
					}
				}
				cacheOfT[obj][typeof(T)] = list;
			}
			return ((List<T>)(cacheOfT[obj][typeof(T)])).ToArray();
		}

		public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				list.Add(item);
			}
		}
	}
}
