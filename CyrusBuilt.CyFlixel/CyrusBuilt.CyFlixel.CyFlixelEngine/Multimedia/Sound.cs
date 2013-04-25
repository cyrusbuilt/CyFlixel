//
//  Sound.cs
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
using Microsoft.Xna.Framework.Audio;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia
{
	/// <summary>
	/// A sound to play in-game by the sound manager.
	/// </summary>
	public class Sound : CyFlixelObject
	{
		#region Private Fields
		private Boolean _survive = false;
		private Boolean _playing = false;
		#endregion

		#region Protected Fields
		protected Boolean _init = false;
		protected SoundEffectInstance _sound = null;
		protected float _position = 0f;
		protected float _volume = 0f;
		protected float _volumeAdjust = 0f;
		protected CyFlixelObject _core = null;
		protected float _radius = 0f;
		protected Boolean _pan = false;
		protected float _fadeoutTimer = 0f;
		protected float _fadeoutTotal = 0f;
		protected Boolean _pauseOnFaedout = false;
		protected float _fadeInTimer = 0f;
		protected float _fadeInTotal = 0f;
		protected Vector2 _point2 = Vector2.Zero;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.Sound"/>
		/// class. This is the default constructor.
		/// </summary>
		public Sound()
			: base() {
			base.Fixed = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether or not this sound should be
		/// automatically destroyed when switching states.
		/// </summary>
		public Boolean Survive {
			get { return this._survive; }
			set { this._survive = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this sound is currently
		/// playing or not.
		/// </summary>
		public Boolean Playing {
			get { return this._playing; }
			set { this._playing = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this sound should be be
		/// played in a loop.
		public Boolean Looped {
			get {
				if (this._sound == null) {
					return false;
				}
				return this._sound.IsLooped;
			}
			set {
				if (this._sound != null) {
					this._sound.IsLooped = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		public float Volume {
			get { return this._volume; }
			set {
				this._volume = value;
				if (this._volume < 0) {
					this._volume = 0;
				}
				else if (this._volume > 1) {
					this._volume = 1;
				}
				this.UpdateTransform();
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initializes this sound instance by setting default values.
		/// </summary>
		protected void Init() {
			this._sound = null;
			this._position = 0;
			this._volume = 1.0f;
			this._volumeAdjust = 1.0f;
			this.Looped = false;
			this._core = null;
			this._radius = 0;
			this._pan = false;
			this._fadeoutTimer = 0;
			this._fadeoutTotal = 0;
			this._pauseOnFaedout = false;
			this._fadeInTimer = 0;
			this._fadeInTotal = 0;
			base.Active = false;
			base.Visible = false;
			base.Solid = false;
			this._playing = false;
			SoundManager.Init();
		}

		/// <summary>
		/// Stops playing the sound.
		/// </summary>
		public void Stop() {
			this._position = 0;
			this._playing = false;
			base.Active = false;
			if (this._sound != null) {
				this._sound.Stop();
			}
		}

		/// <summary>
		/// Internal function used to help organize and adjust the volume of
		/// the sound.
		/// </summary>
		internal void UpdateTransform() {
			if (this._sound == null) {
				return;
			}
			this._sound.Volume = (EngineGlobal.GetMuteValue() * EngineGlobal.Volume * this._volume * this._volumeAdjust);
		}

		/// <summary>
		/// Loads a sound from an embedded MP3 or sound effect.
		/// </summary>
		/// <returns>
		/// This instance with the new sound loaded. Useful for chaining stuff
		/// together.
		/// </returns>
		/// <param name="embeddedSound">
		/// The name of an embedded class object representing an MP3 file or
		/// sound effect.
		/// </param>
		/// <param name="looped">
		/// Whether or not this sound should loop endlessly.
		/// </param>
		public Sound LoadEmbedded(String embeddedSound, Boolean looped) {
			this.Stop();
			this.Init();
			//NOTE: can't pull ID3 info from embedded sound currently
			this._sound = SoundManager.GetSound(embeddedSound);
			this.Looped = looped;
			this.UpdateTransform();
			base.Active = true;
			return this;
		}

		/// <summary>
		/// Sets this sound to change based on distance from a particular
		/// <see cref="CyFlixelObject"/>.
		/// </summary>
		/// <param name="x">
		/// The X position of the sound.
		/// </param>
		/// <param name="y">
		/// The Y position of the sound.
		/// </param>
		/// <param name="core">
		/// The object you want to track.
		/// </param>
		/// <param name="radius">
		/// The maximum distance this sound can travel.
		/// </param>
		/// <param name="pan">
		/// Set true to pan the sound.
		/// </param>
		/// <returns>
		/// This sound instance (updated). Useful for chaining stuff together.
		/// </returns>			
		public Sound Proximity(float x, float y, CyFlixelObject core, float radius, Boolean pan) {
			base.X = x;
			base.Y = y;
			this._core = core;
			this._radius = radius;
			this._pan = pan;
			return this;
		}

		/// <summary>
		/// Sets this sound to change based on distance from a particular
		/// <see cref="CyFlixelObject"/>.
		/// </summary>
		/// <param name="x">
		/// The X position of the sound.
		/// </param>
		/// <param name="y">
		/// The Y position of the sound.
		/// </param>
		/// <param name="core">
		/// The object you want to track.
		/// </param>
		/// <param name="radius">
		/// The maximum distance this sound can travel.
		/// </param>
		/// <returns>
		/// This sound instance (updated). Useful for chaining stuff together.
		/// </returns>	
		public Sound Proximity(float x, float y, CyFlixelObject core, float radius) {
			return this.Proximity(x, y, core, radius, true);
		}

		/// <summary>
		/// Plays the sound.
		/// </summary>
		public void Play() {
			if (this._sound == null) {
				return;
			}
			this._sound.Play();
			this._playing = true;
			base.Active = true;
			this._position = 0;
		}

		/// <summary>
		/// Pauses the playback of this sound.
		/// </summary>
		public void Pause() {
			if (this._sound == null) {
				return;
			}
			this._sound.Pause();
			this._playing = false;
		}

		/// <summary>
		/// Makes this sound fade out over the specified time interval.
		/// </summary>
		/// <param name="seconds">
		/// The amount of time the fade out operation should take.
		/// </param>
		/// <param name="pause">
		/// Tells the sound to pause on fadeout, instead of stopping.
		/// </param>
		public void FadeOut(float seconds, Boolean pause) {
			this._pauseOnFaedout = pause;
			this._fadeInTimer = 0;
			this._fadeoutTimer = seconds;
			this._fadeoutTotal = this._fadeoutTimer;
		}

		/// <summary>
		/// Makes this sound fade out over the specified time interval.
		/// </summary>
		/// <param name="seconds">
		/// The amount of time the fade out operation should take.
		/// </param>
		public void FadeOut(float seconds) {
			this.FadeOut(seconds, false);
		}

		/// <summary>
		/// Fades the sound in over the specified period of time.
		/// </summary>
		/// <param name="seconds">
		/// The amount of time the fade-in operation should take.
		/// </param>
		public void FadeIn(float seconds) {
			this._fadeoutTimer = 0;
			this._fadeInTimer = seconds;
			this._fadeInTotal = this._fadeInTimer;
			this.Play();
		}

		/// <summary>
		/// Performs the actual logical updates to the sound object. This does
		/// not do much except optional proximity and fade calculations.
		/// </summary>
		protected void UpdateSound() {
			if (this._position != 0) {
				return;
			}

			float radial = 1.0f;
			float fade = 1.0f;
			if (this._sound.State != SoundState.Playing) {
				base.Active = false;
			}

			// Distance-based volume control.
			if (this._core != null) {
				this._point = this._core.GetScreenXY();
				this._point2 = base.GetScreenXY();
				float dx = (this._point.X - this._point2.X);
				float dy = (this._point.Y - this._point2.Y);
				radial = (float)(this._radius - Math.Sqrt(dx * dx + dy * dy)) / this._radius;
				if (radial < 0) {
					radial = 0;
				}
				else if (radial > 1) {
					radial = 1;
				}

				if (this._pan) {
					float d = (-dx / this._radius);
					if (d < -1) {
						d = -1;
					}
					else if (d > 1) {
						d = 1;
					}

					if (this._sound != null) {
						this._sound.Pan = d;
					}
				}
			}

			// Cross-fading volume control.
			if (this._fadeoutTimer > 0) {
				this._fadeoutTimer -= EngineGlobal.Elapsed;
				if (this._fadeoutTimer <= 0) {
					if (this._pauseOnFaedout) {
						this.Pause();
					}
					else {
						this.Stop();
					}
					fade = (this._fadeoutTimer / this._fadeoutTotal);
					if (fade < 0) {
						fade = 0;
					}
				}
			}
			else if (this._fadeInTimer > 0) {
				this._fadeInTimer -= EngineGlobal.Elapsed;
				fade = (this._fadeInTimer / this._fadeInTotal);
				if (fade < 0) {
					fade = 0;
				}
				fade = (1 - fade);
			}
			this._volumeAdjust = (radial * fade);
			this.UpdateTransform();
		}

		/// <summary>
		/// Called by the main game loop, handles motion/physics and game logic.
		/// This overload also updates the sound object automatically.
		/// </summary>
		public override void Update() {
			base.Update();
			this.UpdateSound();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.Sound"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.Sound"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.Sound"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.Sound"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.Sound"/>
		/// was occupying.
		/// </remarks>
		public override void Dispose() {
			if ((base.Active) || (this._playing)) {
				this.Stop();
			}

			if (this._sound != null) {
				this._sound.Dispose();
				this._sound = null;
			}

			if (this._core != null) {
				this._core.Dispose();
				this._core = null;
			}

			this._point2 = Vector2.Zero;
			base.Dispose();
		}
		#endregion
	}
}

