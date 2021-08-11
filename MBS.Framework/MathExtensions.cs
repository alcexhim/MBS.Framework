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

		/// <summary>
		/// Returns a random <see cref="long" /> between
		/// <paramref name="minValue"/> and <paramref name="maxValue" />.
		/// </summary>
		/// <returns>The random number.</returns>
		/// <param name="random">The instance of <see cref="Random" /> being extended.</param>
		/// <param name="minValue">The inclusive minimum bound of the resulting random value.</param>
		/// <param name="maxValue">The exclusive maximum bound of the resulting random value.</param>
		public static long NextLong (this Random random, long minValue = 0, long maxValue = long.MaxValue)
		{
			return (long)((random.NextDouble() * (maxValue - minValue)) + minValue);
		}
	}
}
