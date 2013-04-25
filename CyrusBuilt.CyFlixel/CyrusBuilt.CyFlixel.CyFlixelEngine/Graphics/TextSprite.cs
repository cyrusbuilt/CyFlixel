//
//  TextSprite.cs
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
	/// Extends <see cref="FlixelSprite"/> to support rendering text. Can tint,
	/// fade, rotate, and scale just like a sprite. Doesn't really animate, though.
	/// Also does nice pixel-perfect centering on pixel fonts as long as they
	/// are only one-liners.
	/// </summary>
	public class TextSprite : FlixelSprite
	{
		#region Fields
		private String _text = String.Empty;
		private SpriteFont _font = null;
		private Vector2 _fontMeasure = Vector2.Zero;
		private float _scale = 0f;
		private TextJustification _alignment = TextJustification.Left;
		private Color _shadow = Color.White;
		private Color _backColor = Color.White;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.TextSprite"/>
		/// class with all the text parameters.
		/// </summary>
		/// <param name="x">
		/// The X position of the text.
		/// </param>
		/// <param name="y">
		/// The Y position of the text.
		/// </param>
		/// <param name="width">
		/// The width of the text object.
		/// </param>
		/// <param name="height">
		/// The height of the text object (should be proportional to the width).
		/// </param>
		/// <param name="text">
		/// The actual text to display.
		/// </param>
		/// <param name="color">
		/// The color of the text.
		/// </param>
		/// <param name="font">
		/// The font face of the text.
		/// </param>
		/// <param name="scale">
		/// The scale (size) of the text.
		/// </param>
		/// <param name="alignment">
		/// The text justification (alignment).
		/// </param>
		/// <param name="angle">
		/// The angle of the text in relation to the camera.
		/// </param>
		public TextSprite(float x, float y, float width, float height, String text, Color color, SpriteFont font,
		                  float scale, TextJustification alignment, float angle)
			: base(x, y) {
			this._text = text;
			base.SpriteColor = color;
			this._shadow = Color.Black;
			this._backColor = new Color(0xFF, 0xFF, 0xFF, 0x0);
			if (font == null) {
				font = EngineGlobal.Font;
			}
			this._font = font;
			base.Angle = angle;
			this._scale = scale;
			this._alignment = alignment;
			base.Width = width;
			base.Height = height;
			base.ScrollFactor = Vector2.Zero;
			base.Solid = false;
			base.CanMove = false;
			this.RecalculateMeasurements();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.TextSprite"/>
		/// class with all the text parameters.
		/// </summary>
		/// <param name="x">
		/// The X position of the text.
		/// </param>
		/// <param name="y">
		/// The Y position of the text.
		/// </param>
		/// <param name="width">
		/// The width of the text object.
		/// </param>
		/// <param name="text">
		/// The actual text to display.
		/// </param>
		public TextSprite(float x, float y, float width, String text)
			: this(x, y, width, 10, text, Color.White, EngineGlobal.Font, 1, TextJustification.Center, 0) {
			if (text == null) {
				this._text = String.Empty;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.TextSprite"/>
		/// class with all the text parameters.
		/// </summary>
		/// <param name="x">
		/// The X position of the text.
		/// </param>
		/// <param name="y">
		/// The Y position of the text.
		/// </param>
		/// <param name="width">
		/// The width of the text object.
		/// </param>
		public TextSprite(float x, float y, float width)
			: this(x, y, width, 10, String.Empty, Color.White, EngineGlobal.Font, 1, TextJustification.Center, 0) {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the width of the text.
		/// </summary>
		public Int32 TextWidth {
			get { return (Int32)_fontMeasure.X; }
		}

		/// <summary>
		/// Gets the height of the text.
		/// </summary>
		public Int32 TextHeight {
			get { return (Int32)_fontMeasure.Y; }
		}

		/// <summary>
		/// Gets or sets the origin. WARNING: The origin of the object (sprite)
		/// will default to its center. If you change this, the visuals and the
		/// collisions will likely be pretty out-of-sync if you do any rotation.
		/// </summary>
		public override Vector2 Origin {
			get {
				return (base.Origin * this._scale);
			}
			set {
				base.Origin = (value / this._scale);
			}
		}

		/// <summary>
		/// Gets or sets the text alignment.
		/// </summary>
		public TextJustification Alignment {
			get { return this._alignment; }
			set { this._alignment = value; }
		}

		/// <summary>
		/// Gets or sets the color of the text's shadow.
		/// </summary>
		public Color Shadow {
			get { return this._shadow; }
			set { this._shadow = value; }
		}

		/// <summary>
		/// Gets or sets the color of the background behind the text.
		/// </summary>
		public Color BackColor {
			get { return this._backColor; }
			set { this._backColor = value; }
		}

		/// <summary>
		/// Gets or sets the text being displayed.
		/// </summary>
		public String Text {
			get { return this._text; }
			set {
				this._text = value;
				this.RecalculateMeasurements();
			}
		}

		/// <summary>
		/// Gets or sets the size of the text being displayed.
		/// </summary>
		public new float Scale {
			get { return this._scale; }
			set {
				base.Origin *= this._scale;
				this._scale = value;
				base.Origin /= this._scale;
				this.RecalculateMeasurements();
			}
		}

		/// <summary>
		/// Gets or sets the font used for the text.
		/// </summary>
		public SpriteFont Font {
			get { return this._font; }
			set {
				this._font = value;
				if (this._font == null) {
					this._font = EngineGlobal.Font;
					this.RecalculateMeasurements();
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Ensures the <see cref="TextWidth"/> and <see cref="TextHeight"/>
		/// properties are always up to date.
		/// </summary>
		public void RecalculateMeasurements() {
			try {
				this._fontMeasure = (this._font.MeasureString(this._text) * this._scale);
				base.Origin = new Vector2((this._fontMeasure.X / 2), (this._fontMeasure.Y / 2));
			}
			catch {
				this._fontMeasure = Vector2.Zero;
			}
		}

		/// <summary>
		/// Used to set the text format. This is useful if you have a lot of
		/// text parameters to set instead of the individual properties.
		/// </summary>
		/// <returns>
		/// This instance with the formatting applied.
		/// </returns>
		/// <param name="font">
		/// The font face to apply to the text display.
		/// </param>
		/// <param name="scale">
		/// The scale (size) of the font.
		/// </param>
		/// <param name="color">
		/// The color of the text.
		/// </param>
		/// <param name="alignment">
		/// The desired alignment.
		/// </param>
		/// <param name="shadow">
		/// The text shadow color.
		/// </param>
		public TextSprite SetFormat(SpriteFont font, float scale, Color color, TextJustification alignment, Color shadow) {
			if (font == null) {
				font = EngineGlobal.Font;
			}
			this._font = font;
			this._scale = scale;
			base.SpriteColor = color;
			this._alignment = alignment;
			this._shadow = shadow;
			this.RecalculateMeasurements();
			return this;
		}

		/// <summary>
		/// Renders the text sprite.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public override void Render(SpriteBatch spriteBatch) {
			if ((!base.Visible) || (!base.Exists)) {
				return;
			}

			Vector2 pos = (new Vector2(base.X, base.Y) + base.Origin);
			pos += (EngineGlobal.Scroll * base.ScrollFactor);
			if (spriteBatch == null) {
				return;
			}

			if (this._backColor.A > 0) {
				Rectangle r1 = new Rectangle(1, 1, 1, 1);
				Rectangle r2 = new Rectangle((Int32)base.X, (Int32)base.Y, (Int32)base.Width, (Int32)base.Height);
				spriteBatch.Draw(EngineGlobal.XnaSheet, r2, r1, this._backColor);
			}

			if (this._shadow != base.SpriteColor) {
				pos += new Vector2(1, 1);
				switch (this._alignment) {
					case TextJustification.Left:
						spriteBatch.DrawString(this._font, this._text, pos, this._shadow,
						                       base.Radians, base.Origin, this._scale,
						                       SpriteEffects.None, 0f);
						break;
					case TextJustification.Right:
						float x = (pos.X + base.Width - this.TextWidth);
						Vector2 v = new Vector2(x, pos.Y);
						spriteBatch.DrawString(this._font, this._text, v, this._shadow,
						                       base.Radians, base.Origin, this._scale,
						                       SpriteEffects.None, 0f);
						break;
					case TextJustification.Center:
						float x2 = (pos.X + (base.Width - this.TextWidth) / 2);
						Vector2 v2 = new Vector2(x2, pos.Y);
						spriteBatch.DrawString(this._font, this._text, v2, this._shadow,
						                       base.Radians, base.Origin, this._scale,
						                       SpriteEffects.None, 0f);
						break;
				}
				pos += new Vector2(-1, -1);
			}

			switch (this._alignment) {
				case TextJustification.Left:
					spriteBatch.DrawString(this._font, this._text, pos, this._color,
					                       this._radians, base.Origin, this._scale,
					                       SpriteEffects.None, 0f);
					break;
				case TextJustification.Right:
					float x = (pos.X + base.Width - this.TextWidth);
					Vector2 r = new Vector2(x, pos.Y);
					spriteBatch.DrawString(this._font, this._text, r, this._color,
					                       this._radians, base.Origin, this._scale,
					                       SpriteEffects.None, 0f);
					break;
				case TextJustification.Center:
					float x2 = (pos.X + ((base.Width - this.TextWidth) / 2));
					Vector2 v = new Vector2(x2, pos.Y);
					spriteBatch.DrawString(this._font, this._text, v, this._color,
					                       this._radians, base.Origin, this._scale,
					                       SpriteEffects.None, 0f);
					break;
			}
		}
		#endregion
	}
}

