//
//  MouseEventArgs.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Events
{
	/// <summary>
	/// Mouse event arguments class.
	/// </summary>
	public class MouseEventArgs : EventArgs
	{
		private MouseEventTypes _mouseEvent = MouseEventTypes.MouseDown;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.MouseEventArgs"/>
		/// class with the type of mouse event that occurred.
		/// </summary>
		/// <param name="mouseEvent">
		/// The type of mouse event that occurred.
		/// </param>
		public MouseEventArgs(MouseEventTypes mouseEvent)
			: base() {
			this._mouseEvent = mouseEvent;
		}

		/// <summary>
		/// Gets the type of mouse event that occurred.
		/// </summary>
		public MouseEventTypes EventType {
			get { return this._mouseEvent; }
		}
	}
}

