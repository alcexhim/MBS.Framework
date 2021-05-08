//
//  ISupportsExtraData.cs - interface for providing GetExtraData / SetExtraData methods
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2019-2021 Mike Becker's Software
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
	/// <summary>
	/// Provides methods to store and retrieve extra data parameters on objects.
	/// </summary>
	public interface ISupportsExtraData
	{
		/// <summary>
		/// Gets the extra data with the specified key. If the extra data with the given key is not present,
		/// return the specified default value.
		/// </summary>
		/// <returns>The extra data with the specified key, or <paramref name="defaultValue" /> if not present.</returns>
		/// <param name="key">The key to look up.</param>
		/// <param name="defaultValue">The default value to return if the key is not present.</param>
		/// <typeparam name="T">The type of data item to return.</typeparam>
		T GetExtraData<T>(string key, T defaultValue = default(T));
		/// <summary>
		/// Sets the extra data with the specified key and value.
		/// </summary>
		/// <param name="key">The name of the data item to set.</param>
		/// <param name="value">The value to set.</param>
		/// <typeparam name="T">The type of data item to set.</typeparam>
		void SetExtraData<T>(string key, T value);
		/// <summary>
		/// Gets the extra data with the specified key. If the extra data with the given key is not present,
		/// return the specified default value.
		/// </summary>
		/// <returns>The extra data with the specified key, or <paramref name="defaultValue" /> if not present.</returns>
		/// <param name="key">The key to look up.</param>
		/// <param name="defaultValue">The default value to return if the key is not present.</param>
		object GetExtraData(string key, object defaultValue = null);
		/// <summary>
		/// Sets the extra data with the specified key and value.
		/// </summary>
		/// <param name="key">The name of the data item to set.</param>
		/// <param name="value">The value to set.</param>
		void SetExtraData(string key, object value);
	}
}
