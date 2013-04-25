//
//  Button.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.UI
{
	/// <summary>
	/// A simple button class that fires an event when clicked by the mouse.
	/// Supports labels, highlite states, and parallax scrolling.
	/// </summary>
	public class Button : FlixelGroup
	{
		#region Fields
		private Boolean _pauseProof = true;
		private Boolean _onToggle = false;
		private FlixelSprite _off = null;
		private FlixelSprite _on = null;
		private TextSprite _offT = null;
		private TextSprite _onT = null;
		private Boolean _isPressed = false;
		private Boolean _initialized = false;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the button is clicked.
		/// </summary>
		public EventHandler<EventArgs> ButtonClicked;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/>
		/// class with a gray background.
		/// </summary>
		/// <param name="x">
		/// The X coordinate of the button's position.
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the button's position.
		/// </param>
		public Button(Int32 x, Int32 y)
			: base() {
			base.X = x;
			base.Y = y;
			base.Width = 100;
			base.Height = 20;

			Color gray = new Color(0x7f, 0x7f, 0x7f);
			this._off = new FlixelSprite().CreateGraphic((Int32)base.Width, (Int32)base.Height, gray);
			this._off.Solid = false;
			base.Add(this._off, true);

			this._on = new FlixelSprite().CreateGraphic((Int32)base.Width, (Int32)base.Height, Color.White);
			this._on.Solid = false;
			base.Add(this._on, true);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether this button should raise
		/// events even while the game is paused. Default is true.
		/// </summary>
		public Boolean PauseProof {
			get { return this._pauseProof; }
			set { this._pauseProof = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/>
		/// is initialized.
		/// </summary>
		public Boolean Initialized {
			get { return this._initialized; }
		}

		/// <summary>
		/// Gets a value indicating whether this button is currently pressed.
		/// </summary>
		public Boolean IsPressed {
			get { return this._isPressed; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to toggle checkbox-style
		/// behavior.
		/// </summary>
		public Boolean On {
			get { return this._onToggle; }
			set { this._onToggle = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/>
		/// so the garbage collector can reclaim the memory that the 
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.UI.Button"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (this._off != null) {
				this._off.Dispose();
				this._off = null;
			}

			if (this._on != null) {
				this._on.Dispose();
				this._on = null;
			}

			if (this._offT != null) {
				this._offT.Dispose();
				this._offT = null;
			}

			if (this._onT != null) {
				this._onT.Dispose();
				this._onT = null;
			}

			if (EngineGlobal.Mouse != null) {
				EngineGlobal.Mouse.RemoveMouseListener(this.OnMouseUp);
			}

			this._isPressed = false;
			this._initialized = false;
			base.Dispose();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the specified image as the button background.
		/// </summary>
		/// <returns>
		/// This instance with the updated graphic.
		/// </returns>
		/// <param name="image">
		/// A sprite to use for the button background.
		/// </param>
		/// <param name="imageHighlite">
		/// A sprite to use for the button background when highlited (optional,
		/// can be null).
		/// </param>
		public Button LoadGraphic(FlixelSprite image, FlixelSprite imageHighlite) {
			this._off = base.Replace(this._off, image) as FlixelSprite;
			if (imageHighlite == null) {
				if (this._on != this._off) {
					base.Remove(this._on);
				}
				this._on = this._off;
			}
			else {
				this._on = base.Replace(this._on, imageHighlite) as FlixelSprite;
			}

			this._on.Solid = this._off.Solid = false;
			this._off.ScrollFactor = base.ScrollFactor;
			this._on.ScrollFactor = base.ScrollFactor;
			base.Width = this._off.Width;
			base.Height = this._off.Height;
			base.RefreshHulls();
			return this;
		}

		/// <summary>
		/// Adds a text label to the button.
		/// </summary>
		/// <returns>
		/// This instance updated with the text label.
		/// </returns>
		/// <param name="text">
		/// A text sprite to use to display text on this button (can be null).
		/// </param>
		/// <param name="textHighlite">
		/// A text sprite to use when the button is highlited (can be null).
		/// </param>
		public Button LoadText(TextSprite text, TextSprite textHighlite) {
			if (text != null) {
				if (this._offT == null) {
					this._offT = text;
					base.Add(this._offT);
				}
				else {
					this._offT = base.Replace(this._offT, text) as TextSprite;
				}
			}

			if (textHighlite == null) {
				this._onT = this._offT;
			}
			else {
				if (this._onT == null) {
					this._onT = textHighlite;
					base.Add(this._onT);
				}
				else {
					this._onT = base.Replace(this._onT, textHighlite) as TextSprite;
				}
			}

			this._offT.ScrollFactor = base.ScrollFactor;
			this._onT.ScrollFactor = base.ScrollFactor;
			return this;
		}

		/// <summary>
		/// Sets the visibility of the off and on graphics.
		/// </summary>
		/// <param name="on">
		/// Whether the button should be on or off.
		/// </param>
		private void SetVisibility(Boolean on) {
			if (on) {
				this._off.Visible = false;
				if (this._offT != null) {
					this._offT.Visible = false;
				}

				this._on.Visible = true;
				if (this._onT != null) {
					this._onT.Visible = true;
				}
			}
			else {
				this._on.Visible = false;
				if (this._onT != null) {
					this._onT.Visible = false;
				}

				this._off.Visible = true;
				if (this._offT != null) {
					this._offT.Visible = true;
				}
			}
		}

		/// <summary>
		/// Called by the main game loop, handles mouse-over and click detection.
		/// </summary>
		public override void Update() {
			if (!this._initialized) {
				if ((EngineGlobal.State == null) || (EngineGlobal.Mouse == null)) {
					return;
				}
				EngineGlobal.Mouse.AddMouseListener(this.OnMouseUp);
				this._initialized = true;
			}

			base.Update();
			this.SetVisibility(false);
			if (base.OverlapsPoint(EngineGlobal.Mouse.X, EngineGlobal.Mouse.Y)) {
				if (!EngineGlobal.Mouse.Pressed()) {
					this._isPressed = false;
				}
				else if (!this._isPressed) {
					this._isPressed = true;
				}
				this.SetVisibility(true);
			}

			if (this._onToggle) {
				this.SetVisibility(this._off.Visible);
			}
		}

		/// <summary>
		/// Draws this button on the screen.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public override void Render(SpriteBatch spriteBatch) {
			if (spriteBatch == null) {
				return;
			}

			base.Render(spriteBatch);
			if ((this._off != null) && (this._off.Exists) && (this._off.Visible)) {
				this._off.Render(spriteBatch);
			}

			if ((this._on != null) && (this._on.Exists) && (this._on.Visible)) {
				this._on.Render(spriteBatch);
			}

			if (this._offT != null) {
				if ((this._offT.Exists) && (this._offT.Visible)) {
					this._offT.Render(spriteBatch);
				}

				if ((this._onT != null) && (this._onT.Exists) && (this._onT.Visible)) {
					this._onT.Render(spriteBatch);
				}
			}
		}

		/// <summary>
		/// Raises the button clicked event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnButtonClicked(EventArgs e) {
			if (this.ButtonClicked != null) {
				this.ButtonClicked(this, e);
			}
		}

		/// <summary>
		/// Handles the mouse up event, and in turn, fires the button clicked
		/// event if the button is in a valid state for raising events and the
		/// mouse click event occurred while the mouse cursor is positioned
		/// over this button.
		/// </summary>
		/// <param name="sender">
		/// The object sending the event call (mouse).
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void OnMouseUp(Object sender, MouseEventArgs e) {
			if ((!base.Exists) || (!base.Visible) || (!base.Active) || (!EngineGlobal.Mouse.JustReleased()) ||
			    ((EngineGlobal.Pause) && (!this._pauseProof))) {
				return;
			}

			if (base.OverlapsPoint(EngineGlobal.Mouse.X, EngineGlobal.Mouse.Y)) {
				this.OnButtonClicked(EventArgs.Empty);
			}
		}
		#endregion
	}
}

