//
//  PauseScreen.cs
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics
{
	/// <summary>
	/// Pause overlay used for when the game is inactive or has been manually
	/// paused.
	/// </summary>
	public class PauseScreen
	{
		#region Fields
		private Rectangle _pauseRect = Rectangle.Empty;
		private Color _pauseColor = Color.White;
		private Int32 _pauseScale = 0;
		private TextSprite _pauseText = null;
		private Texture2D _imgKeyX = null;
		private String _strKeyX = "A Button";
		private Vector2 _posKeyX = Vector2.Zero;
		private Texture2D _imgKeyC = null;
		private String _strKeyC = "B Button";
		private Vector2 _posKeyC = Vector2.Zero;
		private Texture2D _imgKeyMouse = null;
		private String _strKeyMouse = "Mouse";
		private Vector2 _posKeyMouse = Vector2.Zero;
		private Texture2D _imgKeyArrows = null;
		private String _strKeysArrows = "Move";
		private Vector2 _posKeyArrows = Vector2.Zero;
		private Texture2D _imgKeyMinus = null;
		private String _strKeyMinus = "Sound Down";
		private Vector2 _posKeyMinus = Vector2.Zero;
		private Texture2D _imgKeyPlus = null;
		private String _strKeyPlus = "Sound up";
		private Vector2 _posKeyPlus = Vector2.Zero;
		private Texture2D _imgKey0 = null;
		private String _strKey0 = "Mute";
		private Vector2 _posKey0 = Vector2.Zero;
		private Texture2D _imgKey1 = null;
		private String _strKey1 = "Console";
		private Vector2 _posKey1 = Vector2.Zero;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics.PauseScreen"/>
		/// class. This is the default constructor.
		/// </summary>
		public PauseScreen() {
			// Icons for the pause screen.
			this._imgKeyX = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_x");
			this._imgKeyC = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_c");
			this._imgKeyMouse = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_mouse");
			this._imgKeyArrows = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/keys_arrows");
			this._imgKeyMinus = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_minus");
			this._imgKeyPlus = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_plus");
			this._imgKey0 = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_0");
			this._imgKey1 = EngineGlobal.Content.Load<Texture2D>(@"cyflixel/key_1");

			// Icon positions.
			this._posKeyX = new Vector2(4, 36);
			this._posKeyC = new Vector2(4, (36 + 14));
			this._posKeyMouse = new Vector2(4, (36 + (14 * 2)));
			this._posKeyArrows = new Vector2(4, (36 + (14 * 3)));
			this._posKeyMinus = new Vector2(84, 36);
			this._posKeyPlus = new Vector2(84, (36 + 14));
			this._posKey0 = new Vector2(84, (36 + (14 * 2)));
			this._posKey1 = new Vector2(84, (36 + (14 * 3)));

			if (EngineGlobal.Height > 240) {
				this._pauseScale = 2;
			}

			this._pauseRect.Width *= this._pauseScale;
			this._pauseRect.Height *= this._pauseScale;
			this._pauseRect.X = ((EngineGlobal.Width - this._pauseRect.Width) / 2);
			this._pauseRect.Y = ((EngineGlobal.Height - this._pauseRect.Height) / 2);

			this._pauseText = new TextSprite(this._pauseRect.X, this._pauseRect.Y + (7 * this._pauseScale), this._pauseRect.Width, "GAME PAUSED");
			this._pauseText.SetFormat(null, ((2 * this._pauseScale) - 1), Color.White, TextJustification.Center, Color.White);

			this._posKeyX *= this._pauseScale;
			this._posKeyC *= this._pauseScale;
			this._posKeyMouse *= this._pauseScale;
			this._posKeyArrows *= this._pauseScale;
			this._posKeyMinus *= this._pauseScale;
			this._posKeyPlus *= this._pauseScale;
			this._posKey0 *= this._pauseScale;
			this._posKey1 *= this._pauseScale;

			Int32 rectX = this._pauseRect.X;
			this._posKeyX.X += rectX;
			this._posKeyC.X += rectX;
			this._posKeyMouse.X += rectX;
			this._posKeyArrows.X += rectX;
			this._posKeyMinus.X += rectX;
			this._posKeyPlus.X += rectX;
			this._posKey0.X += rectX;
			this._posKey1.X += rectX;

			Int32 rectY = this._pauseRect.Y;
			this._posKeyX.Y += rectY;
			this._posKeyC.Y += rectY;
			this._posKeyMouse.Y += rectY;
			this._posKeyArrows.Y += rectY;
			this._posKeyMinus.Y += rectY;
			this._posKeyPlus.Y += rectY;
			this._posKey0.Y += rectY;
			this._posKey1.Y += rectY;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the help text for the X button.
		/// </summary>
		public String HelpX {
			get { return this._strKeyX; }
			set { this._strKeyX = value; }
		}

		/// <summary>
		/// Gets or sets the help text for the C button.
		/// </summary>
		public String HelpC {
			get { return this._strKeyC; }
			set { this._strKeyC = value; }
		}

		/// <summary>
		/// Gets or sets the help text for the mouse button.
		/// </summary>
		public String HelpMouse {
			get { return this._strKeyMouse; }
			set { this._strKeyMouse = value; }
		}

		/// <summary>
		/// Gets or sets the help text for the arrow buttons.
		/// </summary>
		public String HelpArrows {
			get { return this._strKeysArrows; }
			set { this._strKeysArrows = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Renders the pause screen overlay.
		/// </summary>
		/// <param name="spriteBatch">
		/// Sprite batch.
		/// </param>
		public void Render(SpriteBatch spriteBatch) {
			if (spriteBatch == null) {
				throw new ArgumentNullException("spriteBatch");
			}
			Rectangle squareonerect = new Rectangle(1, 1, 1, 1);
			spriteBatch.Draw(EngineGlobal.XnaSheet, this._pauseRect, squareonerect, this._pauseColor);
			this._pauseText.Render(spriteBatch);

			spriteBatch.Draw(this._imgKeyX, this._posKeyX, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);

			Vector2 posKeyVect = new Vector2((this._posKeyX.X + (14 * this._pauseScale)), this._posKeyX.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKeyX, posKeyVect, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);

			spriteBatch.Draw(this._imgKeyC, this._posKeyC, null, Color.White, 0, Vector2.Zero,
			                 this._pauseScale, SpriteEffects.None, 0f);
			Vector2 posKeyVectC = new Vector2((this._posKeyC.X + (14 * this._pauseScale)), this._posKeyC.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKeyC, posKeyVectC, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);

#if !XBOX360
			spriteBatch.Draw(this._imgKeyMouse, this._posKeyMouse, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);
			Vector2 mouseVect = new Vector2((this._posKeyMouse.X + (14 * this._pauseScale)), this._posKeyMouse.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKeyMouse, mouseVect, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);
#endif

			spriteBatch.Draw(this._imgKeyArrows, this._posKeyArrows, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);
			Vector2 pauseVect = new Vector2((this._posKeyArrows.X + (14 * this._pauseScale)), this._posKeyArrows.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKeysArrows, pauseVect, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);

			spriteBatch.Draw(this._imgKeyMinus, this._posKeyMinus, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);
			Vector2 minusVect = new Vector2((this._posKeyMinus.X + (14 * this._pauseScale)), this._posKeyMinus.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKeyMinus, minusVect, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);

			spriteBatch.Draw(this._imgKeyPlus, this._posKeyPlus, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);
			Vector2 plusVect = new Vector2((this._posKeyPlus.X + (14 * this._pauseScale)), this._posKeyPlus.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKeyPlus, plusVect, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);

			spriteBatch.Draw(this._imgKey0, this._posKey0, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);
			Vector2 imgVect0 = new Vector2((this._posKey0.X + (14 * this._pauseScale)), this._posKey0.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKey0, imgVect0, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);

			spriteBatch.Draw(this._imgKey1, this._posKey1, null, Color.White, 0,
			                 Vector2.Zero, this._pauseScale, SpriteEffects.None, 0f);
			Vector2 imgVect1 = new Vector2((this._posKey1.X + (14 * this._pauseScale)), this._posKey1.Y);
			spriteBatch.DrawString(EngineGlobal.Font, this._strKey1, imgVect1, Color.White, 0,
			                       Vector2.Zero, this._pauseScale, SpriteEffects.None, 0);
		}
		#endregion
	}
}

