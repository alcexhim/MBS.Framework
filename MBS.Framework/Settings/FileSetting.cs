//
//  FileSetting.cs
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
	public enum FileSettingMode
	{
		Open,
		Save,
		SelectFolder,
		CreateFolder
	}
	public class FileSetting : TextSetting
	{
		public bool RequireExistingFile { get; set; } = true;
		public FileSettingMode Mode { get; set; } = FileSettingMode.Open;
		/// <summary>
		/// A semicolon-separated list of glob-style (*.xxx) filters used in the file selection dialog for this <see cref="FileSetting" />.
		/// </summary>
		/// <value>The file name filter.</value>
		public string FileNameFilter { get; set; } = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSetting" /> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="title">Title.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="requireExistingFile">If set to <c>true</c> require existing file.</param>
		/// <param name="fileNameFilter">A semicolon-separated list of glob-style (*.xxx) filters used in the file selection dialog for this <see cref="FileSetting" />.</param>
		public FileSetting(string name, string title, string defaultValue = "", bool requireExistingFile = true, string fileNameFilter = null) : base(name, title, defaultValue)
		{
			RequireExistingFile = requireExistingFile;
			FileNameFilter = fileNameFilter;
		}
	}
}
