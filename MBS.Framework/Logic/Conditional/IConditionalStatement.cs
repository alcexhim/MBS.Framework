//
//  IConditionalStatement.cs - interface defining the minimum functionality required to implement a conditional statement
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

namespace MBS.Framework.Logic.Conditional
{
	/// <summary>
	/// Interface defining the minimum functionality required to implement a conditional statement (either a <see cref="Condition" /> itself or a
	/// <see cref="ConditionGroup" /> of multiple <see cref="Condition" />s.
	/// </summary>
	public interface IConditionalStatement
	{
		/// <summary>
		/// Evaluates the conditional statement based on the given criteria.
		/// </summary>
		/// <param name="propertyValues">The set of values against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		bool Test(params System.Collections.Generic.KeyValuePair<string, object>[] propertyValues);
		/// <summary>
		/// Evaluates the conditional statement based on the given criteria.
		/// </summary>
		/// <param name="propertyValues">The set of values against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		bool Test(System.Collections.Generic.Dictionary<string, object> propertyValues);
		/// <summary>
		/// Evaluates the conditional statement based on the given criterion.
		/// </summary>
		/// <param name="value">The value against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		bool Test(object value);
	}
}
