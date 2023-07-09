//
//  PluginPropertyBag.cs
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
namespace MBS.Framework
{
	public class PluginPropertyBag : PropertyBag
	{
		private Plugin Plugin { get; }

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			string[] paths = Application.Instance.EnumerateDataPaths();
			foreach (string datapath in paths)
			{
				string path = String.Format("{0}/plugins/{1}/config.xml", datapath, this.Plugin.ID.ToString("b"));
				if (System.IO.File.Exists(path))
				{
					Console.WriteLine("found config in {0}", path);
				}
			}
		}

		internal PluginPropertyBag(Plugin plugin)
		{
			Plugin = plugin;
		}

		protected override void OnPropertyValueRequested<T>(PropertyValueRequestedEventArgs<T> e)
		{
			base.OnPropertyValueRequested(e);
		}
	}
}
