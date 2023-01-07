//
//  NanoId.cs - .NET implementation of cryptographically-strong ID generator
//
//  Author:
//       zhu yu <https://github.com/codeyu>
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2017 zhu yu
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MBS.Framework
{
	public struct NanoId
	{
		private readonly string _value;
		private readonly bool _isNotEmpty;

		public bool IsEmpty { get { return !_isNotEmpty; } }

		public static readonly NanoId Empty;

		private NanoId(string value)
		{
			_value = value;
			_isNotEmpty = true;
		}
		public override string ToString()
		{
			return _value;
		}

		public static bool operator ==(NanoId left, NanoId right)
		{
			return (left.IsEmpty == right.IsEmpty && left._value == right._value);
		}
		public static bool operator !=(NanoId left, NanoId right)
		{
			return (left.IsEmpty != right.IsEmpty || left._value != right._value);
		}

		public override bool Equals(object obj)
		{
			if (obj is NanoId)
			{
				return ((NanoId)obj == this);
			}
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			if (IsEmpty)
				return base.GetHashCode();
			return _value.GetHashCode();
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		private class CryptoRandom : Random
		{
			private static RandomNumberGenerator _r;
#if !NETSTANDARD2_1
			private readonly byte[] _uint32Buffer = new byte[4];
#endif
			/// <inheritdoc />
			/// <summary>
			/// </summary>
			public CryptoRandom()
			{
				_r = RandomNumberGenerator.Create();
			}

			/// <summary>
			///
			/// </summary>
			/// <param name="buffer"></param>
			/// <exception cref="ArgumentNullException"></exception>
			public override void NextBytes(byte[] buffer)
			{
				if (buffer == null) throw new ArgumentNullException(nameof(buffer));
				_r.GetBytes(buffer);
			}

#if NETSTANDARD2_1
			/// <inheritdoc/>
			public override void NextBytes(Span<byte> buffer)
			{
				RandomNumberGenerator.Fill(buffer);
			}
#endif

			/// <inheritdoc />
			/// <summary>
			/// </summary>
			/// <returns></returns>
			public override double NextDouble()
			{
#if NETSTANDARD2_1
				Span<byte> uint32Buffer = stackalloc byte[4];
				RandomNumberGenerator.Fill(uint32Buffer);
				return BitConverter.ToUInt32(uint32Buffer) / (1.0 + UInt32.MaxValue);
#else
				_r.GetBytes(_uint32Buffer);
				return BitConverter.ToUInt32(_uint32Buffer, 0) / (1.0 + UInt32.MaxValue);
#endif
			}
			/// <inheritdoc />
			/// <summary>
			/// </summary>
			/// <param name="minValue"></param>
			/// <param name="maxValue"></param>
			/// <returns></returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
			public override int Next(int minValue, int maxValue)
			{
				if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
				if (minValue == maxValue) return minValue;
				var range = (long)maxValue - minValue;
				return (int)((long)Math.Floor(NextDouble() * range) + minValue);
			}
			/// <inheritdoc />
			/// <summary>
			/// </summary>
			/// <returns></returns>
			public override int Next()
			{
				return Next(0, int.MaxValue);
			}
			/// <inheritdoc />
			/// <summary>
			/// </summary>
			/// <param name="maxValue"></param>
			/// <returns></returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
			public override int Next(int maxValue)
			{
				if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue));
				return Next(0, maxValue);
			}
		}


		private const string DefaultAlphabet = "_-0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static readonly CryptoRandom Random = new CryptoRandom();
		/// <summary>
		///
		/// </summary>
		/// <param name="alphabet"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		// public static async Task<string> GenerateAsync(string alphabet = DefaultAlphabet, int size = 21) => await Task.Run(() => Generate(Random, alphabet, size));

		/// <summary>
		///
		/// </summary>
		/// <param name="alphabet"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static NanoId Generate(string alphabet = DefaultAlphabet, int size = 21) => Generate(Random, alphabet, size);

		/// <summary>
		///
		/// </summary>
		/// <param name="random"></param>
		/// <param name="alphabet"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static NanoId Generate(Random random, string alphabet = DefaultAlphabet, int size = 21)
		{

			if (random == null)
			{
				throw new ArgumentNullException("random cannot be null.");
			}

			if (alphabet == null)
			{
				throw new ArgumentNullException("alphabet cannot be null.");
			}

			if (alphabet.Length <= 0 || alphabet.Length >= 256)
			{
				throw new ArgumentOutOfRangeException("alphabet must contain between 1 and 255 symbols.");
			}

			if (size <= 0)
			{
				throw new ArgumentOutOfRangeException("size must be greater than zero.");
			}

			// See https://github.com/ai/nanoid/blob/master/format.js for
			// explanation why masking is use (`random % alphabet` is a common
			// mistake security-wise).
			var mask = (2 << 31 - Clz32((alphabet.Length - 1) | 1)) - 1;
			var step = (int)Math.Ceiling(1.6 * mask * size / alphabet.Length);

#if NETSTANDARD2_1
			Span<char> idBuilder = stackalloc char[size];
			Span<byte> bytes = stackalloc byte[step];
#else
			var idBuilder = new char[size];
			var bytes = new byte[step];
#endif

			int cnt = 0;

			while (true)
			{

				random.NextBytes(bytes);

				for (var i = 0; i < step; i++)
				{

					var alphabetIndex = bytes[i] & mask;

					if (alphabetIndex >= alphabet.Length) continue;
					idBuilder[cnt] = alphabet[alphabetIndex];
					if (++cnt == size)
					{
						return new NanoId(new string(idBuilder));
					}

				}

			}

		}

		/// <summary>
		/// Counts leading zeros of <paramref name="x"/>.
		/// </summary>
		/// <param name="x">Input number.</param>
		/// <returns>Number of leading zeros.</returns>
		/// <remarks>
		/// Courtesy of spender/Sunsetquest see https://stackoverflow.com/a/10439333/623392.
		/// </remarks>
		internal static int Clz32(int x)
		{
			const int numIntBits = sizeof(int) * 8; //compile time constant
													//do the smearing
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			//count the ones
			x -= x >> 1 & 0x55555555;
			x = (x >> 2 & 0x33333333) + (x & 0x33333333);
			x = (x >> 4) + x & 0x0f0f0f0f;
			x += x >> 8;
			x += x >> 16;
			return numIntBits - (x & 0x0000003f); //subtract # of 1s from 32
		}
	}
}
