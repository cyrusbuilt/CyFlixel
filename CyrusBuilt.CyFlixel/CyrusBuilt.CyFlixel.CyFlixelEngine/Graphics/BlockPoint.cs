//
//  BlockPoint.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics
{
	/// <summary>
	/// A block point.
	/// </summary>
	public struct BlockPoint
	{
		/// <summary>
		/// The X position of the point.
		/// </summary>
		public Int32 X;

		/// <summary>
		/// The Y position of the point.
		/// </summary>
		public Int32 Y;

		/// <summary>
		/// The point data.
		/// </summary>
		public Int32 Data;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.BlockPoint"/>
		/// struct.
		/// </summary>
		/// <param name="x">
		/// The X position.
		/// </param>
		/// <param name="y">
		/// The Y position.
		/// </param>
		/// <param name="data">
		/// The point data.
		/// </param>
		public BlockPoint(Int32 x, Int32 y, Int32 data) {
			this.X = x;
			this.Y = y;
			this.Data = data;
		}
	}
}

