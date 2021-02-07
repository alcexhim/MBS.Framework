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
	public abstract class Setting
	{
		public Setting(string name, string title, object defaultValue = null)
		{
			Name = name;
			Title = title;
			DefaultValue = defaultValue;
			mvarValue = defaultValue;
		}

		public Guid ID { get; set; } = Guid.Empty;

		[Obsolete("This is no longer implemented. Please set a proper ID in order for your setting to be saved.")]
		public string Name { get; set; } = String.Empty;

		public string Title { get; set; } = String.Empty;
		public string Description { get; set; } = String.Empty;

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
		}

		protected Setting()
		{
		}

		public object DefaultValue { get; set; } = null;
		public SettingsValue.SettingsValueCollection ScopedValues { get; } = new SettingsValue.SettingsValueCollection();

		private object mvarValue = null;

		public virtual object GetValue(Guid? scopeId = null)
		{
			if (scopeId != null)
			{
				if (ScopedValues.Contains(scopeId.Value))
				{
					return ScopedValues[scopeId.Value].Value;
				}
			}
			return mvarValue;
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
			}
		}
		public T GetValue<T>(T defaultValue = default(T), Guid? scopeId = null)
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
		}
	}
}

