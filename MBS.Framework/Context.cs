//
//  Context.cs
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
	public class Context
	{
		public class ContextCollection
			: System.Collections.ObjectModel.Collection<Context>
		{
			private Dictionary<Guid, Context> _ItemsByID = new Dictionary<Guid, Context>();

			public bool Contains(Guid contextID)
			{
				return _ItemsByID.ContainsKey(contextID);
			}

			public Context this[Guid id]
			{
				get
				{
					if (_ItemsByID.ContainsKey(id))
						return _ItemsByID[id];
					return null;
				}
			}

			protected override void ClearItems()
			{
				base.ClearItems();
				_ItemsByID.Clear();
			}
			protected override void InsertItem(int index, Context item)
			{
				base.InsertItem(index, item);
				_ItemsByID[item.ID] = item;
			}
			protected override void RemoveItem(int index)
			{
				_ItemsByID.Remove(this[index].ID);
				base.RemoveItem(index);
			}
		}

		public Guid ID { get; private set; } = Guid.Empty;
		public string Name { get; private set; } = String.Empty;

		// public MenuBar MenuBar { get; } = new MenuBar();
		public Command.CommandCollection Commands { get; } = new Command.CommandCollection();

		public Context(Guid id, string name)
		{
			ID = id;
			Name = name;
		}

		public override string ToString()
		{
			return String.Format("{0}     {1}", Name, ID);
		}

		private Dictionary<string, List<EventHandler>> _CommandEventHandlers = new Dictionary<string, List<EventHandler>>();
		/// <summary>
		/// Attachs an event handler for the command with the ID specified by <paramref name="commandID" />.
		/// </summary>
		/// <returns><c>true</c>, if command event handler was attached, <c>false</c> otherwise.</returns>
		/// <param name="commandID">The ID of the <see cref="Command" /> for which to attach an event handler.</param>
		/// <param name="handler">The event handler to invoke when the <see cref="Command" /> is executed.</param>
		public bool AttachCommandEventHandler(string commandID, EventHandler handler)
		{
			// handle command event handlers attached without a Command instance
			if (!_CommandEventHandlers.ContainsKey(commandID))
			{
				_CommandEventHandlers.Add(commandID, new List<EventHandler>());
			}
			if (!_CommandEventHandlers[commandID].Contains(handler))
			{
				_CommandEventHandlers[commandID].Add(handler);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Executes the command (i.e., calls all attached <see cref="EventHandler" />s for the <see cref="Command" />) with
		/// the given <paramref name="commandID" />, passing in the given parameters in an instance of <see cref="CommandEventArgs" />.
		/// </summary>
		/// <returns><c>true</c>, if command was executed, <c>false</c> otherwise.</returns>
		/// <param name="commandID">Command identifier.</param>
		/// <param name="namedParameters">Named parameters.</param>
		public bool ExecuteCommand(string commandID, KeyValuePair<string, object>[] namedParameters = null)
		{
			if (_CommandEventHandlers.ContainsKey(commandID))
			{
				for (int i = 0; i < _CommandEventHandlers[commandID].Count; i++)
				{
					_CommandEventHandlers[commandID][i](this, new CommandEventArgs(Commands[commandID], namedParameters));
				}
			}
			return false;
		}
	}
}
