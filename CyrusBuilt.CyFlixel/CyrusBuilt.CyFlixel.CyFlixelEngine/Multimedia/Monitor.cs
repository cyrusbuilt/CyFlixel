//
//  Monitor.cs
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
using System.Collections.Generic;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia
{
	/// <summary>
	/// A simple class to aggregate and average data. CyFlixel uses this to
	/// display framerate and profiling data in the developer console. It's
	/// nice for keeping track of things that might be changing too fast from
	/// frame to frame.
	/// </summary>
	public class Monitor : IDisposable
	{
		#region Protected Fields
		protected Int32 _size = 0;
		protected Int32 _itr = 0;
		protected List<float> _data = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Monitor"/>
		/// class with the desired internal capacity size and default entry value.
		/// This creates the internal tracking list and sets its size.
		/// </summary>
		/// <param name="size">
		/// The desired size of the internal tracking list.
		/// </param>
		/// <param name="defaultValue">
		/// The default value of all the entries in the list.
		/// </param>
		public Monitor(Int32 size, float defaultValue) {
			this._size = size;
			if (this._size <= 0) {
				this._size = 1;
			}
			this._itr = 0;
			this._data = new List<float>(this._size);
			Int32 i = 0;
			while (i < this._size) {
				this._data[i++] = defaultValue;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds an entry to the array of data.
		/// </summary>
		/// <param name="data">
		/// The value you want to track and average.
		/// </param>
		public void Add(float data) {
			if (this._itr < this._data.Count) {
				this._data[this._itr++] = data;
			}

			if (this._itr >= this._size) {
				this._itr = 0;
			}
		}

		/// <summary>
		/// Averages the value of all the numbers in the monitor window.
		/// </summary>
		/// <returns>
		/// The average value of all the numbers in the monitor window.
		/// </returns>			
		public float Average() {
			float sum = 0;
			Int32 i = 0;
			while (i < this._size) {
				sum += this._data[i++];
			}
			return (sum / this._size);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Monitor"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Monitor"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Monitor"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Monitor"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Monitor"/> was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._data != null) {
				this._data.Clear();
				this._data = null;
			}
			this._size = 0;
			this._itr = 0;
		}
		#endregion
	}
}

