//
//  WeaponEmptyEventArgs.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Events
{
	/// <summary>
	/// Weapon empty event arguments class.
	/// </summary>
	public class WeaponEmptyEventArgs : EventArgs
	{
		private WeaponTypes _weaponType = WeaponTypes.None;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponEmptyEventArgs"/>
		/// class with the type of weapon that went dry.
		/// </summary>
		/// <param name="weaponType">
		/// Weapon type.
		/// </param>
		public WeaponEmptyEventArgs(WeaponTypes weaponType)
			: base() {
			this._weaponType = weaponType;
		}

		/// <summary>
		/// Gets the type of the weapon.
		/// </summary>
		public WeaponTypes WeaponType {
			get { return this._weaponType; }
		}
	}
}

