//
//  GamepadInput.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Input
{
	/// <summary>
	/// Handles gamepad input in-game.
	/// </summary>
	public class GamepadInput : IDisposable
	{
		#region Constants
		/// <summary>
		/// The maximum number of player gamepad inputs.
		/// </summary>
		public const Int32 MAX_INPUTS = 4;

		private const float REPEAT_TIMER = 0.1f;
		private const float REPEAT_TIMER_LONG = 0.5f;
		#endregion

		#region Fields
		private PlayerIndex _pi;
		private GamePadState[] _currentGamePad = null;
		private GamePadState[] _lastGamePad = null;
		private float _timerCountDown = 0.0f;
		private readonly Boolean[] _gamePadWasConnected = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.GamepadInput"/>
		/// class. This is the default constructor.
		/// </summary>
		public GamepadInput() {
			this._currentGamePad = new GamePadState[MAX_INPUTS];
			this._lastGamePad = new GamePadState[MAX_INPUTS];
			this._gamePadWasConnected = new Boolean[MAX_INPUTS];
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the state of the current game pad.
		/// </summary>
		public GamePadState[] CurrentGamePadState {
			get { return this._currentGamePad; }
		}

		/// <summary>
		/// Tracks which gamepads are connected.
		/// </summary>
		public Boolean[] GamePadWasConnected {
			get { return this._gamePadWasConnected; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.GamepadInput"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.GamepadInput"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.GamepadInput"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.GamepadInput"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.GamepadInput"/>
		/// was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._currentGamePad != null) {
				if (this._currentGamePad.Length > 0) {
					Array.Clear(this._currentGamePad, 0, this._currentGamePad.Length);
				}
				this._currentGamePad = null;
			}

			if (this._lastGamePad != null) {
				if (this._lastGamePad.Length > 0) {
					Array.Clear(this._lastGamePad, 0, this._lastGamePad.Length);
				}
				this._lastGamePad = null;
			}
		}

		/// <summary>
		/// Called by the game loop to refresh the counter and track which
		/// controllers are connected.
		/// </summary>
		public void Update() {
			if (this._timerCountDown > 0.0f) {
				this._timerCountDown -= EngineGlobal.Elapsed;
			}

			// Keep track of whether a gamepad has ever been connected, so we
			// can detect if it is unplugged.
			for (Int32 i = 0; i < MAX_INPUTS; i++) {
				this._lastGamePad[i] = this._currentGamePad[i];
				this._currentGamePad[i] = GamePad.GetState((PlayerIndex)i);
				if (this._currentGamePad[i].IsConnected) {
					this._gamePadWasConnected[i] = true;
				}
			}
		}

		/// <summary>
		/// Checks to see if the specified button was pressed by the specified player,
		/// or any player if the specified player did not press it.
		/// </summary>
		/// <returns>
		/// true if the key was pressed; Otherwise, false.
		/// </returns>
		/// <param name="button">
		/// The button that was pressed.
		/// </param>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the specified button, then all other players will be checked.
		/// </param>
		/// <param name="pi">
		/// The index of the player found to have pressed the button, or null if
		/// no player pressed it.
		/// </param>
		public Boolean IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex pi) {
			if 	(controllingPlayer.HasValue) {
				pi = controllingPlayer.Value;
				Int32 i = (Int32)pi;
				return ((this._currentGamePad[i].IsButtonDown(button)) &&
				        (this._lastGamePad[i].IsButtonUp(button)));
			}
			else {
				return ((this.IsNewButtonPress(button, PlayerIndex.One, out pi)) ||
				        (this.IsNewButtonPress(button, PlayerIndex.Two, out pi)) ||
				        (this.IsNewButtonPress(button, PlayerIndex.Three, out pi)) ||
				        (this.IsNewButtonPress(button, PlayerIndex.Four, out pi)));
			}
		}

		/// <summary>
		/// Checks to see if the specified button was pressed by the specified player,
		/// or any player if the current player did not press it.
		/// </summary>
		/// <returns>
		/// true if the button was pressed; Otherwise, false.
		/// </returns>
		/// <param name="button">
		/// The button that was pressed.
		/// </param>
		public Boolean IsNewButtonPress(Buttons button) {
			return this.IsNewButtonPress(button, EngineGlobal.ControllingPlayer, out this._pi);
		}

		/// <summary>
		/// Checks to see if the specified button was released by the specified player,
		/// or any player if the specified player did not release it.
		/// </summary>
		/// <returns>
		/// true if the button was released; Otherwise, false.
		/// </returns>
		/// <param name="button">
		/// The button that was released.
		/// </param>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// release the specified button, then all other players will be checked.
		/// </param>
		/// <param name="pi">
		/// The index of the player found to have release the button, or null if
		/// no player pressed it.
		/// </param>
		public Boolean IsNewButtonRelease(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex pi) {
			if (controllingPlayer.HasValue) {
				pi = controllingPlayer.Value;
				Int32 i = (Int32)pi;
				return ((this._currentGamePad[i].IsButtonUp(button)) &&
				        (this._lastGamePad[i].IsButtonDown(button)));
			}
			else {
				return ((this.IsNewButtonRelease(button, PlayerIndex.One, out pi)) ||
				        (this.IsNewButtonRelease(button, PlayerIndex.Two, out pi)) ||
				        (this.IsNewButtonRelease(button, PlayerIndex.Three, out pi)) ||
				        (this.IsNewButtonRelease(button, PlayerIndex.Four, out pi)));
			}
		}

		/// <summary>
		/// Checks to see if the specified button was released by the specified player,
		/// or any player if the current player did not release it.
		/// </summary>
		/// <returns>
		/// true if the button was released; Otherwise, false.
		/// </returns>
		/// <param name="button">
		/// The button that was released.
		/// </param>
		public Boolean IsNewButtonRelease(Buttons button) {
			return this.IsNewButtonRelease(button, EngineGlobal.ControllingPlayer, out this._pi);
		}

		/// <summary>
		/// Checks to see if the specified button was pressed down by the
		/// specified player, or any player if the specified player did not
		/// press it.
		/// </summary>
		/// <returns>
		/// true if the button was pressed; Otherwise, false.
		/// </returns>
		/// <param name="button">
		/// The button that was pressed.
		/// </param>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// pressed the specified button, then all other players will be checked.
		/// </param>
		/// <param name="pi">
		/// The index of the player found to have pressed the button, or null if
		/// no player pressed it.
		/// </param>
		public Boolean IsButtonDown(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex pi) {
			if (controllingPlayer.HasValue) {
				pi = controllingPlayer.Value;
				Int32 i = (Int32)pi;
				Boolean rv = false;
				switch (button) {
					case Buttons.LeftThumbstickLeft:
						rv = (this._currentGamePad[i].ThumbSticks.Left.X < -0.25);
						break;
					case Buttons.LeftThumbstickRight:
						rv = (this._currentGamePad[i].ThumbSticks.Left.X > 0.25);
						break;
					case Buttons.RightThumbstickUp:
						rv = (this._currentGamePad[i].ThumbSticks.Left.Y > 0.25);
						break;
					case Buttons.LeftThumbstickDown:
						rv = (this._currentGamePad[i].ThumbSticks.Left.Y < -0.25);
						break;
					default:
						rv = this._currentGamePad[i].IsButtonDown(button);
						break;
				}
				return rv;
			}
			else {
				return ((this.IsButtonDown(button, PlayerIndex.One, out pi)) ||
				        (this.IsButtonDown(button, PlayerIndex.Two, out pi)) ||
				        (this.IsButtonDown(button, PlayerIndex.Three, out pi)) ||
				        (this.IsButtonDown(button, PlayerIndex.Four, out pi)));
			}
		}

		/// <summary>
		/// Checks to see if the specified button was pressed down by the
		/// specified player, or any player if the specified player did not
		/// press it.
		/// </summary>
		/// <returns>
		/// true if the button was pressed; Otherwise, false.
		/// </returns>
		/// <param name="button">
		/// The button that was pressed.
		/// </param>
		public Boolean IsButtonDown(Buttons button) {
			return this.IsButtonDown(button, EngineGlobal.ControllingPlayer, out this._pi);
		}

		/// <summary>
		/// Called by the menu navigation functions, and by the screen manager
		/// during screen transitions to reset the gamepad input states.
		/// </summary>
		public void Reset() {
			for (Int32 i = 0; i < MAX_INPUTS; i++) {
				this._currentGamePad[i] = this._lastGamePad[i];
			}
		}

		/// <summary>
		/// Checks to see if a player has selected a menu item.
		/// </summary>
		/// <returns>
		/// true if a player selected a menu item by checking to see if the
		/// specified player has pressed the Start button or A button. The
		/// specified player did not press either button, then all other players
		/// are checked. If no players pressed either button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the button, then all other players will be checked.
		/// </param>
		/// <param name="pi">
		/// The index of the player found to have pressed the key, or null if
		/// no player released it.
		/// </param>
		public Boolean IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex pi) {
			return ((this.IsNewButtonPress(Buttons.A, controllingPlayer, out pi)) ||
			        (this.IsNewButtonPress(Buttons.Start, controllingPlayer, out pi)));
		}

		/// <summary>
		/// Checks to see if a player has cancelled out of a menu.
		/// </summary>
		/// <returns>
		/// true if a player cancelled from a menu by checking to see if the
		/// specified player has pressed back. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the back button, then all other players will be checked.
		/// </param>
		/// <param name="pi">
		/// The index of the player found to have pressed the button, or null if
		/// no player pressed it.''''
		/// LL
		/// </param>
		public Boolean IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex pi) {
			return ((this.IsNewButtonPress(Buttons.B, controllingPlayer, out pi)) ||
			        (this.IsNewButtonPress(Buttons.Back, controllingPlayer, out pi)));
		}

		/// <summary>
		/// Checks to see if a player has pressed up in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed up in a menu by checking to see if the
		/// specified player has the up on the D-pad or on the left thumbstick.
		/// If the specified player did not press the button, then all other
		/// players are checked. If no players pressed the button, then returns
		/// false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the up button, then all other players will be checked.
		/// </param>
		public Boolean IsMenuUp(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out pi)) {
				this._timerCountDown = REPEAT_TIMER_LONG;
				return true;
			}
			else if (this.IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out pi)) {
				GamePadState gs = this._currentGamePad[(Int32)pi];
				if (gs.ThumbSticks.Left.Y > 0.25) {
					this._timerCountDown = REPEAT_TIMER_LONG;
					return true;
				}
				else {
					this.Reset();
				}
			}

			GamePadState repeatPadState = this._currentGamePad[(Int32)pi];
			if (this._timerCountDown <= 0.0f) {
				if ((repeatPadState.ThumbSticks.Left.Y > 0.25) ||
				    (repeatPadState.IsButtonDown(Buttons.DPadUp))) {
					this._timerCountDown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has pressed down in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed down in a menu by checking to see if the
		/// specified player has pressed down on the D-pad or on the left
		/// thumbstick. If the specified player did not press the button, then
		/// all other players are checked. If no players pressed the button,
		/// then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the down button, then all other players will be checked.
		/// </param>
		public Boolean IsMenuDown(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out pi)) {
				this._timerCountDown = REPEAT_TIMER_LONG;
				return true;
			}
			else if (this.IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out pi)) {
				GamePadState gs = this._currentGamePad[(Int32)pi];
				if (gs.ThumbSticks.Left.Y < -0.25) {
					this._timerCountDown = REPEAT_TIMER_LONG;
					return true;
				}
				else {
					this.Reset();
				}
			}

			GamePadState repeatPadState = this._currentGamePad[(Int32)pi];
			if (this._timerCountDown <= 0.0f) {
				if ((repeatPadState.ThumbSticks.Left.Y < -0.25f) ||
				    (repeatPadState.IsButtonDown(Buttons.DPadDown))) {
					this._timerCountDown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has pressed left in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed left in a menu by checking to see if the
		/// specified player has pressed the left on the D-pad or left on the
		/// left thumbstick. The specified player did not press the button,
		/// then all other players are checked. If no players pressed the button,
		/// then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the left button, then all other players will be checked.
		/// </param>
		public Boolean IsMenuLeft(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsNewButtonPress(Buttons.DPadLeft, controllingPlayer, out pi)) {
				this._timerCountDown = REPEAT_TIMER_LONG;
				return true;
			}
			else if (this.IsNewButtonPress(Buttons.LeftThumbstickLeft, controllingPlayer, out pi)) {
				GamePadState gs = this._currentGamePad[(Int32)pi];
				if (gs.ThumbSticks.Left.X < -0.25) {
					this._timerCountDown = REPEAT_TIMER_LONG;
					return true;
				}
				else {
					this.Reset();
				}
			}

			GamePadState repeatPadState = this._currentGamePad[(Int32)pi];
			if (this._timerCountDown <= 0.0f) {
				if ((repeatPadState.ThumbSticks.Left.X < -0.25) ||
				    (repeatPadState.IsButtonDown(Buttons.DPadLeft))) {
					this._timerCountDown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has pressed right in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed right in a menu by checking to see if the
		/// specified player has pressed the right on the D-pad or right on the
		/// left thumbstick. The specified player did not press the button,
		/// then all other players are checked. If no players pressed the button,
		/// then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the right button, then all other players will be checked.
		/// </param>
		public Boolean IsMenuRight(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsNewButtonPress(Buttons.DPadRight, controllingPlayer, out pi)) {
				this._timerCountDown = REPEAT_TIMER_LONG;
				return true;
			}
			else if (this.IsNewButtonPress(Buttons.LeftThumbstickRight, controllingPlayer, out pi)) {
				GamePadState gs = this._currentGamePad[(Int32)pi];
				if (gs.ThumbSticks.Left.X > 0.25) {
					this._timerCountDown = REPEAT_TIMER_LONG;
					return true;
				}
				else {
					this.Reset();
				}
			}

			GamePadState repeatPadState = this._currentGamePad[(Int32)pi];
			if (this._timerCountDown <= 0.0f) {
				if ((repeatPadState.ThumbSticks.Left.X > 0.25) ||
				    (repeatPadState.IsButtonDown(Buttons.DPadRight))) {
					this._timerCountDown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has paused the game by pressing the pause
		/// button ("Start").
		/// </summary>
		/// <returns>
		/// true if a player has paused by checking to see if the
		/// specified player has pressed the pause button. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the pause button, then all other players will be checked.
		/// </param>
		public Boolean IsPauseGame(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			return this.IsNewButtonPress(Buttons.Start, controllingPlayer, out pi);
		}

		/// <summary>
		/// Checks to see if the specified player moved the left thumbstick
		/// to the left.
		/// </summary>
		/// <returns>
		/// true if the left thumbstick was moved left by either the specified
		/// player or any other player.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or if the specified
		/// player did not move, then the other players will be checked.
		/// </param>
		public Boolean IsNewThumbStickLeft(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsButtonDown(Buttons.LeftThumbstickLeft, controllingPlayer, out pi)) {
				if ((this._currentGamePad[(Int32)pi].ThumbSticks.Left.X < -0.25) &&
				    (this._lastGamePad[(Int32)pi].ThumbSticks.Left.X >= -0.25)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if the specified player moved the left thumbstick
		/// to the right.
		/// </summary>
		/// <returns>
		/// true if the left thumbstick was moved right by either the specified
		/// player or any other player.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or if the specified
		/// player did not move, then the other players will be checked.
		/// </param>
		public Boolean IsNewThumbStickRight(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsButtonDown(Buttons.LeftThumbstickRight, controllingPlayer, out pi)) {
				if ((this._currentGamePad[(Int32)pi].ThumbSticks.Left.X > 0.25) &&
				    (this._lastGamePad[(Int32)pi].ThumbSticks.Left.X <= 0.25)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if the specified player moved the left thumbstick up.
		/// </summary>
		/// <returns>
		/// true if the left thumbstick was moved up by either the specified
		/// player or any other player.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or if the specified
		/// player did not move up, then the other players will be checked.
		/// </param>
		public Boolean IsNewThumbStickUp(PlayerIndex? controlllingPlayer) {
			PlayerIndex pi;
			if (this.IsButtonDown(Buttons.LeftThumbstickUp, controlllingPlayer, out pi)) {
				if ((this._currentGamePad[(Int32)pi].ThumbSticks.Left.Y > 0.25) &&
				    (this._lastGamePad[(Int32)pi].ThumbSticks.Left.Y <= 0.25)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if the specified player moved the left thumbstick down.
		/// </summary>
		/// <returns>
		/// true if the left thumbstick was moved down by either the specified
		/// player or any other player.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or if the specified
		/// player did not move down, then the other players will be checked.
		/// </param>
		public Boolean IsNewThumbStickDown(PlayerIndex? controllingPlayer) {
			PlayerIndex pi;
			if (this.IsButtonDown(Buttons.LeftThumbstickDown, controllingPlayer, out pi)) {
				if ((this._currentGamePad[(Int32)pi].ThumbSticks.Left.Y < -0.25) &&
				    (this._lastGamePad[(Int32)pi].ThumbSticks.Left.Y >= -.025)) {
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}

