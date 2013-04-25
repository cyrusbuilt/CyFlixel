//
//  MouseInput.cs
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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Input
{
	/// <summary>
	/// Handles mouse input in-game.
	/// </summary>
	public class MouseInput : IDisposable
	{
		#region Fields
		private Texture2D _imgDefaultCursor = null;
		private MouseState _curMouse;
		private MouseState _lastMouse;
		private EventHandler<MouseEventArgs> _mouseEvent = null;
		private Int32 _wheel = 0;
		private Int32 _screenX = 0;
		private Int32 _screenY = 0;
		private FlixelSprite _cursor = null;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.MouseInput"/>
		/// class. This is the default constructor.
		/// </summary>
		public MouseInput() {
			this._cursor = new FlixelSprite();
			this._cursor.Visible = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a flag indicating whether this instance is disposed.
		/// </summary>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets the X coordinate of the mouse cursor.
		/// </summary>
		public float X {
			get {
#if XBOX360
				return 0;
#else
				return (float)(((this._curMouse.X - EngineGlobal.GameObject.TargetLeft) / EngineGlobal.Scale) - EngineGlobal.Scroll.X);
#endif
			}
		}

		/// <summary>
		/// Gets the Y coordinate of the mouse cursor.
		/// </summary>
		public float Y {
			get {
#if XBOX360
				return 0;
#else
				return (((float)this._curMouse.Y / EngineGlobal.Scale) - EngineGlobal.Scroll.Y);
#endif
			}
		}

		/// <summary>
		/// Gets or sets the current delta value of the mouse wheel. If the
		/// wheel was scrolled up, the value will be positive. If the wheel
		/// scrolled down, the value will be negative.
		/// </summary>
		public Int32 Wheel {
			get { return this._wheel; }
		}
		
		/// <summary>
		/// Gets the current X position of the mouse pointer on the screen.
		/// </summary>
		public Int32 MousePosX {
			get { return this._screenX; }
		}
		
		/// <summary>
		/// Gets the current Y position of the mouse pointer on the screen.
		/// </summary>
		public Int32 MousePosY {
			get { return this._screenY; }
		}
		
		/// <summary>
		/// Gets or sets the graphical representation of the mouse pointer.
		/// </summary>
		public FlixelSprite Cursor {
			get { return this._cursor; }
			set { this._cursor = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.MouseInput"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.MouseInput"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.MouseInput"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.MouseInput"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.MouseInput"/>
		/// was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._cursor != null) {
				this._cursor.Dispose();
				this._cursor = null;
			}

			if (this._imgDefaultCursor != null) {
				this._imgDefaultCursor.Dispose();
				this._imgDefaultCursor = null;
			}

			this._mouseEvent = null;
			this._isDisposed = true;
		}

		/// <summary>
		/// Adds the mouse event listener.
		/// </summary>
		/// <param name="listener">
		/// The event listener.
		/// </param>
		public void AddMouseListener(EventHandler<MouseEventArgs> listener) {
			this._mouseEvent += listener;
		}

		/// <summary>
		/// Removes the mouse listener.
		/// </summary>
		/// <param name="listener">
		/// The event listener.
		/// </param>
		public void RemoveMouseListener(EventHandler<MouseEventArgs> listener) {
			this._mouseEvent -= listener;
		}

		/// <summary>
		/// Loads a new mouse cursor graphic.
		/// </summary>
		/// <param name="graphic">
		/// The image to use for the mouse cursor.
		/// </param>
		/// <param name="xoffset">
		/// The number of pixels between the mouse's screen position and the
		/// left of the graphic.
		/// </param>
		/// <param name="yoffset">
		/// The number of pixels between the mouse's screen position and the
		/// top of the graphic.
		/// </param>
		public void Load(Texture2D graphic, Int32 xoffset, Int32 yoffset) {
			if (graphic == null) {
				graphic = this._imgDefaultCursor;
			}
			this._cursor = new FlixelSprite(this._screenX, this._screenY, graphic);
			this._cursor.Solid = false;
			this._cursor.Offset = new Point(xoffset, yoffset);
		}

		/// <summary>
		/// Loads a new mouse cursor graphic.
		/// </summary>
		/// <param name="graphic">
		/// The image to use for the mouse cursor.
		/// </param>
		public void Load(Texture2D graphic) {
			this.Load(graphic, 0, 0);
		}

		/// <summary>
		/// Shows an existing mouse cursor or loads a new one.
		/// </summary>
		/// <param name="graphic">
		/// The graphic to load as the new mouse cursor. If null, then load the
		/// existing one.
		/// </param>
		/// <param name="xoffset">
		/// The number of pixels between the mouse's screen position and the
		/// left of the graphic.
		/// </param>
		/// <param name="yoffset">
		/// The number of pixels between the mouse's screen position and the
		/// top of the graphic.
		/// </param>
		public void Show(Texture2D graphic, Int32 xoffset, Int32 yoffset) {
			if (graphic != null) {
				this.Load(graphic, xoffset, yoffset);
			}
			else if (this._cursor != null) {
				this._cursor.Visible = true;
			}
			else {
				this.Load(null);
			}
		}

		/// <summary>
		/// Shows an existing mouse cursor or loads a new one.
		/// </summary>
		/// <param name="graphic">
		/// The graphic to load as the new mouse cursor. If null, then load the
		/// existing one.
		/// </param>
		public void Show(Texture2D graphic) {
			this.Show(graphic, 0, 0);
		}

		/// <summary>
		/// Hides the mouse cursor.
		/// </summary>
		public void Hide() {
			if (this._cursor != null) {
				this._cursor.Visible = false;
			}
		}

		/// <summary>
		/// Unloads the current cursor graphic. If the current cursor is visible,
		/// then the default system cursor is loaded up to replace the old one.
		/// </summary>
		public void Unload() {
			if (this._cursor != null) {
				if (this._cursor.Visible) {
					this.Load(null);
				}
				else {
					this._cursor = null;
				}
			}
		}

		/// <summary>
		/// Checks to see if either mouse button was just pressed.
		/// </summary>
		/// <returns>
		/// true, if a mouse button was press, false otherwise.
		/// </returns>
		public Boolean JustPressed() {
			return ((this._curMouse.LeftButton == ButtonState.Pressed) &&
			        (this._lastMouse.LeftButton == ButtonState.Released) &&
			        (this._curMouse.RightButton == ButtonState.Pressed) &&
			        (this._lastMouse.RightButton == ButtonState.Released));
		}

		/// <summary>
		/// Checks to see if either mouse button was just released.
		/// </summary>
		/// <returns>
		/// true, if either button was just released, false otherwise.
		/// </returns>
		public Boolean JustReleased() {
			return ((this._curMouse.LeftButton == ButtonState.Released) &&
			        (this._lastMouse.LeftButton == ButtonState.Pressed) &&
			        (this._curMouse.RightButton == ButtonState.Released) &&
			        (this._lastMouse.RightButton == ButtonState.Pressed));
		}

		/// <summary>
		/// Called by the game loop to update the mouse pointer's position in
		/// the game world. Also updates the pressed/just released flags.
		/// </summary>
		public void Update() {
			this._lastMouse = this._curMouse;
			this._curMouse = Mouse.GetState();
			this._cursor.X = this.X;
			this._cursor.Y = this.Y;
			if (this._mouseEvent != null) {
				if (this.JustPressed()) {
					this._mouseEvent(this, new MouseEventArgs(MouseEventTypes.MouseDown));
				}
				else if (this.JustReleased()) {
					this._mouseEvent(this, new MouseEventArgs(MouseEventTypes.MouseUp));
				}
			}
		}

		/// <summary>
		/// Restores the mouse cursor graphic back to the previous one and
		/// removes all event listeners.
		/// </summary>
		public void Reset() {
			this._curMouse = this._lastMouse;
			this._mouseEvent = null;
		}

		/// <summary>
		/// Checks to see if either mouse button is currently pressed.
		/// </summary>
		/// <returns>
		/// true if one of the mouse buttons is pressed; Otherwise, false.
		/// </returns>			
		public Boolean Pressed() {
			return ((this._curMouse.LeftButton == ButtonState.Pressed) ||
			        (this._curMouse.RightButton == ButtonState.Pressed));
		}
		#endregion
	}
}

