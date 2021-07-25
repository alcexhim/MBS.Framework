using System;
namespace MBS.Framework
{
	public static class MathExtensions
	{
		/// <summary>
		/// Multiplies a floating point value x by the number 2 raised to the exp power.
		/// </summary>
		/// <returns>The ldexp.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="exp">Exp.</param>
		public static float ldexp(float x, int exp)
		{
			return (float)(x * Math.Pow(2, exp));
		}
		/// <summary>
		/// Multiplies a floating point value x by the number 2 raised to the exp power.
		/// </summary>
		/// <returns>The ldexp.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="exp">Exp.</param>
		public static double ldexp(double x, int exp)
		{
			return (x * Math.Pow(2, exp));
		}
	}
}
