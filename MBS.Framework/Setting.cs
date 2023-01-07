//
//  Option.cs
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
	public abstract class Setting : ICloneable
	{
		public Setting(string name, string title, object defaultValue = null, bool enabled = true, bool visible = true)
		{
			Name = name;
			Title = title;
			DefaultValue = defaultValue;
			mvarValue = defaultValue;
			Enabled = enabled;
			Visible = visible;
		}

		public Guid ID { get; set; } = Guid.Empty;

		[Obsolete("This is no longer implemented. Please set a proper ID in order for your setting to be saved.")]
		public string Name { get; set; } = String.Empty;

		public string Title { get; set; } = String.Empty;
		public string Description { get; set; } = String.Empty;

		public bool Enabled { get; set; } = true;
		public bool Visible { get; set; } = true;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Setting"/> is required to have a value.
		/// </summary>
		/// <value><c>true</c> if this <see cref="Setting" /> is required to have a value; otherwise, <c>false</c>.</value>
		public bool Required { get; set; } = false;

		public string Prefix { get; set; } = null;
		public string Suffix { get; set; } = null;

		public class SettingCollection
			: System.Collections.ObjectModel.Collection<Setting>
		{

			public Setting this[string name]
			{
				get
				{
					foreach (Setting item in this)
					{
						if (item.Title.Replace("_", String.Empty).Replace(' ', '_').Equals(name))
						{
							return item;
						}
					}
					return null;
				}
			}

			private Dictionary<Guid, Setting> _itemsByID = new Dictionary<Guid, Setting>();
			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByID.Clear();
			}
			protected override void InsertItem(int index, Setting item)
			{
				base.InsertItem(index, item);
				_itemsByID[item.ID] = item;
			}
			protected override void RemoveItem(int index)
			{
				_itemsByID.Remove(this[index].ID);
				base.RemoveItem(index);
			}
			public bool Contains(Guid id)
			{
				return _itemsByID.ContainsKey(id);
			}
			public Setting this[Guid id]
			{
				get
				{
					if (_itemsByID.ContainsKey(id))
						return _itemsByID[id];
					return null;
				}
			}
		}

		protected Setting()
		{
		}

		public object DefaultValue { get; set; } = null;
		public SettingsValue.SettingsValueCollection ScopedValues { get; } = new SettingsValue.SettingsValueCollection();

		private object mvarValue = null;
		private bool _valueSet = false;

		public virtual object GetValue(Guid? scopeId = null)
		{
			if (scopeId != null)
			{
				if (ScopedValues.Contains(scopeId.Value))
				{
					return ScopedValues[scopeId.Value].Value;
				}
			}
			if (_valueSet)
				return mvarValue;
			return DefaultValue;
		}
		public virtual void SetValue(object value, Guid? scopeId = null)
		{
			if (scopeId != null)
			{
				if (ScopedValues.Contains(scopeId.Value))
				{
					ScopedValues[scopeId.Value].Value = value;
				}
				else
				{
					ScopedValues.Add(scopeId.Value, value);
				}
			}
			else
			{
				mvarValue = value;
				_valueSet = true;
			}
		}
		public T GetValue<T>(Guid? scopeId = null)
		{
			return GetValue<T>(DefaultValue is T ? (T)DefaultValue : default(T), scopeId);
		}
		public T GetValue<T>(T defaultValue, Guid? scopeId = null)
		{
			try
			{
				object val = GetValue(scopeId);
				if (val is T)
					return (T)val;
				if (val is string)
				{
					return (val as string).Parse<T>();
				}
				return defaultValue;
			}
			catch
			{
				return defaultValue;
			}
		}

		public void SetValue<T>(T value)
		{
			mvarValue = value;
			_valueSet = true;
		}

		public abstract object Clone();
	}
}
