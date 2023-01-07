//
//  InstallationStatus.cs
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
namespace MBS.Framework
{
	public enum InstallationStatus
	{
		/// <summary>
		/// The application installation status cannot be determined.
		/// </summary>
		Unknown,
		/// <summary>
		/// Indicates that the application has been launched before.
		/// </summary>
		Installed,
		/// <summary>
		/// Indicates that the application is launching for the first time, or the user has cleared the shared application data.
		/// </summary>
		New,
		/// <summary>
		/// Indicates that the application has been upgraded since it was last run.
		/// </summary>
		Upgraded,
		/// <summary>
		/// On Microsoft Windows, indicates an application that has been advertised by the Windows Installer system but has not actually been installed on the user's computer.
		/// </summary>
		Advertised
	}
}
