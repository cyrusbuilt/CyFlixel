//
//  GameStateBase.cs
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
using Microsoft.Xna.Framework.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// This is the basic game "state" object - e.g. in a simple game you might
	/// have a menu state and a play state. It acts as a kind of container for
	/// all your game objects. You can also access the game's background color
	/// and screen buffer through this object. This is kind of a funny class
	/// from the technical side, it is just a regular Sprite display object,
	/// with one member variable: a CyFlixel <code>FlixelGroup</code>. This
	/// means you can load it up with regular Flash stuff or with CyFlixel
	/// elements, whatever works!
	/// </summary>
	public abstract class GameStateBase : IDisposable
	{
		#region Fields
		private static Color _bgColor = Color.White;
		private FlixelGroup _defaultGroup = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.GameStateBase"/>
		/// class. This is the default constructor.
		/// </summary>
		public GameStateBase() {
			this._defaultGroup = new FlixelGroup();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the "clear color" or default background color of the
		/// game. You may change this value at any time.
		/// </summary>
		public static Color BackgroundColor {
			get { return _bgColor; }
			set { _bgColor = value; }
		}

		/// <summary>
		/// Gets or sets the default group, which is a group used to organize
		/// and display objects you add to this state.
		/// </summary>
		public FlixelGroup DefaultGroup {
			get { return this._defaultGroup; }
			set { this._defaultGroup = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// In an overridden class, this method is used to set up the game state.
		/// This is where you create your groups and game objects.
		/// </summary>
		public virtual void Create() {
			_bgColor = EngineGlobal.BackgroundColor;
			// Nothing else to do here initially.
		}

		/// <summary>
		/// Adds a new object to the game loop.
		/// </summary>
		/// <param name="core">
		/// The object you want to add to the game loop.
		/// </param>
		public virtual CyFlixelObject Add(CyFlixelObject core) {
			if (core == null) {
				return null;
			}
			return this._defaultGroup.Add(core);
		}

		/// <summary>
		/// Override this function to do special pre-processing FX like motion blur.
		/// By default, this simply overwrites the buffer with the background color.
		/// </summary>
		/// <param name="sb">
		/// The sprite group to manipulate.
		/// </param>
		public virtual void PreProcess(SpriteBatch sb) {
			if (sb == null) {
				throw new ArgumentNullException("sb");
			}
			sb.GraphicsDevice.Clear(_bgColor);
		}

		/// <summary>
		/// Automatically goes through and calls update on everything you added
		/// to the game loop, override this function to handle custom input and
		/// perform collisions.
		/// </summary>
		public virtual void Update() {
			if (this._defaultGroup != null) {
				this._defaultGroup.Update();
			}
		}

		/// <summary>
		/// This method collides <see cref="DefaultGroup"/> against itself.
		/// (Basically everything you added to this state).
		/// </summary>
		public virtual void Collide() {
			if (this._defaultGroup != null) {
				GraphicsUtils.Collide(this._defaultGroup, this._defaultGroup);
			}
		}

		/// <summary>
		/// Automatically goes through and calls render on everything you added
		/// to the game loop. Override this loop to manually control the
		/// rendering process.
		/// </summary>
		/// <param name="sb">
		/// The sprite group to manipulate.
		/// </param>
		public virtual void Render(SpriteBatch sb) {
			if ((sb != null) && (this._defaultGroup != null)) {
				SpriteSortMode ssm = SpriteSortMode.Immediate;
				BlendState bs = BlendState.NonPremultiplied;
				SamplerState ss = SamplerState.PointClamp;
				DepthStencilState dss = DepthStencilState.None;
				RasterizerState rs = RasterizerState.CullCounterClockwise;
				sb.Begin(ssm, bs, ss, dss, rs);
				this._defaultGroup.Render(sb);
				sb.End();
			}
		}

		/// <summary>
		/// Override this function to do special post-processing FX like light
		/// bloom. By default, this method does not.
		/// </summary>
		/// <param name="sb">
		/// The sprite group to manipulate.
		/// </param>
		public virtual void PostProcess(SpriteBatch sb) {
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.GameStateBase"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.GameStateBase"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.GameStateBase"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.GameStateBase"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.GameStateBase"/> was occupying.
		/// </remarks>
		public virtual void Dispose() {
			if (this._defaultGroup != null) {
				this._defaultGroup.Dispose();
				this._defaultGroup = null;
			}
		}
		#endregion
	}
}

