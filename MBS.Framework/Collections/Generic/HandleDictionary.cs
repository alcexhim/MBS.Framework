//
//  HandleDictionary.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2021 Mike Becker's Software
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
namespace MBS.Framework.Collections.Generic
{
	public class HandleDictionary<TObject> : HandleDictionary<TObject, IntPtr>
	{
	}
	public class HandleDictionary<TObject, THandle>
	{
		private BidirectionalDictionary<THandle, TObject> _dict = new BidirectionalDictionary<THandle, TObject>();

		public int Count { get { return _dict.Count; } }

		public TObject GetObject(THandle handle)
		{
			return _dict.GetValue2(handle);
		}
		public THandle GetHandle(TObject obj)
		{
			return _dict.GetValue1(obj);
		}
		public void Add(THandle handle, TObject obj)
		{
			_dict.Add(handle, obj);
		}

		public bool Contains(TObject obj)
		{
			return _dict.ContainsValue2(obj);
		}
		public bool Contains(THandle handle)
		{
			return _dict.ContainsValue1(handle);
		}

		/// <summary>
		/// Removes the <typeparamref name="THandle" /> of the given <paramref name="obj" />.
		/// </summary>
		/// <param name="obj">The object to remove.</param>
		public void Remove(TObject obj)
		{
			_dict.RemoveByValue2(obj);
		}
		/// <summary>
		/// Removes the <typeparamref name="TObject" /> represented by the given <paramref name="handle" />.
		/// </summary>
		/// <param name="handle">The handle of the <typeparamref name="TObject" /> to remove.</param>
		public void Remove(THandle handle)
		{
			_dict.RemoveByValue1(handle);
		}
	}
	public static class HandleDictionaryExtensions
	{
		public static ushort Add<TObject>(this HandleDictionary<TObject, ushort> dict, TObject obj)
		{
			ushort index = (ushort)(dict.Count + 1);
			dict.Add(index, obj);
			return index;
		}
	}
}
