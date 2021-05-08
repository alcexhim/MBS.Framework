//
//  RangeSetting.cs
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
namespace MBS.Framework.Settings
{
	public class RangeSetting : Setting
	{
		public decimal? MinimumValue { get; set; } = null;
		public decimal? MaximumValue { get; set; } = null;

		public RangeSetting(string name, string title, decimal defaultValue = 0.0M, decimal? minimumValue = null, decimal? maximumValue = null) : base(name, title, defaultValue)
		{
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
		}
	}
}
