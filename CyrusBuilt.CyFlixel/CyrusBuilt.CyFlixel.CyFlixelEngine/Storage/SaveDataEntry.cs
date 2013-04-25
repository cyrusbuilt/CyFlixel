//
//  SaveDataEntry.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Storage
{
	/// <summary>
	/// Save data entry. Used to store a data entry to be stored in a save game.
	/// </summary>
	public sealed class SaveDataEntry
	{
		// TODO I feel like 'value' should probably be an object instead.
		// We would need to refactor SaveData and GameStorage if we change it tho.

		#region Fields
		private String _key = String.Empty;
		private String _value = String.Empty;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.SaveDataEntry"/>
		/// class. This is the default constructor.
		/// </summary>
		public SaveDataEntry() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.SaveDataEntry"/>
		/// class the key name and value to store.
		/// </summary>
		/// <param name="key">
		/// The key name.
		/// </param>
		/// <param name="val">
		/// The value to store.
		/// </param>
		public SaveDataEntry(String key, String val) {
			this._key = key;
			this._value = val;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the key name.
		/// </summary>
		public String Key {
			get { return this._key; }
			set { this._value = value; }
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public String Value {
			get { return this._value; }
			set { this._value = value; }
		}
		#endregion
	}
}

