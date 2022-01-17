//
//  EventFilter.cs - provides a way to hook application-wide events
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
namespace MBS.Framework
{
	public class EventFilter
	{
		public class EventFilterCollection
			: System.Collections.ObjectModel.Collection<EventFilter>
		{

		}

		public EventFilterType EventType { get; private set; } = EventFilterType.None;

		protected EventFilter(EventFilterType eventType)
		{
			EventType = eventType;
		}

		private EventFilterDelegate<EventArgs> _processAction = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="EventFilter" /> class
		/// with the given process function.
		/// </summary>
		/// <param name="processAction">
		/// The function to call when the <see cref="Process" /> function is
		/// called.
		/// </param>
		public EventFilter(EventFilterDelegate<EventArgs> processAction, EventFilterType eventType = EventFilterType.All)
		{
			_processAction = processAction;
			EventType = eventType;
		}

		/// <summary>
		/// Calls the <see cref="EventFilter" />'s internal process function and
		/// returns its value.
		/// </summary>
		/// <returns>
		/// A value indicating whether the event was successfully processed.
		/// </returns>
		/// <param name="e">
		/// The <see cref="EventArgs" /> to pass to the underlying process function.
		/// </param>
		public bool Process(ref EventArgs e, EventFilterType type)
		{
			if (_processAction != null)
				return _processAction(type, ref e);

			return true;
		}
	}
	/// <summary>
	/// Provides a way to hook application-wide events.
	/// </summary>
	public class EventFilter<T> : EventFilter where T : EventArgs
	{
		private EventFilterDelegate<T> _processAction = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="EventFilter" /> class
		/// with the given process function.
		/// </summary>
		/// <param name="processAction">
		/// The function to call when the <see cref="Process" /> function is
		/// called.
		/// </param>
		public EventFilter(EventFilterDelegate<T> processAction, EventFilterType eventType = EventFilterType.All) : base(eventType)
		{
			_processAction = processAction;
		}

		/// <summary>
		/// Calls the <see cref="EventFilter" />'s internal process function and
		/// returns its value.
		/// </summary>
		/// <returns>
		/// A value indicating whether the event was successfully processed.
		/// </returns>
		/// <param name="e">
		/// The <see cref="EventArgs" /> to pass to the underlying process function.
		/// </param>
		public bool Process(ref T e, EventFilterType type)
		{
			if (_processAction != null)
				return _processAction(type, ref e);

			return true;
		}
	}
}
