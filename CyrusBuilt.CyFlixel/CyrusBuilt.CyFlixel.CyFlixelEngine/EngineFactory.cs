//
//  EngineFactory.cs
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
#if !WINDOWS_PHONE
using Microsoft.Xna.Framework.GamerServices;
#endif
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// Factory class for bootstrapping the engine. Your application (game)
	/// should use this class to instantiate the engine.
	/// </summary>
	public sealed class EngineFactory : Game
	{
		#region Display Buffer Constants
#if XBOX360
		private readonly Boolean FULLSCREEN = true;
#else
		private readonly Boolean FULLSCREEN = false;
#endif
		#endregion

		#region Fields
#if !WINDOWS_PHONE
		private Int32 _resX = 1280;
		private Int32 _resY = 720;
#else
		private Int32 _resX = 480;
		private Int32 _resY = 800;
#endif
		private GraphicsDeviceManager _graphics = null;
		private CyFlixelGame _game = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EngineFactory"/>
		/// class. This is the default constructor.
		/// </summary>
		public EngineFactory()
			: base() {
			this._graphics = new GraphicsDeviceManager(this);
			base.Content.RootDirectory = "Content";
			if (FULLSCREEN) {
				DisplayMode mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
				this._resX = mode.Width;
				this._resY = mode.Height;
				if (GraphicsAdapter.DefaultAdapter.IsWideScreen) {
					// If user has it set to widescreen, let's make sure this
					// is *actually* a widescreen resolution.
					if (((this._resX / 16) * 9) != this._resY) {
						this._resX = ((this._resY / 9) * 16);
					}
				}
			}

			// Regardless of the comments found in the X-Flixel source that this
			// engine is derived from, we *DO* prefer new-fangled pixel processing
			// because this option will enable anti-aliasing, which will get rid
			// of the jagged edges on our sprites. NOTE: This option is ignored
			// if no hardware is available that supports multi-sampling.
			this._graphics.PreferMultiSampling = true;

			// Set preferred screen resolution. This is NOT the same thing as
			// the game's actual resolution.
			this._graphics.PreferredBackBufferWidth = this._resX;
			this._graphics.PreferredBackBufferHeight = this._resY;

			// Make sure we're actually running full-screen if the the full-screen
			// preference is set.
			if ((FULLSCREEN) && (!this._graphics.IsFullScreen)) {
				this._graphics.ToggleFullScreen();
			}
			this._graphics.ApplyChanges();

			EngineGlobal.GameReference = this;
#if !WINDOWS_PHONE
			base.Components.Add(new GamerServicesComponent(this));
#endif
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initializes the engine factory. NOTE: This gets called by the
		/// <see cref="Run()"/> method automatically. There is no reason
		/// to call this method directly.
		/// </summary>
		protected override void Initialize() {
			// Load up the master class and away we go!
			this._game = new CyFlixelGame();
			base.Components.Add(this._game);
			base.Initialize();
		}
		#endregion
	}
}

