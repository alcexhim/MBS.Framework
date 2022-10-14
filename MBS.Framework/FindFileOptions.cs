//
//  FindFileOptions.cs
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
namespace MBS.Framework
{
	/// <summary>
	/// Controls the behavior of relative file resolution.
	/// </summary>
	public enum FindFileOptions
	{
		/// <summary>
		/// Returns all matching fully-qualified file paths across all global,
		/// application, and user directories.
		/// </summary>
		All = 0,
		/// <summary>
		/// Returns only file paths that are writable by the user (i.e., in the
		/// user's local or roaming data directory).
		/// </summary>
		UserWritable = 1
	}
}
