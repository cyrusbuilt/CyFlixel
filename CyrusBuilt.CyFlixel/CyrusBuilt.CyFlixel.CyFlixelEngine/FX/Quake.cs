//
//  Quake.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.FX
{
	/// <summary>
	/// A quake effect. Causes the screen to shake.
	/// </summary>
	public class Quake
	{
		#region Fields
		private Int32 _zoom = 0;
		private float _intensity = 0f;
		private float _timer = 0f;
		private Int32 _x = 0;
		private Int32 _y = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FX.Quake"/>
		/// class with the screen zoom level.
		/// </summary>
		/// <param name="zoom">
		/// The screen zoom level.
		/// </param>
		public Quake(Int32 zoom) {
			this._zoom = zoom;
			this.Start(0, 0);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the amount of X distortion to apply to the screen.
		/// </summary>
		public Int32 X {
			get { return this._x; }
			set { this._x = value; }
		}

		/// <summary>
		/// Gets or sets the amount of Y distortion to apply to the screen.
		/// </summary>
		public Int32 Y {
			get { return this._y; }
			set { this._y = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Stops the quake effect.
		/// </summary>
		public void Stop() {
			this._x = 0;
			this._y = 0;
			this._intensity = 0;
			this._timer = 0;
		}

		/// <summary>
		/// Reset and trigger this effect.
		/// </summary>
		/// <param name="intensity">
		/// Percentage of screen size representing the maximum distance that
		/// the screen can move during the 'quake'.
		/// </param>
		/// <param name="duration">
		/// The length in seconds that the 'quake' should last.
		/// </param>
		public void Start(float intensity, float duration) {
			this.Stop();
			this._intensity = intensity;
			this._timer = duration;
		}

		/// <summary>
		/// Updates and/or animates this effect.
		/// </summary>
		public void Update() {
			if (this._timer > 0) {
				this._timer -= EngineGlobal.Elapsed;
				if (this._timer <= 0) {
					this._timer = 0;
					this._x = 0;
					this._y = 0;
				}
				else {
					this._x = (Int32)(GameMath.Random() * this._intensity * EngineGlobal.Width * 2 - this._intensity * EngineGlobal.Width) * this._zoom;
					this._y = (Int32)(GameMath.Random() * this._intensity * EngineGlobal.Height * 2 - this._intensity * EngineGlobal.Height) * this._zoom;
				}
			}
		}
		#endregion
	}
}

