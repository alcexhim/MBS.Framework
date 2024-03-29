//
//  TextSetting.cs
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
	public class TextSetting : Setting
	{
		public int? MaximumLength { get; set; } = null;

		public TextSetting(string name, string title, string defaultValue = "", int? maxLength = null) : base(name, title, defaultValue)
		{
			MaximumLength = maxLength;
		}

		public override object Clone()
		{
			TextSetting clone = new TextSetting(Name, Title, (string)DefaultValue, MaximumLength);
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
