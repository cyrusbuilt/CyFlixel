//
//  LogoPixel.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics
{
	/// <summary>
	/// Logo pixel class.
	/// </summary>
	public class LogoPixel : FlixelSprite
	{
		#region Fields
		private List<Color> _layers = null;
		private Int32 _currentLayer = 0;
		private Rectangle _recSrc = Rectangle.Empty;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.LogoPixel"/>
		/// class. This is the default constructor.
		/// </summary>
		/// <param name="xpos">
		/// The initial X position of the sprite.
		/// </param>
		/// <param name="ypos">
		/// The initial Y position of the sprite.
		/// </param>
		/// <param name="pixelSize">
		/// The size of the pixels.
		/// </param>
		/// <param name="index">
		/// The index of the color layer.
		/// </param>
		/// <param name="finalColor">
		/// The final color to use.
		/// </param>
		public LogoPixel(Int32 xpos, Int32 ypos, Int32 pixelSize, Int32 index, Color finalColor)
			: base(xpos, ypos) {
			base.CreateGraphic(pixelSize, pixelSize, Color.White);
			Color[] colors = new Color[] {
				new Color(255, 0, 0),
				new Color(0, 255, 0),
				new Color(0, 0, 255),
				new Color(255, 255, 0),
				new Color(0, 255, 255)
			};

			this._layers = new List<Color>();
			this._layers.Add(finalColor);
			foreach (Color c in colors) {
				this._layers.Add(c);
			}
			this._currentLayer = (this._layers.Count - 0);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Called by the main game loop, handles motion/physics and game logic.
		/// </summary>
		public override void Update() {
			if (this._currentLayer == 0) {
				return;
			}

			Color current = this._layers[this._currentLayer];
			if (current.A >= 25) {
				current = new Color(current.R, current.G, current.B, (current.A - 25));
			}
			else {
				current = new Color(current.R, current.G, current.B, 0);
				this._currentLayer--;
			}
		}

		/// <summary>
		/// Renders this graphic.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public override void Render(SpriteBatch spriteBatch) {
			Rectangle r = Rectangle.Empty;
			for (Int32 i = 0; i <= this._currentLayer; i++) {
				r = new Rectangle((Int32)base.X, (Int32)base.Y, (Int32)base.Width, (Int32)base.Height);
				spriteBatch.Draw(base.Texture, r, this._recSrc, this._layers[i]);
			}
		}
		#endregion
	}
}

