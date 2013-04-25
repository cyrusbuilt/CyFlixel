//
//  FlixelUtils.cs
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
	/// General utility methods.
	/// </summary>
	public static class FlixelUtils
	{
		/// <summary>
		/// Gets the name of the specified object.
		/// </summary>
		/// <returns>
		/// The class name of the specified object.
		/// </returns>
		/// <param name="obj">
		/// The object to get the name of.
		/// </param>
		/// <param name="simple">
		/// If set true, will return the name only not the full namespace.
		/// </param>
		public static String GetClassName(Object obj, Boolean simple) {
			if (obj == null) {
				return String.Empty;
			}

			if (simple) {
				return obj.GetType().Name;
			}
			else {
				return obj.GetType().FullName;
			}
		}

		/// <summary>
		/// Used with <see cref="EndProfile()"/> to determine how long it takes
		/// to execute specific blocks of code.
		/// </summary>
		/// <returns>
		/// The starting value which should be passed to <see cref="EndProfile()"/>.
		/// </returns>
		public static uint StartProfile() {
			return EngineGlobal.EngineTimer;
		}

		/// <summary>
		/// Used with <see cref="StartProfile"/> to determine how long it takes
		/// to execute specific blocks of code.
		/// </summary>
		/// <returns>
		/// The amount of time (in milliseconds) since the call to <see cref="StartProfile"/>.
		/// </returns>
		/// <param name="start">
		/// The start value returned by <see cref="StartProfile"/>.
		/// </param>
		/// <param name="name">
		/// Optional tag (for debug console to display). Default value is
		/// "Profiler".
		/// </param>
		/// <param name="log">
		/// Whether or not to log the elapsed time to the debug console.
		/// </param>
		public static uint EndProfile(uint start, String name, Boolean log) {
			uint t = EngineGlobal.EngineTimer;
			uint diff = (t - start);
			if (log) {
				if (String.IsNullOrEmpty(name)) {
					name = "Profiler";
				}
				EngineGlobal.Log(name + ": " + (diff / 1000) + "s");
			}
			return diff;
		}
	}
}

