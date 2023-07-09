//
//  PropertyBag.cs
//
//  Author:
//       beckermj <>
//
//  Copyright (c) 2023 ${CopyrightHolder}
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
	public class PropertyBag
	{
		private Dictionary<Guid, object> _settings = new Dictionary<Guid, object>();
		private Dictionary<Guid, string> _names = new Dictionary<Guid, string>();
		public string GetName(Guid id)
		{
			if (_names.ContainsKey(id))
				return _names[id];
			return null;
		}
		public void SetName(Guid id, string name)
		{
			_names[id] = name;
		}

		private Dictionary<Guid, object> _PropertyValues = new Dictionary<Guid, object>();
		public bool Contains(Guid id)
		{
			return _PropertyValues.ContainsKey(id);
		}
		public bool SetValue<T>(Guid id, T value)
		{
			bool changed = false;
			lock (_PropertyValues)
			{
				object oldValue = null;
				if (!_PropertyValues.ContainsKey(id) || (!(
						(_PropertyValues[id] == null && (value as object) == null) ||
						(_PropertyValues[id] != null && _PropertyValues[id].Equals(value)))))
				{
					changed = true;
				}
				if (_PropertyValues.ContainsKey(id))
				{
					oldValue = _PropertyValues[id];
				}

				if (changed)
				{
					PropertyValueChangingEventArgs e = new PropertyValueChangingEventArgs(id, oldValue, value);
					OnPropertyValueChanging(e);
					if (e.Cancel)
					{
						return false;
					}
				}
				_PropertyValues[id] = value;
				if (changed)
				{
					OnPropertyValueChanged(new PropertyValueChangedEventArgs(id, oldValue, value));
				}
			}
			return changed;
		}

		public event EventHandler<PropertyValueChangingEventArgs> PropertyValueChanging;
		protected virtual void OnPropertyValueChanging(PropertyValueChangingEventArgs e)
		{
			PropertyValueChanging?.Invoke(this, e);
		}
		public event EventHandler<PropertyValueChangedEventArgs> PropertyValueChanged;
		protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
		{
			PropertyValueChanged?.Invoke(this, e);
		}

		public T GetValue<T>(Guid id, T defaultValue = default(T))
		{
			lock (_PropertyValues)
			{
				if (_PropertyValues.ContainsKey(id))
				{
					if (_PropertyValues[id] is T val)
					{
						return val;
					}
				}
				else
				{
					PropertyValueRequestedEventArgs<T> e = new PropertyValueRequestedEventArgs<T>(id, defaultValue);
					OnPropertyValueRequested(e);
					if (e.Handled)
					{
						if (e.Cache)
						{
							_PropertyValues[id] = e.Value;
						}
						return e.Value;
					}
				}
			}
			return defaultValue;
		}

		public event EventHandler<PropertyValueRequestedEventArgs> PropertyValueRequested;
		protected virtual void OnPropertyValueRequested<T>(PropertyValueRequestedEventArgs<T> e)
		{
			PropertyValueRequested?.Invoke(this, e);
		}

		public IEnumerable<KeyValuePair<Guid, object>> GetAll()
		{
			return _PropertyValues;
		}

		public bool Initialized { get; private set; } = false;
		protected virtual void OnInitialized(EventArgs e)
		{
		}
		public void Initialize()
		{
			if (Initialized)
				return;

			OnInitialized(EventArgs.Empty);
			Initialized = true;
		}
	}
}
