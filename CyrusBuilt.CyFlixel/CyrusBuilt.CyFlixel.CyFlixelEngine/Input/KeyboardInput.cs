//
//  KeyboardInput.cs
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
	/// Handles keyboard input in-game.
	/// </summary>
	public class KeyboardInput : IDisposable
	{
		#region Constants
		/// <summary>
		/// The maximum number of inputs.
		/// </summary>
		public const Int32 MAX_INPUTS = 4;

		private const float REPEAT_TIMER_LONG = 0.5f;
		private const float REPEAT_TIMER = 0.1f;
		#endregion

		#region Fields
		private KeyboardState[] _curKeyboard = { };
		private KeyboardState[] _lastKeyboard = { };
		private float _timerCountdown = 0.0f;
		private PlayerIndex _pi;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.KeyboardInput"/>
		/// class. This is the default constructor.
		/// </summary>
		public KeyboardInput() {
			this._curKeyboard = new KeyboardState[MAX_INPUTS];
			this._lastKeyboard = new KeyboardState[MAX_INPUTS];
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the state of the current keyboard.
		/// </summary>
		public KeyboardState[] CurrentKeyboardState {
			get { return this._curKeyboard; }
		}

		/// <summary>
		/// Gets a value indicating whether the right arrow key is pressed.
		/// </summary>
		public Boolean Right {
			get { return this.IsKeyDown(Keys.Right, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the left arrow key is pressed.
		/// </summary>
		public Boolean Left {
			get { return this.IsKeyDown(Keys.Left, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the down arrow key is pressed.
		/// </summary>
		public Boolean Down {
			get { return this.IsKeyDown(Keys.Down, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the up arrow key is pressed.
		/// </summary>
		public Boolean Up {
			get { return this.IsKeyDown(Keys.Up, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether space bar was pressed.
		/// </summary>
		public Boolean Space {
			get { return this.IsKeyDown(Keys.Space, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether either ALT key was pressed.
		/// </summary>
		public Boolean Alt {
			get {
				return ((this.IsKeyDown(Keys.LeftAlt, EngineGlobal.ControllingPlayer, out this._pi)) ||
				        (this.IsKeyDown(Keys.RightAlt, EngineGlobal.ControllingPlayer, out this._pi)));
			}
		}

		/// <summary>
		/// Gets a value indicating whether either CTRL key was pressed.
		/// </summary>
		public Boolean Control {
			get {
				return ((this.IsKeyDown(Keys.LeftControl, EngineGlobal.ControllingPlayer, out this._pi)) ||
				        (this.IsKeyDown(Keys.RightControl, EngineGlobal.ControllingPlayer, out this._pi)));
			}
		}

		/// <summary>
		/// Gets a value indicating whether the forward slash (divide) key was
		/// pressed on the numeric keypad.
		/// </summary>
		public Boolean NumPadSlash {
			get { return this.IsKeyDown(Keys.Divide, EngineGlobal.ControllingPlayer, out this._pi); }
		} 

		/// <summary>
		/// Gets a value indicating whether the forward slash (divide) key was
		/// pressed.
		/// </summary>
		public Boolean ForwardSlash {
			get { return this.NumPadSlash; }
		}

		/// <summary>
		/// Gets a value indicating whether or not the period button on the
		/// numeric keypad was pressed.
		/// </summary>
		public Boolean NumPadPeriod {
			get { return this.IsKeyDown(Keys.OemPeriod, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether or not the period key was pressed.
		/// </summary>
		public Boolean Period {
			get { return this.NumPadPeriod; }
		}

		/// <summary>
		/// Gets a value indicating whether the comma key was pressed.
		/// </summary>
		public Boolean Comma {
			get { return this.IsKeyDown(Keys.OemComma, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the M key was pressed.
		/// </summary>
		public Boolean M {
			get { return this.IsKeyDown(Keys.M, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the N key was pressed.
		/// </summary>
		public Boolean N {
			get { return this.IsKeyDown(Keys.N, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the B key was pressed.
		/// </summary>
		public Boolean B {
			get { return this.IsKeyDown(Keys.B, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the V key was pressed.
		/// </summary>
		public Boolean V {
			get { return this.IsKeyDown(Keys.V, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the C key was pressed.
		/// </summary>
		public Boolean C {
			get { return this.IsKeyDown(Keys.C, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the X key was pressed.
		/// </summary>
		public Boolean X {
			get { return this.IsKeyDown(Keys.X, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Z key was pressed.
		/// </summary>
		public Boolean Z {
			get { return this.IsKeyDown(Keys.Z, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether either shift key was pressed.
		/// </summary>
		public Boolean Shift {
			get {
				return ((this.IsKeyDown(Keys.LeftShift, EngineGlobal.ControllingPlayer, out this._pi)) ||
				        (this.IsKeyDown(Keys.RightShift, EngineGlobal.ControllingPlayer, out this._pi)));
			}
		}

		/// <summary>
		/// Gets a value indicating whether the enter key was pressed.
		/// </summary>
		public Boolean Enter {
			get { return this.IsKeyDown(Keys.Enter, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the quotes key was pressed.
		/// </summary>
		public Boolean Quote {
			get { return this.IsKeyDown(Keys.OemQuotes, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the semi-colon key was pressed.
		/// </summary>
		public Boolean SemiColon {
			get { return this.IsKeyDown(Keys.OemSemicolon, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the L key was pressed.
		/// </summary>
		public Boolean L {
			get { return this.IsKeyDown(Keys.L, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the K key was pressed.
		/// </summary>
		public Boolean K {
			get { return this.IsKeyDown(Keys.K, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the J key was pressed.
		/// </summary>
		public Boolean J {
			get { return this.IsKeyDown(Keys.J, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the H key was pressed.
		/// </summary>
		public Boolean H {
			get { return this.IsKeyDown(Keys.H, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the G key was pressed.
		/// </summary>
		public Boolean G {
			get { return this.IsKeyDown(Keys.G, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// ets a value indicating whether the F key was pressed.
		/// </summary>
		public Boolean F {
			get { return this.IsKeyDown(Keys.F, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the D key was pressed.
		/// </summary>
		public Boolean D {
			get { return this.IsKeyDown(Keys.D, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the S key was pressed.
		/// </summary>
		public Boolean S {
			get { return this.IsKeyDown(Keys.S, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the A key was pressed.
		/// </summary>
		public Boolean A {
			get { return this.IsKeyDown(Keys.A, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the CAPSLOCK key was pressed.
		/// </summary>
		public Boolean Capslock {
			get { return this.IsKeyDown(Keys.CapsLock, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Backslash key was pressed.
		/// </summary>
		public Boolean Backslash {
			get { return this.IsKeyDown(Keys.OemBackslash, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the right (close) bracket key was pressed.
		/// </summary>
		public Boolean RightBracket {
			get { return this.IsKeyDown(Keys.OemCloseBrackets, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the left (opening) bracket key was pressed.
		/// </summary>
		public Boolean LeftBracket {
			get { return this.IsKeyDown(Keys.OemOpenBrackets, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the P key was pressed.
		/// </summary>
		public Boolean P {
			get { return this.IsKeyDown(Keys.P, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the O key was pressed.
		/// </summary>
		public Boolean O {
			get { return this.IsKeyDown(Keys.O, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the I key was pressed.
		/// </summary>
		public Boolean I {
			get { return this.IsKeyDown(Keys.I, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the U key was pressed.
		/// </summary>
		public Boolean U {
			get { return this.IsKeyDown(Keys.U, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Y key was pressed.
		/// </summary>
		public Boolean Y {
			get { return this.IsKeyDown(Keys.Y, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the T key was pressed.
		/// </summary>
		public Boolean T {
			get { return this.IsKeyDown(Keys.T, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the R key was pressed.
		/// </summary>
		public Boolean R {
			get { return this.IsKeyDown(Keys.R, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the E key was pressed.
		/// </summary>
		public Boolean E {
			get { return this.IsKeyDown(Keys.E, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the W key was pressed.
		/// </summary>
		public Boolean W {
			get { return this.IsKeyDown(Keys.W, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Q key was pressed.
		/// </summary>
		public Boolean Q {
			get { return this.IsKeyDown(Keys.Q, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Backspace key was pressed.
		/// </summary>
		public Boolean BackSpace {
			get { return this.IsKeyDown(Keys.Back, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Delete key was pressed.
		/// </summary>
		public Boolean Delete {
			get { return this.IsKeyDown(Keys.Delete, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the plus key on the numeric keypad
		/// was pressed.
		/// </summary>
		public Boolean NumPadPlus {
			get { return this.IsKeyDown(Keys.OemPlus, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the plus key was pressed.
		/// </summary>
		public Boolean Plus {
			get { return this.NumPadPlus; }
		}

		/// <summary>
		/// Gets a value indicating whether the minus key on the numeric keypad
		/// is pressed.
		/// </summary>
		public Boolean NumPadMinus {
			get { return this.IsKeyDown(Keys.OemMinus, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the minus key was pressed.
		/// </summary>
		public Boolean Minus {
			get { return this.IsKeyDown(Keys.OemMinus, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 0 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadZero {
			get { return this.IsKeyDown(Keys.NumPad0, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 9 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadNine {
			get { return this.IsKeyDown(Keys.NumPad9, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 8 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadEight {
			get { return this.IsKeyDown(Keys.NumPad8, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 7 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadSeven {
			get { return this.IsKeyDown(Keys.NumPad7, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 8 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadSix {
			get { return this.IsKeyDown(Keys.NumPad6, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 5 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadFive {
			get { return this.IsKeyDown(Keys.NumPad5, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 4 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadFour {
			get { return this.IsKeyDown(Keys.NumPad4, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 3 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadThree {
			get { return this.IsKeyDown(Keys.NumPad3, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 2 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadTwo {
			get { return this.IsKeyDown(Keys.NumPad2, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 1 key on the numeric keypad was
		/// pressed.
		/// </summary>
		public Boolean NumPadOne {
			get { return this.IsKeyDown(Keys.NumPad1, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 0 key was pressed.
		/// </summary>
		public Boolean Zero {
			get { return this.IsKeyDown(Keys.D0, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 9 key was pressed.
		/// </summary>
		public Boolean Nine {
			get { return this.IsKeyDown(Keys.D9, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 8 key was pressed.
		/// </summary>
		public Boolean Eight  {
			get { return this.IsKeyDown(Keys.D8, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 7 key was pressed.
		/// </summary>
		public Boolean Seven {
			get { return this.IsKeyDown(Keys.D7, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 6 key was pressed.
		/// </summary>
		public Boolean Six {
			get { return this.IsKeyDown(Keys.D6, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 5 key was pressed.
		/// </summary>
		public Boolean Five {
			get { return this.IsKeyDown(Keys.D5, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 4 key was pressed.
		/// </summary>
		public Boolean Four {
			get { return this.IsKeyDown(Keys.D4, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 3 key was pressed.
		/// </summary>
		public Boolean Three {
			get { return this.IsKeyDown(Keys.D3, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 2 key was pressed.
		/// </summary>
		public Boolean Two {
			get { return this.IsKeyDown(Keys.D2, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the 1 key was pressed.
		/// </summary>
		public Boolean One {
			get { return this.IsKeyDown(Keys.D1, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F12 key was pressed.
		/// </summary>
		public Boolean F12 {
			get { return this.IsKeyDown(Keys.F12, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F11 key was pressed.
		/// </summary>
		public Boolean F11 {
			get { return this.IsKeyDown(Keys.F11, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F10 key was pressed.
		/// </summary>
		public Boolean F10 {
			get { return this.IsKeyDown(Keys.F10, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F9 key was pressed.
		/// </summary>
		public Boolean F9 {
			get { return this.IsKeyDown(Keys.F9, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F8 key was pressed.
		/// </summary>
		public Boolean F8 {
			get { return this.IsKeyDown(Keys.F8, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F7 key was pressed.
		/// </summary>
		public Boolean F7 {
			get { return this.IsKeyDown(Keys.F7, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F6 key was pressed.
		/// </summary>
		public Boolean F6 {
			get { return this.IsKeyDown(Keys.F6, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F5 key was pressed.
		/// </summary>
		public Boolean F5 {
			get { return this.IsKeyDown(Keys.F5, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F4 key was pressed.
		/// </summary>
		public Boolean F4 {
			get { return this.IsKeyDown(Keys.F4, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F3 key was pressed.
		/// </summary>
		public Boolean F3 {
			get { return this.IsKeyDown(Keys.F3, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F2 key was pressed.
		/// </summary>
		public Boolean F2 {
			get { return this.IsKeyDown(Keys.F2, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the F1 key was pressed.
		/// </summary>
		public Boolean F1 {
			get { return this.IsKeyDown(Keys.F1, EngineGlobal.ControllingPlayer, out this._pi); }
		}

		/// <summary>
		/// Gets a value indicating whether the Escape key was pressed.
		/// </summary>
		public Boolean Escape {
			get { return this.IsKeyDown(Keys.Escape, EngineGlobal.ControllingPlayer, out this._pi); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.KeyboardInput"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.KeyboardInput"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.KeyboardInput"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you
		/// must release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.KeyboardInput"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Input.KeyboardInput"/>
		/// was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._curKeyboard != null) {
				if (this._curKeyboard.Length > 0) {
					Array.Clear(this._curKeyboard, 0, this._curKeyboard.Length);
				}
				this._curKeyboard = null;
			}

			if (this._lastKeyboard != null) {
				if (this._lastKeyboard.Length > 0) {
					Array.Clear(this._lastKeyboard, 0, this._lastKeyboard.Length);
				}
				this._lastKeyboard = null;
			}
		}

		/// <summary>
		/// Called by the menu navigation functions, and by the screen manager
		/// during screen transitions to reset the keyboard input states.
		/// </summary>
		public void Reset() {
			for (Int32 i = 0; i < MAX_INPUTS; i++) {
				this._curKeyboard[i] = this._lastKeyboard[i];
			}
		}

		/// <summary>
		/// Update keyboard timing.
		/// </summary>
		public void Update() {
			if (this._timerCountdown > 0.0f) {
				this._timerCountdown -= EngineGlobal.Elapsed;
			}

			for (Int32 i = 0; i < MAX_INPUTS; i++) {
				this._lastKeyboard[i] = this._curKeyboard[i];
				this._curKeyboard[i] = Keyboard.GetState((PlayerIndex)i);
			} 
		}

		/// <summary>
		/// Checks to see if the specified key was pressed by the specified player,
		/// or any player if the specified player did not press it.
		/// </summary>
		/// <returns>
		/// true if the key was pressed; Otherwise, false.
		/// </returns>
		/// <param name="key">
		/// The key that was pressed.
		/// </param>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the specified key, then all other players will be checked.
		/// </param>
		/// <param name="pindex">
		/// The index of the player found to have pressed the key, or null if
		/// no player pressed it.
		/// </param>
		public Boolean IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex pindex) {
			if (controllingPlayer.HasValue) {
				// Read input from the specified player.
				pindex = controllingPlayer.Value;
				Int32 i = (Int32)pindex;
				return ((this._curKeyboard[i].IsKeyDown(key)) &&
				        (this._lastKeyboard[i].IsKeyUp(key)));
			}
			else {
				// Access input from any player.
				return ((this.IsNewKeyPress(key, PlayerIndex.One, out pindex)) ||
				        (this.IsNewKeyPress(key, PlayerIndex.Two, out pindex)) ||
				        (this.IsNewKeyPress(key, PlayerIndex.Three, out pindex)) ||
				        (this.IsNewKeyPress(key, PlayerIndex.Four, out pindex)));
			}
		}

		/// <summary>
		/// Checks to see if the specified key was pressed and released by the
		/// specified player, or any player if the specified player did not
		/// press it.
		/// </summary>
		/// <returns>
		/// true if the key was released; Otherwise, false.
		/// </returns>
		/// <param name="key">
		/// The key that was released.
		/// </param>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// released the specified key, then all other players will be checked.
		/// </param>
		/// <param name="pindex">
		/// The index of the player found to have released the key, or null if
		/// no player released it.
		/// </param>
		public Boolean IsNewKeyRelease(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex pindex) {
			if (controllingPlayer.HasValue) {
				// Read input from specified player.
				pindex = controllingPlayer.Value;
				Int32 i = (Int32)pindex;
				return ((this._curKeyboard[i]).IsKeyUp(key) &&
				        (this._lastKeyboard[i].IsKeyDown(key)));
			}
			else {
				// Access input from any player.
				return ((this.IsNewKeyRelease(key, PlayerIndex.One, out pindex)) ||
				        (this.IsNewKeyRelease(key, PlayerIndex.Two, out pindex)) ||
				        (this.IsNewKeyRelease(key, PlayerIndex.Three, out pindex)) ||
				        (this.IsNewKeyRelease(key, PlayerIndex.Four, out pindex)));
			}
		}

		/// <summary>
		/// Checks to see if the specified key was pressed but not released by
		/// the specified player, or any player if the specified player did not
		/// press it.
		/// </summary>
		/// <returns>
		/// true if the key was pressed down; Otherwise, false.
		/// </returns>
		/// <param name="key">
		/// The key that was pressed down.
		/// </param>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// released the specified key, then all other players will be checked.
		/// </param>
		/// <param name="pindex">
		/// The index of the player found to have pressed the key, or null if
		/// no player released it.
		/// </param>
		public Boolean IsKeyDown(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex pindex) {
			if (controllingPlayer.HasValue) {
				pindex = controllingPlayer.Value;
				Int32 i = (Int32)pindex;
				return this._curKeyboard[i].IsKeyDown(key);
			}
			else {
				return ((this.IsKeyDown(key, PlayerIndex.One, out pindex)) ||
				        (this.IsKeyDown(key, PlayerIndex.Two, out pindex)) ||
				        (this.IsKeyDown(key, PlayerIndex.Three, out pindex)) ||
				        (this.IsKeyDown(key, PlayerIndex.Four, out pindex)));
			}
		}

		/// <summary>
		/// Checks to see if a player has selected a menu item.
		/// </summary>
		/// <returns>
		/// true if a player selected a menu item by checking to see if the
		/// specified player has pressed enter or space. The specified player
		/// did not press either button, then all other players are checked.
		/// If no players pressed either button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// released either key, then all other players will be checked.
		/// </param>
		/// <param name="pindex">
		/// The index of the player found to have pressed the key, or null if
		/// no player released it.
		/// </param>
		public Boolean IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex pindex) {
			return ((this.IsNewKeyPress(Keys.Space, controllingPlayer, out pindex)) ||
			        (this.IsNewKeyPress(Keys.Enter, controllingPlayer, out pindex)));
		}

		/// <summary>
		/// Checks to see if a player has cancelled out of a menu.
		/// </summary>
		/// <returns>
		/// true if a player cancelled from a menu by checking to see if the
		/// specified player has pressed escape. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the escape key, then all other players will be checked.
		/// </param>
		/// <param name="pindex">
		/// The index of the player found to have pressed the key, or null if
		/// no player pressed it.
		/// </param>
		public Boolean IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex pindex) {
			return this.IsNewKeyPress(Keys.Escape, controllingPlayer, out pindex);
		}

		/// <summary>
		/// Checks to see if a player has pressed up in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed up in a menu by checking to see if the
		/// specified player has the up key. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the up key, then all other players will be checked.
		/// </param>
		public Boolean IsMenuUp(PlayerIndex? controllingPlayer) {
			PlayerIndex pindex;
			if (this.IsNewKeyPress(Keys.Up, controllingPlayer, out pindex)) {
				this._timerCountdown = REPEAT_TIMER_LONG;
				return true;
			}

			KeyboardState repeatKeybdState = this._curKeyboard[(Int32)pindex];
			if (this._timerCountdown <= 0.0f) {
				if (repeatKeybdState.IsKeyDown(Keys.Up)) {
					this._timerCountdown = REPEAT_TIMER;
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
		/// specified player has the down key. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the down key, then all other players will be checked.
		/// </param>
		public Boolean IsMenuDown(PlayerIndex? controllingPlayer) {
			PlayerIndex pindex;
			if (this.IsNewKeyPress(Keys.Down, controllingPlayer, out pindex)) {
				this._timerCountdown = REPEAT_TIMER_LONG;
				return true;
			}
			
			KeyboardState repeatKeybdState = this._curKeyboard[(Int32)pindex];
			if (this._timerCountdown <= 0.0f) {
				if (repeatKeybdState.IsKeyDown(Keys.Down)) {
					this._timerCountdown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has pressed left in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed down in a menu by checking to see if the
		/// specified player has pressed the left arrow key. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the left key, then all other players will be checked.
		/// </param>
		public Boolean IsMenuLeft(PlayerIndex? controllingPlayer) {
			PlayerIndex pindex;
			if (this.IsNewKeyPress(Keys.Left, controllingPlayer, out pindex)) {
				this._timerCountdown = REPEAT_TIMER_LONG;
				return true;
			}
			
			KeyboardState repeatKeybdState = this._curKeyboard[(Int32)pindex];
			if (this._timerCountdown <= 0.0f) {
				if (repeatKeybdState.IsKeyDown(Keys.Left)) {
					this._timerCountdown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has pressed right in a menu.
		/// </summary>
		/// <returns>
		/// true if a player pressed down in a menu by checking to see if the
		/// specified player has pressed the right arrow key. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the right key, then all other players will be checked.
		/// </param>
		public Boolean IsMenuRight(PlayerIndex? controllingPlayer) {
			PlayerIndex pindex;
			if (this.IsNewKeyPress(Keys.Right, controllingPlayer, out pindex)) {
				this._timerCountdown = REPEAT_TIMER_LONG;
				return true;
			}
			
			KeyboardState repeatKeybdState = this._curKeyboard[(Int32)pindex];
			if (this._timerCountdown <= 0.0f) {
				if (repeatKeybdState.IsKeyDown(Keys.Right)) {
					this._timerCountdown = REPEAT_TIMER;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks to see if a player has paused the game by pressing the pause
		/// button (either "P" or "Pause").
		/// </summary>
		/// <returns>
		/// true if a player has paused by checking to see if the
		/// specified player has pressed the pause key. The specified player
		/// did not press the button, then all other players are checked.
		/// If no players pressed the button, then returns false.
		/// </returns>
		/// <param name="controllingPlayer">
		/// The controlling player. If this value is null or the player did not
		/// press the pause key, then all other players will be checked.
		/// </param>
		public Boolean IsPauseGame(PlayerIndex? controllingPlayer) {
			PlayerIndex pindex;
			return ((this.IsNewKeyPress(Keys.P, controllingPlayer, out pindex)) ||
			        (this.IsNewKeyPress(Keys.Pause, controllingPlayer, out pindex)));
		}

		/// <summary>
		/// Checks to see if the specified key was just pressed.
		/// </summary>
		/// <returns>
		/// true, if the key was just pressed; Otherwise, false.
		/// </returns>
		/// <param name="key">
		/// The key to check.
		/// </param>
		public Boolean JustPressed(Keys key) {
			return this.IsNewKeyPress(key, EngineGlobal.ControllingPlayer, out this._pi);
		}
		#endregion
	}
}

