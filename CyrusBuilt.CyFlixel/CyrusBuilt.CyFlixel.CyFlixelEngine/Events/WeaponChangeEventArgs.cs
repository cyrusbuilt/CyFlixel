//
//  WeaponChangeEventArgs.cs
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
	/// Weapon change event arguments class.
	/// </summary>
	public class WeaponChangeEventArgs : EventArgs
	{
		#region Fields
		private Int32 _weaponIndex = -1;
		private Int32 _lastIndex = -1;
		private WeaponSprite _selected = null;
		private WeaponSprite _lastSelected = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponChangeEventArgs"/>
		/// class with the index of the selected weapon along with a reference
		/// to the actual selected weapon.
		/// </summary>
		/// <param name="weaponIndex">
		/// The zero-based index of the selected weapon.
		/// </param>
		/// <param name="selected">
		/// A reference to the selected weapon instance.
		/// </param>
		public WeaponChangeEventArgs(Int32 weaponIndex, WeaponSprite selected)
			: base() {
			this._weaponIndex = weaponIndex;
			this._selected = selected;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.WeaponChangeEventArgs"/>
		/// class with the index of the selected weapon along with a reference
		/// to the actual selected weapon as well as the index and instance
		/// reference to the last weapon that was selected.
		/// </summary>
		/// <param name="weaponIndex">
		/// The zero-based index of the selected weapon.
		/// </param>
		/// <param name="selected">
		/// A reference to the selected weapon instance.
		/// </param>
		/// <param name="lastIndex">
		/// The zero-based index of the previously selected weapon.
		/// </param>
		/// <param name="lastSelected">
		/// A reference to the last weapon that was selected.
		/// </param>
		public WeaponChangeEventArgs(Int32 weaponIndex, WeaponSprite selected, Int32 lastIndex, WeaponSprite lastSelected)
			: base() {
			this._weaponIndex = weaponIndex;
			this._selected = selected;
			this._lastIndex = lastIndex;
			this._lastSelected = lastSelected;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the zero-based index of the selected weapon.
		/// </summary>
		public Int32 WeaponIndex {
			get { return this._weaponIndex; }
		}

		/// <summary>
		/// Gets the selected weapon instance.
		/// </summary>
		public WeaponSprite SelectedWeapon {
			get { return this._selected; }
		}

		/// <summary>
		/// Gets the zero-based index of the last selected weapon.
		/// </summary>
		public Int32 LastIndex {
			get { return this._lastIndex; }
		}

		/// <summary>
		/// Gets the last selected weapon instance.
		/// </summary>
		public WeaponSprite LastSelected {
			get { return this._lastSelected; }
		}
		#endregion
	}
}

