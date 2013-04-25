//
//  EngineGlobal.cs
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
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CyrusBuilt.CyFlixel.CyFlixelEngine.FX;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Input;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// This is a global helper class full of useful functions for audio,
	/// input, basic info, and the camera system among other things.
	/// </summary>
	public static class EngineGlobal
	{
		#region Private Fields
		private static Game _xnagame = null;
		private static ContentManager _content = null;
		private static Texture2D _xnaTiles = null;
		private static SpriteFont _font = null;
		private static SpriteBatch _spriteBatch = null;
		private static Version _libVersion = Assembly.GetExecutingAssembly().GetName().Version;
		private static float _elapsed = 0f;
		private static Int32 _width = 1280;
		private static Int32 _height = 720;
		private static Vector2 _scroll = Vector2.Zero;
		private static uint _getTimer = 0;
		private static Boolean _showBounds = false;
		private static Color _backgroundColor = Color.Black;
		private static float _scale = 0f;
		private static PlayerIndex? _controllingPlayer = PlayerIndex.One;
		internal static CyFlixelGame _gameObject = null;
		private static float _volume = 0f;
		private static Boolean _mute = false;
		private static Boolean _pause = false;
		private static Boolean _debug = false;
		private static float _timeScale = 1.0f;
		private static float _maxElapsed = 0.30f;
		private static Boolean _mobile = false;
		private static Int32 _currentScore = 0;
		private static Int32 _currentLevel = 0;
		private static List<Int32> _levels = new List<Int32>();
		private static List<Int32> _scores = new List<Int32>();
#if !WINDOWS_PHONE
		private static List<Int32> _saves = new List<Int32>();
		private static Int32 _lastSave = 0;
#endif
		private static MouseInput _mouse = new MouseInput();
		private static KeyboardInput _keyboard = new KeyboardInput();
		private static GamepadInput _gamepad = new GamepadInput();
		private static Sound _music = null;
		private static List<Sound> _sounds = new List<Sound>();
		private static CyFlixelObject _followTarget = null;
		private static Vector2 _followLead = Vector2.Zero;
		private static float _followLerp = 0f;
		private static Point _followMin = Point.Zero;
		private static Point _followMax = Point.Zero;
		private static Vector2 _scrollTarget = Vector2.Zero;
		private static Quake _quake = null;
		private static Flash _flash = null;
		private static Fade _fade = null;
		private static Boolean _autoHandlePause = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the engine version.
		/// </summary>
		public static Version EngineVersion {
			get { return _libVersion; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not the Engine is in Debug
		/// or release mode. Should be set at startup during game init.
		/// </summary>
		public static Boolean Debug {
			get { return _debug; }
			set { _debug = value; }
		}

		/// <summary>
		/// Gets or sets the game reference.
		/// </summary>
		public static Game GameReference {
			get { return _xnagame; }
			set { _xnagame = value; }
		}

		/// <summary>
		/// Gets or sets the elapsed time counter.
		/// </summary>
		public static float Elapsed {
			get { return _elapsed; }
			set { _elapsed = value; }
		}

		/// <summary>
		/// Gets the scale of the screen size in comparison to the actual game
		/// size.
		/// </summary>
		public static float Scale {
			get { return _scale; }
		}

		/// <summary>
		/// Gets the actual game object.
		/// </summary>
		internal static CyFlixelGame GameObject {
			get { return _gameObject; }
		}

		/// <summary>
		/// Gets or sets the screen width.
		/// </summary>
		public static Int32 Width {
			get { return _width; }
			set { _width = value; }
		}

		/// <summary>
		/// Gets or sets the screen height.
		/// </summary>
		public static Int32 Height {
			get { return _height; }
			set { _height = value; }
		}

		/// <summary>
		/// Gets or sets the basic parallax scrolling values.
		/// </summary>
		public static Vector2 Scroll {
			get { return _scroll; }
			set { _scroll = value; }
		}

		/// <summary>
		/// Gets or sets the engine timer.
		/// </summary>
		public static uint EngineTimer {
			get { return _getTimer; }
			set { _getTimer = value;}
		}

		/// <summary>
		/// Gets the XNA tile sheet.
		/// </summary>
		public static Texture2D XnaSheet {
			get { return _xnaTiles; }
		}

		/// <summary>
		/// Gets or sets whether or not to display the bounding boxes of the
		/// in-game objects.
		/// </summary>
		public static Boolean ShowBounds {
			get { return _showBounds; }
			set { _showBounds = value; }
		}

		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		public static Color BackgroundColor {
			get { return _backgroundColor; }
			set { _backgroundColor = value; }
		}

		/// <summary>
		/// Gets or sets the controlling player.
		/// </summary>
		public static PlayerIndex? ControllingPlayer {
			get { return _controllingPlayer; }
			set { _controllingPlayer = value; }
		}

		/// <summary>
		/// Gets or sets the time scale. This is how fast or slow time should
		/// pass in the game; Default is 1.0.
		/// </summary>
		public static float TimeScale {
			get { return _timeScale; }
			set { _timeScale = value; }
		}

		/// <summary>
		/// Gets or sets the elapsed time. This essentially locks the framerate
		/// to a minimum value - any slower and you'll get lag instead of
		/// frameskip. Default is 1/30th of a second.
		/// </summary>
		public static float MaxElapsed {
			get { return _maxElapsed; }
			set { _maxElapsed = value; }
		}

		/// <summary>
		/// Gets or sets the current game state.
		/// </summary>
		public static GameStateBase State {
			get {
				if (_gameObject != null) {
					return _gameObject.State;
				}
				return null;
			}
			set {
				if (_gameObject != null) {
					_gameObject.SwitchState(value);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this is a mobile version.
		/// Setting this property true will disable/skip stuff that isn't
		/// necessary for mobile platforms like Android or Windows Phone 7/8.
		/// </summary>
		public static Boolean Mobile {
			get { return _mobile; }
			set { _mobile = value; }
		}

		/// <summary>
		/// Gets or sets the current score.
		/// </summary>
		public static Int32 CurrentScore {
			get { return _currentScore; }
			set { _currentScore = value; }
		}

		/// <summary>
		/// Gets or sets the current level.
		/// </summary>
		public static Int32 CurrentLevel {
			get { return _currentLevel; }
			set { _currentLevel = value; }
		}

		/// <summary>
		/// Gets or sets a list of recent scores.
		/// </summary>
		public static List<Int32> Scores {
			get { return _scores; }
			set { _scores = value; }
		}

		/// <summary>
		/// Gets or sets the levels.
		/// </summary>
		public static List<Int32> Levels {
			get { return _levels; }
			set { _levels = value; }
		}

#if !WINDOWS_PHONE
		/// <summary>
		/// Gets or sets the list of game saves.
		/// </summary>
		public static List<Int32> Saves {
			get { return _saves; }
			set { _saves = value; }
		}

		/// <summary>
		/// Gets or sets the last save game.
		/// </summary>
		public static Int32 LastSave {
			get { return _lastSave; }
			set { _lastSave = value; }
		}
#endif
		/// <summary>
		/// Gets or sets a reference to the mouse.
		/// </summary>
		public static MouseInput Mouse {
			get { return _mouse; }
			set { _mouse = value; }
		}

		/// <summary>
		/// Gets or sets a reference to the keyboard.
		/// </summary>
		public static KeyboardInput Keyboard {
			get { return _keyboard; }
			set { _keyboard = value; }
		}

		/// <summary>
		/// Gets or sets a reference to the gamepads.
		/// </summary>
		public static GamepadInput Gamepads {
			get { return _gamepad; }
			set { _gamepad = value; }
		}

		/// <summary>
		/// Gets or sets the container for a background music object.
		/// </summary>
		public static Sound Music {
			get { return _music; }
			set { _music = value; }
		}

		/// <summary>
		/// Gets or sets the list of all the sounds being played in the game.
		/// </summary>
		public static List<Sound> Sounds {
			get { return _sounds; }
			set { _sounds = value; }
		}

		/// <summary>
		/// Gets or sets the object for the camera to follow around.
		/// </summary>
		public static CyFlixelObject FollowTarget {
			get { return _followTarget; }
			set { _followTarget = value; }
		}

		/// <summary>
		/// Gets or sets the vector used to force the camera to look ahead of
		/// the <see cref="FollowTarget"/>.
		/// </summary>
		public static Vector2 FollowLead {
			get { return _followLead; }
			set { _followLead = value; }
		}

		/// <summary>
		/// Used to smoothly track the camera as it follows.
		/// </summary>
		public static float FollowLerp {
			get { return _followLerp; }
			set { _followLerp = value; }
		}

		/// <summary>
		/// Gets or sets the top and left edges of the camera area.
		/// </summary>
		public static Point FollowMin {
			get { return _followMin; }
			set { _followMin = value; }
		}

		/// <summary>
		/// Gets or sets the bottom and right edges of the camera area.
		/// </summary>
		public static Point FollowMax {
			get { return _followMax; }
			set { _followMax = value; }
		}

		/// <summary>
		/// Gets or sets a special effect that shakes the screen.
		/// </summary>
		public static Quake ScreenQuake {
			get { return _quake; }
			set { _quake = value; }
		}

		/// <summary>
		/// Gets or sets a special effect that flashes a color on the screen.
		/// </summary>
		public static Flash ScreenFlash {
			get { return _flash; }
			set { _flash = value; }
		}

		/// <summary>
		/// Gets or sets a special effect that fades a color onto the screen.
		/// </summary>
		public static Fade ScreenFade {
			get { return _fade; }
			set { _fade = value; }
		}

		/// <summary>
		/// Gets the game engine's embedded game content.
		/// </summary>
		public static ContentManager Content {
			get { return _content; }
		}

		/// <summary>
		/// Gets the font.
		/// </summary>
		public static SpriteFont Font {
			get { return _font; }
		}

		/// <summary>
		/// Gets the group of sprites to be rendered.
		/// </summary>
		public static SpriteBatch SpriteBatch {
			get { return _spriteBatch; }
		}

		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>
		/// The volume (between 0 and 1).
		/// </value>
		public static float Volume {
			get { return _volume; }
			set {
				_volume = value;
				if (_volume < 0) {
					_volume = 0;
				}
				else if (_volume > 1) {
					_volume = 1;
				}
				ChangeSounds();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the volume is on or off.
		/// </summary>
		public static Boolean Mute {
			get { return _mute; }
			set {
				if (_mute != value) {
					_mute = value;
					ChangeSounds();
				}
			}
		}

		/// <summary>
		/// Set true to pause the game, all sounds, and display the pause popup.
		/// </summary>
		public static Boolean Pause {
			get { return _pause; }
			set {
				if (_pause != value) {
					_pause = value;
					_gameObject.Paused = _pause;
					if (_pause) {
						PauseSounds();
					}
					else {
						PlaySounds();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to automatically handle pause
		/// requests. Typically, you'd set this to true only for gameplay states,
		/// and set to false for all others (menus, etc).
		/// </summary>
		public static Boolean AutoHandlePause {
			get { return _autoHandlePause; }
			set { _autoHandlePause = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Logs the specified message to the developer console.
		/// </summary>
		/// <param name="message">
		/// The message to log.
		/// </param>
		public static void Log(String message) {
			if (_gameObject != null) {
				_gameObject.Console.Log(message);
			}
		}

		/// <summary>
		/// Gets the mute value.
		/// </summary>
		/// <returns>
		/// 1 if muted; Otherwise 0.
		/// </returns>
		public static Int32 GetMuteValue() {
			return (_mute) ? 1 : 0;
		}

		/// <summary>
		/// Loads embedded game content.
		/// </summary>
		/// <param name="gd">
		/// The graphics device instance 
		/// </param>
		public static void LoadContent(GraphicsDevice gd) {
			_content = _xnagame.Content;
			_spriteBatch = new SpriteBatch(gd);
			_font = _content.Load<SpriteFont>(@"cyflixel/deffont");
			_xnaTiles = _content.Load<Texture2D>(@"cyflixel/tiles");
			_scale = ((float)_gameObject.TargetWidth / (float)_width);
			_quake = new Quake((Int32)_scale);
			_flash = new Flash();
			_fade = new Fade();
		}

		/// <summary>
		/// Resets all the input devices. Useful when changing screens or states.
		/// </summary>
		public static void ResetInput() {
			if (_keyboard != null) {
				_keyboard.Reset();
			}
			
			if (_mouse != null) {
				_mouse.Reset();
			}
			
			if (_gamepad != null) {
				_gamepad.Reset();
			}
		}

		/// <summary>
		/// Setup and play a looping background soundtrack.
		/// </summary>
		/// <param name="music">
		/// The name of the sound file you want to loop in the background.
		/// </param>
		/// <param name="volume">
		/// How lound the sound should be. From 0 to 1.
		/// </param>
		public static void PlayMusic(String music, float volume) {
			// TODO change param "music" to a System.IO.FileInfo so we have
			// something a little more tangible than a string. Plus, we can
			// check file existence, etc. May not work with embedded resources
			// tho, but would allow loading external resources. Look into this.
			if (_music == null) {
				_music = new Sound();
			}
			else if (_music.Active) {
				_music.Stop();
			}
			_music.LoadEmbedded(music, true);
			_music.Volume = volume;
			_music.Survive = true;
			_music.Play();
		}

		/// <summary>
		/// Setup and play a looping background soundtrack.
		/// </summary>
		/// <param name="music">
		/// The name of the sound file you want to loop in the background.
		/// </param>
		public static void PlayMusic(String music) {
			PlayMusic(music, 1.0f);
		}

		/// <summary>
		/// Creates a new <see cref="Sound"/> from an embedded class object.
		/// </summary>
		/// <param name="embeddedSound">
		/// The desired sound to play.
		/// </param>
		/// <param name="volume">
		/// How loud to play the sound (from 0 to 1).
		/// </param>
		/// <param name="looped">
		/// Whether or not to loop the sound.
		/// </param>
		/// <returns>
		/// The new <see cref="Sound"/> object.
		/// </returns>			
		public static Sound Play(String embeddedSound, float volume, Boolean looped) {
			Int32 i = 0;
			Int32 sl = _sounds.Count;
			while (i < sl) {
				if (!_sounds[i].Active) {
					break;
				}
				i++;
			}

			if (i >= sl) {
				_sounds.Add(new Sound());
			}
			_sounds[i].LoadEmbedded(embeddedSound, looped);
			_sounds[i].Volume = volume;
			_sounds[i].Play();
			return _sounds[i];
		}

		/// <summary>
		/// Creates a new <see cref="Sound"/> from an embedded class object.
		/// </summary>
		/// <param name="embeddedSound">
		/// The desired sound to play.
		/// </param>
		/// <param name="volume">
		/// How loud to play the sound (from 0 to 1).
		/// </param>
		/// <returns>
		/// The new <see cref="Sound"/> object.
		/// </returns>
		public static Sound Play(String embeddeSound, float volume) {
			return Play(embeddeSound, volume, false);
		}

		/// <summary>
		/// Creates a new <see cref="Sound"/> from an embedded class object.
		/// </summary>
		/// <param name="embeddedSound">
		/// The desired sound to play.
		/// </param>
		/// <returns>
		/// The new <see cref="Sound"/> object.
		/// </returns>
		public static Sound Play(String embeddedSound) {
			return Play(embeddedSound, 1.0f, false);
		}

		/// <summary>
		/// Adjusts the volume levels and the music channel after a change.
		/// </summary>
		private static void ChangeSounds() {
			if (_music != null) {
				if (_music.Active) {
					_music.UpdateTransform();
				}

				foreach (Sound s in _sounds) {
					if (s.Active) {
						s.UpdateTransform();
					}
				}
			}
		}

		/// <summary>
		/// Makes sure the sounds get updated each frame.
		/// </summary>
		internal static void UpdateSounds() {
			if ((_music != null) && (_music.Active)) {
				_music.Update();
			}

			if (_sounds != null) {
				foreach (Sound s in _sounds) {
					if (s.Active) {
						s.Update();
					}
				}
			}
		}

		/// <summary>
		/// Called on state changes to stop and destroy sounds.
		/// </summary>
		/// <param name="force">
		/// Kill sounds even if they're flagged to survive.
		/// </param>
		internal static void DestroySounds(Boolean force) {
			if (_sounds == null) {
				return;
			}

			if ((_music != null) && (force) && (!_music.Survive)) {
				_music.Dispose();
			}

			foreach (Sound s in _sounds) {
				if ((force) || (!s.Survive)) {
					s.Dispose();
				}	
			}
		}

		/// <summary>
		/// Pauses all game sounds and music.
		/// </summary>
		private static void PauseSounds() {
			if ((_music != null) && (_music.Active)) {
				_music.Pause();
			}

			foreach (Sound s in _sounds) {
				if (s.Active) {
					s.Pause();
				}
			}
		}

		/// <summary>
		/// Allows all game sounds and music to play.
		/// </summary>
		private static void PlaySounds() {
			if ((_music != null) && (_music.Active)) {
				_music.Play();
			}

			foreach (Sound s in _sounds) {
				if (s.Active) {
					s.Play();
				}
			}
		}

		/// <summary>
		/// Tells the camera subsystem what object to follow.
		/// </summary>
		/// <param name="target">
		/// The object to follow.
		/// </param>
		/// <param name="lerp">
		/// How much lag the camera should have (can help smooth out the camera
		/// movement).
		/// </param>
		public static void Follow(CyFlixelObject target, float lerp) {
			_followTarget = target;
			_followLerp = lerp;
			if (target == null) {
				return;
			}

			_scroll.X = _scrollTarget.X = (_width >> 1) - _followTarget.X - ((Int32)_followTarget.Width >> 1);
			_scroll.Y = _scrollTarget.Y = (_height >> 1) - _followTarget.Y - ((Int32)_followTarget.Height >> 1);
		}

		/// <summary>
		/// Specify an additional camera component - the velocity-based "lead",
		/// or amount the camera should track in front of a sprite.
		/// </summary>
		/// <param name="leadX">
		/// Percentage of X velocity to add to the camera's motion.
		/// </param>
		/// <param name="leadY">
		/// Percentage of Y velocity to add to the camera's motion.
		/// </param>
		public static void FollowAdjust(float leadX, float leadY) {
			_followLead = new Vector2(leadX, leadY);
		}

		/// <summary>
		/// Updates the camera and parallax scrolling.
		/// </summary>
		internal static void DoFollow() {
			if (_followTarget != null) {
				if ((_followTarget.Exists) && (!_followTarget.IsDead)) {
					_scrollTarget.X = (_width >> 1) - _followTarget.X - ((Int32)_followTarget.Width >> 1);
					_scrollTarget.Y = (_height >> 1) - _followTarget.Y - ((Int32)_followTarget.Height >> 1);
					if ((_followLead != Vector2.Zero) && (_followTarget is FlixelSprite)) {
						_scrollTarget.X -= (_followTarget as FlixelSprite).Velocity.X * _followLead.X;
						_scrollTarget.Y -= (_followTarget as FlixelSprite).Velocity.Y * _followLead.Y;
					}
				}

				_scroll.X += (_scrollTarget.X - _scroll.X) * _followLerp * EngineGlobal.Elapsed;
				_scroll.Y += (_scrollTarget.Y - _scroll.Y) * _followLerp * EngineGlobal.Elapsed;
				if (_followMin != Point.Zero) {
					if (_scroll.X > _followMin.X) {
						_scroll.X = _followMin.X;
					}

					if (_scroll.Y > _followMin.Y) {
						_scroll.Y = _followMin.Y;
					}
				}

				if (_followMax != Point.Zero) {
					if (_scroll.X < _followMax.X) {
						_scroll.X = _followMax.X;
					}

					if (_scroll.Y < _followMax.Y) {
						_scroll.Y = _followMax.Y;
					}
				}
			}
		}

		/// <summary>
		/// Specifies the boundaries of the level or where the camera is allowed
		/// to move.
		/// </summary>
		/// <param name="minX">
		/// The smallest X value of your level (usually 0).
		/// </param>
		/// <param name="minY">
		/// The smallest Y value of your level (usually 0).
		/// </param>
		/// <param name="maxX">
		/// The largest X value of your level (usually the level width).
		/// </param>
		/// <param name="maxY">
		/// The largest Y value of your level (usually the level height).
		/// </param>
		/// <param name="updateWorldBounds">
		/// Whether the quad tree's dimensions should be updated to match.
		/// </param>
		public static void FollowBounds(Int32 minX, Int32 minY, Int32 maxX, Int32 maxY, Boolean updateWorldBounds) {
			_followMin = new Point(-minX, -minY);
			_followMax = new Point((-maxX + _width), (-maxY + _height));
			if (_followMax.X > _followMin.X) {
				_followMax.X = _followMin.X;
			}

			if (_followMax.Y > _followMin.Y) {
				_followMax.Y = _followMin.Y;
			}

			if (updateWorldBounds) {
				GraphicsUtils.SetWorldBounds(minX, minY, (maxX - minX), (maxY - minY));
			}
			DoFollow();
		}

		/// <summary>
		/// Specifies the boundaries of the level or where the camera is allowed
		/// to move.
		/// </summary>
		/// <param name="minX">
		/// The smallest X value of your level (usually 0).
		/// </param>
		/// <param name="minY">
		/// The smallest Y value of your level (usually 0).
		/// </param>
		/// <param name="maxX">
		/// The largest X value of your level (usually the level width).
		/// </param>
		/// <param name="maxY">
		/// The largest Y value of your level (usually the level height).
		/// </param>
		public static void FollowBounds(Int32 minX, Int32 minY, Int32 maxX, Int32 maxY) {
			FollowBounds(minX, minY, maxX, maxY, true);
		}

		/// <summary>
		/// Stops and resets the camera.
		/// </summary>
		internal static void Unfollow() {
			_followTarget = null;
			_followLead = Vector2.Zero;
			_followLerp = 1;
			_followMin = Point.Zero;
			_followMax = Point.Zero;
			_scroll = new Vector2();
			_scrollTarget = new Vector2();
		}

		/// <summary>
		/// Called by <see cref="CyFlixelGame"/> to set up <see cref="EngineGlobal"/>
		/// during <see cref="CyFlixelGame"/>'s constructor.
		/// </summary>
		/// <param name="game">Game.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		internal static void SetGameData(CyFlixelGame game, Int32 width, Int32 height) {
			_gameObject = game;
			_width = width;
			_height = height;
			_mute = false;
			_volume = 0.5f;
			Unfollow();
			_currentLevel = 0;
			_currentScore = 0;
			_pause = false;
			_timeScale = 1.0f;
			_maxElapsed = 0.0333f;
			EngineGlobal.Elapsed = 0;
			_showBounds = false;
#if !WINDOWS_PHONE
			_mobile = false;
#else
			_mobile = true;
#endif
			GraphicsUtils.SetWorldBounds(0, 0, EngineGlobal.Width, EngineGlobal.Height);
		}
		#endregion
	}
}

