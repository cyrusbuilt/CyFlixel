//
//  TileMap.cs
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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics
{
	/// <summary>
	/// This is a traditional tilemap display and collision class.
	/// It takes a string of comma-separated numbers and then associates
	/// those values with tiles from the sheet you pass in.
	/// It also includes some handy static parsers that can convert
	/// arrays or PNG files into strings that can be successfully loaded.
	/// </summary>
	public class TileMap : CyFlixelObject
	{
		#region Instance Fields
		private Int32 _collideIndex = 0;
		private Int32 _startingIndex = 0;
		private Int32 _drawIndex = 0;
		private TileMode _auto = TileMode.Off;
		private Int32 _widthInTiles = 0;
		private Int32 _heightInTiles = 0;
		private Int32 _totalTiles = 0;
		private Boolean _refresh = false;
		private Rectangle _flashRect = Rectangle.Empty;
		private Rectangle _flashRect2 = Rectangle.Empty;
		private Int32[] _data = { };
		private List<Rectangle> _rects = null;
		private Int32 _tileWidth = 0;
		private Int32 _tileHeight = 0;
		private CyFlixelObject _block = null;
		private Int32 _screenRows = 0;
		private Int32 _screenCols = 0;
		private Texture2D _tileBitmap = null;
		#endregion

		#region Static Fields
		private static Texture2D _imgAuto = null;
		private static Texture2D _imgAutoAlt = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.TileMap"/>
		/// class. This is the default constructor.
		/// </summary>
		public TileMap()
			: base() {
			if ((_imgAuto == null) || (_imgAutoAlt == null)) {
				_imgAuto = EngineGlobal.Content.Load<Texture2D>("cyflixel/autotiles");
				_imgAutoAlt = EngineGlobal.Content.Load<Texture2D>("cyflixel/autotiles_alt");
			}

			this._auto = TileMode.Off;
			this._collideIndex = 1;
			this._startingIndex = 0;
			this._drawIndex = 1;
			this._widthInTiles = 0;
			this._heightInTiles = 0;
			this._totalTiles = 0;
			this._flashRect2 = new Rectangle();
			this._flashRect = this._flashRect2;
			this._data = null;
			this._tileWidth = 0;
			this._tileHeight = 0;
			this._rects = null;
			this._block = new CyFlixelObject();
			this._block.Width = this._block.Height = 0;
			this._block.Fixed = true;
			base.Fixed = true;
			base.CanMove = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the tile index to start colliding with (default is 1).
		/// </summary>
		public Int32 CollideIndex {
			get { return this._collideIndex; }
			set { this._collideIndex = value; }
		}

		/// <summary>
		/// Gets or sets the first index of your tile sheet (default: 0). If you
		/// want to change it, do so before calling <see cref="LoadMap"/>.
		/// </summary>
		public Int32 StartingIndex {
			get { return this._startingIndex; }
			set { this._startingIndex = value; }
		}

		/// <summary>
		/// Gets or sets the tile index to start drawing with (default: 1).
		/// NOTE: This value should always be >= to <see cref="StartingIndex"/>.
		/// If you choose to change this value, do so before calling <see cref="LoadMap"/>.
		/// </summary>
		public Int32 DrawIndex {
			get { return this._drawIndex; }
			set { this._drawIndex = value; }
		}

		/// <summary>
		/// Gets or sets the auto tile mode.
		/// </summary>
		public TileMode AutoTileMode {
			get { return this._auto; }
			set { this._auto = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not to force the tilemap
		/// buffer to refresh on the next render frame.
		/// </summary>
		public Boolean Refresh {
			get { return this._refresh; }
			set { this._refresh = value; }
		}

		/// <summary>
		/// Gets the width in tiles.
		/// </summary>
		public Int32 WidthInTiles {
			get { return this._widthInTiles; }
		}

		/// <summary>
		/// Gets the height in tiles.
		/// </summary>
		public Int32 HeightInTiles {
			get { return this._heightInTiles; }
		}

		/// <summary>
		/// Gets the total tiles.
		/// </summary>
		public Int32 TotalTiles {
			get { return this._totalTiles; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Binary auto-tiler method.
		/// </summary>
		/// <param name="index">
		/// The index of the tile you want to analyze.
		/// </param>
		private void AutoTile(Int32 index) {
			if (this._data[index] == 0) {
				return;
			}

			this._data[index] = 0;
			if (((index - this._widthInTiles) < 0) || (this._data[index - this._widthInTiles] > 0)) {
				this._data[index] += 1;  // Up
			}

			if (((index % this._widthInTiles) >= (this._widthInTiles - 1)) || (this._data[index + 1] > 0)) {
				this._data[index] += 2; // Right
			}

			if (((index + this._widthInTiles) >= this._totalTiles) || (this._data[index + this._widthInTiles] > 0)) {
				this._data[index] += 4; // Down.
			}

			if (((index % this._widthInTiles) <= 0) || (this._data[index - 1] > 0)) {
				this._data[index] += 8; // Left.
			}

			// Alternate algorithm checks for interior corners.
			if ((this._auto == TileMode.Alt) && (this._data[index] == 15)) {
				if (((index % this._widthInTiles) > 0) &&
				    ((index + this._widthInTiles) < this._totalTiles) &&
				    (this._data[index + this._widthInTiles - 1] <= 0)) {
					this._data[index] = 1; // Bottom left open.
				}

				if (((index % this._widthInTiles) > 0) &&
				    ((index - this._widthInTiles) >= 0) &&
				    (this._data[index + this._widthInTiles - 1] <= 0)) {
					this._data[index] = 2; // Top left open.
				}

				if (((index % this._widthInTiles) < (this._widthInTiles - 1)) &&
				    ((index - this._widthInTiles) >= 0) &&
				    (this._data[index - this._widthInTiles + 1] <= 0)) {
					this._data[index] = 4; // Top right open.
				}

				if (((index % this._widthInTiles) < (this._widthInTiles - 1)) &&
				    ((index + this._widthInTiles) < this._totalTiles) &&
				    (this._data[index + this._widthInTiles + 1] <= 0)) {
					this._data[index] = 8; // Bottom right open.
				}
			}
			this._data[index] += 1;
		}

		/// <summary>
		/// Udates the tile map.
		/// </summary>
		/// <param name="index">
		/// The index of the tile you want to update.
		/// </param>
		private void UpdateTile(Int32 index) {
			if (this._data[index] < this._drawIndex) {
				this._rects[index] = Rectangle.Empty;
				return;
			}

			Int32 rx = ((this._data[index] - this._startingIndex) * this._tileWidth);
			Int32 ry = 0;
			if (rx >= this._tileBitmap.Width) {
				rx %= this._tileBitmap.Width;
			}

			this._rects[index] = new Rectangle(rx, ry, this._tileWidth, this._tileHeight);
		}

		/// <summary>
		/// Generates a bounding box version of the tiles. Should be called
		/// automatically when necessary.
		/// </summary>
		protected void GenerateBoundingTiles() {
			// TODO Figure out what to do here. Original source just has an empty method.
			// This may be a method that was meant to be overridden.
		}

		/// <summary>
		/// Rebuild basic collision data for this object.
		/// </summary>
		public override void RefreshHulls() {
			base.CollisionHullX.X = 0;
			base.CollisionHullX.Y = 0;
			base.CollisionHullX.Width = this._tileWidth;
			base.CollisionHullX.Height = this._tileHeight;

			base.CollisionHullY.X = 0;
			base.CollisionHullY.Y = 0;
			base.CollisionHullY.Width = this._tileWidth;
			base.CollisionHullY.Height = this._tileHeight;
		}

		/// <summary>
		/// Loads the tile map with string data and a tile graphic.
		/// </summary>
		/// <returns>
		/// A pointer to this instance of tile map.
		/// </returns>
		/// <param name="mapData">
		/// A string of comma and line-return delineated indices indicating what
		/// order the tiles should go in.
		/// </param>
		/// <param name="tileGraphic">
		/// All the tiles you want to use, arranged in a strip corresponding to
		/// the numbers in <paramref name="mapData"/>.
		/// </param>
		/// <param name="tileWidth">
		/// The width of your tiles (e.g. 8) - defaults to height of the tile
		/// graphic if unspecified.
		/// </param>
		/// <param name="tileHeight">
		/// The height of your tiles (e.g. 8) - defaults to width if unspecified.
		/// </param>
		public TileMap LoadMap(String mapData, Texture2D tileGraphic, Int32 tileWidth, Int32 tileHeight) {
			this._refresh = true;
			this._tileBitmap = tileGraphic;
			String[] rows = mapData.Split('\n');
			this._heightInTiles = rows.Length;
			Int32 r = 0;
			Int32 c = 0;
			Int32 temp = 0;

			String[] cols = rows[r].Split(',');
			this._data = new Int32[rows.Length * cols.Length];
			while (r < this._heightInTiles) {
				cols = rows[r++].Split(',');
				if (cols.Length <= 1) {
					this._heightInTiles -= 1;
					continue;
				}

				if (this._widthInTiles == 0) {
					this._widthInTiles = cols.Length;
				}

				c = 0;
				while (c < this._widthInTiles) {
					if (Int32.TryParse(cols[c++], out temp)) {
						this._data[((r - 1) * this._widthInTiles) + c] = temp;
					}
				}
			}

			// Pre-process the map data if it's auto-tiled.
			Int32 i = 0;
			this._totalTiles = (this._widthInTiles * this._heightInTiles);
			if (this._auto == TileMode.Off) {
				this._collideIndex = this._startingIndex = this._drawIndex = 1;
				i = 0;
				while (i < this._totalTiles) {
					this.AutoTile(i++);
				}
			}

			// Figure out the size of the tiles.
			this._tileWidth = tileWidth;
			if (this._tileWidth == 0) {
				this._tileWidth = tileGraphic.Height;
			}

			this._tileHeight = tileHeight;
			if (this._tileHeight == 0) {
				this._tileHeight = this._tileWidth;
			}

			this._block.Width = this._tileWidth;
			this._block.Height = this._tileHeight;
			this._rects = new List<Rectangle>();
			i = 0;
			while (i < this._totalTiles) {
				this._rects.Add(Rectangle.Empty);
				this.UpdateTile(i++);
			}

			// Pre-set some helper variables for later.
			this._screenRows = (Int32)GameMath.Ceil(((float)EngineGlobal.Height / (float)this._tileHeight) + 1);
			if (this._screenRows > this._heightInTiles) {
				this._screenRows = this._heightInTiles;
			}

			this._screenCols = (Int32)GameMath.Ceil(((float)EngineGlobal.Width / (float)this._tileWidth) + 1);
			if (this._screenCols > this._widthInTiles) {
				this._screenCols = this._widthInTiles;
			}

			this.GenerateBoundingTiles();
			this.RefreshHulls();

			this._flashRect.X = 0;
			this._flashRect.Y = 0;
			this._flashRect.Width = (Int32)(GameMath.Ceil(((float)EngineGlobal.Width / (float)this._tileWidth) + 1) * this._tileWidth);
			this._flashRect.Height = (Int32)(GameMath.Ceil(((float)EngineGlobal.Height / (float)this._tileHeight) + 1) * this._tileHeight);
			return this;
		}

		/// <summary>
		/// Loads the tile map with string data and a tile graphic.
		/// </summary>
		/// <returns>
		/// A pointer to this instance of tile map.
		/// </returns>
		/// <param name="mapData">
		/// A string of comma and line-return delineated indices indicating what
		/// order the tiles should go in.
		/// </param>
		/// <param name="tileGraphic">
		/// All the tiles you want to use, arranged in a strip corresponding to
		/// the numbers in <paramref name="mapData"/>.
		/// </param>
		public TileMap LoadMap(String mapData, Texture2D tileGraphic) {
			return this.LoadMap(mapData, tileGraphic, 0, 0);
		}

		/// <summary>
		/// Draws the tilemap.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="spriteBatch"/> cannot be null.
		/// </exception>
		public override void Render(SpriteBatch spriteBatch) {
			if (spriteBatch == null) {
				throw new ArgumentNullException("spriteBatch");
			}

			// NOTE: While this will only draw the tiles that are actually on
			// screen, it will ALWAYS draw one screen's worth of tiles.
			this._point = base.GetScreenXY();
			Int32 bx = (Int32)Math.Floor(-this._point.X / this._tileWidth);
			Int32 by = (Int32)Math.Floor(-this._point.Y / this._tileHeight);
			Int32 ex = (Int32)Math.Floor((-this._point.X + EngineGlobal.Width - 1) / this._tileWidth);
			Int32 ey = (Int32)Math.Floor((-this._point.Y + EngineGlobal.Height - 1) / this._tileHeight);

			if (bx < 0) {
				bx = 0;
			}

			if (by < 0) {
				by = 0;
			}

			if (ex >= this._widthInTiles) {
				ex = (this._widthInTiles - 1);
			}

			if (ey >= this._heightInTiles) {
				ey = (this._heightInTiles - 1);
			}

			Rectangle rct = Rectangle.Empty;
			Int32 ri = (by * this._widthInTiles + bx);
			Int32 cri = 0;
			for (Int32 iy = by; iy <= ey; iy++) {
				cri = ri;
				for (Int32 ix = bx; ix <= ex; ix++) {
					if (this._rects[cri] != Rectangle.Empty) {
						rct = new Rectangle(((ix + this._tileWidth) + (Int32)Math.Floor(EngineGlobal.Scroll.X * base.ScrollFactor.X)),
						                    ((iy * this._tileHeight) + (Int32)Math.Floor(EngineGlobal.Scroll.Y * base.ScrollFactor.Y)),
						                    this._tileWidth, this._tileHeight);
						spriteBatch.Draw(this._tileBitmap, rct, this._rects[iy * this._widthInTiles + ix], Color.White);
					}
					cri++;
				}
				ri += this._widthInTiles;
			}
		}

		/// <summary>
		/// Checks for overlaps between the provided object and any tiles above
		/// the collision index.
		/// </summary>
		/// <param name="obj">
		/// The object being tested.
		/// </param>
		/// <returns>
		/// true if the objects overlap; Otherwise, false.
		/// </returns>
		public override Boolean Overlaps(CyFlixelObject obj) {
			Int32 d = 0;
			Int32 dd = 0;
			List<BlockPoint> blocks = new List<BlockPoint>();

			// First make a list of all the blocks we'll use for collision.
			Int32 ix = (Int32)Math.Floor((obj.X - base.X) / this._tileWidth);
			Int32 iy = (Int32)Math.Floor((obj.Y - base.Y) / this._tileHeight);
			Int32 iw = (Int32)GameMath.Ceil(((float)obj.Width / (float)this._tileWidth) + 1);
			Int32 ih = (Int32)GameMath.Ceil(((float)obj.Height / (float)this._tileHeight) + 1);
			Int32 r = 0;
			Int32 c = 0;

			Int32 w = 0;
			Int32 h = 0;
			BlockPoint bp;
			while (r < ih) {
				if (r >= this._heightInTiles) {
					break;
				}

				d = ((iy + r) * this._widthInTiles + ix);
				c = 0;
				while (c < iw) {
					if (c >= this._widthInTiles) {
						break;
					}

					dd = this._data[d + c];
					if (dd >= this._collideIndex) {
						w = (Int32)(base.X + (ix + c) * this._tileWidth);
						h = (Int32)(base.Y + (iy + r) * this._tileHeight);
						bp = new BlockPoint(w, h, dd);
						blocks.Add(bp);
					}
					c++;
				}
				r++;
			}

			// Then check for overlaps.
			Boolean olaps = false;
			Int32 bl = blocks.Count;
			Int32 i = 0;
			while (i < bl) {
				this._block.X = blocks[i].X;
				this._block.Y = blocks[i++].Y;
				if (this._block.Overlaps(obj)) {
					olaps = true;
					break;
				}
			}
			return olaps;
		}

		/// <summary>
		/// Gets the value of a tile in the tilemap by index.
		/// </summary>
		/// <returns>
		/// If successful, the value of the requested tile; Otherwise, -1;
		/// </returns>
		/// <param name="index">
		/// The slot in the data array (Y * _widthInTiles + X) where this tile
		/// is stored.
		/// </param>
		public Int32 GetTileByIndex(Int32 index) {
			if (this._data != null) {
				if (this._data.Length > 0) {
					return this._data[index];
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the value of a particular tile.
		/// </summary>
		/// <returns>
		/// If successful, the value of the requested tile; Otherwise, -1;
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the tile (in tiles, not pixels).
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the tile (in tiles, not pixels).
		/// </param>
		public Int32 GetTile(Int32 x, Int32 y) {
			return this.GetTileByIndex(y * this._widthInTiles + x);
		}

		/// <summary>
		/// Checks to see if a 2D point in space overlaps a solid tile.
		/// </summary>
		/// <returns>
		/// true, if point overlaps, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the point.
		/// </param>
		/// <param name="y">
		/// The y coordinate of the point.
		/// </param>
		/// <param name="perPixel">
		/// Ignored in this overridden method.
		/// </param>
		public override Boolean OverlapsPoint(float x, float y, bool perPixel) {
			Int32 opx = (Int32)((x - base.X) / this._tileWidth);
			Int32 opy = (Int32)((y - base.Y) / this._tileHeight);
			return (this.GetTile(opx, opy) >= this._collideIndex);
		}

		/// <summary>
		/// Checks to see if a 2D point in space overlaps a solid tile.
		/// </summary>
		/// <returns>
		/// true, if point overlaps, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the point.
		/// </param>
		/// <param name="y">
		/// The y coordinate of the point.
		/// </param>
		public override Boolean OverlapsPoint(float x, float y) {
			return this.OverlapsPoint(x, y, false);
		}

		/// <summary>
		/// This method will be called each time two objects are compared to
		/// see if they collide. It does not necessarily mean these objects
		/// *WILL* collide, however.
		/// </summary>
		/// <param name="obj">
		/// The object you're about to run into.
		/// </param>
		public override void PreCollide(CyFlixelObject obj) {
			if (obj == null) {
				// TODO Should we throw an exception here instead?
				return;
			}

			base.CollisionHullX.X = 0;
			base.CollisionHullX.Y = 0;
			base.CollisionHullY.X = 0;
			base.CollisionHullY.Y = 0;

			Int32 r = 0;
			Int32 c = 0;
			Int32 rs = 0;
			Int32 ix = (Int32)GameMath.Floor((obj.X - base.X) / this._tileWidth);
			Int32 iy = (Int32)GameMath.Floor((obj.Y - base.Y) / this._tileHeight);
			Int32 iw = (ix + (Int32)GameMath.Ceil((float)obj.Width / (float)this._tileWidth) + 1);
			Int32 ih = (iy + (Int32)GameMath.Ceil((float)obj.Height / (float)this._tileHeight) + 1);

			if (ix < 0) {
				ix = 0;
			}

			if (iy < 0) {
				iy = 0;
			}

			if (iw > this._widthInTiles) {
				iw = this._widthInTiles;
			}

			if (ih > this._heightInTiles) {
				ih = this._heightInTiles;
			}

			Vector2 v = Vector2.Zero;
			rs = (iy * this._widthInTiles);
			r = iy;
			base.CollisionOffsets.Clear();
			while (r < ih) {
				c = ix;
				while (c < iw) {
					if (_data[rs + c] >= this._collideIndex) {
						v = new Vector2((base.X + c * this._tileWidth), (base.Y + r * this._tileHeight));
						base.CollisionOffsets.Add(v);
					}
					c++;
				}
				rs += this._widthInTiles;
				r++;
			}
		}

		/// <summary>
		/// Change the data and graphic of a tile in the tilemap.
		/// </summary>
		/// <returns>
		/// true, if the tile was actually changed, false otherwise.
		/// </returns>
		/// <param name="index">
		/// The slot in the data array where this tile is stored.
		/// </param>
		/// <param name="tile">
		/// The new integer data you want to inject.
		/// </param>
		/// <param name="updateGraphics">
		/// If set true if the graphical representation of this tile should change.
		/// </param>
		public Boolean SetTileByIndex(Int32 index, Int32 tile, Boolean updateGraphics) {
			if (this._data == null) {
				return false;
			}

			if (index >= this._data.Length) {
				return false;
			}

			Boolean ok = true;
			this._data[index] = tile;
			if (!updateGraphics) {
				return ok;
			}

			if (this._auto == TileMode.Off) {
				this.UpdateTile(index);
				return ok;
			}

			// If this map is auto-tiled and it changes, locally update the arrangement.
			Int32 i = 0;
			Int32 r = ((Int32)(index / this._widthInTiles) - 1);
			Int32 rl = (r + 3);
			Int32 c = (index % this._widthInTiles - 1);
			Int32 cl = (c + 3);
			while (r < rl) {
				c = (cl - 3);
				while (c < cl) {
					if ((r >= 0) && (r < this._heightInTiles) &&
					    (c >= 0) && (c < this._widthInTiles)) {
						i = (r * this._widthInTiles + c);
						this.AutoTile(i);
						this.UpdateTile(i);
					}
					c++;
				}
				r++;
			}
			return ok;
		}

		/// <summary>
		/// Change the data and graphic of a tile in the tilemap.
		/// </summary>
		/// <returns>
		/// true, if the tile was actually changed, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the tile (in tiles, not pixels).
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the tile (in tiles, not pixels).
		/// </param>
		/// <param name="tile">
		/// The new integer data you want to inject.
		/// </param>
		/// <param name="updateGraphics">
		/// Set true if the graphical representation of this tile should change.
		/// </param>
		public Boolean SetTile(Int32 x, Int32 y, Int32 tile, Boolean updateGraphics) {
			if ((x >= this._widthInTiles) || (y >= this._heightInTiles)) {
				return false;
			}
			return this.SetTileByIndex((y * this._widthInTiles + x), tile, updateGraphics);
		}

		/// <summary>
		/// Change the data and graphic of a tile in the tilemap. This overload
		/// assumes the graphic will be updated.
		/// </summary>
		/// <returns>
		/// true, if the tile was actually changed, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the tile (in tiles, not pixels).
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the tile (in tiles, not pixels).
		/// </param>
		/// <param name="tile">
		/// The new integer data you want to inject.
		/// </param>
		public Boolean SetTile(Int32 x, Int32 y, Int32 tile) {
			if ((x >= this._widthInTiles) || (y >= this._heightInTiles)) {
				return false;
			}
			return this.SetTileByIndex((y * this._widthInTiles + x), tile, true);
		}

		/// <summary>
		/// Locks the automatic camera to the map's edges.
		/// </summary>
		/// <param name="border">
		/// Adjusts the camera follow boundary by whatever number of tiles you
		/// specify here. Handy for blocking off dead-ends that are off-screen,
		/// etc. Use a negative number to add padding instead of hiding edges.
		/// </param>
		public void Follow(Int32 border) {
			Int32 minx = (Int32)base.X + border * this._tileWidth;
			Int32 miny = (Int32)base.Y + border * this._tileHeight;
			Int32 maxx = (Int32)base.Width - border * this._tileWidth;
			Int32 maxy = (Int32)base.Height - border * this._tileHeight;
			EngineGlobal.FollowBounds(minx, miny, maxx, maxy);
		}

		/// <summary>
		/// Locks the automatic camera to the map's edges.
		/// </summary>
		public void Follow() {
			this.Follow(0);
		}

		/// <summary>
		/// Shoots a ray from the start point to the end point. If and when it
		/// passes through a tile, it stores and returns that point.
		/// </summary>
		/// <param name="startX">
		/// The X coordinate of the ray's start.
		/// </param>
		/// <param name="startY">
		/// The Y coordinate of the ray's start.
		/// </param>
		/// <param name="endX">
		/// The X coordinate of the ray's end.
		/// </param>
		/// <param name="endY">
		/// The Y coordinate of the ray's end.
		/// </param>
		/// <param name="result">
		/// The resulting vector containing the first wall impact. This must
		/// be declared and initialized prior to use.
		/// </param>
		/// <param name="resolution">
		/// Defaults to 1, meaning check every tile or so. Higher means more checks!
		/// </param>
		/// <returns>
		/// true if there was a collision between the ray and a colliding tile.
		/// </returns>			
		public Boolean Ray(Int32 startX, Int32 startY, Int32 endX, Int32 endY, out Vector2 result, Int32 resolution) {
			// TODO Should combine startX and startY into a Point.
			// TODO Should combin endX and endY into a Point.
			Int32 step = this._tileWidth;
			if (this._tileHeight < this._tileWidth) {
				step = this._tileHeight;
			}

			step /= resolution;
			Int32 dx = (endX - startX);
			Int32 dy = (endY - startY);
			float distance = (float)Math.Sqrt(dx * dx + dy * dy);
			Int32 steps = (Int32)GameMath.Ceil(distance / step);
			Int32 stepX = (dx / steps);
			Int32 stepY = (dy / steps);
			Int32 curX = (startX - stepX);
			Int32 curY = (startY - stepY);
			Int32 tx = 0;
			Int32 ty = 0;
			Int32 i = 0;
			Int32 rx = 0;
			Int32 ry = 0;
			Int32 q = 0;
			Int32 lx = 0;
			Int32 ly = 0;

			Boolean collision = false;
			while (i < steps) {
				curX += stepX;
				curY += stepY;
				if ((curX < 0) || (curX > base.Width) || (curY < 0) || (curY > base.Height)) {
					i++;
					continue;
				}

				tx = (curX / this._tileWidth);
				ty = (curY / this._tileHeight);
				if (this._data[ty * this._widthInTiles + tx] >= this._collideIndex) {
					// Some basic helper stuff.
					tx *= this._tileWidth;
					ty *= this._tileHeight;
					rx = 0;
					ry = 0;
					lx = (curX - stepX);
					ly = (curY - stepY);

					// Figure out if it crosses the Y boundary.
					q = ty;
					if (dy < 0) {
						q += this._tileHeight;
					}

					rx = (lx + stepX * ((q - ly) / stepY));
					ry = q;
					if ((rx > tx) && (rx < tx + this._tileWidth)) {
						result.X = rx;
						result.Y = ry;
						collision = true;
					}
					break;
				}
				i++;
			}
			return collision;
		}

		/// <summary>
		/// Converts a one-dimensional array of tile data to a comma-separated
		/// string.
		/// </summary>
		/// <returns>
		/// A comman-separated string containing the level data in a
		/// <see cref="TileMap"/>-friendly format.
		/// </returns>
		/// <param name="data">
		/// An array full of integer tile references.
		/// </param>
		/// <param name="width">
		/// The number of tiles in each row.
		/// </param>
		public static String ArrayToCSV(Int32[]data, Int32 width) {
			if ((data == null) || (data.Length == 0)) {
				return String.Empty;
			}

			Int32 r = 0;
			Int32 c = 0;
			String csv = String.Empty;
			Int32 height = (data.Length / width);
			while (r < height) {
				c = 0;
				while (c < width) {
					if (c == 0) {
						if (r == 0) {
							csv += data[0];
						}
						else {
							csv += "\n" + data[r * width];
						}
					}
					else {
						csv += ", " + data[r * width + c];
					}
					c++;
				}
				r++;
			}
			return csv;
		}

		/// <summary>
		/// Converts bitmap data to a comma-separated string. Black pixels are
		/// flagged as 'solid' by default, non-black pixels are set as non-colliding.
		/// Black pixels must be PURE black.
		/// </summary>
		/// <returns>
		/// A comma-separated string containing the level data in a
		/// <see cref="TileMap"/>-friendly format.
		/// </returns>
		/// <param name="bitmapData">
		/// A <see cref="Texture2D"/>, preferrably black and white.
		/// </param>
		/// <param name="invert">
		/// If set to true, loads white pixels as solid instead.
		/// </param>
		public static String BitmapToCSV(Texture2D bitmapData, Boolean invert) {
			if (bitmapData == null) {
				return String.Empty;
			}

			// Walk image and export pixel values.
			Int32 r = 0;
			Int32 c = 0;
			uint p = 0;
			String csv = String.Empty;
			Int32 w = bitmapData.Width;
			Int32 h = bitmapData.Height;
			uint[]bitData = new uint[1];

			bitmapData.GetData<uint>(bitData);
			while (r < h) {
				c = 0;
				while (c < w) {
					// Decide if this pixel/tile is solid (1) or not (0);
					p = bitData[(r * w) + c];
					if (((invert) && (p > 0)) || ((!invert) && (p == 0))) {
						p = 1;
					}
					else {
						p = 0;
					}

					// Write result to string.
					if (c == 0) {
						if (r == 0) {
							csv += p;
						}
						else {
							csv += "\n" + p;
						}
					}
					else {
						csv += ", " + p;
					}
					c++;
				}
				r++;
			}
			return csv;
		}

		/// <summary>
		/// Converts bitmap data to a comma-separated string. Black pixels are
		/// flagged as 'solid' by default, non-black pixels are set as non-colliding.
		/// Black pixels must be PURE black.
		/// </summary>
		/// <returns>
		/// A comma-separated string containing the level data in a
		/// <see cref="TileMap"/>-friendly format.
		/// </returns>
		/// <param name="bitmapData">
		/// A <see cref="Texture2D"/>, preferrably black and white.
		/// </param>
		public static String BitmapToCSV(Texture2D bitmapData) {
			return BitmapToCSV(bitmapData, false);
		}
		#endregion
	}
}

