//
//  TileBlock.cs
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
	/// This is the basic "environment object" class, used to create simple walls
	/// and floors. It can be filled with a random selection of tiles to quickly
	/// add detail.
	/// </summary>
	public class TileBlock : FlixelSprite
	{
		#region Fields
		private Rectangle[] _rects = { };
		private Int32 _tileWidth = 0;
		private Int32 _tileHeight = 0;
		private Int32 _empties = 0;
		private Vector2 _p = Vector2.Zero;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.TileBlock"/>
		/// class with the dimensions of the tile.
		/// </summary>
		/// <param name="x">
		/// The X position of the block.
		/// </param>
		/// <param name="y">
		/// The Y position of the block.
		/// </param>
		/// <param name="width">
		/// The width of the block.
		/// </param>
		/// <param name="height">
		/// The height of the block.
		/// </param>
		public TileBlock(Int32 x, Int32 y, Int32 width, Int32 height)
			: base(x, y) {
			base.CreateGraphic(width, height, Color.Black);
			base.Fixed = true;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Regenerates the rectangles.
		/// </summary>
		private void RegenRectangles() {
			Int32 widthInTiles = ((Int32)base.Width / this._tileWidth);
			Int32 heightInTiles = ((Int32)base.Height / this._tileHeight);
			base.Width = (widthInTiles * this._tileWidth);
			base.Height = (heightInTiles * this._tileHeight);
			Int32 tileCount = (widthInTiles * heightInTiles);
			Int32 numGraphics = (base.Texture.Width / this._tileWidth);

			this._rects = new Rectangle[tileCount];
			for (Int32 i = 0; i < tileCount; i++) {
				if ((GameMath.Random() * (numGraphics + this._empties)) > this._empties) {
					this._rects[i] = new Rectangle(this._tileWidth * (Int32)(GameMath.Random() * numGraphics),
					                               0, this._tileWidth, this._tileHeight);
				}
				else {
					this._rects[i] = Rectangle.Empty;
				}
			}
		}

		/// <summary>
		/// Fills the block with a randomly arranged selection of graphics from
		/// the image provided.
		/// </summary>
		/// <returns>
		/// Returns this instance with the tiles loaded.
		/// </returns>
		/// <param name="tileGraphic">
		/// The graphic that contains the tiles that should fill this block.
		/// </param>
		/// <param name="width">
		/// The width of the block.
		/// </param>
		/// <param name="height">
		/// The height of the block.
		/// </param>
		/// <param name="empties">
		/// The number of empty tiles to add to the auto-fill algorithm (eg.
		/// 8 tiles + 4 empties = 1/3 of a block will be open holes).
		/// </param>
		public TileBlock LoadTiles(Texture2D tileGraphic, Int32 width, Int32 height, Int32 empties) {
			if (tileGraphic == null) {
				return this;
			}

			if (width == 0) {
				width = tileGraphic.Width;
			}

			if (height == 0) {
				height = tileGraphic.Height;
			}

			base.Texture = tileGraphic;
			this._tileWidth = width;
			this._tileHeight = height;
			this._empties = empties;
			this.RegenRectangles();
			base.CanMove = false;
			return this;
		}

		/// <summary>
		/// Fills the block with a randomly arranged selection of graphics from
		/// the image provided.
		/// </summary>
		/// <returns>
		/// Returns this instance with the tiles loaded.
		/// </returns>
		/// <param name="tileGraphic">
		/// The graphic that contains the tiles that should fill this block.
		/// </param>
		public TileBlock LoadTiles(Texture2D tileGraphic) {
			return this.LoadTiles(tileGraphic, 0, 0, 0);
		}

		/// <summary>
		/// Fills the block with a randomly arranged selection of graphics from
		/// the image provided.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is useful for chaining stuff together.
		/// </returns>
		/// <param name="graphic">
		/// The image that you want to use.
		/// </param>
		/// <remarks>
		/// NOTE: In this case, this method is functionally no different than
		/// <see cref="LoadTiles"/>.
		/// </remarks>			
		public override FlixelSprite LoadGraphic(Texture2D graphic) {
			return this.LoadTiles(graphic);
		}

		/// <summary>
		/// Draws this block.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public override void Render(SpriteBatch spriteBatch) {
			if ((base.Texture == null) || (spriteBatch == null)) {
				return;
			}

			this._p = base.GetScreenXY();
			Int32 opx = (Int32)this._p.X;
			for (Int32 i = 0; i < this._rects.Length; i++) {
				if (this._rects[i] != Rectangle.Empty) {
					spriteBatch.Draw(base.Texture, this._p, this._rects[i], Color.White);
				}

				this._p.X += this._tileWidth;
				if (this._p.X >= (opx + base.Width)) {
					this._p.X = opx;
					this._p.Y += this._tileHeight;
				}
			}
		}
		#endregion
	}
}

