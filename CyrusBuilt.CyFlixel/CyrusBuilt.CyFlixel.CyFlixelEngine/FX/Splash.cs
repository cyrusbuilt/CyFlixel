//
//  Splash.cs
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.FX
{
	/// <summary>
	/// Splash screen. Moves all of the logo screen stuff into a game state.
	/// </summary>
	public class Splash : GameStateBase
	{
		#region Fields
		// TODO I don't think we really need _f, since we never actually use it for anything.
		private List<LogoPixel> _f = null;
		private float _logoTimer = 0f;
		private Texture2D _powerdBy = null;
		private SoundEffect _fSound = null;
		private static Color _fc = Color.Gray;
		private static GameStateBase _nextScreen = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FX.Splash"/>
		/// class. This is the default constructor.
		/// </summary>
		public Splash()
			: base() {
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets up the splash state.
		/// </summary>
		public override void Create() {
			base.Create();
			this._f = null;
			this._powerdBy = EngineGlobal.Content.Load<Texture2D>("cyflixel/poweredby");
			this._fSound = EngineGlobal.Content.Load<SoundEffect>("cyflixel/cyflixel");
			EngineGlobal.ScreenFlash.Start(EngineGlobal.BackgroundColor, 1f, null, false);
		}

		/// <summary>
		/// Sets the splash info.
		/// </summary>
		/// <param name="flxColor">
		/// The splash color.
		/// </param>
		/// <param name="nextScreen">
		/// The next screen.
		/// </param>
		public static void SetSplashInfo(Color flxColor, GameStateBase nextScreen) {
			_fc = flxColor;
			_nextScreen = nextScreen;
		}

		/// <summary>
		/// Automatically goes through and moves all of the logo screen stuff
		/// into a game state.
		/// </summary>
		public override void Update() {
			if (this._f == null) {
				// TODO See, this is what I mean. We instantiate the list, but
				// do nothing with it.
				this._f = new List<LogoPixel>();
				Int32 scale = 10;
				float pwrscale = 0f;
				Int32 pixelsize = (EngineGlobal.Height / scale);
				Int32 top = ((EngineGlobal.Height / 2) - (pixelsize * 2));
				Int32 left = ((EngineGlobal.Width / 2) - pixelsize);
				pwrscale = ((float)pixelsize / 24f);

				// Add logo pixels.
				base.Add(new LogoPixel((left + pixelsize), top, pixelsize, 1, _fc));
				base.Add(new LogoPixel(left, (top + pixelsize), pixelsize, 1, _fc));
				base.Add(new LogoPixel(left, (top + (pixelsize * 2)), pixelsize, 2, _fc));
				base.Add(new LogoPixel((left + pixelsize), (top + (pixelsize * 2)), pixelsize, 3, _fc));
				base.Add(new LogoPixel(left, (top + (pixelsize * 3)), pixelsize, 4, _fc));

				FlixelSprite pwr = new FlixelSprite((EngineGlobal.Width - (Int32)((float)this._powerdBy.Width * pwrscale)) / 2,
				                                    (top + (pixelsize * 4) + 16), this._powerdBy);
				pwr.LoadGraphic(this._powerdBy, false, false, (Int32)((float)this._powerdBy.Width * pwrscale),
				                (Int32)((float)this._powerdBy.Height * pwrscale));

				pwr.SpriteColor = _fc;
				pwr.Scale = pwrscale;
				base.Add(pwr);

				if (this._fSound != null) {
					this._fSound.Play(EngineGlobal.Volume, 0f, 0f);
				}
			}

			this._logoTimer += EngineGlobal.Elapsed;
			base.Update();
			if (this._logoTimer > 2.5f) {
				EngineGlobal.State = _nextScreen;
			}
		}
		#endregion
	}
}

