//
//  FlixelAnimation.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Data
{
	/// <summary>
	/// Flixel animation. A helper structure for the CyFlixel sprite animation
	/// system.
	/// </summary>
	public class FlixelAnimation
	{
		private const Int32 HASH_MULTIPLIER = 31;

		#region Fields
		private Guid _id = Guid.Empty;
		private String _name = String.Empty;
		private float _delay = 0f;
		private Int32[] _frames = { };
		private Boolean _looped = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelAnimation"/>
		/// class with the name, frame order, frame rate, and a flag indicating
		/// whether or not the animation is looped or just plays once.
		/// </summary>
		/// <param name="name">
		/// What this animation should be called.
		/// </param>
		/// <param name="frames">
		/// An array of numbers indicating what frames to play and in what order.
		/// </param>
		/// <param name="framerate">
		/// The speed in frames-per-second that the animation should play at
		/// (eg. 40 fps).
		/// </param>
		/// <param name="looped">
		/// Whether or not the animation is looped or just plays once.
		/// </param>
		public FlixelAnimation(String name, Int32[] frames, Int32 framerate, Boolean looped) {
			this._id = Guid.NewGuid();
			this._name = name;
			this._frames = frames;
			this._delay = (1.0f / (float)framerate);
			this._looped = looped;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelAnimation"/>
		/// class with the name, frame order, and frame rate.
		/// </summary>
		/// <param name="name">
		/// What this animation should be called.
		/// </param>
		/// <param name="frames">
		/// An array of numbers indicating what frames to play and in what order.
		/// </param>
		/// <param name="framerate">
		/// The speed in frames-per-second that the animation should play at
		/// (eg. 40 fps).
		/// </param>
		public FlixelAnimation(String name, Int32[] frames, Int32 framerate) {
			this._id = Guid.NewGuid();
			this._name = name;
			this._frames = frames;
			this._delay = (1.0f / (float)framerate);
			this._looped = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelAnimation"/>
		/// class with the name and frame order.
		/// </summary>
		/// <param name="name">
		/// What this animation should be called.
		/// </param>
		/// <param name="frames">
		/// An array of numbers indicating what frames to play and in what order.
		/// </param>
		public FlixelAnimation(String name, Int32[] frames) {
			this._id = Guid.NewGuid();
			this._name = name;
			this._frames = frames;
			this._delay = 0f;
			this._looped = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of this animation.
		/// </summary>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets or sets the frames that should be played.
		/// </summary>
		public Int32[] Frames {
			get { return this._frames; }
			set { this._frames = value; }
		}

		/// <summary>
		/// Gets or sets the delay.
		/// </summary>
		public float Delay {
			get { return this._delay; }
			set { this._delay = value; }
		}

		/// <summary>
		/// Gets or sets whether or not this animation is looped or just plays
		/// once.
		/// </summary>
		public Boolean Looped {
			get { return this._looped; }
			set { this._looped = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelAnimation"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelAnimation"/>.
		/// </returns>
		public override String ToString() {
			return this._name;
		}

		/// <summary>
		/// Provides a hashcode identifier of this instance.
		/// </summary>
		/// <returns>
		/// The hashcode identifier for this instance.
		/// </returns>
		public override Int32 GetHashCode() {
			unchecked {
				Int32 hashCode = base.GetType().GetHashCode();
				return (hashCode * HASH_MULTIPLIER) ^ this._id.GetHashCode();
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is
		/// equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="System.Object"/> is equal to the
		/// current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>;
		/// Otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj) {
			if (obj == null) {
				return false;
			}

			FlixelAnimation anim = obj as FlixelAnimation;
			if ((Object)anim == null) {
				return false;
			}

			return ((this._delay == anim.Delay) && (this._frames == anim.Frames) &&
			        (this._looped == anim.Looped) && (this._name == anim.Name));
		}

		/// <summary>
		/// Determines whether the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>.
		/// </summary>
		/// <param name="anim">
		/// The <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>
		/// to compare with the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelAnimation"/>;
		/// Otherwise, false.
		/// </returns>
		public Boolean Equals(FlixelAnimation anim) {
			if (anim == null) {
				return false;
			}

			if ((Object)anim == null) {
				return false;
			}
			
			return ((this._delay == anim.Delay) && (this._frames == anim.Frames) &&
			        (this._looped == anim.Looped) && (this._name == anim.Name));
		}
		#endregion
	}
}

