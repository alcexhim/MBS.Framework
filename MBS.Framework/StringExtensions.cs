using System;
using System.Collections.Generic;
using System.Text;

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

		public static bool IsAllUpperCase(this string str)
		{
			return (!(new System.Text.RegularExpressions.Regex("[a-z]")).IsMatch(str));
		}
		public static string UrlEncode(this string value)
		{
			return value;
		}
		public static string UrlDecode(this string input)
		{
			StringBuilder output = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '%')
				{
					char c = input[i + 1];
					string arg_45_0 = c.ToString();
					c = input[i + 2];
					string numeric = arg_45_0 + c.ToString();
					int hexcode = int.Parse(numeric, System.Globalization.NumberStyles.HexNumber);
					i += 2;
					output.Append((char)hexcode);
				}
				else
				{
					output.Append(input[i]);
				}
			}
			return output.ToString();
		}
		public static bool Match(this string input, params string[] filters)
		{
			foreach (string filter in filters)
			{
				if (input.Match(filter)) return true;
			}
			return false;
		}
		public static bool Match(this string input, string filter)
		{
			if (filter == null)
				return false;

			string wildcardToRegex = "^" + System.Text.RegularExpressions.Regex.Escape(filter).Replace("\\*", ".*").Replace("\\?", ".") + "$";
			return new System.Text.RegularExpressions.Regex(wildcardToRegex).IsMatch(input);
		}

		/// <summary>
		/// Removes all characters past and including the first occurrence of a
		/// NUL (0x00) byte from the given string.
		/// </summary>
		/// <returns>
		/// All characters before the first occurrence of a NUL (0x00) byte in
		/// <paramref name="value" />.
		/// </returns>
		/// <param name="value">The string to trim.</param>
		public static string TrimNull(this string value)
		{
			int i = value.IndexOf('\0');
			if (i > -1) return value.Substring(0, i);
			return value;
		}

		public static string Format(this string input, Dictionary<string, string> formatWhat)
		{
			return Format(input, formatWhat, "$(", ")");
		}
		public static string Format(this string input, Dictionary<string, string> formatWhat, string formatBegin, string formatEnd)
		{
			string val = input;
			foreach (KeyValuePair<string, string> kvp in formatWhat)
			{
				val = val.Replace(formatBegin + kvp.Key + formatEnd, kvp.Value);
			}
			return val;
		}
		/// <summary>
		/// Inserts the specified value "count" times, with "spacing" characters between.
		/// </summary>
		/// <param name="count">The number of times to insert value.</param>
		/// <param name="spacing">The amount of characters to leave between insertions.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns></returns>
		public static string Insert(this string input, int count, int spacing, string value)
		{
			int j = 0;
			string retval = String.Empty;
			for (int i = 0; i < count; i++)
			{
				retval += input.Substring(j, spacing) + value;
				j += spacing;
			}
			retval += input.Substring(j);
			return retval;
		}


		public static T[] Split<T>(this string value, params char[] separator)
		{
			return value.Split<T>(separator, -1, StringSplitOptions.None);
		}
		public static T[] Split<T>(this string value, char[] separator, int count)
		{
			return value.Split<T>(separator, count, StringSplitOptions.None);
		}
		public static T[] Split<T>(this string value, char[] separator, int count, StringSplitOptions options)
		{
			string[] separators = new string[separator.Length];
			for (int i = 0; i < separator.Length; i++)
			{
				separators[i] = separator[i].ToString();
			}
			return value.Split<T>(separators, count, options);
		}
		public static T[] Split<T>(this string value, params string[] separator)
		{
			return value.Split<T>(separator, -1, StringSplitOptions.None);
		}
		public static T[] Split<T>(this string value, string[] separator, int count)
		{
			return value.Split<T>(separator, count, StringSplitOptions.None);
		}
		public static T[] Split<T>(this string value, string[] separator, int count, StringSplitOptions options)
		{
			string[] splitt = null;
			if (count < 0)
			{
				splitt = value.Split(separator, options);
			}
			else
			{
				splitt = value.Split(separator, count, options);
			}
			T[] values = new T[splitt.Length];
			for (int i = 0; i < splitt.Length; i++)
			{
				if (!string.IsNullOrEmpty(splitt[i]))
				{
					values[i] = (T)Convert.ChangeType(splitt[i], typeof(T));
				}
			}
			return values;
		}
		public static string[] Split(this string value, string separator)
		{
			return value.Split(new string[] { separator });
		}
		public static string[] Split(this string value, string[] separator)
		{
			return value.Split(separator, StringSplitOptions.None);
		}
		public static string[] Split(this string value, string[] separator, string ignore)
		{
			return value.Split(separator, ignore, ignore);
		}
		public static string[] Split(this string value, string[] separator, string ignoreBegin, string ignoreEnd)
		{
			return value.Split(separator, ignoreBegin, ignoreEnd, StringSplitOptions.None, -1);
		}
		public static string[] Split(this string value, string[] separator, StringSplitOptions options, int count, string ignore)
		{
			return value.Split(separator, ignore, ignore, options, count);
		}
		public static string[] Split(this string value, char[] separator, string ignore)
		{
			return value.Split(separator, ignore, ignore);
		}
		public static string[] Split(this string value, char[] separator, string ignoreBegin, string ignoreEnd)
		{
			return value.Split(separator, ignoreBegin, ignoreEnd, StringSplitOptions.None, -1);
		}
		public static string[] Split(this string value, char[] separator, string ignore, StringSplitOptions options, int count)
		{
			return value.Split(separator, ignore, ignore, options, count);
		}
		public static string[] Split(this string value, char[] separator, string ignoreBegin, string ignoreEnd, StringSplitOptions options, int count)
		{
			List<string> entries = new List<string>();
			for (int i = 0; i < separator.Length; i++)
			{
				char sep = separator[i];
				entries.Add(sep.ToString());
			}
			return value.Split(entries.ToArray(), ignoreBegin, ignoreEnd, options, count);
		}
		public static string[] Split(this string value, string[] separator, string ignoreBegin, string ignoreEnd, StringSplitOptions options, int count)
		{
			return value.Split(separator, ignoreBegin, ignoreEnd, options, count, true);
		}
		public static string[] Split(this string value, string[] separator, string ignoreBegin, string ignoreEnd, StringSplitOptions options, int count, bool discardIgnoreString)
		{
			List<string> entries = new List<string>();
			bool ignoring = false;
			bool continueOutside = false;
			string next = string.Empty;
			int i = 0;
			while (i < value.Length)
			{
				if (i + ignoreBegin.Length > value.Length)
				{
					goto IL_70;
				}
				if (ignoring || !(value.Substring(i, ignoreBegin.Length) == ignoreBegin))
				{
					goto IL_70;
				}
				ignoring = true;
				if (!discardIgnoreString)
				{
					next += ignoreBegin;
				}
			IL_16F:
				i++;
				continue;
			IL_70:
				if (i + ignoreEnd.Length <= value.Length)
				{
					if (ignoring && value.Substring(i, ignoreEnd.Length) == ignoreEnd)
					{
						ignoring = false;
						if (!discardIgnoreString)
						{
							next += ignoreEnd;
						}
						goto IL_16F;
					}
				}
				if (!ignoring)
				{
					int j = 0;
					while (j < separator.Length)
					{
						if (i + separator[j].Length <= value.Length)
						{
							if (value.Substring(i, separator[j].Length) == separator[j])
							{
								if (count > -1 && (entries.Count >= count - 1))
								{
									next = value.Substring(i - next.Length);
									entries.Add(next);
									i = value.Length - 1;
									break;
								}
								else
								{
									entries.Add(next);
									next = string.Empty;
									i += separator[j].Length - 1;
									continueOutside = true;
								}
							}
						}

						j++;
						continue;
					}
				}
				if (continueOutside)
				{
					continueOutside = false;
					goto IL_16F;
				}
				next += value[i];
				goto IL_16F;
			}
			if (!string.IsNullOrEmpty(next))
			{
				entries.Add(next);
				next = null;
			}
			return entries.ToArray();
		}
	}
}
