//
//  WeaponAddedEventArgs.cs
//
//  Author:
//       chris brunner <cyrusbuilt at gmail dot com>
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Events
{
	/// <summary>
	/// Weapon added event arguments class.
	/// </summary>
	public class WeaponAddedEventArgs : EventArgs
	{
		#region Fields
		private Int32 _weaponIndex = -1;
		private Int32 _weaponCount = 0;
		private WeaponTypes _type = WeaponTypes.None;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponAddedEventArgs"/>
		/// class with the index of the weapon in the inventory and weapon type.
		/// </summary>
		/// <param name="weaponIndex">
		/// The zero-based index of the weapon in inventory.
		/// </param>
		/// <param name="type">
		/// The type of weapon added.
		/// </param>
		public WeaponAddedEventArgs(Int32 weaponIndex, WeaponTypes type)
			: base() {
			this._weaponIndex = weaponIndex;
			this._type = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponAddedEventArgs"/>
		/// class with the index of the weapon in the inventory, weapon type,
		/// and total count of weapons in inventory.
		/// </summary>
		/// <param name="weaponIndex">
		/// The zero-based index of the weapon in inventory.
		/// </param>
		/// <param name="type">
		/// The type of weapon added.
		/// </param>
		/// <param name="weaponCount">
		/// The total count of weapons in inventory.
		/// </param>
		public WeaponAddedEventArgs(Int32 weaponIndex, WeaponTypes type, Int32 weaponCount)
			: base() {
			this._weaponIndex = weaponIndex;
			this._weaponCount = weaponCount;
			this._type = type;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the index of the weapon in inventory.
		/// </summary>
		public Int32 WeaponIndex {
			get { return this._weaponIndex; }
		}

		/// <summary>
		/// Gets the type of the weapon.
		/// </summary>
		public WeaponTypes WeaponType {
			get { return this._type; }
		}

		/// <summary>
		/// Gets the count of weapons in inventory.
		/// </summary>
		public Int32 Count {
			get { return this._weaponCount; }
		}
		#endregion
	}
}

