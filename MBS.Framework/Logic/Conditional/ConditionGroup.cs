//
//  ConditionGroup.cs - a group of IConditionalStatements joined by a ConditionCombination
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

namespace MBS.Framework.Logic.Conditional
{
	/// <summary>
	/// A group of <see cref="IConditionalStatement" />s joined by a <see cref="ConditionCombination" />.
	/// </summary>
	public class ConditionGroup : IConditionalStatement
	{
		/// <summary>
		/// Creates a new <see cref="ConditionGroup" /> with no conditional statements specified and a
		/// default <see cref="ConditionCombination" /> of <see cref="ConditionCombination.And" />.
		/// </summary>
		public ConditionGroup()
		{
			// I know it's initialized to this but I'm doing it here for clarity's sake (and because
			// it's documented here... if you change it, make sure to update the documentation! don't
			// rely on the field initializer)
			mvarCombination = ConditionCombination.And;
		}
		/// <summary>
		/// Creates a new <see cref="ConditionGroup" /> with the specified
		/// <see cref="ConditionCombination" /> and <see cref="IConditionalStatement" />s.
		/// </summary>
		/// <param name="combination">The <see cref="ConditionCombination" /> used to join <see cref="IConditionalStatement" />s when testing this <see cref="ConditionGroup" />.</param>
		/// <param name="statements">The <see cref="Condition" />s and <see cref="ConditionGroup" />s that are part of this <see cref="ConditionGroup" />.</param>
		public ConditionGroup(ConditionCombination combination, params IConditionalStatement[] statements)
		{
			mvarCombination = combination;
			for (int i = 0; i < statements.Length; i++)
			{
				mvarConditions.Add(statements[i]);
			}
		}

		private ConditionalStatementCollection mvarConditions = new ConditionalStatementCollection();
		/// <summary>
		/// Gets all <see cref="IConditionalStatement" />s in this <see cref="ConditionGroup" />.
		/// </summary>
		public ConditionalStatementCollection Conditions
		{
			get { return mvarConditions; }
		}

		private ConditionCombination mvarCombination = ConditionCombination.And;
		/// <summary>
		/// The type of combination used to join the <see cref="Condition" />s in this
		/// <see cref="ConditionGroup" />.
		/// </summary>
		public ConditionCombination Combination
		{
			get { return mvarCombination; }
			set { mvarCombination = value; }
		}

		/// <summary>
		/// Evaluates the conditional statement based on the given criteria.
		/// </summary>
		/// <param name="propertyValues">The set of values against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		public bool Test(params System.Collections.Generic.KeyValuePair<string, object>[] propertyValues)
		{
			bool retval = false;
			if (mvarCombination == ConditionCombination.And)
			{
				retval = true;
			}
			for (int i = 0; i < mvarConditions.Count; i++)
			{
				switch (mvarCombination)
				{
					case ConditionCombination.And:
					{
						retval &= mvarConditions[i].Test(propertyValues);
						break;
					}
					case ConditionCombination.Or:
					{
						retval |= mvarConditions[i].Test(propertyValues);
						break;
					}
					case ConditionCombination.Xor:
					{
						retval ^= mvarConditions[i].Test(propertyValues);
						break;
					}
				}
			}
			return retval;
		}
		/// <summary>
		/// Evaluates the conditional statement based on the given criteria.
		/// </summary>
		/// <param name="propertyValues">The set of values against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		public bool Test(System.Collections.Generic.Dictionary<string, object> propertyValues)
		{
			bool retval = false;
			if (mvarCombination == ConditionCombination.And)
			{
				retval = true;
			}
			for (int i = 0; i < mvarConditions.Count; i++)
			{
				switch (mvarCombination)
				{
					case ConditionCombination.And:
					{
						retval &= mvarConditions[i].Test(propertyValues);
						break;
					}
					case ConditionCombination.Or:
					{
						retval |= mvarConditions[i].Test(propertyValues);
						break;
					}
					case ConditionCombination.Xor:
					{
						retval ^= mvarConditions[i].Test(propertyValues);
						break;
					}
				}
			}
			return retval;
		}

		/// <summary>
		/// Evaluates the conditional statement based on the given criterion.
		/// </summary>
		/// <param name="value">The value against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		public bool Test(object value)
		{
			bool retval = true;

			for (int i = 0; i < mvarConditions.Count; i++)
			{
				switch (mvarCombination)
				{
					case ConditionCombination.And:
					{
						retval &= mvarConditions[i].Test(value);
						break;
					}
					case ConditionCombination.Or:
					{
						retval |= mvarConditions[i].Test(value);
						break;
					}
					case ConditionCombination.Xor:
					{
						retval ^= mvarConditions[i].Test(value);
						break;
					}
				}
			}
			return retval;
		}
	}
}
