//
//  Game1.cs
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
#region File Description
//-----------------------------------------------------------------------------
// CyrusBuilt.CyFlixel.CyFlixelGameGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using CyrusBuilt.CyFlixel.CyFlixelEngine;

#endregion

namespace CyrusBuilt.CyFlixel.CyFlixelGame
{
	/// <summary>
	/// Default Project Template
	/// </summary>
	public class Game1 : Game
	{

	#region Fields
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D logoTexture;
	#endregion

	#region Initialization

		public Game1() {

			graphics = new GraphicsDeviceManager(this);
			
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = false;

		}

		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize() {
			base.Initialize();
		}


		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
			
			// TODO: use this.Content to load your game content here eg.
			logoTexture = Content.Load<Texture2D>("logo");
		}

	#endregion

	#region Update and Draw

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			// TODO: Add your update logic here			
            		
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			// draw the logo
			spriteBatch.Draw(logoTexture, new Vector2(130, 200), Color.White);

			spriteBatch.End();

			//TODO: Add your drawing code here
			base.Draw(gameTime);
		}

	#endregion
	}
}
