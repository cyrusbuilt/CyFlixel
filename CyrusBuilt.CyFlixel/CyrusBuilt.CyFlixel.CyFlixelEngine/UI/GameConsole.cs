//
//  GameConsole.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.UI
{
	/// <summary>
	/// In-game developer/debug console.
	/// </summary>
	public sealed class GameConsole : IDisposable
	{
		#region Fields
		private Int32 _maxConsoleLines = 256;
		private Boolean _visible = false;
		private Rectangle _srcRect = Rectangle.Empty;
		private Rectangle _consoleRect = Rectangle.Empty;
		private Color _consoleColor = Color.Black;  // TODO Not sure about this. Need to test.
		private TextSprite _consoleText = null;
		private TextSprite _consoleFPS = null;
		private Int32[] _fps = { };
		private Int32 _currentFPS = 0;
		private List<String> _consoleLines = null;
		private float _consoleY = 0f;
		private float _consoleYT = 0f;
		private Boolean _fpsUpdate = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/>
		/// class with the top-left starting coordinate and width of the console
		/// window.
		/// </summary>
		/// <param name="targetLeft">
		/// The top-left starting position of the console window.
		/// </param>
		/// <param name="targetWidth">
		/// The width of the console window.
		/// </param>
		public GameConsole(Int32 targetLeft, Int32 targetWidth) {
			this._fps = new Int32[8];
			Int32 screenw = EngineGlobal.SpriteBatch.GraphicsDevice.Viewport.Width;
			Int32 screenh = EngineGlobal.SpriteBatch.GraphicsDevice.Viewport.Height;
			this._consoleRect = new Rectangle(0, 0, screenw, screenh);
			this._consoleColor = new Color(0, 0, 0, 0x7F);

			this._consoleText = new TextSprite(targetLeft, -800, targetWidth, String.Empty);
			this._consoleText.SetFormat(null, 1, Color.White, TextJustification.Left, Color.White);
			this._consoleText.Height = EngineGlobal.Height;

			float x = (targetLeft + targetWidth - 30);
			this._consoleFPS = new TextSprite(x, -800, 30, String.Empty);
			this._consoleFPS.SetFormat(null, 1, Color.White, TextJustification.Right, Color.White);

			this._srcRect = new Rectangle(1, 1, 1, 1);
			this._consoleLines = new List<String>();
			this._maxConsoleLines = (screenh / (Int32)this._consoleText.Font.MeasureString("Qq").Y) - 1;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the color of the console.
		/// </summary>
		public Color ConsoleColor {
			get { return this._consoleColor; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/>
		/// is visible.
		/// </summary>
		public Boolean Visible {
			get { return this._visible; }
			set { this._visible = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/>
		/// object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.GameConsole"/> was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._consoleText != null) {
				this._consoleText.Dispose();
				this._consoleText = null;
			}

			if (this._consoleFPS != null) {
				this._consoleFPS.Dispose();
				this._consoleFPS = null;
			}

			if (this._fps != null) {
				Array.Clear(this._fps, 0, this._fps.Length);
				this._fps = null;
			}

			if (this._consoleLines != null) {
				this._consoleLines.Clear();
				this._consoleLines = null;
			}

			this._srcRect = Rectangle.Empty;
			this._consoleRect = Rectangle.Empty;
			this._visible = false;
		}

		/// <summary>
		/// Log the specified message to the developer console.
		/// </summary>
		/// <param name="message">
		/// The message to log to the console.
		/// </param>
		public void Log(String message) {
			if (String.IsNullOrEmpty(message)) {
				message = "ERROR: NULL GAME LOG MESSAGE";
			}

			this._consoleLines.Add(message);
			if (this._consoleLines.Count > this._maxConsoleLines) {
				this._consoleLines.RemoveAt(0);
				String newText = String.Empty;
				for (Int32 i = 0; i < this._consoleLines.Count; i++) {
					newText += this._consoleLines[i] + "\n";
				}
				this._consoleText.Text = newText;
			}
			else {
				this._consoleText.Text += message + "\n";
			}
		}

		/// <summary>
		/// Shows/hides the console.
		/// </summary>
		public void Toggle() {
			Int32 h = EngineGlobal.SpriteBatch.GraphicsDevice.Viewport.Height;
			if (this._consoleYT == h) {
				this._consoleYT = 0;
			}
			else {
				this._consoleYT = h;
				this._visible = true;
			}
		}

		/// <summary>
		/// Updates and/or animates the dev console.
		/// </summary>
		public void Update() {
			Int32 h = EngineGlobal.SpriteBatch.GraphicsDevice.Viewport.Height;
			if (this._visible) {
				this._consoleText.Y = (-h + this._consoleRect.Height + 8);
				this._consoleFPS.Y = this._consoleText.Y;
			}

			if (this._consoleY < this._consoleYT) {
				this._consoleY += EngineGlobal.Height * 10 * EngineGlobal.Elapsed;
			}
			else if (this._consoleY > this._consoleYT) {
				this._consoleY -= EngineGlobal.Height * 10 * EngineGlobal.Elapsed;
			}

			if (this._consoleY > h) {
				this._consoleY = h;
			}
			else if (this._consoleY < 0) {
				this._consoleY = 0;
				this._visible = false;
			}
			this._consoleRect.Height = (Int32)Math.Floor(this._consoleY);
		}

		/// <summary>
		/// Draws the developer console.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public void Render(SpriteBatch spriteBatch) {
			if (spriteBatch == null) {
				return;
			}

			this._fps[this._currentFPS] = (Int32)(1f / EngineGlobal.Elapsed);
			if (this._currentFPS++ >= this._fps.Length) {
				this._currentFPS = 0;
			}

			this._fpsUpdate = !this._fpsUpdate;
			if (this._fpsUpdate) {
				Int32 fps = 0;
				for (Int32 i = 0; i < this._fps.Length; i++) {
					fps += this._fps[i];
				}
				this._consoleFPS.Text = ((Int32)Math.Floor((double)(fps / this._fps.Length))).ToString() + " fps";
			}

			spriteBatch.Draw(EngineGlobal.XnaSheet, this._consoleRect, this._srcRect, this._consoleColor);
			this._consoleText.Render(spriteBatch);
			this._consoleFPS.Render(spriteBatch);
		}
		#endregion
	}
}

