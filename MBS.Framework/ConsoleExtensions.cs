//
//  ConsoleExtensions.cs
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
	public static class ConsoleExtensions
	{
		public static void LogMSBuildMessage(MessageSeverity severity, string message, string code = null, string filename = null, int? line = null, int? column = null, string subcategory = null)
		{
			string severityString = "error";
			switch (severity)
			{
				case MessageSeverity.Warning:
				{
					severityString = "warning";
					break;
				}
				case MessageSeverity.Error:
				{
					severityString = "error";
					break;
				}
				case MessageSeverity.Message:
				{
					severityString = "information";
					break;
				}
			}
			if (code != null)
			{
				if (filename != null)
				{
					if (line != null)
					{
						if (column == null)
						{
							column = 1;
						}
						Console.Error.WriteLine(String.Format("{0}({1},{2}): {6} {3} {4}: {5}", filename, line, column, severityString, code, message, subcategory));
					}
					else
					{
						Console.Error.WriteLine(String.Format("{0}: {4} {1} {2}: {3}", filename, severityString, code, message, subcategory));
					}
				}
				else
				{
					Console.Error.WriteLine(String.Format("{0} {3} {1}: {2}", severityString, code, message, subcategory));
				}
			}
			else
			{
				Console.Error.WriteLine(String.Format("{2} {0}: {1}", severityString, message, subcategory));
			}
		}


	}
}
