using System;
using System.Collections.Generic;

namespace MBS.Framework
{
	public static class StringExtensions
	{
		public static string Capitalize(this string value)
		{
			if (String.IsNullOrEmpty(value)) return value;
			if (value.Length == 1) return value.ToUpper();
			return value[0].ToString().ToUpper() + value.Substring(1);
		}
		public static string ReplaceVariables(this string value, IEnumerable<KeyValuePair<string, object>> dict)
		{
			string retval = value;
			foreach (KeyValuePair<string, object> kvp in dict)
			{
				retval = retval.Replace("$(" + kvp.Key + ")", kvp.Value == null ? String.Empty : kvp.Value.ToString());
			}
			return retval;
		}

		/// <summary>
		/// Parses the given <paramref name="value" /> with the specified <paramref name="type" />'s public static Parse(<see cref="String" />) method. If no such method exists,
		/// returns <paramref name="value" />.
		/// </summary>
		/// <returns>The <see cref="Object" /> parsed from the given <paramref name="value" />.</returns>
		/// <param name="value">The value to attempt to parse.</param>
		/// <param name="type">Type.</param>
		public static object Parse(this string value, Type type)
		{
			return Parse(value, type, value);
		}
		/// <summary>
		/// Parses the given <paramref name="value" /> with the specified <paramref name="type" />'s public static Parse(<see cref="String" />) method. If no such method exists,
		/// returns <paramref name="defaultValue" />.
		/// </summary>
		/// <returns>The parse.</returns>
		/// <param name="value">Value.</param>
		/// <param name="type">Type.</param>
		/// <param name="defaultValue">Default value.</param>
		public static object Parse(this string value, Type type, object defaultValue)
		{
			System.Reflection.MethodInfo miParse = type.GetMethod("Parse", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, System.Reflection.CallingConventions.Any, new Type[] { typeof(string) }, null);
			if (miParse != null)
			{
				// the given type implements a public static Parse(String) method, so use it
				return miParse.Invoke(null, new object[] { value });
			}
			return defaultValue;
		}
		public static T Parse<T>(this string value)
		{
			return (T)Parse(value, typeof(T));
		}

		public static T TryParse<T>(this string value, T defaultValue = default(T))
		{
			if (TryParse(value, typeof(T), out object output))
				return (T)output;
			return defaultValue;
		}


		/// <summary>
		/// Parses the given <paramref name="value" /> with the specified <paramref name="type" />'s public static Parse(<see cref="String" />) method. If no such method exists,
		/// returns <paramref name="value" />.
		/// </summary>
		/// <returns>The <see cref="Object" /> parsed from the given <paramref name="value" />.</returns>
		/// <param name="value">The value to attempt to parse.</param>
		/// <param name="type">Type.</param>
		public static bool TryParse(this string value, Type type, out object output)
		{
			return TryParse(value, type, value, out output);
		}

		/// <summary>
		/// Parses the given <paramref name="value" /> with the specified <paramref name="type" />'s public static Parse(<see cref="String" />) method. If no such method exists,
		/// returns <paramref name="defaultValue" />.
		/// </summary>
		/// <returns>The parse.</returns>
		/// <param name="value">Value.</param>
		/// <param name="type">Type.</param>
		/// <param name="defaultValue">Default value.</param>
		public static bool TryParse(this string value, Type type, object defaultValue, out object output)
		{
			System.Reflection.MethodInfo miParse = type.GetMethod("TryParse", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new Type[] { typeof(string), type.MakeByRefType() }, null);
			if (miParse != null)
			{
				// the given type implements a public static Parse(String) method, so use it
				object retval = null;
				object[] parms = new object[] { value, retval };
				bool ret = (bool)miParse.Invoke(null, parms);
				retval = parms[1];
				if (ret)
				{
					output = retval;
					return ret;
				}
			}
			output = null;
			return false;
		}
		public static bool TryParse<T>(this string value, out T output)
		{
			object retval = null;
			bool ret = TryParse(value, typeof(T), out retval);
			if (ret)
			{
				output = (T)retval;
				return ret;
			}
			output = default(T);
			return false;
		}
	}
}
