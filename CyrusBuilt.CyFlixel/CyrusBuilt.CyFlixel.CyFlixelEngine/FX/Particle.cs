//
//  Particle.cs
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
using Microsoft.Xna.Framework;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.FX
{
	/// <summary>
	/// An implemenatation of <see cref="FlixelSprite"/> that represents a particle.
	/// </summary>
	public class Particle : FlixelSprite
	{
		protected float _bounce = 0f;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FX.Particle"/>
		/// class with the amount of bounce to apply to the particle.
		/// </summary>
		/// <param name="bounce">
		/// The amount of bounce to apply to the particle.
		/// </param>
		public Particle(float bounce)
			: base() {
			this._bounce = bounce;
		}

		/// <summary>
		/// Should be called when this object collides with another object on
		/// either side. Since most games have identical behavior for running
		/// into walls, you can just override this function instead of overriding
		/// both <see cref="HitLeft()"/> or <see cref="HitRight()"/>.
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public override void HitSide(CyFlixelObject contact, float velocity) {
			float vy = base.Velocity.Y;
			float vx = (-base.Velocity.X * this._bounce);
			base.Velocity = new Vector2(vx, vy);
			if (base.AngularVelocity != 0) {
				base.AngularVelocity = (-base.AngularVelocity * this._bounce);
			}
		}

		/// <summary>
		/// Should be called when this object's bottom edge collides with the
		/// top of another object.
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public override void HitBottom(CyFlixelObject contact, float velocity) {
			float vx = base.Velocity.X;
			float vy = base.Velocity.Y;
			base.OnFloor = true;
			if (((base.Velocity.Y > 0) ? base.Velocity.Y : -base.Velocity.Y) > (this._bounce * 100)) {
				vy = (-base.Velocity.Y * this._bounce);
				if (base.AngularVelocity != 0) {
					base.AngularVelocity *= -this._bounce;
				}
			}
			else {
				base.AngularVelocity = 0;
				base.HitBottom(contact, velocity);
			}
			vx *= this._bounce;
			base.Velocity = new Vector2(vx, vy);
		}
	}
}

