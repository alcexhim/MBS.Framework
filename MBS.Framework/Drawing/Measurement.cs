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
using MBS.Framework.Collections.Generic;

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

		public override bool Equals(object obj)
		{
			if (!(obj is Measurement))
				return false;

			if (((Measurement)obj).Value.Equals(Value) && (((Measurement)obj).Unit == Unit))
				return true;

			return base.Equals(obj);
		}

		public static bool operator == (Measurement left, Measurement right)
		{
			return (left.Equals(right));
		}
		public static bool operator != (Measurement left, Measurement right)
		{
			return (!left.Equals(right));
		}

		public static Measurement Parse(string value)
		{
			if (value == null)
				return Measurement.Empty;

			DoubleStringSplitterResult dssr = NumericStringSplitter.SplitDoubleStringParts(value);
			double val = dssr.DoublePart;
			MeasurementUnit unit = MeasurementUnitFromString(dssr.StringPart);
			return new Measurement(val, unit);
		}

		static Measurement()
		{
			RegisterMeasurementUnit(MeasurementUnit.Cm, "cm");
			RegisterMeasurementUnit(MeasurementUnit.Em, "em");
			RegisterMeasurementUnit(MeasurementUnit.Ex, "ex");
			RegisterMeasurementUnit(MeasurementUnit.Inch, "in");
			RegisterMeasurementUnit(MeasurementUnit.Mm, "mm");
			RegisterMeasurementUnit(MeasurementUnit.Percentage, "%");
			RegisterMeasurementUnit(MeasurementUnit.Pica, "pc");
			RegisterMeasurementUnit(MeasurementUnit.Pixel, "px");
			RegisterMeasurementUnit(MeasurementUnit.Point, "pt");

			RegisterMeasurementUnit(MeasurementUnit.Degrees, "deg");
			RegisterMeasurementUnit(MeasurementUnit.Radians, "rad");
			RegisterMeasurementUnit(MeasurementUnit.Gradians, "grad");
		}

		private static BidirectionalDictionary<MeasurementUnit, string> _measurementUnits = new BidirectionalDictionary<MeasurementUnit, string>();
		private static void RegisterMeasurementUnit(MeasurementUnit unit, string value)
		{
			_measurementUnits.Add(unit, value);
		}

		public static MeasurementUnit MeasurementUnitFromString(string value)
		{
			if (_measurementUnits.ContainsValue2(value))
				return _measurementUnits.GetValue1(value);

			throw new ArgumentException("must be a known MeasurementUnit abbreviation value", nameof(value));
		}
		public static string MeasurementUnitToString(MeasurementUnit unit)
		{
			if (_measurementUnits.ContainsValue1(unit))
				return _measurementUnits.GetValue2(unit);

			throw new ArgumentException("must be a known MeasurementUnit enumeration value", nameof(unit));
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

		public override string ToString()
		{
			return String.Format("{0}{1}", Value, MeasurementUnitToString(Unit));
		}
	}
}
