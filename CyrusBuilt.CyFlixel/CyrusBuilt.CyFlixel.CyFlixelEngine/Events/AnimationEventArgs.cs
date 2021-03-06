//
//  AnimationEventArgs.cs
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
	/// Animation event arguments class.
	/// </summary>
	public class AnimationEventArgs : EventArgs
	{
		#region Fields
		private String _name = String.Empty;
		private uint _frame = 0;
		private Int32 _frameIndex = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.AnimationEventArgs"/>
		/// class with the name, frame number, and frame index.
		/// </summary>
		/// <param name="name">
		/// The name of the animation.
		/// </param>
		/// <param name="frame">
		/// The current frame number.
		/// </param>
		/// <param name="frameIndex">
		/// The frame index.
		/// </param>
		public AnimationEventArgs(String name, uint frame, Int32 frameIndex)
			: base() {
			this._name = name;
			this._frame = frame;
			this._frameIndex = frameIndex;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the animation.
		/// </summary>
		public String Name {
			get { return this._name; }
		}

		/// <summary>
		/// Gets the frame being played.
		/// </summary>
		public uint Frame {
			get { return this._frame; }
		}

		/// <summary>
		/// Gets the index of the frame.
		/// </summary>
		public Int32 FrameIndex {
			get { return this._frameIndex; }
		}
		#endregion
	}
}

