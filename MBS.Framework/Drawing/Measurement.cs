//
//  Measurement.cs - represents a tuple of a numeric value and a unit of measure
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

namespace MBS.Framework.Drawing
{
	/// <summary>
	/// Represents a tuple of a numeric value and a unit of measure.
	/// </summary>
	public struct Measurement
	{
		public static readonly Measurement Empty;

		public Measurement(double value, MeasurementUnit unit)
		{
			Unit = unit;
			Value = value;
			mvarIsFull = true;
		}

		public double Value { get; set; }
		public MeasurementUnit Unit { get; set; }

		private bool mvarIsFull;
		public bool IsEmpty { get { return !mvarIsFull; } }

		public static Measurement Parse(string value)
		{
			DoubleStringSplitterResult dssr = NumericStringSplitter.SplitDoubleStringParts(value);
			double val = dssr.DoublePart;
			MeasurementUnit unit;
			switch (dssr.StringPart.ToLower())
			{
				case "cm": unit = MeasurementUnit.Cm; break;
				case "em": unit = MeasurementUnit.Em; break;
				case "ex": unit = MeasurementUnit.Ex; break;
				case "in": unit = MeasurementUnit.Inch; break;
				case "mm": unit = MeasurementUnit.Mm; break;
				case "%": unit = MeasurementUnit.Percentage; break;
				case "pc": unit = MeasurementUnit.Pica; break;
				case "px": unit = MeasurementUnit.Pixel; break;
				case "pt": unit = MeasurementUnit.Point; break;
				default: unit = MeasurementUnit.Pixel; break;
			}
			return new Measurement(val, unit);
		}

		public double GetValue(MeasurementUnit unit = MeasurementUnit.Pixel, int dpi = 96, Rectangle parentRect = default(Rectangle))
		{
			if (Unit == unit)
				return Value;

			double conversionFactor = GetConversionFactor(Unit, unit, dpi, parentRect);
			return Value * (1 / conversionFactor);
		}

		private double GetConversionFactor(MeasurementUnit unit1, MeasurementUnit unit2, int dpi, Rectangle parentRect)
		{
			if (unit1 == unit2)
				return 1.0;

			switch (unit1)
			{
				case MeasurementUnit.Cm:
				{
					switch (unit2)
					{
						case MeasurementUnit.Em:
						{
							break;
						}
						case MeasurementUnit.Ex:
						{
							break;
						}
						case MeasurementUnit.Inch:
						{
							return 0.393701;
						}
						case MeasurementUnit.Mm:
						{
							return 10;
						}
						case MeasurementUnit.Percentage:
						{
							break;
						}
						case MeasurementUnit.Pica:
						{
							return 2.36222;
						}
						case MeasurementUnit.Pixel:
						{
							// 1 in = 2.54 cm
							return 2.54 / dpi;
						}
						case MeasurementUnit.Point:
						{
							return 28.3464567;
						}
					}
					break;
				}
				case MeasurementUnit.Em:
					break;
				case MeasurementUnit.Ex:
					break;
				case MeasurementUnit.Inch:
				{
					switch (unit2)
					{
						case MeasurementUnit.Cm:
						{
							return 2.54;
						}
						case MeasurementUnit.Em:
						{
							break;
						}
						case MeasurementUnit.Ex:
						{
							break;
						}
						case MeasurementUnit.Mm:
						{
							return 25.4;
						}
						case MeasurementUnit.Percentage:
						{
							break;
						}
						case MeasurementUnit.Pica:
						{
							return 6.00005;
						}
						case MeasurementUnit.Pixel:
						{
							// 1 in = 2.54 cm
							return dpi;
						}
						case MeasurementUnit.Point:
						{
							return 72;
						}
					}
					break;
				}
				case MeasurementUnit.Pixel:
				{
					switch (unit2)
					{
						case MeasurementUnit.Inch:
						{
							return 1 / dpi;
						}
						case MeasurementUnit.Cm:
						{
							return dpi / 2.54;
						}
						case MeasurementUnit.Em:
						{
							break;
						}
						case MeasurementUnit.Ex:
						{
							break;
						}
						case MeasurementUnit.Mm:
						{
							return dpi / 25.4;
						}
						case MeasurementUnit.Percentage:
						{
							break;
						}
						case MeasurementUnit.Pica:
						{
							return dpi / 6.00005;
						}
						case MeasurementUnit.Pixel:
						{
							return 1.0;
						}
						case MeasurementUnit.Point:
						{
							return 72;
						}
					}
					break;
				}
			}
			throw new NotSupportedException();
		}

		public static Measurement operator -(Measurement left, Measurement right)
		{
			return new Measurement((left.Value - right.GetValue(left.Unit)), left.Unit);
		}
		public static Measurement operator +(Measurement left, Measurement right)
		{
			return new Measurement((left.Value + right.GetValue(left.Unit)), left.Unit);
		}

		public static Measurement operator /(Measurement left, Measurement right)
		{
			return new Measurement((left.Value / right.GetValue(left.Unit)), left.Unit);
		}
		public static Measurement operator *(Measurement left, Measurement right)
		{
			return new Measurement((left.Value * right.GetValue(left.Unit)), left.Unit);
		}
	}
}
