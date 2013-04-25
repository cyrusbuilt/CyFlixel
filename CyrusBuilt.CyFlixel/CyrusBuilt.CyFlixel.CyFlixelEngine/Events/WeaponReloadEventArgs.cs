//
//  WeaponReloadEventArgs.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Events
{
	/// <summary>
	/// Weapon reload event arguments class.
	/// </summary>
	public class WeaponReloadEventArgs : EventArgs
	{
		#region Fields
		private Int32 _totalAmmo = 0;
		private Int32 _ammoCount = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponReloadEventArgs"/>
		/// class with the total amount of ammo remaining and the current amount
		/// of ammo (in magazine) available until next reload.
		/// </summary>
		/// <param name="totalAmmo">
		/// Total remaining ammo.
		/// </param>
		/// <param name="ammoCount">
		/// Ammo count left in magazine until next reload.
		/// </param>
		public WeaponReloadEventArgs(Int32 totalAmmo, Int32 ammoCount)
			: base() {
			this._totalAmmo = totalAmmo;
			this._ammoCount = ammoCount;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the total available ammo.
		/// </summary>
		public Int32 TotalAmmo {
			get { return this._totalAmmo; }
		}

		/// <summary>
		/// Gets the ammo count left in magazine until next reload.
		/// </summary>
		public Int32 AmmoCount {
			get { return this._ammoCount; }
		}
		#endregion
	}
}

