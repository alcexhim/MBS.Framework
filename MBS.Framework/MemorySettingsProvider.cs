//
//  MemorySettingsProvider.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2022 Mike Becker's Software
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
using MBS.Framework.Settings;

namespace MBS.Framework
{
	public class MemorySettingsProvider : SettingsProvider
	{
		private Dictionary<Guid, object> _values = new Dictionary<Guid, object>();
		protected override void LoadSettingsInternal()
		{
			base.LoadSettingsInternal();
			foreach (SettingsGroup sg in SettingsGroups)
			{
				foreach (Setting s in sg.Settings)
				{
					LoadSetting(s);
				}
			}
		}
		protected override void SaveSettingsInternal()
		{
			base.SaveSettingsInternal();
			foreach (SettingsGroup sg in SettingsGroups)
			{
				foreach (Setting s in sg.Settings)
				{
					SaveSetting(s);
				}
			}
		}

		private void LoadSetting(Setting s)
		{
			if (s is GroupSetting)
			{
				foreach (Setting s2 in ((GroupSetting)s).Options)
				{
					LoadSetting(s2);
				}
			}
			else
			{
				s.SetValue(_values[s.ID]);
			}
		}
		private void SaveSetting(Setting s)
		{
			if (s is GroupSetting)
			{
				foreach (Setting s2 in ((GroupSetting)s).Options)
				{
					SaveSetting(s2);
				}
			}
			else
			{
				_values[s.ID] = s.GetValue();
			}
		}
	}
}
