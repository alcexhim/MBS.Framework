//
//  ReadOnlyLinkedListt.cs
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
using System.Collections.Generic;

namespace MBS.Framework.Collections.Generic
{
	public class AppendOnlyLinkedList<T> : ICollection<T>
	{
		private LinkedList<T> _list = new LinkedList<T>();

		protected LinkedListNode<T> Last { get { return _list.Last; } }

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public int Count { get { return _list.Count; } }

		bool ICollection<T>.IsReadOnly { get; } = false;

		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		protected virtual void InsertItem(T item)
		{
			_list.AddLast(item);
		}
		public void Add(T item)
		{
			InsertItem(item);
		}
	}
}
