//
//  Measurement2D.cs
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
namespace MBS.Framework.Drawing
{
	public struct MeasurementDimension2D
	{
		public Measurement Width { get; set; }
		public Measurement Height { get; set; }

		private bool _isNotEmpty;
		public bool IsEmpty { get { return !_isNotEmpty; } }

		public static readonly MeasurementDimension2D Empty;

		public override bool Equals(object obj)
		{
			if (!(obj is MeasurementDimension2D))
			{
				return false;
			}

			return
			(
				((MeasurementDimension2D)obj).Width == this.Width &&
				((MeasurementDimension2D)obj).Height == this.Height &&
				((MeasurementDimension2D)obj).IsEmpty == this.IsEmpty
			);
		}

		public MeasurementDimension2D(Measurement width, Measurement height)
		{
			Width = width;
			Height = height;
			_isNotEmpty = true;
		}
	}
}
