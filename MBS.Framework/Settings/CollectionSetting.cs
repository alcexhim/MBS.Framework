//
//  CollectionSetting.cs
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
	public class CollectionSetting : Setting
	{
		public Setting.SettingCollection Settings { get; } = new Setting.SettingCollection();
		public SettingsGroup.SettingsGroupCollection Items { get; } = new SettingsGroup.SettingsGroupCollection();

		public string SingularItemTitle { get; set; } = null;

		public CollectionSetting(string name, string title, SettingsGroup group = null, string singularItemTitle = null) : base(name, title, null)
		{
			if (group != null)
			{
				for (int i = 0; i < group.Settings.Count; i++)
				{
					Settings.Add(group.Settings[i]);
				}
			}
			SingularItemTitle = singularItemTitle;
		}

		public override object Clone()
		{
			CollectionSetting clone = new CollectionSetting(Name, Title);
			clone.Required = Required;
			clone.Prefix = Prefix;
			clone.Description = Description;
			clone.Enabled = Enabled;
			clone.Visible = Visible;
			clone.Suffix = Suffix;
			foreach (Setting setting in Settings)
			{
				clone.Settings.Add(setting.Clone() as Setting);
			}
			return clone;
		}
	}
}
