//
//  GameMath.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 CyrusBuilt
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
using System;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// Math utility functions.
	/// </summary>
	public static class GameMath
	{
		private static Random _randGlobal = new Random();

		[ThreadStatic]
		private static Random _rand = null;

		/// <summary>
		/// Helps to eliminate false collisions and/or rendering glitches caused
		/// by rounding errors
		/// </summary>
		internal static readonly float RoundingError = 0.0001f;

		/// <summary>
		/// Returns the absolute value of the specified number.
		/// </summary>
		/// <param name="n">
		/// The number to get the aboslute value of.
		/// </param>
		/// <returns>
		/// The absolute value of the specified number.
		/// </returns>			
		public static float Abs(float n) {
			return (n > 0) ? n : -n;
		}

		/// <summary>
		/// Rounds the specified number down to the nearest integer.
		/// </summary>
		/// <param name="n">
		/// The number to round down.
		/// </param>
		/// <returns>
		/// The specified number rounded down to the nearest integer.
		/// </returns>			
		public static float Floor(float n) {
			float num = (Int32)n;
			return (n > 0) ? num : ((num != n) ? (num - 1) : num);
		}

		/// <summary>
		/// Rounds the specified number up to the nearest integer.
		/// </summary>
		/// <param name="n">
		/// The number to round up.
		/// </param>
		/// <returns>
		/// The specified number rounded up to the nearest integer.
		/// </returns>			
		public static float Ceil(float n) {
			float num = (Int32)n;
			return (n > 0) ? ((num != n) ? (num + 1) : num) : num;
		}

		/// <summary>
		/// Gets the number with the lowest value.
		/// </summary>
		/// <param name="n1">
		/// The first number to compare.
		/// </param>
		/// <param name="n2">
		/// The second number to compare.
		/// </param>
		/// <returns>
		/// The number with the lowest value.
		/// </returns>			
		public static float Min(float n1, float n2) {
			return (n1 <= n2) ? n1 : n2;
		}

		/// <summary>
		/// Gets the number with the highest value.
		/// </summary>
		/// <param name="n1">
		/// The first number to compare.
		/// </param>
		/// <param name="n2">
		/// The second number to compare.
		/// </param>
		/// <returns>
		/// The number with the highest value.
		/// </returns>			
		public static float Max(float n1, float n2) {
			return (n1 >= n2) ? n1 : n2;
		}

		/// <summary>
		/// Generates a random number.
		/// </summary>
		public static float Random() {
			if (_rand == null) {
				Int32 seed = 0;
				lock (_randGlobal) {
					seed = _randGlobal.Next();
				}
				_rand = new Random(seed);
			}
			return (float)_rand.NextDouble();
		}

		/// <summary>
		/// Generates a random number. NOTE: To create a series of predictable
		/// random numbers, add the random number you generate each time to the
		/// <paramref name="seed"/> value before calling <see cref="Random()"/>
		/// again.
		/// </summary>
		/// <param name="seed">
		/// The value used to calculate a predictable random number.
		/// </para>
		/// <returns>
		/// A number between 0 and 1.
		/// </returns>			
		public static float Random(float seed) {
			// Make sure seed value is ok.
			if (seed == 0) {
				seed = float.MinValue;
			}

			if (seed >= 1) {
				if ((seed % 1) == 0) {
					seed /= (float)Math.PI;
				}
				seed %= 1;
			}
			else if (seed < 0) {
				seed = (seed % 1) + 1;
			}

			// Then do an LCG thing and return a predictable random number.
			return ((69621 * (Int32)(seed * 0x7FFFFFFF)) % 0x7FFFFFFF) / 0x7FFFFFFF;
		}
	}
}

