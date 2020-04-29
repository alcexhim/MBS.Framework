//
//  ConditionComparison.cs - indicates the type of comparison to use with a conditional statement
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2011-2020 Mike Becker's Software
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

namespace MBS.Framework.Logic.Conditional
{
	/// <summary>
	/// Indicates the type of comparison to use with a conditional statement.
	/// </summary>
	[Flags()]
	public enum ConditionComparison
	{
		/// <summary>
		/// Returns true if the two values are equal by value.
		/// </summary>
		Equal = 1,
		/// <summary>
		/// Returns true if the two values are equal by reference (or by value if they are value types).
		/// </summary>
		ReferenceEqual = 2,
		/// <summary>
		/// Returns true if the first value is greater than the second value.
		/// </summary>
		GreaterThan = 4,
		/// <summary>
		/// Returns true if the first value is less than the second value.
		/// </summary>
		LessThan = 8,
		/// <summary>
		/// Negates the conditional comparison.
		/// </summary>
		Not = 16
	}
}
