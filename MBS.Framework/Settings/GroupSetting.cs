//
//  GroupSetting.cs
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
namespace MBS.Framework.Settings
{
	public class GroupSetting : Setting
	{
		public Setting.SettingCollection Options { get; } = new Setting.SettingCollection();
		public Setting.SettingCollection HeaderSettings { get; } = new Setting.SettingCollection();
		public int Count
		{
			get
			{
				int count = Options.Count;
				for (int i = 0; i < Options.Count; i++)
				{
					if (Options[i] is GroupSetting)
					{
						count += (Options[i] as GroupSetting).Count;
					}
				}
				return count;
			}
		}

		public GroupSetting(string name, string title, Setting[] options = null) : base(name, title)
		{
			if (options != null)
			{
				foreach (Setting option in options)
				{
					Options.Add(option);
				}
			}
		}

		public Setting FindSetting(string name)
		{
			foreach (Setting s in Options)
			{
				if (s is GroupSetting)
				{
					Setting r = (s as GroupSetting).FindSetting(name);
					if (r != null) return r;
				}
				else
				{
					if (s.Name == name)
						return s;
				}
			}
			return null;
		}
		public Setting FindSetting(Guid id)
		{
			foreach (Setting s in Options)
			{
				if (s is GroupSetting)
				{
					Setting r = (s as GroupSetting).FindSetting(id);
					if (r != null) return r;
				}
				else
				{
					if (s.ID == id)
						return s;
				}
			}
			return null;
		}

		public override object Clone()
		{
			GroupSetting clone = new GroupSetting(Name, Title);
			clone.Required = Required;
			clone.Prefix = Prefix;
			clone.Description = Description;
			clone.Enabled = Enabled;
			clone.Visible = Visible;
			clone.Suffix = Suffix;
			foreach (Setting setting in HeaderSettings)
			{
				clone.HeaderSettings.Add(setting.Clone() as Setting);
			}
			foreach (Setting setting in Options)
			{
				clone.Options.Add(setting.Clone() as Setting);
			}
			clone.SetValue(GetValue());
			return clone;
		}
	}
}
