//
//  WeaponFireEventArgs.cs
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
	/// Weapon fire event arguments class.
	/// </summary>
	public class WeaponFireEventArgs : EventArgs
	{
		#region Fields
		private WeaponTypes _weaponType = WeaponTypes.None;
		private Int32 _ammoCount = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponFireEventArgs"/>
		/// class with the type of weapon being fired and the amount of ammo
		/// remaining in the magazine.
		/// </summary>
		/// <param name="weaponType">
		/// Weapon type.
		/// </param>
		/// <param name="ammoCount">
		/// Ammo count.
		/// </param>
		public WeaponFireEventArgs(WeaponTypes weaponType, Int32 ammoCount)
			: base() {
			this._weaponType = weaponType;
			this._ammoCount = ammoCount;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the type of the weapon.
		/// </summary>
		public WeaponTypes WeaponType {
			get { return this._weaponType; }
		}

		/// <summary>
		/// Gets the ammo count.
		/// </summary>
		public Int32 AmmoCount {
			get { return this._ammoCount; }
		}
		#endregion
	}
}

