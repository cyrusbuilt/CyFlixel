//
//  ParticleEmitter.cs
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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CyrusBuilt.CyFlixel.CyFlixelEngine.FX;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// A lightweight particle emitter. This can be used for one-time explosions
	/// or for continous FX like rain and fire. This is not optimized; all it
	/// does is launch <see cref="FlixelSprite"/> objects out at set intervals
	/// by setting their positions and velocities accordingly. This is easy to
	/// use and relatively efficient, since it automatically redelays its sprites
	/// and/or kills them once they've been launched.
	/// </summary>
	public class ParticleEmitter : FlixelGroup
	{
		#region Private Fields
		private Vector2 _minPartSpeed = Vector2.Zero;
		private Vector2 _maxPartSpeed = Vector2.Zero;
		private Vector2 _particleDrag = Vector2.Zero;
		private float _minRotation = 0f;
		private float _maxRotation = 0f;
		private float _gravity = 0f;
		private Boolean _on = false;
		private float _delay = 0f;
		private Int32 _quantity = 0;
		private Boolean _justEmitted = false;
		#endregion

		#region Protected Fields
		protected Boolean _explode = false;
		protected float _timer = 0f;
		protected Int32 _particle = 0;
		protected Int32 _counter = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.ParticleEmitter"/>
		/// class with the X and Y coordinates of the emitter.
		/// </summary>
		/// <param name="x">
		/// The X coordinate of the emitter position.
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the emitter position.
		/// </param>
		public ParticleEmitter(Int32 x, Int32 y)
			: base() {
			base.X = x;
			base.Y = y;
			base.Width = 0;
			base.Height = 0;

			this._minPartSpeed = new Vector2(-100, -100);
			this._maxPartSpeed = new Vector2(100, 100);
			this._minRotation = -360;
			this._maxRotation = 360;
			this._gravity = 400;
			this._particleDrag = new Vector2();
			this._explode = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.ParticleEmitter"/>
		/// class at the top left corner of the bounding box.
		/// </summary>
		public ParticleEmitter()
			: this(0, 0) {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the minimum possible velocity of a particle. Default
		/// is (-100, -100).
		/// </summary>
		public Vector2 MinParticleSpeed {
			get { return this._minPartSpeed; }
			set { this._minPartSpeed = value; }
		}

		/// <summary>
		/// Gets or sets the maximum possible velocity of a particle. Default
		/// is (100, 100).
		/// </summary>
		public Vector2 MaxParticleSpeed {
			get { return this._maxPartSpeed; }
			set { this._maxPartSpeed = value; }
		}

		/// <summary>
		/// Gets or sets the X and Y drag components of particles launched from
		/// the emitter.
		/// </summary>
		public Vector2 ParticleDrag {
			get { return this._particleDrag; }
			set { this._particleDrag = value; }
		}

		/// <summary>
		/// Gets or sets the minimum possible angular velocity of a particle.
		/// The default value is -360.
		/// </summary>
		/// <remarks>
		/// NOTE: Rotating particles are more expensive to draw than
		/// non-rotating ones!
		/// </remarks>			
		public float MinRotation {
			get { return this._minRotation; }
			set { this._minRotation = value; }
		}

		/// <summary>
		/// Gets or sets the maximum possible angular velocity of a particle.
		/// The default value is 360.
		/// </summary>
		/// <remarks>
		/// NOTE: Rotating particles are more expensive to draw than
		/// non-rotating ones!
		/// </remarks>			
		public float MaxRotation {
			get { return this._maxRotation; }
			set { this._maxRotation = value; }
		}

		/// <summary>
		/// Gets or sets the particle gravity.
		/// </summary>
		public float Gravity {
			get { return this._gravity; }
			set { this._gravity = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this emitter is currently
		/// emitting particles.
		/// </summary>
		public Boolean On {
			get { return this._on; }
			set { this._on = value; }
		}

		/// <summary>
		/// Gets or sets the effect delay. This has different effects depending
		/// on what kind of emission it is. During an explosion, delay controls
		/// the lifespan of the particles. During normal emission, delay controls
		/// the time between particle launches.
		/// </summary>
		public float Delay {
			get { return this._delay; }
			set { this._delay = value; }
		}

		/// <summary>
		/// Gets or sets the number of particles to launch at a time.
		/// </summary>
		public Int32 Quantity {
			get { return this._quantity; }
			set { this._quantity = value; }
		}

		/// <summary>
		/// Gets a value indicating whether a particle was already fired in
		/// this frame.
		/// </summary>
		public Boolean JustEmitted {
			get { return this._justEmitted; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Generates a new array of sprites to attach to the emitter.
		/// </summary>
		/// <returns>
		/// This instance (useful for chaining stuff together).
		/// </returns>
		/// <param name="graphics">
		/// If you opted not to pre-configure an array of <see cref="FlixelSprite"/>
		/// objects, you can simply pass in a particle image or sprite sheet.
		/// </param>
		/// <param name="quantity">
		/// The number of particles to generate if <paramref name="graphics"/>
		/// is not null.
		/// </param>
		/// <param name="multiple">
		/// Whether the image in the <param name="graphics"> param is a single
		/// particle or a bunch of particles (if it's a bunch, they need to be
		/// square!)
		/// </param>
		/// <param name="collide">
		/// Whether to particles should be flagged as "alive" (non-colliding
		/// particles are higher performance). 0 means no collisions.
		/// </param>
		/// <param name="bounce">
		/// Set true if the particles should bounce after colliding with things.
		/// 0 means no bounce, 1 means full reflection.
		/// </param>
		public ParticleEmitter CreateSprites(Texture2D graphics, Int32 quantity, Boolean multiple, float collide, float bounce) {
			base.Members = new List<CyFlixelObject>();
			Int32 r = 0;
			FlixelSprite s = null;
			Int32 tf = 1;
			float sw = 0f;
			float sh = 0f;

			if (multiple) {
				s = new FlixelSprite();
				s.LoadGraphic(graphics, true);
				tf = s.FrameCount;
			}

			Int32 i = 0;
			while (i < this._quantity) {
				if ((collide > 0) && (bounce > 0)) {
					s = new Particle(bounce) as FlixelSprite;
				}
				else {
					s = new FlixelSprite();
				}

				if (multiple) {
					r = (Int32)(GameMath.Random() * tf);
					s.LoadGraphic(graphics, true);
					s.CurrentAnimationFrame = r;
				}
				else {
					s.LoadGraphic(graphics);
				}

				if (collide > 0) {
					sw = s.Width;
					sh = s.Height;
					s.Width = (Int32)(s.Width * collide);
					s.Height = (Int32)(s.Height * collide);
					Int32 px = ((Int32)(sw - s.Width) / 2);
					Int32 py = ((Int32)(sh - s.Height) / 2);
					s.Offset = new Point(px, py);
					s.Solid = true;
				}
				else {
					s.Solid = false;
					s.Exists = false;
					s.ScrollFactor = base.ScrollFactor;
					base.Add(s);
					i++;
				}
			}
			return this;
		}

		/// <summary>
		/// Generates a new array of sprites to attach to the emitter.
		/// </summary>
		/// <returns>
		/// This instance (useful for chaining stuff together).
		/// </returns>
		/// <param name="graphics">
		/// If you opted not to pre-configure an array of <see cref="FlixelSprite"/>
		/// objects, you can simply pass in a particle image or sprite sheet.
		/// </param>
		/// <param name="quantity">
		/// The number of particles to generate if <paramref name="graphics"/>
		/// is not null.
		/// </param>
		/// <param name="multiple">
		/// Whether the image in the <param name="graphics"> param is a single
		/// particle or a bunch of particles (if it's a bunch, they need to be
		/// square!)
		/// </param>
		public ParticleEmitter CreateSprites(Texture2D graphics, Int32 quantity, Boolean multiple) {
			return this.CreateSprites(graphics, quantity, multiple, 0, 0);
		}

		/// <summary>
		/// Generates a new array of sprites to attach to the emitter.
		/// </summary>
		/// <returns>
		/// This instance (useful for chaining stuff together).
		/// </returns>
		/// <param name="graphics">
		/// If you opted not to pre-configure an array of <see cref="FlixelSprite"/>
		/// objects, you can simply pass in a particle image or sprite sheet.
		/// </param>
		/// <param name="quantity">
		/// The number of particles to generate if <paramref name="graphics"/>
		/// is not null.
		/// </param>
		public ParticleEmitter CreateSprites(Texture2D graphics, Int32 quantity) {
			return this.CreateSprites(graphics, quantity, true, 0, 0);
		}

		/// <summary>
		/// Convenience method for setting the width and height to the emitter.
		/// </summary>
		/// <param name="width">
		/// The desired witdth of the emitter (particles are spawned randomly
		/// within these dimensions).
		/// </param>
		/// <param name="height">
		/// The desired height of the emitter.
		/// </param>
		public void SetSize(Int32 width, Int32 height) {
			base.Width = width;
			base.Height = height;
		}

		/// <summary>
		/// Convenience method for setting the X velocity of the emitter.
		/// </summary>
		/// <param name="min">
		/// The minimum value for this range.
		/// </param>
		/// <param name="max">
		/// The maximum value for this range.
		/// </param>
		public void SetXSpeed(float min, float max) {
			this._minPartSpeed.X = min;
			this._maxPartSpeed.X = max;
		}

		/// <summary>
		/// Convenience method for setting the X velocity of the emitter.
		/// This overload sets it to mid-range (zero-point).
		/// </summary>
		public void SetXSpeed() {
			this.SetXSpeed(0, 0);
		}

		/// <summary>
		/// Convenience method for setting the Y velocity of the emitter.
		/// </summary>
		/// <param name="min">
		/// The minimum value for this range.
		/// </param>
		/// <param name="max">
		/// The maximum value for this range.
		/// </param>
		public void SetYSpeed(float min, float max) {
			this._minPartSpeed.Y = min;
			this._maxPartSpeed.Y = max;
		}

		/// <summary>
		/// Convenience method for setting the Y velocity of the emitter.
		/// This overload sets it to mid-range (zero-point).
		/// </summary>
		public void SetYSpeed() {
			this.SetYSpeed(0, 0);
		}

		/// <summary>
		/// Convenience method for setting the angular velocity constraints of
		/// the emitter.
		/// </summary>
		/// <param name="min">
		/// The minimum value for this range.
		/// </param>
		/// <param name="max">
		/// The maximum value for this range.
		/// </param>
		public void SetRotation(float min, float max) {
			this._minRotation = min;
			this._maxRotation = max;
		}

		/// <summary>
		/// Convenience method for setting the angular velocity constraints of
		/// the emitter. This overload sets it to mid-range (zero-point).
		/// </summary>
		public void SetRotation() {
			this.SetRotation(0, 0);
		}

		/// <summary>
		/// Emits the next particle.
		/// </summary>
		public void EmitParticle() {
			this._counter++;
			FlixelSprite s = base.Members[this._particle] as FlixelSprite;
			s.Visible = true;
			s.Exists = true;
			s.Active = true;
			s.X = (base.X - ((Int32)s.Width >> 1) + GameMath.Random() * base.Width);
			s.Y = (base.Y - ((Int32)s.Height >> 1) + GameMath.Random() * base.Height);
	
			float velx = this._minPartSpeed.X;
			if (this._minPartSpeed.X != this._maxPartSpeed.X) {
				velx += (GameMath.Random() * (this._maxPartSpeed.X - this._minPartSpeed.X));
			}

			float vely = this._minPartSpeed.Y; 
			if (this._minPartSpeed.Y != this._maxPartSpeed.Y) {
				vely += (GameMath.Random() * (this._maxPartSpeed.Y - this._minPartSpeed.Y));
			}
			s.Velocity = new Vector2(velx, vely);

			float ax = s.Acceleration.X;
			float ay = this._gravity;
			s.Acceleration = new Vector2(ax, ay);
			s.AngularVelocity = this._minRotation;
			if (this._minRotation != this._maxRotation) {
				s.AngularVelocity += (GameMath.Random() * (this._maxRotation - this._minRotation));
			}

			if (s.AngularVelocity != 0) {
				s.Angle = (GameMath.Random() * 360 - 188);
			}

			float px = this._particleDrag.X;
			float py = this._particleDrag.Y;
			s.Drag = new Vector2(px, py);
			this._particle++;
			if (this._particle >= base.Members.Count) {
				this._particle = 0;
			}

			s.OnEmit();
			this._justEmitted = true;
		}

		/// <summary>
		/// Actually performs the emitter update (called by <see cref="Update()"/>).
		/// </summary>
		protected void UpdateEmitter() {
			if (this._explode) {
				this._timer += EngineGlobal.Elapsed;
				if ((this._delay > 0) && (this._timer > this._delay)) {
					base.Kill();
					return;
				}

				if (this._on) {
					this._on = false;
					Int32 i = this._particle;
					Int32 l = base.Members.Count;
					if (this._quantity > 0) {
						l = this._quantity;
					}

					l += this._particle;
					while (i < l) {
						this.EmitParticle();
						i++;
					}
				}
				return;
			}

			if (!this._on) {
				return;
			}

			this._timer += EngineGlobal.Elapsed;
			while ((this._timer > this._delay) && ((this._quantity <= 0) || (this._counter < this._quantity))) {
				this._timer -= this._delay;
				this.EmitParticle();
			}
		}

		/// <summary>
		/// Goes through and updates all the group members. Overridden here to
		/// remove the position update code normally used by a <see cref="FlixelGroup"/>.
		/// </summary>
		protected override void UpdateMembers() {
			foreach (CyFlixelObject o in base.Members) {
				if ((o.Exists) && (o.Active)) {
					o.Update();
				}
			}
		}

		/// <summary>
		/// Called automatically by the game loop, decides when to launch
		/// particles and when to "die".
		/// </summary>
		public override void Update() {
			this._justEmitted = false;
			base.Update();
			this.UpdateEmitter();
		}

		/// <summary>
		/// Starts emitting particles.
		/// </summary>
		/// <param name="explode">
		/// Whether the particles should all burst out at once.
		/// </param>
		/// <param name="delay">
		/// The delay (or lifespan).
		/// </param>
		/// <param name="quantity">
		/// How many particles to launch. Default value is 0, or "all particles".
		/// </param>
		public void Start(Boolean explode, float delay, Int32 quantity) {
			if (base.Members.Count <= 0) {
				// TODO Call EngineGlobal.Log()
				return;
			}

			this._explode = explode;
			if (!this._explode) {
				this._counter = 0;
			}

			if (!base.Exists) {
				this._particle = 0;
			}

			base.Exists = true;
			base.Visible = true;
			base.Active = true;
			base.IsDead = false;
			this._on = true;
			this._timer = 0;
			if (this._quantity == 0) {
				this._quantity = quantity;
			}
			else if (quantity != 0) {
				this._quantity = quantity;
			}

			if (delay != 0) {
				this._delay = delay;
			}

			if (this._delay < 0) {
				this._delay = -this._delay;
			}

			if (this._delay == 0) {
				if (explode) {
					this._delay = 3;
				}
				else {
					this._delay = 0.1f;
				}
			}
		}

		/// <summary>
		/// Starts emitting particles.
		/// </summary>
		/// <param name="explode">
		/// Whether the particles should all burst out at once.
		/// </param>
		/// <param name="delay">
		/// The delay (or lifespan).
		/// </param>
		public void Start(Boolean explode, float delay) {
			this.Start(explode, delay, 0);
		}

		/// <summary>
		/// Starts emitting particles.
		/// </summary>
		public void Start() {
			this.Start(true, 0, 0);
		}

		/// <summary>
		/// Stops the emitter without killing it.
		/// </summary>
		/// <param name="delay">
		/// How long to wait before killing all the particles. Set to 0 to never
		/// kill them.
		/// </param>
		public void Stop(float delay) {
			this._explode = true;
			this._delay = delay;
			if (this._delay < 0) {
				this._delay = -delay;
			}
			this._on = false;
		}

		/// <summary>
		/// Stops the emitter without killing it.
		/// </summary>
		public void Stop() {
			this.Stop(3f);
		}

		/// <summary>
		/// Change the emitter's position to the origin of a <see cref="CyFlixelObject"/>.
		/// </summary>
		/// <param name="obj">
		/// The object that needs to spew particles.
		/// </param>
		public void At(CyFlixelObject obj) {
			if (obj == null) {
				base.X = (obj.X + obj.Origin.X);
				base.Y = (obj.Y + obj.Origin.Y);
			}
		}

		/// <summary>
		/// Turn off all the particles and the emitter.
		/// </summary>
		public override void Kill() {
			base.Kill();
			this._on = false;
		}
 		#endregion
	}
}

