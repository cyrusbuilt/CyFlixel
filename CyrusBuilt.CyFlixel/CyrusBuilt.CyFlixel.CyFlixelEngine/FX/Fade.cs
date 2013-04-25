//
//  Fade.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.FX
{
	/// <summary>
	/// A screen fade effect.
	/// </summary>
	public class Fade : FlixelSprite
	{
		#region Fields
		private float _delay = 0;
		private EventHandler<EffectCompletedEventArgs> _completed = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FX.Fade"/>
		/// class. This is the default constructor.
		/// </summary>
		public Fade()
			: base(0, 0) {
			base.CreateGraphic(EngineGlobal.Width, EngineGlobal.Height, Color.Black);
			base.ScrollFactor = new Vector2(0, 0);
			base.Exists = false;
			base.Solid = false;
			base.Fixed = true;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Stops and hides this screen effect.
		/// </summary>
		public void Stop() {
			base.Exists = false;
		}

		/// <summary>
		/// Reset and trigger this screen effect.
		/// </summary>
		/// <param name="color">
		/// The desired color to use.
		/// </param>
		/// <param name="duration">
		/// How long it should take in seconds for the screen to fade out.
		/// </param>
		/// <param name="completedCallback">
		/// Callback method to call when the effect finishes.
		/// </param>
		/// <param name="force">
		/// Forces the effect to reset.
		/// </param>
		public void Start(Color color, float duration, EventHandler<EffectCompletedEventArgs> completedCallback, Boolean force) {
			if ((!force) && (base.Exists)) {
				return;
			}
			base.SpriteColor = color;
			this._delay = duration;
			this._completed = completedCallback;
			base.Exists = true;
		}

		/// <summary>
		/// Reset and trigger this screen effect.
		/// </summary>
		/// <param name="color">
		/// The desired color to use.
		/// </param>
		/// <param name="duration">
		/// How long it should take in seconds for the screen to fade out.
		/// </param>
		public void Start(Color color, float duration) {
			this.Start(color, duration, null, false);
		}

		/// <summary>
		/// Reset and trigger this screen effect.
		/// </summary>
		/// <param name="color">
		/// The desired color to use.
		/// </param>
		public void Start(Color color) {
			this.Start(color, 1f, null, false);
		}

		/// <summary>
		/// Updates and/or animates this effect.
		/// </summary>
		public override void Update() {
			base.Alpha += (EngineGlobal.Elapsed / this._delay);
			if (base.Alpha >= 1) {
				base.Alpha = 1;
				if (this._completed != null) {
					this._completed(this, new EffectCompletedEventArgs(EffectTypes.FadeOut));
				}
			}
		}
		#endregion
	}
}

