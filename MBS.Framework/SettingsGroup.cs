﻿//
//  OptionPanel.cs
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
	public class SettingsGroup : IComparable<SettingsGroup>
	{
		public class SettingsGroupCollection
			: System.Collections.ObjectModel.Collection<SettingsGroup>
		{
			public SettingsGroup Add(string path, Setting[] options)
			{
				string[] paths = new string[0];
				if (!String.IsNullOrEmpty (path)) {
					paths = path.Split (new char[] { ':' });
				}
				return Add (paths, options);
			}
			public SettingsGroup Add(string[] path, Setting[] options)
			{
				SettingsGroup grp = new SettingsGroup();
				grp.Path = path;
				if (options != null) {
					foreach (Setting option in options) {
						grp.Settings.Add (option);
					}
				}
				Add (grp);
				return grp;
			}

			private Dictionary<Guid, SettingsGroup> _itemsByID = new Dictionary<Guid, SettingsGroup>();
			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByID.Clear();
			}
			protected override void InsertItem(int index, SettingsGroup item)
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
			public SettingsGroup this[Guid id]
			{
				get
				{
					if (_itemsByID.ContainsKey(id))
						return _itemsByID[id];
					return null;
				}
			}
		}

		public SettingsGroup()
		{
		}
		public SettingsGroup(string path, Setting[] options = null)
		{
			string[] paths = new string[0];
			if (!String.IsNullOrEmpty (path)) {
				paths = path.Split (new char[] { ':' });
			}
			Path = paths;
			if (options != null)
			{
				foreach (Setting option in options)
				{
					Settings.Add(option);
				}
			}
		}
		public SettingsGroup(string[] paths, Setting[] options = null)
		{
			Path = paths;
			if (options != null)
			{
				foreach (Setting option in options)
				{
					Settings.Add(option);
				}
			}
		}

		public int CompareTo(SettingsGroup other)
		{
			int xprior = this.Priority;
			int yprior = other.Priority;
			if (xprior == -1 && yprior == -1)
			{
				string xpath = String.Join(":", this.GetPath());
				string ypath = String.Join(":", other.GetPath());
				return xpath.CompareTo(ypath);
			}
			else
			{
				return yprior.CompareTo(xprior);
			}
		}

		public Guid ID { get; set; } = Guid.Empty;

		public string[] GetPath()
		{
			if (Path == null) return new string[0];
			return Path;
		}

		public string[] Path { get; set; } = null;
		public string Title
		{
			get
			{
				if (Path.Length > 0) return Path[Path.Length - 1];
				return null;
			}
		}
		public Setting.SettingCollection Settings { get; } = new Setting.SettingCollection();
		public int Priority { get; set; } = -1;

		public override string ToString ()
		{
			return String.Join (":", Path);
		}
	}
}

