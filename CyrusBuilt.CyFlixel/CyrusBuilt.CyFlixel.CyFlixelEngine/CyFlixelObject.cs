//
//  MyClass.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// Base class for most display objects (such as sprites, etc). It includes
	/// some basic attributes about game objects, including retro-style
	/// flickering, basic state information, sizes, scrolling, and basic physics
	/// and motion.
	/// </summary>
	public class CyFlixelObject : CyFlixelRect, IDisposable
	{
		// TODO These should only be for a "player" subclass.
		// WeaponInventory = List<CyFlixelObject>
		// GiveWeapon()
		// DropWeapon()
		// FireWeapon()
		// ItemInventory = List<CyFlixelObject>
		// GiveItem()
		// DropItem()
		// UseItem()

		#region Constants
		private const Int32 HASH_MULTIPLIER = 31;

		/// <summary>
		/// The maximum amount of health possible.
		/// </summary>
		public const float MAX_HEALTH = 100;

		/// <summary>
		/// The maximum amount of shield possible.
		/// </summary>
		public const float MAX_SHIELD = 100;
		#endregion

		#region Private Fields
		private Guid _id = Guid.Empty;
		private Boolean _exists = false;
		private Boolean _active = false;
		private Boolean _visible = false;
		private Boolean _dead = false;
		private Boolean _moves = false;
		private Boolean _onFloor = false;
		private Boolean _collideLeft = false;
		private Boolean _collideRight = false;
		private Boolean _collideTop = false;
		private Boolean _collideBottom = false;
		private Vector2 _velocity = Vector2.Zero;
		private Vector2 _drag = Vector2.Zero;
		private Vector2 _maxVelocity = Vector2.Zero;
		private Vector2 _origin = Vector2.Zero;
		private Vector2 _scrollFactor = Vector2.Zero;
		private Vector2 _colVector = Vector2.Zero;
		private Vector2 _acceleration = Vector2.Zero;
		private float _angularVelocity = 0f;
		private float _angularAcceleration = 0f;
		private float _angularDrag = 0f;
		private float _maxAngular = 0f;
		private float _thrust = 0f;
		private float _maxThrust = 0f;
		private float _health = 0f;
		private float _shield = 0f;
		private CyFlixelRect _colHullX = CyFlixelRect.Empty;
		private CyFlixelRect _colHullY = CyFlixelRect.Empty;
		private List<Vector2> _colOffsets = null;
		private Boolean _group = false;
		#endregion

		#region Protected Fields
		protected float _angle = 0f;
		protected float _radians = 0f;
		protected float _flickerTimer = 0f;
		protected Boolean _solid = false;
		protected Boolean _fixed = false;
		protected Boolean _flicker = false;
		protected Vector2 _point = Vector2.Zero;
		protected Vector2 _flashPoint = Vector2.Zero;
		protected Rectangle _rect = Rectangle.Empty;
		protected static Vector2 _pZero = Vector2.Zero;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// class. This is the default constructor.
		/// </summary>
		public CyFlixelObject()
			: this(0f, 0f, 0f, 0f) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// class with the object dimensions.
		/// </summary>
		/// <param name="x">
		/// The X coordinate.
		/// </param>
		/// <param name="y">
		/// The Y coordinate.
		/// </param>
		/// <param name="width">
		/// The width of the object.
		/// </param>
		/// <param name="height">
		/// The height of the object.
		/// </param>
		public CyFlixelObject(float x, float y, float width, float height)
			: base(x, y, width, height) {
			this._id = Guid.NewGuid();
			this._colOffsets = new List<Vector2>();
			this._colOffsets.Add(Vector2.Zero);
			this._exists = true;
			this._active = true;
			this._visible = true;
			this._solid = true;
			this._moves = true;
			this._collideLeft = true;
			this._collideRight = true;
			this._collideTop = true;
			this._collideBottom = true;
			this._maxVelocity = new Vector2(10000, 10000);
			this._maxAngular = 10000;
			this._scrollFactor = new Vector2(1, 1);
			this._flickerTimer = -1;
			this._health = MAX_HEALTH;
			this._shield = MAX_SHIELD;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the angle of a sprite (used for rotation). WARNING:
		/// Rotating sprites decreases rendering performance for this sprite
		/// by a factor of 10px!!!
		/// </summary>
		public float Angle {
			get { return this._angle; }
			set { 
				this._angle = value;
				this._radians = MathHelper.ToRadians(this._angle);
			}
		}

		/// <summary>
		/// Gets or sets the origin. WARNING: The origin of the object (sprite)
		/// will default to its center. If you change this, the visuals and the
		/// collisions will likely be pretty out-of-sync if you do any rotation.
		/// </summary>
		public virtual Vector2 Origin {
			get { return this._origin; }
			set { this._origin = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is solid. Set <b>true</b> if you want to collide with this object.
		/// </summary>
		public Boolean Solid {
			get { return this._solid; }
			set { this._solid = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is fixed. Set true if you want the object to stay in place during
		/// collisions. Useful for levels and other environment objects.
		/// </summary>
		public Boolean Fixed {
			get { return this._fixed; }
			set { this._fixed = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// exists. This is a flag for any object descended from this class.
		/// </summary>
		public Boolean Exists {
			get { return this._exists; }
			set { this._exists = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is active. If an object is not active, the game loop should
		/// automatically call <see cref="Update()"/> on it.
		/// </summary>
		public Boolean Active {
			get { return this._active; }
			set { this._active = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is visible. If an object is not visible, the game loop should *not*
		/// automatically call <see cref="Render()"/> on it.
		/// </summary>
		public Boolean Visible {
			get { return this._visible; }
			set { this._visible = value; }
		}

		/// <summary>
		/// Gets or sets the basic speed of this object.
		/// </summary>
		public Vector2 Velocity {
			get { return this._velocity; }
			set { this._velocity = value; }
		}

		/// <summary>
		/// Gets or sets how fast the speed of this object is changing. Useful
		/// for smooth movement and gravity.
		/// </summary>
		public Vector2 Acceleration {
			get { return this._acceleration; }
			set { this._acceleration = value; }
		}

		/// <summary>
		/// Gets or sets the drag. This is basically a deceleration that is
		/// only applied when acceleration is not affecting the object.
		/// </summary>
		public Vector2 Drag {
			get { return this._drag; }
			set { this._drag = value; }
		}

		/// <summary>
		/// Gets or sets the max velocity. If you are using acceleration, you
		/// can use this with it to cap the speed automatically.
		/// </summary>
		public Vector2 MaxVelocity {
			get { return this._maxVelocity; }
			set { this._maxVelocity = value; }
		}

		/// <summary>
		/// Gets or sets the angular velocity. This controls the speed at which
		/// the object will spin.
		/// </summary>
		public float AngularVelocity {
			get { return this._angularVelocity; }
			set { this._angularVelocity = value; }
		}

		/// <summary>
		/// Gets or sets the angular acceleration. This is how fast the spin
		/// rate should change.
		/// </summary>
		public float AngularAcceleration {
			get { return this._angularAcceleration; }
			set { this._angularAcceleration = value; }
		}

		/// <summary>
		/// Gets or sets the angular drag. This is the deceleration applied to
		/// the spin of the object.
		/// </summary>
		public float AngularDrag {
			get { return this._angularDrag; }
			set { this._angularDrag = value; }
		}

		/// <summary>
		/// Gets or sets the max angular. Use in conjunction <see cref="AngularAcceleration"/>
		/// for fluid spin speed and control.
		/// </summary>
		public float MaxAngular {
			get { return this._maxAngular; }
			set { this._maxAngular = value; }
		}

		/// <summary>
		/// Gets or sets the thrust. You can use this instead of directly
		/// accessing the objects velocity or acceleration.
		/// </summary>
		public float Thrust {
			get { return this._thrust; }
			set { this._thrust = value; }
		}

		/// <summary>
		/// Gets or sets the maximum thrust. This is used to cap the amount of
		/// thrust that can be applied.
		/// </summary>
		public float MaxThrust {
			get { return this._maxThrust; }
			set { this._maxThrust = value; }
		}

		/// <summary>
		/// Gets or sets the scroll factor. This is a point that can store
		/// numbers from 0 to 1 (for X and Y independently) that governs how
		/// much this object is affected by the camera subsystem. 0 means it
		/// never moves, like a HUD element or far background graphic. 1 means
		/// it scrolls along the same speed as the foreground layer.
		/// </summary>
		public Vector2 ScrollFactor {
			get { return this._scrollFactor; }
			set { this._scrollFactor = value; }
		}

		/// <summary>
		/// Gets or sets the shield. This is used to store the remaining armor.
		/// </summary>
		public float Shield {
			get { return this._shield; }
			set { this._shield = value; }
		}

		/// <summary>
		/// Gets or sets the health. This is used to store the remaining health.
		/// </summary>
		public float Health {
			get { return this._health; }
			set { this._health = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is dead. Handy
		/// for tracking gameplay or animations.
		/// </summary>
		public Boolean IsDead {
			get { return this._dead; }
			set { this._dead = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance can move.
		/// Set this to false if you want to skip automatic motion/movement.
		/// </summary>
		public Boolean CanMove {
			get { return this._moves; }
			set { this._moves = value; }
		}

		/// <summary>
		/// Gets or sets the hull for colliding with the X coordinate. Used for
		/// speeding up collision resolution.
		/// </summary>
		public CyFlixelRect CollisionHullX {
			get { return this._colHullX; }
			set { this._colHullX = value; }
		}

		/// <summary>
		/// Gets or sets the hull for colliding with the Y coordinate. Used for
		/// speeding up the collision resolution.
		/// </summary>
		public CyFlixelRect CollisionHullY {
			get { return this._colHullY; }
			set { this._colHullY = value; }
		}

		/// <summary>
		/// Gets or sets the collision vector. Used for speeding up collision
		/// resolution.
		/// </summary>
		public Vector2 CollisionVector {
			get { return this._colVector; }
			set { this._colVector = value; }
		}

		/// <summary>
		/// Gets or sets the collision offsets. By default, contains a single
		/// offset (0, 0).
		/// </summary>
		public List<Vector2> CollisionOffsets {
			get { return this._colOffsets; }
			set { this._colOffsets = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is on the floor. Primarily useful for platformers, this flag is
		/// reset during <see cref="UpdateMotion()"/>.
		/// </summary>
		public Boolean OnFloor {
			get { return this._onFloor; }
			set { this._onFloor = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// collide left. This is left collision resolution.
		/// </summary>
		public Boolean CollideLeft {
			get { return this._collideLeft; }
			set { this._collideLeft = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// collide right. This is right collision resolution.
		/// </summary>
		public Boolean CollideRight {
			get { return this._collideRight; }
			set { this._collideRight = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// collide top. This is top collision resolution.
		/// </summary>
		public Boolean CollideTop {
			get { return this._collideTop; }
			set { this._collideTop = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// collide bottom. This is bottom collision resolution.
		/// </summary>
		public Boolean CollideBottom {
			get { return this._collideBottom; }
			set { this._collideBottom = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is group.
		/// </summary>
		/// <value><c>true</c> if this instance is group; otherwise, <c>false</c>.</value>
		internal Boolean IsGroup {
			get { return this._group; }
			set { this._group = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is flickering.
		/// </summary>
		public Boolean Flickering {
			get { return this._flickerTimer >= 0; }
		}

		/// <summary>
		/// Gets the instance identifier.
		/// </summary>
		public Guid InstanceId {
			get { return this._id; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/> was occupying.</remarks>
		public virtual void Dispose() {
			if (this._colOffsets != null) {
				this._colOffsets.Clear();
				this._colOffsets = null;
			}
			this._maxVelocity = Vector2.Zero;
			this._scrollFactor = Vector2.Zero;
			this._velocity = Vector2.Zero;
			this._drag = Vector2.Zero;
			this._acceleration = Vector2.Zero;
			this._colVector = Vector2.Zero;
			this._flashPoint = Vector2.Zero;
			this._origin = Vector2.Zero;
			this._point = Vector2.Zero;
			this._angle = 0f;
			this._angularAcceleration = 0f;
			this._angularDrag = 0f;
			this._angularVelocity = 0f;
			this._flickerTimer = 0f;
			this._health = 0f;
			this._shield = 0f;
			this._maxAngular = 0f;
			this._maxThrust = 0f;
			this._radians = 0f;
			this._thrust = 0f;
			this._colHullX = CyFlixelRect.Empty;
			this._colHullY = CyFlixelRect.Empty;
			this._rect = Rectangle.Empty;
			this._active = false;
			this._collideBottom = false;
			this._collideLeft = false;
			this._collideRight = false;
			this._collideTop = false;
			this._dead = false;
			this._exists = false;
			this._fixed = false;
			this._flicker = false;
			this._group = false;
			this._moves = false;
			this._onFloor = false;
			this._solid = false;
			this._visible = false;
		}

		/// <summary>
		/// Refreshes the hulls. Called by <see cref="UpdateMotion()"/> and
		/// some constructors to rebuild the basic collision data for this
		/// object.
		/// </summary>
		public virtual void RefreshHulls() {
			this._colHullX.X = base.X;
			this._colHullX.Y = base.Y;
			this._colHullX.Width = base.Width;
			this._colHullX.Height = base.Height;
			this._colHullY.X = base.X;
			this._colHullY.Y = base.Y;
			this._colHullY.Width = base.Width;
			this._colHullY.Height = base.Height;
		}

		/// <summary>
		/// Revive game object. Resets existence flag and position, including
		/// LAST position.
		/// </summary>
		/// <param name="x">
		/// The new X coordinate of this object.
		/// </param>
		/// <param name="y">
		/// The new Y coordinate of this object.
		/// </param>
		public virtual void Reset(float x, float y) {
			base.X = x;
			base.Y = y;
			this._exists = true;
			this._dead = false;
			this._health = MAX_HEALTH;
			this._shield = MAX_SHIELD;
		}

		/// <summary>
		/// Gets the on-screen position of the object.
		/// </summary>
		/// <returns>
		/// The on-screen position of this object.
		/// </returns>
		public virtual Vector2 GetScreenXY() {
			Vector2 point = Vector2.Zero;
			float px1 = GameMath.Floor(base.X + GameMath.RoundingError);
			float px2 = GameMath.Floor(EngineGlobal.Scroll.X * this.ScrollFactor.X);
			float py1 = GameMath.Floor(base.Y + GameMath.RoundingError);
			float py2 = GameMath.Floor(EngineGlobal.Scroll.Y * this.ScrollFactor.Y);
			point.X = (px1 + px2);
			point.Y = (py1 + py2);
			return point;
		}

		/// <summary>
		/// Determines whether this instance is currently on the screen.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is on screen; otherwise, <c>false</c>.
		/// </returns>
		public virtual Boolean IsOnScreen() {
			this._point = this.GetScreenXY();
			if ((this._point.X + base.Width < 0) ||
			    (this._point.X > EngineGlobal.Width) ||
			    (this._point.Y + base.Height < 0) ||
			    (this._point.Y > EngineGlobal.Height)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Gets the appropriate color for the bounding box depending on object
		/// state.
		/// </summary>
		/// <returns>
		/// The bounding box color.
		/// </returns>
		public Color GetBoundingColor() {
			if (this._solid) {
				if (this._fixed) {
					return new Color(0x00, 0xf2, 0x25, 0x7f);
				}
				else {
					return new Color(0xff, 0x00, 0x12, 0x7f);
				}
			}
			else {
				return new Color(0x00, 0x90, 0xe9, 0x7f);
			}
		}

		/// <summary>
		/// Kills this instance so that it no longer "exists".
		/// </summary>
		public virtual void Kill() {
			this._exists = false;
			this._dead = true;
			this._health = 0f;
			this._shield = 0f;
		}

		/// <summary>
		/// Cause damage to this object (of give health bonus).
		/// </summary>
		/// <param name="damage">
		/// How much health to take away (use negative number to give health
		/// bonus).
		/// </param>
		public virtual void Hurt(float damage) {
			// drain the shield first. When its gone, drain health.
			this._shield -= damage;
			if (this._shield <= 0f) {
				this._health -= damage;
			}

			// Player is dead when all shield & health is gone.
			if (this._health <= 0f) {
				this.Kill();
			}
		}

		/// <summary>
		/// Tells this object to flicker, retro style.
		/// </summary>
		/// <param name="duration">
		/// How many seconds to flicker for.
		/// </param>
		public void Flicker(float duration) {
			this._flickerTimer = duration;
			if (this._flickerTimer < 0) {
				this._flicker = false;
				this._visible = true;
			}
		}

		/// <summary>
		/// Should be called when this object's bottom edge collides with the
		/// top of another object. 
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public virtual void HitBottom(CyFlixelObject contact, float velocity) {
			if (contact == null) {
				return;
			}

			this._onFloor = true;
			if ((!this._fixed) || 
			    ((contact.Fixed) && ((this._velocity.Y != 0) || (this._velocity.X != 0)))) {
				this._velocity.Y = velocity;
			}
		}

		/// <summary>
		/// Should be called when this object's top edge collides with the
		/// bottom of another object.
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public virtual void HitTop(CyFlixelObject contact, float velocity) {
			if (contact == null) {
				return;
			}

			if ((!this._fixed) ||
			    (contact.Fixed) && ((this._velocity.Y != 0) || (this._velocity.X != 0))) {
				this._velocity.Y = velocity;
			}
		}

		/// <summary>
		/// Should be called when this object collides with another object on
		/// either side. Since most games have identical behavior for running
		/// into walls, you can just override this function instead of overriding
		/// both <see cref="HitLeft()"/> or <see cref="HitRight()"/>.
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public virtual void HitSide(CyFlixelObject contact, float velocity) {
			if (contact == null) {
				return;
			}

			if ((!this._fixed) ||
			    (contact.Fixed) && ((this._velocity.Y != 0) || (this._velocity.X != 0))) {
				this._velocity.Y = velocity;
			}
		}

		/// <summary>
		/// Should be called with this object's left side collides with another
		/// object's right side.
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public virtual void HitLeft(CyFlixelObject contact, float velocity) {
			this.HitSide(contact, velocity);
		}

		/// <summary>
		/// Should be called with this object's right side collides with another
		/// object's left side.
		/// </summary>
		/// <param name="contact">
		/// The object you just ran into.
		/// </param>
		/// <param name="velocity">
		/// The suggested new velocity for this object.
		/// </param>
		public virtual void HitRight(CyFlixelObject contact, float velocity) {
			this.HitSide(contact, velocity);
		}

		/// <summary>
		/// This method will be called each time two objects are compared to
		/// see if they collide. It does not necessarily mean these objects
		/// *WILL* collide, however. This method does nothing by itself, and
		/// thus should be overriden to provide functionality, if needed.
		/// </summary>
		/// <param name="obj">
		/// The object you're about to run into.
		/// </param>
		public virtual void PreCollide(CyFlixelObject obj) {
			// Most objects have nothing to do here.
		}

		/// <summary>
		/// Updates the position and speed of this object. Useful for cases
		/// when you need to update this but are buried down in too many supers.
		/// </summary>
		protected void UpdateMotion() {
			if (!this._moves) {
				return;
			}

			if (this._solid) {
				this.RefreshHulls();
			}

			// Motion/Physics.
			this._onFloor = false;
			float av = this._angularVelocity;
			float aa = this._angularAcceleration;
			float ad = this._angularDrag;
			float ma = this._maxAngular;
			this._angularVelocity = GraphicsUtils.ComputeVelocity(av, aa, ad, ma);
			this._angle += (this._angularVelocity * EngineGlobal.Elapsed);
			Vector2 thrustComponents = Vector2.Zero;
			if (this._thrust != 0) {
				thrustComponents = GraphicsUtils.RotatePoint(-this._thrust, 0, 0, 0, this._angle);
				Vector2 maxComponents = GraphicsUtils.RotatePoint(-this._maxThrust, 0, 0, 0, this._angle);
				float max = Math.Abs(maxComponents.Y);
				if (max > Math.Abs(maxComponents.Y)) {
					maxComponents.Y = max;
				}
				else {
					max = Math.Abs(maxComponents.Y);
				}

				float absmax = Math.Abs(max);
				this._maxVelocity.X = absmax;
				this._maxVelocity.Y = absmax;
			}

			float velx = this._velocity.X;
			float vely = this._velocity.Y;
			float accelx = (this._acceleration.X + thrustComponents.X);
			float accely = (this._acceleration.Y + thrustComponents.Y);
			float dragx = this._drag.X;
			float dragy = this._drag.Y;
			float maxvelx = this._maxVelocity.X;
			float maxvely = this._maxVelocity.Y;
			this._velocity.X = GraphicsUtils.ComputeVelocity(velx, accelx, dragx, maxvelx);
			this._velocity.Y = GraphicsUtils.ComputeVelocity(vely, accely, dragy, maxvely);
			base.X += (this._velocity.X * EngineGlobal.Elapsed);
			base.Y += (this._velocity.Y * EngineGlobal.Elapsed);

			// Update collision data with new movement results.
			if (!this._solid) {
				return;
			}

			this._colVector.X = (this._velocity.X * EngineGlobal.Elapsed);
			this._colVector.Y = (this._velocity.Y * EngineGlobal.Elapsed);
			this._colHullX.Width += ((this._colVector.X > 0) ? this._colVector.X : -this._colVector.X);
			if (this._colVector.X < 0) {
				this._colHullX.X += this._colVector.X;
			}

			this._colHullY.X = base.X;
			this._colHullY.Height += ((this._colVector.Y > 0) ? this._colVector.Y : -this._colVector.Y);
			if (this._colVector.Y < 0) {
				this._colHullY.Y += this._colVector.Y;
			}
		}

		/// <summary>
		/// Just updates the retro-style flickering. Considered update logic
		/// rather than rendering because it toggles visibility.
		/// </summary>
		public virtual void UpdateFlickering() {
			if (this.Flickering) {
				if (this._flickerTimer > 0) {
					this._flickerTimer = EngineGlobal.Elapsed;
					if (this._flickerTimer == 0) {
						this._flickerTimer = -1;
					}
				}

				if (this._flickerTimer < 0) {
					this.Flicker(-1);
				}
				else {
					this._flicker = !this._flicker;
					this._visible = !this._flicker;
				}
			}
		}

		/// <summary>
		/// Called by the main game loop, handles motion/physics and game logic.
		/// </summary>
		public virtual void Update() {
			this.UpdateMotion();
			this.UpdateFlickering();
		}

		/// <summary>
		/// Override this method to draw graphics.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public virtual void Render(SpriteBatch spriteBatch) {
			// objects don't have any visual logic/display of their own.
		}

		/// <summary>
		/// Checks to see if the specified object overlaps with this object.
		/// </summary>
		/// <param name="obj">
		/// The object being tested.
		/// </param>
		/// <returns>
		/// true if the objects overlap; Otherwise, false.
		/// </returns>			
		public virtual Boolean Overlaps(CyFlixelObject obj) {
			if (obj == null) {
				return false;
			}

			this._point = this.GetScreenXY();
			float tx = this._point.X;
			float ty = this._point.Y;
			this._point = obj.GetScreenXY();
			if ((this._point.X <= (tx - obj.Width)) || (this._point.X >= (tx + base.Width)) ||
			    (this._point.Y <= (ty - obj.Height)) || (this._point.Y >= (ty + base.Height))) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Checks to see if a 2D point in space overlaps this object.
		/// </summary>
		/// <returns>
		/// true, if point was overlapsed, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The x coordinate of the point.
		/// </param>
		/// <param name="y">
		/// The y coordinate of the point.
		/// </param>
		/// <param name="perPixel">
		/// Whether or not to use per-pixel collision checking (only available
		/// in sprite subclasses).
		/// </param>
		public virtual Boolean OverlapsPoint(float x, float y, Boolean perPixel) {
			base.X = (x + GameMath.Floor(EngineGlobal.Scroll.X));
			base.Y = (y + GameMath.Floor(EngineGlobal.Scroll.Y));
			this._point = this.GetScreenXY();
			if ((base.X <= this._point.X) || (base.X >= (this._point.X + base.Width)) ||
			    (base.Y <= this._point.Y) || (base.Y >= (this._point.Y + base.Height))) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Checks to see if a 2D point in space overlaps this object.
		/// </summary>
		/// <returns>
		/// true, if point was overlapsed, false otherwise.
		/// </returns>
		/// <param name="x">
		/// The x coordinate of the point.
		/// </param>
		/// <param name="y">
		/// The y coordinate of the point.
		/// </param>
		public virtual Boolean OverlapsPoint(float x, float y) {
			return this.OverlapsPoint(x, y, false);
		}

		/// <summary>
		/// Collide with the specified object. This object will against itself
		/// if the specified object is null.
		/// </summary>
		/// <param name="obj">
		/// The object to collide with (or null to collide with self).
		/// </param>
		public virtual void Collide(CyFlixelObject obj) {
			// TODO return FlixelUtilities.collide(this,((Object==null)?this:Object));
		}

		/// <summary>
		/// Gives more health points to this object.
		/// </summary>
		/// <param name="health">
		/// The amount of health to give.
		/// </param>
		public void GiveHealth(float health) {
			if (health < 0f) {
				this._health = 0f;
				return;
			}

			if (health > MAX_HEALTH) {
				this._health = MAX_HEALTH;
				return;
			}
			this._health = health;
		}

		/// <summary>
		/// Gives this object more shield points.
		/// </summary>
		/// <param name="shield">
		/// The amount of shield to give to this object.
		/// </param>
		public void GiveShield(float shield) {
			if (shield < 0f) {
				this._shield = 0f;
				return;
			}

			if (shield > MAX_SHIELD) {
				this._shield = MAX_SHIELD;
				return;
			}
			this._shield = shield;
		}

		/// <summary>
		/// Revive this instance (bring back to life).
		/// </summary>
		public virtual void Revive() {
			this.Reset(base.X, base.Y);
		}

		/// <summary>
		/// Respawn this object at the specified point in 2D space.
		/// </summary>
		/// <param name="position">
		/// The position where this object should respawn.
		/// </param>
		/// <param name="sb">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public void Respawn(Point position, SpriteBatch sb) {
			this.Reset(position.X, position.Y);
			this.Render(sb);
		}

		/// <summary>
		/// Provides a hashcode identifier of this instance.
		/// </summary>
		/// <returns>
		/// The hashcode identifier for this instance.
		/// </returns>
		public override Int32 GetHashCode() {
			unchecked {
				Int32 hashCode = base.GetType().GetHashCode();
				return (hashCode * HASH_MULTIPLIER) ^ this._id.GetHashCode();
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is
		/// equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with
		/// the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="System.Object"/> is equal to the
		/// current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>;
		/// Otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj) {
			if (obj == null) {
				return false;
			}

			CyFlixelObject cfobj = obj as CyFlixelObject;
			if ((Object)cfobj == null) {
				return false;
			}

			return ((this._acceleration == cfobj.Acceleration) && (this._active == cfobj.Active) &&
			        (this._angle == cfobj.Angle) && (this._angularAcceleration == cfobj.AngularAcceleration) &&
			        (this._angularDrag == cfobj.AngularDrag) && (this._angularVelocity == cfobj.AngularVelocity) &&
			        (this._colHullX == cfobj.CollisionHullX) && (this._colHullY == cfobj.CollisionHullY) &&
			        (this._collideBottom == cfobj.CollideBottom) && (this._collideLeft == cfobj.CollideLeft) &&
			        (this._collideRight == cfobj.CollideRight) && (this._collideTop == cfobj.CollideTop) &&
			        (this._colOffsets == cfobj.CollisionOffsets) && (this._colVector == cfobj.CollisionVector) &&
			        (this._dead == cfobj.IsDead) && (this._drag == cfobj.Drag) && (this._exists == cfobj.Exists) &&
			        (this._fixed == cfobj.Fixed) && (this._group == cfobj.IsGroup) && (this._health == cfobj.Health) &&
			        (this._id == cfobj.InstanceId) && (this._maxAngular == cfobj.MaxAngular) &&
			        (this._maxThrust == cfobj.MaxThrust) && (this._maxVelocity == cfobj.MaxVelocity) &&
			        (this._moves == cfobj.CanMove) && (this._onFloor == cfobj.OnFloor) && 
			        (this._origin == cfobj.Origin) && (this._scrollFactor == cfobj.ScrollFactor) &&
			        (this._shield == cfobj.Shield) && (this._solid == cfobj.Solid) &&
			        (this._thrust == cfobj.Thrust) && (this._velocity == cfobj.Velocity) &&
			        (this._visible == cfobj.Visible) && (base.X == cfobj.X) && (base.Y == cfobj.Y) &&
			        (base.Width == cfobj.Width) && (base.Height == cfobj.Height));
		}

		/// <summary>
		/// Determines whether the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// to compare with the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>;
		/// Otherwise, false.
		/// </returns>
		public Boolean Equals(CyFlixelObject cfobj) {
			if (cfobj == null) {
				return false;
			}

			if ((Object)cfobj == null) {
				return false;
			}
			
			return ((this._acceleration == cfobj.Acceleration) && (this._active == cfobj.Active) &&
			        (this._angle == cfobj.Angle) && (this._angularAcceleration == cfobj.AngularAcceleration) &&
			        (this._angularDrag == cfobj.AngularDrag) && (this._angularVelocity == cfobj.AngularVelocity) &&
			        (this._colHullX == cfobj.CollisionHullX) && (this._colHullY == cfobj.CollisionHullY) &&
			        (this._collideBottom == cfobj.CollideBottom) && (this._collideLeft == cfobj.CollideLeft) &&
			        (this._collideRight == cfobj.CollideRight) && (this._collideTop == cfobj.CollideTop) &&
			        (this._colOffsets == cfobj.CollisionOffsets) && (this._colVector == cfobj.CollisionVector) &&
			        (this._dead == cfobj.IsDead) && (this._drag == cfobj.Drag) && (this._exists == cfobj.Exists) &&
			        (this._fixed == cfobj.Fixed) && (this._group == cfobj.IsGroup) && (this._health == cfobj.Health) &&
			        (this._id == cfobj.InstanceId) && (this._maxAngular == cfobj.MaxAngular) &&
			        (this._maxThrust == cfobj.MaxThrust) && (this._maxVelocity == cfobj.MaxVelocity) &&
			        (this._moves == cfobj.CanMove) && (this._onFloor == cfobj.OnFloor) && 
			        (this._origin == cfobj.Origin) && (this._scrollFactor == cfobj.ScrollFactor) &&
			        (this._shield == cfobj.Shield) && (this._solid == cfobj.Solid) &&
			        (this._thrust == cfobj.Thrust) && (this._velocity == cfobj.Velocity) &&
			        (this._visible == cfobj.Visible) && (base.X == cfobj.X) && (base.Y == cfobj.Y) &&
			        (base.Width == cfobj.Width) && (base.Height == cfobj.Height));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelObject"/>.
		/// </returns>
		public override String ToString() {
			return this._id.ToString();
		}
		#endregion
	}
}

