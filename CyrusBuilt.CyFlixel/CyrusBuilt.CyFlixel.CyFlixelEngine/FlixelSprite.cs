//
//  FlixelSprite.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Data;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// This is the primary game object. Handles basic physics and animation.
	/// </summary>
	public class FlixelSprite : CyFlixelObject
	{
		#region Private Fields
		private Boolean _stretchToFit = false;
		private Point _offset = Point.Zero;
		private float _scale = 1f;
		private BlendingModes _blendMode = BlendingModes.NotUsed;
		private Boolean _antialiasing = false;
		private Boolean _finished = false;
		private Int32 _frameWidth = 0;
		private Int32 _frameHeight = 0;
		private Int32 _frames = 0;
		private List<FlixelAnimation> _animations = null;
		private uint _flipped = 0;
		private float _frameTimer = 0;
		private AnimationCallback _callback = null;
		private Facing2D _facing = Facing2D.NotUsed;
		private Byte _bytealpha = 0xff;
		#endregion

		#region Protected Fields
		protected Texture2D _tex = null;
		protected FlixelAnimation _currentAnim = null;
		protected Int32 _currentFrame = 0;
		protected Int32 _caf = 0;
		protected Rectangle _flashRect = Rectangle.Empty;
		protected Rectangle _flashRect2 = Rectangle.Empty;
		protected Point _flashPointZero = Point.Zero;
		protected float _alpha = 0;
		protected Color _color = Color.White;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// class with the X and Y coordinates and the graphic to display. This
		/// creates an 8x8 square at the specified location. If <paramref name="simpleGraphic"/>
		/// is specified (not null) this will load a simple, one-frame graphic
		/// instead.
		/// </summary>
		/// <param name="x">
		/// The initial X position of the sprite.
		/// </param>
		/// <param name="y">
		/// The initial Y position of the sprite.
		/// </param>
		/// <param name="simpleGraphic">
		/// The graphic you want to display (set null to create an 8x8 square).
		/// This should be used for simple stuff only. Do NOT use for animated
		/// images.
		/// </param>
		public FlixelSprite(float x, float y, Texture2D simpleGraphic)
			: base() {
			base.X = x;
			base.Y = y;

			this._flashRect = new Rectangle();
			this._flashRect2 = new Rectangle();
			this._flashPointZero = new Point();
			this._offset = new Point();

			this._scale = 1f;
			this._alpha = 1;
			this._color = Color.White;
			this._blendMode = BlendingModes.NotUsed;
			this._antialiasing = false;

			this._finished = false;
			this._facing = Facing2D.NotUsed;
			this._animations = new List<FlixelAnimation>();
			this._flipped = 0;
			this._currentAnim = null;
			this._currentFrame = 0;
			this._caf = 0;
			this._frameTimer = 0;

			this._callback = null;
			if (simpleGraphic == null) {
				this.CreateGraphic(8, 8, this._color);
			}
			else {
				this.LoadGraphic(simpleGraphic);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// class with the X and Y coordinates. This creates an 8x8 square at
		/// the specified location.
		/// </summary>
		/// <param name="x">
		/// The initial X position of the sprite.
		/// </param>
		/// <param name="y">
		/// The initial Y position of the sprite.
		/// </param>
		public FlixelSprite(float x, float y)
			: this(x, y, null) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// class. This is the default constructor. This creates an 8x8 square
		/// at the top-left corner of the bounding box.
		/// </summary>
		public FlixelSprite()
			: this(0, 0, null) {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the offset to the bounding box from the top-left corner
		/// of the sprite. This comes in handy if you shrank the size of the
		/// sprite to the bounding box.
		/// </summary>
		public Point Offset {
			get { return this._offset; }
			set { this._offset = value; }
		}

		/// <summary>
		/// Gets or sets the size of the graphic. NOTE: Scale doesn't currently
		/// affect collisions automatically, you will need to adjust the width,
		/// height, and offset manually.
		/// </summary>
		public float Scale {
			get { return this._scale; }
			set { this._scale = value; }
		}

		/// <summary>
		/// Gets or sets the blending mode, similar to Photoshop.
		/// </summary>
		public BlendingModes Blend {
			get { return this._blendMode; }
			set { this._blendMode = value; }
		}

		/// <summary>
		/// Gets or sets whether or not the object is smoothed when rotated.
		/// Affects performance.
		/// </summary>
		public Boolean AntiAliasing {
			get { return this._antialiasing; }
			set { this._antialiasing = value; }
		}

		/// <summary>
		/// Gets whether or not the current animation has finished its first
		/// (or only) loop.
		/// </summary>
		public Boolean Finished {
			get { return this._finished; }
		}

		/// <summary>
		/// Gets the width of the actual graphic or image being displayed
		/// (not necessarily the game object/bounding box).
		/// </summary>
		public Int32 FrameWidth {
			get { return this._frameWidth; }
		}

		/// <summary>
		/// Gets the height of the actual graphic or image being displayed
		/// (not necessarily the game object/bounding box).
		/// </summary>
		public Int32 FrameHeight {
			get { return this._frameHeight; }
		}

		/// <summary>
		/// Gets the radians.
		/// </summary>
		protected float Radians {
			get { return this._radians; }
		}

		/// <summary>
		/// Gets or sets the total number of frames in the image (assumes each
		/// row is full).
		/// </summary>
		public Int32 FrameCount {
			get { return this._frames; }
			set { this._frames = value; }
		}

		/// <summary>
		/// Gets or sets the color of the sprite.
		/// </summary>
		public Color SpriteColor {
			get { return this._color; }
			set { this._color = new Color(value.R, value.G, value.B, this._bytealpha); }
		}

		/// <summary>
		/// Gets or sets the alpha channel.
		/// </summary>
		public float Alpha {
			get { return this._alpha; }
			set {
				this._alpha = value;
				this._bytealpha = (Byte)(255f * this._alpha);
				this._color = new Color(this._color.R, this._color.G, this._color.B, this._bytealpha);
			}
		}

		/// <summary>
		/// Gets the animations.
		/// </summary>
		public List<FlixelAnimation> Animations {
			get { return this._animations; }
		}

		/// <summary>
		/// Gets or sets the direction the this sprite should be facing.
		/// </summary>
		public Facing2D Facing {
			get { return this._facing; }
			set {
				if (this._facing != value) {
					this._facing = value;
					this.CalculateFrame();
				}
			}
		}

		/// <summary>
		/// Gets or sets the current animation frame.
		/// </summary>
		public Int32 CurrentAnimationFrame {
			get { return this._caf; }
			set {
				if (this._caf != value) {
					this._currentAnim = null;
					this._caf = value;
					this.CalculateFrame();
				}
			}
		}

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		protected Texture2D Texture {
			get { return this._tex; }
			set { this._tex = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Resets important variables for sprite optimization and rendering.
		/// </summary>
		protected void ResetHelpers() {
			this._flashRect2.X = 0;
			this._flashRect2.Y = 0;
			if (!this._stretchToFit) {
				this._flashRect.X = 0;
				this._flashRect.Y = 0;
				this._flashRect.Width = this._frameWidth;
				this._flashRect.Height = this._frameHeight;
				this._flashRect2.Width = this._tex.Width;
				this._flashRect2.Height = this._tex.Height;
			}
			else {
				this._flashRect.X = 1;
				this._flashRect.Y = 1;
				this._flashRect.Width = this._frameWidth;
				this._flashRect.Height = this._frameHeight;
				this._flashRect2.Width = (Int32)base.Width;
				this._flashRect2.Height = (Int32)base.Height;
			}

			base.Origin = new Vector2((this._frameWidth * 0.5f), (this._frameHeight * 0.5f));
			this._frames = ((this._flashRect2.Width / this._flashRect.Width) * (this._flashRect2.Height / this._flashRect.Height));
			this._caf = 0;
			base.RefreshHulls();
		}

		/// <summary>
		/// Loads an image from an embedded graphic.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is useful for chaining stuff together.
		/// </returns>
		/// <param name="graphic">
		/// The image that you want to use.
		/// </param>
		/// <param name="animated">
		/// Whether or not <paramref name="graphic"/> is a single sprite or a
		/// row of sprites.
		/// </param>
		/// <param name="reverse">
		/// Whether or not you need this class to generate horizontally flipped
		/// versions of the animation frames.
		/// </param>
		/// <param name="width">
		/// Specifies the width of your sprite (helps <see cref="FlixelSprite"/>
		/// figure out what to do with non-square sprites or sprite sheets).
		/// </param>
		/// <param name="height">
		/// Specifies the height of your sprite (helps <see cref="FlixelSprite"/>
		/// figure out what to do with non-square sprites or sprite sheets.
		/// </param>
		public virtual FlixelSprite LoadGraphic(Texture2D graphic, Boolean animated, Boolean reverse, Int32 width, Int32 height) {
			this._stretchToFit = false;
			this._tex = graphic;
			if (reverse) {
				this._flipped = (uint)graphic.Width >> 1;
			}
			else {
				this._flipped = 0;
			}

			if (width == 0) {
				if (animated) {
					width = graphic.Height;
				}
				else if (this._flipped > 0) {
					width = (Int32)((float)graphic.Width * 0.5f);
				}
				else {
					width = graphic.Width;
				}
			}

			base.Width = this._frameWidth = width;
			if (height == 0) {
				if (animated) {
					height = (Int32)base.Width;
				}
				else {
					height = graphic.Height;
				}
			}

			base.Height = this._frameHeight = height;
			this.ResetHelpers();
			return this;
		}

		/// <summary>
		/// Loads an image from an embedded graphic.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is useful for chaining stuff together.
		/// </returns>
		/// <param name="graphic">
		/// The image that you want to use.
		/// </param>
		/// <param name="animated">
		/// Whether or not <paramref name="graphic"/> is a single sprite or a
		/// row of sprites.
		/// </param>
		/// <param name="reverse">
		/// Whether or not you need this class to generate horizontally flipped
		/// versions of the animation frames.
		/// </param>
		/// <param name="width">
		/// Specifies the width of your sprite (helps <see cref="FlixelSprite"/>
		/// figure out what to do with non-square sprites or sprite sheets).
		/// </param>
		public virtual FlixelSprite LoadGraphic(Texture2D graphic, Boolean animated, Boolean reverse, Int32 width) {
			return this.LoadGraphic(graphic, animated, reverse, width, 0);
		}

		/// <summary>
		/// Loads an image from an embedded graphic.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is useful for chaining stuff together.
		/// </returns>
		/// <param name="graphic">
		/// The image that you want to use.
		/// </param>
		/// <param name="animated">
		/// Whether or not <paramref name="graphic"/> is a single sprite or a
		/// row of sprites.
		/// </param>
		/// <param name="reverse">
		/// Whether or not you need this class to generate horizontally flipped
		/// versions of the animation frames.
		/// </param>
		public virtual FlixelSprite LoadGraphic(Texture2D graphic, Boolean animated, Boolean reverse) {
			return this.LoadGraphic(graphic, animated, reverse, 0, 0);
		}

		/// <summary>
		/// Loads an image from an embedded graphic.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is useful for chaining stuff together.
		/// </returns>
		/// <param name="graphic">
		/// The image that you want to use.
		/// </param>
		/// <param name="animated">
		/// Whether or not <paramref name="graphic"/> is a single sprite or a
		/// row of sprites.
		/// </param>
		public virtual FlixelSprite LoadGraphic(Texture2D graphic, Boolean animated) {
			return this.LoadGraphic(graphic, animated, false, 0, 0);
		}

		/// <summary>
		/// Loads an image from an embedded graphic.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is useful for chaining stuff together.
		/// </returns>
		/// <param name="graphic">
		/// The image that you want to use.
		/// </param>
		public virtual FlixelSprite LoadGraphic(Texture2D graphic) {
			return this.LoadGraphic(graphic, false, false, 0, 0);
		}

		/// <summary>
		/// Creates a flat colored square image dynamically.
		/// </summary>
		/// <returns>
		/// This instance (modified). This is helpful for chaining things together.
		/// </returns>
		/// <param name="width">
		/// The width of the sprite you want to generate.
		/// </param>
		/// <param name="height">
		/// The height of the sprite you want to generate.
		/// </param>
		/// <param name="spriteColor">
		/// Specifies the color of the generated block.
		/// </param>
		public FlixelSprite CreateGraphic(Int32 width, Int32 height, Color spriteColor) {
			this._tex = EngineGlobal.XnaSheet;
			this._stretchToFit = true;
			this._facing = Facing2D.NotUsed;
			this._frameWidth = 1;
			this._frameHeight = 1;
			base.Width = width;
			base.Height = height;
			this._color = spriteColor;
			this.ResetHelpers();
			return this;
		}

		/// <summary>
		/// Updates the current animation frame.
		/// </summary>
		protected void CalculateFrame() {
			if (this._stretchToFit) {
				this._flashRect = new Rectangle(0, 0, this._frameWidth, this._frameHeight);
				return;
			}
			else {
				uint rx = (uint)(this._caf * this._frameWidth);
				uint ry = 0;

				// Handle sprite sheets.
				uint w = (uint)this._tex.Width;
				if (rx >= w) {
					ry = (uint)(rx / w) * (uint)this._frameHeight;
					rx %= w;
				}

				// Handle reversed sprites.
				if ((this._facing == Facing2D.Left) && (this._flipped > 0)) {
					rx = ((this._flipped << 1) - rx - (uint)this._frameWidth);
					this._flashRect = new Rectangle((Int32)rx, (Int32)ry, this._frameWidth, this._frameHeight);
				}
			}

			// If we have an animation and a callback, then execute the callback.
			if ((this._callback != null) && (this._currentAnim != null)) {
				this._callback(this, new AnimationEventArgs(this._currentAnim.Name, (uint)this._currentFrame, this._caf));
			}
		}

		/// <summary>
		/// Updates the sprite's animations. Useful for cases when you need to
		/// update this but are buried down in too many supers. This method is
		/// called automatically by <see cref="Update()"/>.
		/// </summary>
		protected void UpdateAnimations() {
			if ((this._currentAnim != null) && (this._currentAnim.Delay > 0) &&
			    ((this._currentAnim.Looped) || (!this._finished))) {
				this._frameTimer += EngineGlobal.Elapsed;
				while (this._frameTimer > this._currentAnim.Delay) {
					this._frameTimer -= this._currentAnim.Delay;
					if (this._currentFrame == (this._currentAnim.Frames.Length - 1)) {
						if (this._currentAnim.Looped) {
							this._currentFrame = 0;
						}
						this._finished = true;
					}
					else {
						this._currentFrame++;
					}

					this._caf = this._currentAnim.Frames[this._currentFrame];
					this.CalculateFrame();
				}
			}
		}

		/// <summary>
		/// The main game loop update method. Override this to create your own
		/// sprite logic. If overriden, it would be prudent to call <see cref="base.Update()"/>
		/// at some point in the overridden method.
		/// </summary>
		public override void Update() {
			// We need things to happen in a certain order, so we can't just
			// call base.Update().
			base.UpdateMotion();
			this.UpdateAnimations();
			base.UpdateFlickering();
		}

		/// <summary>
		/// Gets the on-screen position of the object.
		/// </summary>
		/// <returns>
		/// The on-screen position of this object.
		/// </returns>
		public override Vector2 GetScreenXY() {
			Vector2 point = Vector2.Zero;
			float floorx = GameMath.Floor(base.X + GameMath.RoundingError);
			float floory = GameMath.Floor(base.Y + GameMath.RoundingError);
			float scrollx = GameMath.Floor(EngineGlobal.Scroll.X * base.ScrollFactor.X);
			float scrolly = GameMath.Floor(EngineGlobal.Scroll.Y * base.ScrollFactor.Y);
			point.X = (floorx + scrollx - this._offset.X);
			point.Y = (floory + scrolly - this._offset.Y);
			return point;
		}

		/// <summary>
		/// Draws the sprite bounding box.
		/// </summary>
		/// <param name="sb">
		/// The sprite group to draw.
		/// </param>
		/// <param name="x">
		/// The X position of the sprite.
		/// </param>
		/// <param name="y">
		/// The Y position of the sprite.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="sb"/> cannot be null.
		/// </exception>
		protected void DrawBounds(SpriteBatch sb, Int32 x, Int32 y) {
			if (sb == null) {
				throw new ArgumentNullException("sb");
			}

			float floorx = GameMath.Floor(base.X + GameMath.RoundingError);
			float floory = GameMath.Floor(base.Y + GameMath.RoundingError);
			float scrollx = GameMath.Floor(EngineGlobal.Scroll.X * base.ScrollFactor.X);
			float scrolly = GameMath.Floor(EngineGlobal.Scroll.Y * base.ScrollFactor.Y);
			Int32 i1 = (Int32)(floorx + scrollx);
			Int32 i2 = (Int32)(floory + scrolly);
			Rectangle square = new Rectangle(1, 1, 1, 1);
			Rectangle r1 = new Rectangle(i1, i2, (Int32)base.Width, (Int32)base.Height);
			sb.Draw(EngineGlobal.XnaSheet, r1, square, this.GetBoundingColor());
		}
			
		/// <summary>
		/// Rendering method called by the game loop, updates then blits or
		/// renders current frame of animation to the screen.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="spriteBatch"/> cannot be null.
		/// </exception>
		public override void Render(SpriteBatch spriteBatch) {
			if ((!base.Visible) || (!base.Exists)) {
				return;
			}

			if (spriteBatch == null) {
				throw new ArgumentNullException("spriteBatch");
			}

			// Adjust for center origin.
			Vector2 pos = (this.GetScreenXY() + base.Origin);
			Vector2 v1 = new Vector2((this._flashRect.Width - base.Width), (this._flashRect.Height - base.Height));
			Vector2 v2 = (base.Origin / new Vector2(base.Width, base.Height));
			pos += (v1 * v2);

			// The origin must be recalculated based on the difference between
			// the object's actual (collision) dimensions and its art (animation)
			// dimensions.
			Vector2 vc = new Vector2(this._flashRect.Width, this._flashRect.Height);
			if (!this._stretchToFit) {
				vc *= (base.Origin / new Vector2(base.Width, base.Height));
			}
			else {
				vc *= (base.Origin / new Vector2((base.Width + 1), (base.Height + 1)));
			}

			if (this._facing == Facing2D.NotUsed) {
				if (this._stretchToFit) {
					spriteBatch.Draw(this._tex, new Rectangle((Int32)pos.X, (Int32)pos.Y, (Int32)base.Width, (Int32)base.Height),
					                 this._flashRect, this._color,
					                 this._radians, vc, SpriteEffects.None, 0);
				}
				else {
					spriteBatch.Draw(this._tex, pos, this._flashRect, this._color,
					                 this._radians, vc, this._scale,
					                 SpriteEffects.None, 0);
				}
			}
			else if (this._facing == Facing2D.Right) {
				spriteBatch.Draw(this._tex, pos, this._flashRect, this._color,
				                 this._radians, vc, this._scale,
				                 SpriteEffects.None, 0);
			}
			else {
				spriteBatch.Draw(this._tex, pos, this._flashRect, this._color,
				                 this._radians, vc, this._scale,
				                 SpriteEffects.FlipHorizontally, 0);
			}

			if (EngineGlobal.ShowBounds) {
				pos = this.GetScreenXY();
				pos.X += this._offset.X;
				pos.Y += this._offset.Y;
				this.DrawBounds(spriteBatch, (Int32)pos.X, (Int32)pos.Y);
			}
		}

		/// <summary>
		/// Checks to see if a 2D point in space overlaps this object.
		/// </summary>
		/// <returns>
		/// true, if point was overlapsed, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the point.
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the point.
		/// </param>
		/// <param name="perPixel">
		/// Whether or not to use per-pixel collision checking (only available
		/// in sprite subclasses).
		/// </param>
		public override bool OverlapsPoint(float x, float y, Boolean perPixel) {
			x += GameMath.Floor(EngineGlobal.Scroll.X);
			y += GameMath.Floor(EngineGlobal.Scroll.Y);
			this._point = this.GetScreenXY();
			if (!this._stretchToFit) {
				if ((x <= this._point.X) || (x >= (this._point.X + this._frameWidth)) ||
				    (y <= this._point.Y) || (y >= (this._point.Y + this._frameHeight))) {
					return false;
				}
			}
			else {
				if ((x <= this._point.X) || (x >= (this._point.X + base.Width)) ||
				    (y <= this._point.Y) || (y >= (this._point.Y + base.Height))) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Called whenever this sprite is launched by a <see cref="FlixelEmitter"/>.
		/// Override this method to add emitter handler logic.
		/// </summary>
		public virtual void OnEmit() {
			// Nothing to do here. Only when overridden.
		}

		/// <summary>
		/// Adds a new animation to the sprite.
		/// </summary>
		/// <param name="name">
		/// The name of the sprite to add.
		/// </param>
		/// <param name="frames">
		/// An array of frame numbers to play in what order.
		/// </param>
		/// <param name="framerate">
		/// The speed in frames-per-second that the animation should play at.
		/// </param>
		/// <param name="looped">
		/// Whether or not the animation is looped or just plays once.
		/// </param>
		public void AddAnimation(String name, Int32[] frames, Int32 framerate, Boolean looped) {
			this._animations.Add(new FlixelAnimation(name, frames, framerate, looped));
		}

		/// <summary>
		/// Adds a new animation to the sprite.
		/// </summary>
		/// <param name="name">
		/// The name of the sprite to add.
		/// </param>
		/// <param name="frames">
		/// An array of frame numbers to play in what order.
		/// </param>
		/// <param name="framerate">
		/// The speed in frames-per-second that the animation should play at.
		/// </param>
		public void AddAnimation(String name, Int32[] frames, Int32 framerate) {
			this._animations.Add(new FlixelAnimation(name, frames, framerate, true));
		}

		/// <summary>
		/// Adds a new animation to the sprite.
		/// </summary>
		/// <param name="name">
		/// The name of the sprite to add.
		/// </param>
		/// <param name="frames">
		/// An array of frame numbers to play in what order.
		/// </param>
		public void AddAnimation(String name, Int32[] frames) {
			this._animations.Add(new FlixelAnimation(name, frames, 0, true));
		}

		/// <summary>
		/// Adds a callback to be called whenever this sprite's animation changes.
		/// </summary>
		/// <param name="ac">
		/// The callback to execute.
		/// </param>
		public void AddAnimationCallback(AnimationCallback ac) {
			this._callback = ac;
		}

		/// <summary>
		/// Plays an existing animation. If an existing animation is specified,
		/// then it will be ignored.
		/// </summary>
		/// <param name="animationName">
		/// The name of the animation you want to play.
		/// </param>
		/// <param name="force">
		/// Whether or not to force the animation to restart.
		/// </param>
		public void Play(String animationName, Boolean force) {
			if ((!force) && (this._currentAnim != null) && (animationName == this._currentAnim.Name)) {
				return;
			}

			this._currentFrame = 0;
			this._caf = 0;
			this._frameTimer = 0;
			for (Int32 i = 0; i < this._animations.Count; i++) {
				if (this._animations[i].Name == animationName) {
					this._currentAnim = this._animations[i];
					this._finished = (this._currentAnim.Delay <= 0);
					this._caf = this._currentAnim.Frames[this._currentFrame];
					this.CalculateFrame();
					break;
				}
			}
		}

		/// <summary>
		/// Plays an existing animation. If an existing animation is specified,
		/// then it will be ignored.
		/// </summary>
		/// <param name="animationName">
		/// The name of the animation you want to play.
		/// </param>
		public void Play(String name) {
			this.Play(name, false);
		}

		/// <summary>
		/// Tells the sprite to change to a random frame of animation. This
		/// method is useful for instantiating particles and other weird things.
		/// </summary>
		public void RandomFrame() {
			this._currentAnim = null;
			this._caf = (Int32)(GameMath.Random() * (this._tex.Width / this._frameWidth));
			this.CalculateFrame();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the
		/// current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="System.Object"/> is equal
		/// to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>;
		/// Otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj) {
			if (obj == null) {
				return false;
			}

			FlixelSprite fs = obj as FlixelSprite;
			if ((Object)fs == null) {
				return false;
			}

			return ((base.Acceleration == fs.Acceleration) && (base.Active == fs.Active) &&
			        (base.Angle == fs.Angle) && (base.AngularAcceleration == fs.AngularAcceleration) &&
			        (base.AngularDrag == fs.AngularDrag) && (base.AngularVelocity == fs.AngularVelocity) &&
			        (base.CanMove == fs.CanMove) && (base.CollideBottom == fs.CollideBottom) &&
			        (base.CollideLeft == fs.CollideLeft) && (base.CollideRight == fs.CollideRight) &&
			        (base.CollideTop == fs.CollideTop) && (base.CollisionHullX == fs.CollisionHullX) &&
			        (base.CollisionHullY == fs.CollisionHullY) && (base.CollisionOffsets == fs.CollisionOffsets) &&
			        (base.CollisionVector == fs.CollisionVector) && (base.Drag == fs.Drag) &&
			        (base.Exists == fs.Exists) && (base.Fixed == fs.Fixed) && (base.Flickering == fs.Flickering) &&
			        (base.Health == fs.Height) && (base.Height == fs.Height) && (base.InstanceId == fs.InstanceId) &&
			        (base.IsDead == fs.IsDead) && (base.IsGroup == fs.IsGroup) &&
			        (base.MaxAngular == fs.MaxAngular) && (base.MaxThrust == fs.MaxThrust) &&
			        (base.MaxVelocity == fs.MaxVelocity) && (base.OnFloor == fs.OnFloor) &&
			        (base.Origin == fs.Origin) && (base.ScrollFactor == fs.ScrollFactor) &&
			        (base.Shield == fs.Shield) && (base.Solid == fs.Solid) && (base.Thrust == fs.Thrust) &&
			        (base.Velocity == fs.Velocity) && (base.Visible == fs.Visible) &&
			        (base.Width == fs.Width) && (base.X == fs.X) && (base.Y == fs.Y) &&
			        (this._offset == fs.Offset) && (this._scale == fs.Scale) && (this._blendMode == fs.Blend) &&
			        (this._antialiasing == fs.AntiAliasing) && (this._finished == fs.Finished) &&
			        (this._frameWidth == fs.FrameWidth) && (this._frameHeight == fs.FrameHeight) &&
			        (this._frames == fs.FrameCount) && (this._color == fs.SpriteColor) &&
			        (this._alpha == fs.Alpha) && (this._animations == fs.Animations) &&
			        (this._facing == fs.Facing) && (this._caf == fs.CurrentAnimationFrame));
		}

		/// <summary>
		/// Determines whether the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>.
		/// </summary>
		/// <param name="fs">
		/// The <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// to compare with the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>;
		/// Otherwise, false.
		/// </returns>
		public Boolean Equals(FlixelSprite fs) {
			if (fs == null) {
				return false;
			}

			if ((Object)fs == null) {
				return false;
			}
			
			return ((base.Acceleration == fs.Acceleration) && (base.Active == fs.Active) &&
			        (base.Angle == fs.Angle) && (base.AngularAcceleration == fs.AngularAcceleration) &&
			        (base.AngularDrag == fs.AngularDrag) && (base.AngularVelocity == fs.AngularVelocity) &&
			        (base.CanMove == fs.CanMove) && (base.CollideBottom == fs.CollideBottom) &&
			        (base.CollideLeft == fs.CollideLeft) && (base.CollideRight == fs.CollideRight) &&
			        (base.CollideTop == fs.CollideTop) && (base.CollisionHullX == fs.CollisionHullX) &&
			        (base.CollisionHullY == fs.CollisionHullY) && (base.CollisionOffsets == fs.CollisionOffsets) &&
			        (base.CollisionVector == fs.CollisionVector) && (base.Drag == fs.Drag) &&
			        (base.Exists == fs.Exists) && (base.Fixed == fs.Fixed) && (base.Flickering == fs.Flickering) &&
			        (base.Health == fs.Height) && (base.Height == fs.Height) && (base.InstanceId == fs.InstanceId) &&
			        (base.IsDead == fs.IsDead) && (base.IsGroup == fs.IsGroup) &&
			        (base.MaxAngular == fs.MaxAngular) && (base.MaxThrust == fs.MaxThrust) &&
			        (base.MaxVelocity == fs.MaxVelocity) && (base.OnFloor == fs.OnFloor) &&
			        (base.Origin == fs.Origin) && (base.ScrollFactor == fs.ScrollFactor) &&
			        (base.Shield == fs.Shield) && (base.Solid == fs.Solid) && (base.Thrust == fs.Thrust) &&
			        (base.Velocity == fs.Velocity) && (base.Visible == fs.Visible) &&
			        (base.Width == fs.Width) && (base.X == fs.X) && (base.Y == fs.Y) &&
			        (this._offset == fs.Offset) && (this._scale == fs.Scale) && (this._blendMode == fs.Blend) &&
			        (this._antialiasing == fs.AntiAliasing) && (this._finished == fs.Finished) &&
			        (this._frameWidth == fs.FrameWidth) && (this._frameHeight == fs.FrameHeight) &&
			        (this._frames == fs.FrameCount) && (this._color == fs.SpriteColor) &&
			        (this._alpha == fs.Alpha) && (this._animations == fs.Animations) &&
			        (this._facing == fs.Facing) && (this._caf == fs.CurrentAnimationFrame));
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelSprite"/>
		/// object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in
		/// hashing algorithms and data structures such as a hash table.</returns>
		public override Int32 GetHashCode() {
			return base.GetHashCode();
		}
		#endregion
	}
}

