//
//  SoundManager.cs
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
using Microsoft.Xna.Framework.Audio;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia
{
	/// <summary>
	/// Manages game sound effects.
	/// </summary>
	public static class SoundManager
	{
		#region Fields
		private static List<SoundEffect> _sounds = null;
		private static Boolean _initialized = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Multimedia.SoundManager"/>
		/// is initialized.
		/// </summary>
		public static Boolean Initialized {
			get { return _initialized; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initializes the sound manager.
		/// </summary>
		/// <remarks>
		/// This method MUST be called prior to calling <see cref="GetSound"/>.
		/// </remarks>			
		public static void Init() {
			if (!_initialized) {
				_sounds = new List<SoundEffect>();
				_initialized = true;
			}
		}

		/// <summary>
		/// Attempts to retrieve a <see cref="SoundEffectInstance"/> of the
		/// specified sound effect. For performance, the in-memory sound cache
		/// is searched first. If the requested sound effect does not exist in
		/// the sound cache, then the content manager will be searched. If the
		/// sound effect is found, then it will be loaded into the sound cache
		/// prior to returning an instance.
		/// </summary>
		/// <returns>
		/// If successful, an instance of the requested sound effect; Otherwise,
		/// null if a matching sound effect could not be located in the sound
		/// cache or the content manager.
		/// </returns>
		/// <param name="embeddedSound">
		/// The name of the embedded sound to retrieve.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="embeddedSound"/> cannot be a null or empty string.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Sound manager not initialized. Call <see cref="Init()"/> first.
		/// </exception>
		public static SoundEffectInstance GetSound(String embeddedSound) {
			if (String.IsNullOrEmpty(embeddedSound)) {
				throw new ArgumentNullException("embeddedSound");
			}

			if (!_initialized) {
				throw new InvalidOperationException("You call Init() prior to accessing the sound cache.");
			}

			SoundEffectInstance sei = null;
			foreach (SoundEffect sndeffect in _sounds) {
				if (sndeffect.Name.CompareTo(embeddedSound) == 0) {
					sei = sndeffect.CreateInstance();
				}
			}

			if (sei != null) {
				return sei;
			}

			try {
				_sounds.Add(EngineGlobal.Content.Load<SoundEffect>(embeddedSound));
				_sounds[_sounds.Count - 1].Name = embeddedSound;
				return _sounds[_sounds.Count - 1].CreateInstance();
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// Shuts down the sound manager. This will stop playing any sounds in
		/// the sound cache and dispose them, then dispose the sound cache itself.
		/// </summary>
		public static void Shutdown() {
			if (_initialized) {
				if (_sounds != null) {
					foreach (SoundEffect se in _sounds) {
						se.Dispose();
					}
					_sounds.Clear();
					_sounds = null;
				}
				_initialized = false;
			}
		}
		#endregion
	}
}

