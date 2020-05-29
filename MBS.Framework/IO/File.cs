//
//  File.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
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
namespace MBS.Framework.IO
{
	public class File
	{
		public static string Find(string filename, CaseSensitiveHandling caseSensitiveHandling = CaseSensitiveHandling.System)
		{
			string fn1 = System.IO.Path.GetFileName(filename);

			if ((Environment.OSVersion.Platform == PlatformID.Unix && caseSensitiveHandling == CaseSensitiveHandling.CaseInsensitive) ||
				((Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Win32Windows) && caseSensitiveHandling == CaseSensitiveHandling.CaseSensitive))
			{
				// do some extra work to return case-insensitive file on *nix
				string dirname = System.IO.Path.GetDirectoryName(filename);
				if (String.IsNullOrEmpty(dirname))
					return filename;

				string[] files = System.IO.Directory.GetFiles(dirname, "*", System.IO.SearchOption.TopDirectoryOnly);
				for (int i = 0; i < files.Length; i++)
				{
					string fn = System.IO.Path.GetFileName(files[i]);
					if ((caseSensitiveHandling == CaseSensitiveHandling.CaseInsensitive && fn.ToUpper() == fn1.ToUpper())
						|| (caseSensitiveHandling == CaseSensitiveHandling.CaseSensitive && fn == fn1))
						return files[i];
				}
				return null;
			}
			else
			{
				if (System.IO.File.Exists(filename))
					return filename;
				return null;
			}
		}
	}
}
