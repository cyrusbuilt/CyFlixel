//
//  SpriteCollisionEventArgs.cs
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
	/// Sprite collision event arguments class.
	/// </summary>
	public class SpriteCollisionEventArgs : EventArgs
	{
		#region Fields
		private CyFlixelObject _attacker = null;
		private CyFlixelObject _target = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.SpriteCollisionEventArgs"/>
		/// class with the attacker and target.
		/// </summary>
		/// <param name="attacker">
		/// The attacking sprite.
		/// </param>
		/// <param name="target">
		/// The sprite that is the target of the attack.
		/// </param>
		public SpriteCollisionEventArgs(CyFlixelObject attacker, CyFlixelObject target)
			: base() {
			this._attacker = attacker;
			this._target = target;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the attacking sprite.
		/// </summary>
		public CyFlixelObject Attacker {
			get { return _attacker; }
		}

		/// <summary>
		/// Gets the sprite that is the target of the attack.
		/// </summary>
		public CyFlixelObject Target {
			get { return this._target; }
		}
		#endregion
	}
}

