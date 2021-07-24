//
//  ChoiceSetting.cs
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
	public class ChoiceSetting : Setting
	{
		public ChoiceSetting(string name, string title, object defaultValue = null, ChoiceSettingValue[] values = null, bool multiple = false) : base(name, title, null)
		{
			if (values == null)
			{
				values = new ChoiceSettingValue[0];
			}
			if (defaultValue != null)
			{
				if (defaultValue != null)
				{
					base.DefaultValue = defaultValue;
				}
				else
				{
					base.DefaultValue = null;
				}
			}

			foreach (ChoiceSettingValue value in values)
			{
				ValidValues.Add(value);
			}
			MultipleSelect = multiple;
		}

		public class ChoiceSettingValue
		{
			public class ChoiceSettingValueCollection
				: System.Collections.ObjectModel.Collection<ChoiceSettingValue>
			{
			}

			public string Name { get; set; } = String.Empty;
			public string Title { get; set; } = String.Empty;
			public object Value { get; set; } = null;

			public ChoiceSettingValue(object value)
			{
				if (value != null)
				{
					Name = value.ToString();
					Title = value.ToString();
				}
				Value = value;
			}
			public ChoiceSettingValue(string name, string title, object value)
			{
				Name = name;
				Title = title;
				Value = value;
			}
		}

		public ChoiceSettingValue.ChoiceSettingValueCollection ValidValues { get; } = new ChoiceSettingValue.ChoiceSettingValueCollection();
		public object SelectedValue { get; set; } = null;

		public bool RequireSelectionFromList { get; set; } = true;
		public bool MultipleSelect { get; set; } = false;
	}
}
