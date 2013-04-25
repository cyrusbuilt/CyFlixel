//
//  CyFlixelGame.cs
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CyrusBuilt.CyFlixel.CyFlixelEngine.FX;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Graphics;
using CyrusBuilt.CyFlixel.CyFlixelEngine.UI;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// Main class of the game engine. This class contains all the logic
	/// required by the main game loop to render the game screen, handle
	/// state transition, pause, etc.
	/// </summary>
	public partial class CyFlixelGame : DrawableGameComponent
	{
		#region Fields
		private GameStateBase _firstScreen = null;
		private SoundEffect _sndBeep = null;
		private RenderTarget2D _backRender = null;
		private Int32 _targetWidth = 0;
		private Int32 _targetLeft = 0;
		private Boolean _paused = false;
		private String[] _helpStrings = { };
		private Point _quakeOffset = Point.Zero;
		internal GameStateBase _state = null;
		internal GameConsole _console = null;
		internal PauseScreen _pausePanel = null;
		internal Boolean _soundTrayVisible = false;
		internal Rectangle _soundTrayRect = Rectangle.Empty;
		internal float _soundTrayTimer = 0f;
		internal FlixelSprite[] _soundTrayBars = { };
		internal TextSprite _soundCaption = null;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the game is paused.
		/// </summary>
		public event EventHandler<EventArgs> GamePaused;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelGame"/>
		/// class with all the initial game startup parameters.
		/// </summary>
		/// <param name="game">
		/// A reference to the XNA game parent class.
		/// </param>
		/// <param name="gameSizeX">
		/// The width of your game in pixels (eg. 320).
		/// </param>
		/// <param name="gameSizeY">
		/// The height of your game in pixels (eg. 240).
		/// </param>
		/// <param name="initialState">
		/// The state you want to create and switch to first (eg. MenuState).
		/// </param>
		/// <param name="bgcolor">
		/// The color of the game's background.
		/// </param>
		/// <param name="showLogo">
		/// Set true to show the game logo.
		/// </param>
		/// <param name="logoColor">
		/// The color of the great big "f" in the flixel logo.
		/// </param>
		public CyFlixelGame(Game game, Int32 gameSizeX, Int32 gameSizeY, GameStateBase initialState,
		                    Color bgcolor, Boolean showLogo, Color logoColor)
			: base(game) {
			this.InitGame(gameSizeX, gameSizeY, initialState, bgcolor, showLogo, logoColor);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelGame"/>
		/// class with a reference to the XNA game parent class.
		/// </summary>
		/// <param name="game">Game.</param>
		public CyFlixelGame(Game game)
			: base(game) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelGame"/>
		/// class. This is the default constructor.
		/// </summary>
		public CyFlixelGame() 
			: base(EngineGlobal.GameReference) {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the width of the target.
		/// </summary>
		internal Int32 TargetWidth {
			get { return this._targetWidth; }
			set { this._targetWidth = value; }
		}

		/// <summary>
		/// Gets or sets the target top-left corner of the game screen.
		/// </summary>
		internal Int32 TargetLeft {
			get { return this._targetLeft; }
			set { this._targetLeft = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelGame"/> is
		/// paused.
		/// </summary>
		internal Boolean Paused {
			get { return this._paused; }
			set {
				if (this._paused != value) {
					this._paused = value;
					this.OnGamePaused(EventArgs.Empty);
					if (!this._paused) {
						EngineGlobal.ResetInput();
					}
				}
			}
		}

		/// <summary>
		/// Gets the console.
		/// </summary>
		internal GameConsole Console {
			get { return this._console; }
		}

		/// <summary>
		/// Gets the game state.
		/// </summary>
		public GameStateBase State {
			get { return this._state; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Inits the game state.
		/// </summary>
		/// <param name="gameSizeX">
		/// The width of your game in pixels (eg. 320).
		/// </param>
		/// <param name="gameSizeY">
		/// The height of your game in pixels (eg. 240).
		/// </param>
		/// <param name="initialState">
		/// The state you want to create and switch to first (eg. MenuState).
		/// </param>
		/// <param name="bgcolor">
		/// The color of the game's background.
		/// </param>
		/// <param name="showLogo">
		/// Set true to show the game logo.
		/// </param>
		/// <param name="logoColor">
		/// The color of the great big "f" in the flixel logo.
		/// </param>
		public void InitGame(Int32 gameSizeX, Int32 gameSizeY, GameStateBase initialState,
		                     Color bgcolor, Boolean showLogo, Color logoColor) {
			EngineGlobal.BackgroundColor = bgcolor;
			EngineGlobal.SetGameData(this, gameSizeX, gameSizeY);
			this._paused = false;

			// Activate the first screen.
			if (!showLogo) {
				this._firstScreen = initialState;
			}
			else {
				Splash.SetSplashInfo(logoColor, initialState);
				this._firstScreen = new Splash();
			}
		}

		/// <summary>
		/// Raises the <see cref="GamePaused"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnGamePaused(EventArgs e) {
			if (this.GamePaused != null) {
				this.GamePaused(this, e);
			}
		}

		/// <summary>
		/// Sets up the strings that are displayed on the left side of the pause
		/// game popup.
		/// </summary>
		/// <param name="x">
		/// What to display next to the X button.
		/// </param>
		/// <param name="c">
		/// What to display next to the C button.
		/// </param>
		/// <param name="mouse">
		/// What to display next to the mouse icon.
		/// </param>
		/// <param name="arrows">
		/// What to display next to the arrows icon.
		/// </param>
		private void Help(String x, String c, String mouse, String arrows) {
			this._helpStrings = new String[4];
			if (!String.IsNullOrEmpty(x)) {
				this._helpStrings[0] = x;
			}

			if (!String.IsNullOrEmpty(c)) {
				this._helpStrings[1] = c;
			}

			if (!String.IsNullOrEmpty(mouse)) {
				this._helpStrings[2] = mouse;
			}

			if (!String.IsNullOrEmpty(arrows)) {
				this._helpStrings[3] = arrows;
			}

			if (this._pausePanel != null) {
				this._pausePanel.HelpX = this._helpStrings[0];
				this._pausePanel.HelpC = this._helpStrings[1];
				this._pausePanel.HelpMouse = this._helpStrings[2];
				this._pausePanel.HelpArrows = this._helpStrings[3];
			}
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public override void Initialize() {
			base.Initialize();
			this._backRender = new RenderTarget2D(base.GraphicsDevice, EngineGlobal.Width, EngineGlobal.Height,
			                                      false, SurfaceFormat.Color, DepthFormat.None, 0,
			                                      RenderTargetUsage.DiscardContents);
		}

		/// <summary>
		/// Initializes the console, the pause overlay, and the soundbar.
		/// </summary>
		private void InitConsole() {
			// Init console.
			this._console = new GameConsole(this._targetLeft, this._targetWidth);
			this._console.Log("CyFlixelEngine v" + EngineGlobal.EngineVersion.Major.ToString() +
			                  "." + EngineGlobal.EngineVersion.Minor.ToString());
			this._console.Log("---------------------------------------");

			// Pause screen popup.
			this._pausePanel = new PauseScreen();
			if (this._helpStrings != null) {
				this._pausePanel.HelpX = this._helpStrings[0];
				this._pausePanel.HelpC = this._helpStrings[1];
				this._pausePanel.HelpMouse = this._helpStrings[2];
				this._pausePanel.HelpArrows = this._helpStrings[3];
			}

			// Sound tray popup.
			this._soundTrayRect = new Rectangle(((EngineGlobal.Width - 80) / 2), -30, 80, 30);
			this._soundTrayVisible = false;
			this._soundCaption = new TextSprite(((EngineGlobal.Width - 80) / 2), -10, 80, "VOLUME");
			this._soundCaption.SetFormat(null, 1, Color.White, TextJustification.Center, Color.White);
			this._soundCaption.Height = 10;

			Int32 bx = 10;
			Int32 by = 14;
			FlixelSprite fp = null;
			this._soundTrayBars = new FlixelSprite[10];
			for (Int32 i = 0; i < this._soundTrayBars.Length; i++) {
				// TODO WTF?? This can't be right.... (bx * 1)? Anything times 1 is just the number.
				// ie: 3 x 1 = 3, 5 x 1 = 5, 24 x 1 = 24, etc. Derp.
				fp = new FlixelSprite((this._soundTrayRect.X + (bx * 1)), -i, null);
				fp.Width = 4;
				fp.Height = i++;
				fp.ScrollFactor = Vector2.Zero;
				this._soundTrayBars[i] = fp;
				bx += 6;
				by--;
			}
		}

		/// <summary>
		/// Loads the game content.
		/// </summary>
		protected override void LoadContent() {
			// Load up graphical content used for the engine.
			Viewport vp = base.GraphicsDevice.Viewport;
			this._targetWidth = (Int32)(vp.Height * ((float)EngineGlobal.Width / (float)EngineGlobal.Height));
			this._targetLeft = ((vp.Width - this._targetWidth) / 2);
			EngineGlobal.LoadContent(base.GraphicsDevice);
			this._sndBeep = EngineGlobal.Content.Load<SoundEffect>("cyflixel/beep");

			this.InitConsole();
			if (this._firstScreen != null) {
				EngineGlobal.State = this._firstScreen;
				this._firstScreen = null;
			}
		}

		/// <summary>
		/// Unloads the game content.
		/// </summary>
		protected override void UnloadContent() {
			if (this._sndBeep != null) {
				this._sndBeep.Dispose();
			}

			if (EngineGlobal.State != null) {
				EngineGlobal.State.Dispose();
			}
		}

		/// <summary>
		/// Switches from one game state to another (ie. transition between
		/// screens).
		/// </summary>
		/// <param name="newScreen">
		/// The state to switch to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="newScreen"/> cannot be null.
		/// </exception>
		public void SwitchState(GameStateBase newScreen) {
			if (newScreen == null) {
				throw new ArgumentNullException("newScreen");
			}

			// Stop camera follow.
			EngineGlobal.Unfollow();

			// Reset inputs.
			EngineGlobal.Keyboard.Reset();
			EngineGlobal.Gamepads.Reset();
			EngineGlobal.Mouse.Reset();

			// Stop animations.
			EngineGlobal.ScreenFlash.Stop();
			EngineGlobal.ScreenFade.Stop();
			EngineGlobal.ScreenQuake.Stop();

			// Reset state.
			if (this._state != null) {
				this._state.Dispose();
			}

			this._state = newScreen;
			this._state.Create();
		}

		/// <summary>
		/// Shows the sound tray.
		/// </summary>
		private void ShowSoundTray() {
			if (!EngineGlobal.Mute) {
				this._sndBeep.Play(EngineGlobal.Volume, 0f, 0f);
			}

			this._soundTrayTimer = 1;
			this._soundTrayRect.Y = 0;
			this._soundTrayVisible = true;
			this._soundCaption.Y = (this._soundTrayRect.Y + 4);
			Int32 gv = (Int32)Math.Round(EngineGlobal.Volume * 10);
			if (EngineGlobal.Mute) {
				gv = 0;
			}

			for (Int32 i = 0; i < this._soundTrayBars.Length; i++) {
				this._soundTrayBars[i].Y = (this._soundTrayRect.Y + this._soundTrayRect.Height -
				                            this._soundTrayBars[i].Height - 2);
				if (i < gv) {
					this._soundTrayBars[i].Alpha = 1;
				}
				else {
					this._soundTrayBars[i].Alpha = 0.5f;
				}
			}
		}

		/// <summary>
		/// This is the primary game update logic. This should be called from
		/// the main game loop (executed each frame) to update game state,
		/// run time, score, camera, etc. as well as poll inputs.
		/// </summary>
		/// <param name="gameTime">
		/// Game timing state.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="gameTime"/> cannot be null.
		/// </exception>
		public override void Update(GameTime gameTime) {
			if (gameTime == null) {
				throw new ArgumentNullException("gameTime");
			}

			PlayerIndex pi = PlayerIndex.One;

			// Frame timing.
			EngineGlobal.EngineTimer = (uint)gameTime.TotalGameTime.TotalMilliseconds;
			EngineGlobal.Elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (EngineGlobal.Elapsed > EngineGlobal.MaxElapsed) {
				EngineGlobal.Elapsed = EngineGlobal.MaxElapsed;
			}
			EngineGlobal.Elapsed *= EngineGlobal.TimeScale;

			// Animate HUD elements.
			this._console.Update();
			if (this._soundTrayTimer > 0) {
				this._soundTrayTimer -= EngineGlobal.Elapsed;
			}
			else if (this._soundTrayRect.Y > -this._soundTrayRect.Height) {
				this._soundTrayRect.Y -= (Int32)(EngineGlobal.Elapsed * EngineGlobal.Height * 2);
				this._soundCaption.Y = (this._soundTrayRect.Y + 4);
				for (Int32 i = 0; i < this._soundTrayBars.Length; i++) {
					this._soundTrayBars[i].Y = (this._soundTrayRect.Y + this._soundTrayRect.Height -
					                            this._soundTrayBars[i].Height - 2);
				}

				if (this._soundTrayRect.Y < -this._soundTrayRect.Height) {
					this._soundTrayVisible = false;
				}
			}

			// State updating.
			EngineGlobal.Keyboard.Update();
			EngineGlobal.Gamepads.Update();
			EngineGlobal.Mouse.Update();
			EngineGlobal.UpdateSounds();

			if (EngineGlobal.Keyboard.IsNewKeyPress(Keys.D0, null, out pi)) {
				EngineGlobal.Mute = !EngineGlobal.Mute;
				this.ShowSoundTray();
			}
			else if (EngineGlobal.Keyboard.IsNewKeyPress(Keys.OemMinus, null, out pi)) {
				EngineGlobal.Mute = false;
				EngineGlobal.Volume -= 0.1f;
				this.ShowSoundTray();
			}
			else if (EngineGlobal.Keyboard.IsNewKeyPress(Keys.OemPlus, null, out pi)) {
				EngineGlobal.Mute = false;
				EngineGlobal.Volume += 0.1f;
				this.ShowSoundTray();
			}
			else if ((EngineGlobal.Keyboard.IsNewKeyPress(Keys.D1, null, out pi)) ||
			         (EngineGlobal.Keyboard.IsNewKeyPress(Keys.OemTilde, null, out pi))) {
				this._console.Toggle();
			}
			else if (((EngineGlobal.AutoHandlePause) && (EngineGlobal.Keyboard.IsPauseGame(EngineGlobal.ControllingPlayer))) ||
			         EngineGlobal.Gamepads.IsPauseGame(EngineGlobal.ControllingPlayer)) {
				EngineGlobal.Pause = !EngineGlobal.Pause;
			}

			if (this._paused) {
				return;
			}

			if (EngineGlobal.State != null) {
				// Update the camera and game state.
				EngineGlobal.DoFollow();
				EngineGlobal.State.Update();

				// Update special FX.
				if (EngineGlobal.ScreenFlash.Exists) {
					EngineGlobal.ScreenFlash.Update();
				}

				if (EngineGlobal.ScreenFade.Exists) {
					EngineGlobal.ScreenFade.Update();
				}

				EngineGlobal.ScreenQuake.Update();
				this._quakeOffset.X = EngineGlobal.ScreenQuake.X;
				this._quakeOffset.Y = EngineGlobal.ScreenQuake.Y;
			}
		}

		/// <summary>
		/// Draws everything on screen that needs to be.
		/// </summary>
		/// <param name="gameTime">
		/// Game timing state. NOTE: This parameter is ignored in this overload.
		/// Value can be null.
		/// </param>
		public override void Draw(GameTime gameTime) {
			// Render the screen to our internal game-sized back buffer.
			base.GraphicsDevice.SetRenderTarget(this._backRender);
			if (EngineGlobal.State != null) {
				EngineGlobal.State.PreProcess(EngineGlobal.SpriteBatch);
				EngineGlobal.State.Render(EngineGlobal.SpriteBatch);

				EngineGlobal.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
				                               SamplerState.PointClamp, DepthStencilState.None,
				                               RasterizerState.CullCounterClockwise);
				if (EngineGlobal.ScreenFlash.Exists) {
					EngineGlobal.ScreenFlash.Render(EngineGlobal.SpriteBatch);
				}

				if (EngineGlobal.ScreenFade.Exists) {
					EngineGlobal.ScreenFade.Render(EngineGlobal.SpriteBatch);
				}

				if (EngineGlobal.Mouse.Cursor.Visible) {
					EngineGlobal.Mouse.Cursor.Render(EngineGlobal.SpriteBatch);
				}

				EngineGlobal.SpriteBatch.End();
				EngineGlobal.State.PostProcess(EngineGlobal.SpriteBatch);
			}

			// Render sound tray if necessary.
			if ((this._soundTrayVisible) || (this._paused)) {
				EngineGlobal.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
				                               SamplerState.PointClamp, DepthStencilState.None,
				                               RasterizerState.CullCounterClockwise);
				if (this._soundTrayVisible) {
					Rectangle trayRect = new Rectangle(1, 1, 1, 1);
					EngineGlobal.SpriteBatch.Draw(EngineGlobal.XnaSheet, this._soundTrayRect,
					                              trayRect, this._console.ConsoleColor);
					this._soundCaption.Render(EngineGlobal.SpriteBatch);
					for (Int32 i = 0; i < this._soundTrayBars.Length; i++) {
						this._soundTrayBars[i].Render(EngineGlobal.SpriteBatch);
					}
				}

				if (this._paused) {
					this._pausePanel.Render(EngineGlobal.SpriteBatch);
				}
				EngineGlobal.SpriteBatch.End();
			}

			// Copy the result to the screen, scaled to fit.
			base.GraphicsDevice.SetRenderTarget(null);
			base.GraphicsDevice.Clear(EngineGlobal.BackgroundColor);
			EngineGlobal.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
			                               SamplerState.PointClamp, DepthStencilState.None,
			                               RasterizerState.CullCounterClockwise);

			Rectangle srect = new Rectangle((this._targetLeft + this._quakeOffset.X), this._quakeOffset.Y,
			                                this._targetWidth, base.GraphicsDevice.Viewport.Height);
			EngineGlobal.SpriteBatch.Draw(this._backRender, srect, Color.White);

			// Render console if necessary.
			if (this._console.Visible) {
				this._console.Render(EngineGlobal.SpriteBatch);
			}
			EngineGlobal.SpriteBatch.End();
		}
		#endregion
	}
}

