//
//  BooleanSetting.cs
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
	public class BooleanSetting : Setting
	{
		public BooleanSetting(string name, string title, bool defaultValue = false, bool enabled = true, bool visible = true) : base(name, title, defaultValue, enabled, visible)
		{
		}

		public override void SetValue(object value, Guid? scopeId = null)
		{
			bool val = false;
			if (value != null)
			{
				val = (value.ToString().ToLower().Equals("true"));
			}
			base.SetValue(val);
		}

		public override object Clone()
		{
			BooleanSetting clone = new BooleanSetting(Name, Title, (bool)DefaultValue, Enabled, Visible);
			clone.Required = Required;
			clone.Prefix = Prefix;
			clone.Description = Description;
			clone.Enabled = Enabled;
			clone.Visible = Visible;
			clone.Suffix = Suffix;
			clone.SetValue(GetValue());
			return clone;
		}
	}
}
