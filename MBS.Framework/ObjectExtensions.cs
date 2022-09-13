//
//  ObjectExtensions.cs
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
	public static class ObjectExtensions
	{
		public static int SizeOf(this object obj)
		{
			if (obj == null) return 0;
			if (obj is byte || obj is sbyte || obj is bool)
			{
				return 1;
			}
			else if (obj is ushort || obj is short)
			{
				return 2;
			}
			else if (obj is uint || obj is int || obj is float)
			{
				return 4;
			}
			else if (obj is ulong || obj is long || obj is double)
			{
				return 8;
			}
			else if (obj is decimal)
			{
				return 8;
			}
			else if (obj is Guid)
			{
				return 16;
			}
			else if (obj is string)
			{
				return ((string)obj).Length;
			}
			return System.Runtime.InteropServices.Marshal.SizeOf(obj);
		}
	}
}
