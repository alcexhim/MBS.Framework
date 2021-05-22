//
//  Convert.cs
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
using MBS.Framework.UserInterface;

namespace MBS.Framework
{
	public static class Conversion
	{
		public static TriState TriStateFromString(string value)
		{
			if (value != null)
			{
				switch (value.ToLower())
				{
					case "true": return TriState.True;
					case "false": return TriState.False;
				}
			}
			return TriState.Unspecified;
		}

		public static HorizontalAlignment HorizontalAlignmentFromString(string value)
		{
			if (value != null)
			{
				switch (value.ToLower())
				{
					case "center": return HorizontalAlignment.Center;
					case "justify": return HorizontalAlignment.Justify;
					case "left": return HorizontalAlignment.Left;
					case "right": return HorizontalAlignment.Right;
				}
			}
			return HorizontalAlignment.Default;
		}
		public static VerticalAlignment VerticalAlignmentFromString(string value)
		{
			if (value != null)
			{
				switch (value.ToLower())
				{
					case "baseline": return VerticalAlignment.Baseline;
					case "bottom": return VerticalAlignment.Bottom;
					case "middle": return VerticalAlignment.Middle;
					case "top": return VerticalAlignment.Top;
				}
			}
			return VerticalAlignment.Default;
		}
	}
}
