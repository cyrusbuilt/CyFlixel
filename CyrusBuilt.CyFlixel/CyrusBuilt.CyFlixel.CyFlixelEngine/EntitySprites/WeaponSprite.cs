//
//  WeaponSprite.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites
{
	/// <summary>
	/// A weapon sprite. Fires a projectile at the intended target.
	/// </summary>
	public class WeaponSprite : FlixelSprite
	{
		#region Fields
		private WeaponTypes _weaponType = WeaponTypes.None;
		private Int32 _ammoCount = 0;
		private Int32 _totalAmmo = 0;
		private Int32 _maxAmmo = 0;
		private Int32 _magazineCapacity = 30;
		private Boolean _unlimitedAmmo = false;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the weapon reloads.
		/// </summary>
		public event WeaponReloadEventHandler Reloaded;

		/// <summary>
		/// Occurs when the weapon goes dry (out of ammo).
		/// </summary>
		public event WeaponEmptyEventHandler WentDry;

		/// <summary>
		/// Occurs when the weapon is fired.
		/// </summary>
		public event WeaponFireEventHandler Fired;
		#endregion

		#region Constructors
		public WeaponSprite(WeaponTypes weaponType, Int32 maxAmmo, Int32 magCapacity)
			: base() {
			this._weaponType = weaponType;
			this._maxAmmo = maxAmmo;
			this._magazineCapacity = magCapacity;
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
		/// Gets the ammo count remaining in the current magazine.
		/// </summary>
		public Int32 AmmoCount {
			get { return this._ammoCount; }
		}

		/// <summary>
		/// Gets the magazine capacity.
		/// </summary>
		public Int32 MagazineCapacity {
			get { return this._magazineCapacity; }
		}

		/// <summary>
		/// Gets the maximum amount of ammo this weapon can have.
		/// </summary>
		public Int32 MaxAmmo {
			get { return this._maxAmmo; }
		}

		/// <summary>
		/// Gets the number of magazines (both full and partial) remaining.
		/// </summary>
		public Int32 MagazineCount {
			get {
				if (this._ammoCount > 0) {
					Int32 mags = 1;
					if (this._totalAmmo > this._magazineCapacity) {
						mags += (this._totalAmmo / this._magazineCapacity);
					}
					return mags;
				}
				return 0;
			}
		}

		/// <summary>
		/// Gets or sets the total available ammo for this weapon.
		/// </summary>
		public Int32 TotalAmmo {
			get { return this._totalAmmo; }
			set {
				if (this._totalAmmo != value) {
					if (value > this._maxAmmo) {
						throw new ArgumentOutOfRangeException("WeaponSprite.TotalAmmo",
						                                      "Value cannot be greater than MaxAmmo.");
					}
					this._totalAmmo = value;
					this.OnReloaded(new WeaponReloadEventArgs(this._totalAmmo, this._ammoCount));
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this weapon has unlimited ammo.
		/// </summary
		public Boolean UnlimitedAmmo {
			get { return this._unlimitedAmmo; }
			set { this._unlimitedAmmo = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the <see cref="Reloaded"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnReloaded(WeaponReloadEventArgs e) {
			if (this.Reloaded != null) {
				this.Reloaded(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="WentDry"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnWentDry(WeaponEmptyEventArgs e) {
			if (this.WentDry != null) {
				this.WentDry(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="Fired"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnFired(WeaponFireEventArgs e) {
			if (this.Fired != null) {
				this.Fired(this, e);
			}
		}

		/// <summary>
		/// Initialize this weapon.
		/// </summary>
		public void Initialize() {
			this._totalAmmo = this._maxAmmo;
			this._ammoCount = this._magazineCapacity;
			this._totalAmmo -= this._ammoCount;
		}

		/// <summary>
		/// Sets the maximum possible amount of ammo for this weapon.
		/// </summary>
		/// <param name="max">
		/// The maximum amount of ammo.
		/// </param>
		public void SetMaxAmmo(Int32 max) {
			if (max < 0) {
				max = 0;
			}
			this._maxAmmo = max;
		}

		/// <summary>
		/// Sets the magazine capacity.
		/// </summary>
		/// <param name="capacity">
		/// The total capacity of the weapon's magazine.
		/// </param>
		public void SetMagazineCapacity(Int32 capacity) {
			if (capacity < 0) {
				capacity = 0;
			}
			this._magazineCapacity = capacity;
		}

		/// <summary>
		/// Fires the weapon.
		/// </summary>
		public void Fire() {
			if (this._ammoCount > 0) {
				this._ammoCount--;
				this.OnFired(new WeaponFireEventArgs(this._weaponType, this._ammoCount));
				// TODO how do we animate/render firing a projectile?
				return;
			}
			this.OnWentDry(new WeaponEmptyEventArgs(this._weaponType));
		}

		/// <summary>
		/// Reload this weapon. This attempts to refill the magazine to capacity
		/// *if* enough ammo remains. If not, whatever remaining ammo is left is
		/// loaded into the magazine to provide a partial magazine. The ammo
		/// loaded into the magazine is subtracted from the total available ammo.
		/// If no ammo remains at the time of reload, then the reload request is
		/// ignored.
		/// </summary>
		public void Reload() {
			if (this._unlimitedAmmo) {
				this._totalAmmo = this._maxAmmo;
				this._ammoCount = this._magazineCapacity;
				this.OnReloaded(new WeaponReloadEventArgs(this._totalAmmo, this._ammoCount));
				return;
			}

			if ((this._ammoCount <= 0) && (this._totalAmmo > 0)) {
				if (this._totalAmmo > this._magazineCapacity) {
					this._ammoCount = (this._totalAmmo - this._magazineCapacity);
					this._totalAmmo -= this._magazineCapacity;
				}
				else {
					this._ammoCount = this._totalAmmo;
					this._totalAmmo = 0;
				}
				this.OnReloaded(new WeaponReloadEventArgs(this._totalAmmo, this._ammoCount));
			}
		}
		#endregion
	}
}

