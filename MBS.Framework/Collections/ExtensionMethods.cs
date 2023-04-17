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
using System.Collections;
using System.Text;

namespace MBS.Framework.Collections
{
	public static class ExtensionMethods
	{
		public static bool ContainsAny<T>(this IEnumerable enumerable, T[] item)
		{
			foreach (object item1 in enumerable)
			{
				foreach (T item2 in item)
				{
					if (item1 is T)
					{
						if (item1.Equals(item2))
							return true;
					}
				}
			}
			return false;
		}
		public static bool ContainsAny<T>(this System.Collections.Generic.IEnumerable<T> enumerable, T[] item)
		{
			foreach (T item1 in enumerable)
			{
				foreach (T item2 in item)
				{
					if (item1.Equals(item2))
						return true;
				}
			}
			return false;
		}
		public static T[] ToArray<T>(this IEnumerable enumerable) where T : class
		{
			System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
			foreach (object name in enumerable)
			{
				if (name is T)
					list.Add((T)name);
			}
			return list.ToArray();
		}
		public static T[] ToNullTerminatedArray<T>(this IEnumerable enumerable) where T : class
		{
			System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
			foreach (object name in enumerable)
			{
				if (name is T)
					list.Add((T)name);
			}
			list.Add(null);
			return list.ToArray();
		}
		/// <summary>
		/// Convenience function to copy from any <see cref="IEnumerable" />
		/// into any <see cref="IList"/>.
		/// </summary>
		public static void CopyTo(this IEnumerable source, IList dest)
		{
			foreach (object o in source)
			{
				dest.Add(o);
			}
		}

		public static string ToString(this IEnumerable source, string separator)
		{
			StringBuilder sb = new StringBuilder();
			foreach (object o in source)
			{
				sb.Append(o);
				sb.Append(separator);
			}
			return sb.ToString();
		}
	}
}
