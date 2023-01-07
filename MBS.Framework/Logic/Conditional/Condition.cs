//
//  Condition.cs - represents a conditional statement which defines a comparison of a named property with a constant value
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
	/// Represents a conditional statement which defines a comparison of a named property with a constant value.
	/// </summary>
	public class Condition : IConditionalStatement
	{
		private string mvarPropertyName = String.Empty;
		/// <summary>
		/// The name of the property against which to test when the <see cref="Test" /> method is called
		/// passing in a property reference.
		/// </summary>
		public string PropertyName
		{
			get { return mvarPropertyName; }
			set { mvarPropertyName = value; }
		}

		private ConditionComparison mvarComparison = ConditionComparison.Equal;
		/// <summary>
		/// The type of comparison to use when testing this <see cref="Condition" />.
		/// </summary>
		public ConditionComparison Comparison
		{
			get { return mvarComparison; }
			set { mvarComparison = value; }
		}

		private object mvarValue = null;
		/// <summary>
		/// The value against which to test when the <see cref="Test" /> method is called.
		/// </summary>
		public object Value
		{
			get { return mvarValue; }
			set { mvarValue = value; }
		}

		/// <summary>
		/// Creates a <see cref="Condition" /> with the specified property name, comparison, and value.
		/// </summary>
		/// <param name="propertyName">The name of the property against which to test when the <see cref="Test" /> method is called passing in a property reference.</param>
		/// <param name="comparison">The type of comparison to use.</param>
		/// <param name="value">The value against which to test when the <see cref="Test" /> method is called.</param>
		public Condition(string propertyName, ConditionComparison comparison, object value)
		{
			mvarPropertyName = propertyName;
			mvarComparison = comparison;
			mvarValue = value;
		}

		/// <summary>
		/// Evaluates the conditional statement based on the given criteria.
		/// </summary>
		/// <param name="propertyValues">The set of values against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		public bool Test(params System.Collections.Generic.KeyValuePair<string, object>[] propertyValues)
		{
			bool retval = true;
			foreach (System.Collections.Generic.KeyValuePair<string, object> propertyValue in propertyValues)
			{
				if (propertyValue.Key == mvarPropertyName)
				{
					retval &= Test(propertyValue.Value);
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
			bool retval = true;
			foreach (System.Collections.Generic.KeyValuePair<string, object> propertyValue in propertyValues)
			{
				if (propertyValue.Key == mvarPropertyName)
				{
					retval &= Test(propertyValue.Value);
				}
			}
			return retval;
		}
		/// <summary>
		/// Evaluates the conditional statement based on the given criterion.
		/// </summary>
		/// <param name="value">The value against which to evaluate the conditional statement.</param>
		/// <returns>True if the conditions are satisfied; false otherwise.</returns>
		public bool Test(object propertyValue)
		{
			// would you like meatballs with your spaghetti code?
			bool returnValue = false;

			if ((mvarComparison & ConditionComparison.Equal) == ConditionComparison.Equal)
			{
				if (propertyValue == null)
				{
					// our comparison object is null, so we can't .Equals it
					// just do regular == with the constant null in that case
					returnValue |= (mvarValue == null);
				}
				else
				{
					returnValue |= (propertyValue.Equals(mvarValue));
				}
			}
			if ((mvarComparison & ConditionComparison.ReferenceEqual) == ConditionComparison.ReferenceEqual)
			{
				if (propertyValue == null)
				{
					// our comparison object is null, so we can't .Equals it
					// just do regular == with the constant null in that case
					returnValue |= (mvarValue == null);
				}
				else
				{
					returnValue |= (propertyValue == mvarValue);
				}
			}
			if (((mvarComparison & ConditionComparison.GreaterThan) == ConditionComparison.GreaterThan) && (propertyValue is IComparable))
			{
				if (propertyValue == null)
				{
					// can ANYTHING ever be greater than or less than null?
					returnValue |= false;
				}
				else
				{
					// we need to directly invoke IComparable.CompareTo here since we can't (usually)
					// do > or < on objects... not sure what to do if the object doesn't implement
					// IComparable though
					returnValue |= ((propertyValue as IComparable).CompareTo(mvarValue) > 0);
				}
			}
			if (((mvarComparison & ConditionComparison.LessThan) == ConditionComparison.LessThan) && (propertyValue is IComparable))
			{
				if (propertyValue == null)
				{
					// can ANYTHING ever be greater than or less than null?
					returnValue |= false;
				}
				else
				{
					// we need to directly invoke IComparable.CompareTo here since we can't (usually)
					// do > or < on objects... not sure what to do if the object doesn't implement
					// IComparable though
					returnValue |= ((propertyValue as IComparable).CompareTo(mvarValue) < 0);
				}
			}
			if ((mvarComparison & ConditionComparison.Not) == ConditionComparison.Not)
			{
				// we have a Not in there, so negate our return value
				returnValue = !returnValue;
			}

			// did you have as much fun reading this as I did writing it?
			bool from_hell = returnValue;
			return from_hell;
		}
	}
}
