//
//  ItemSprite.cs
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
	/// An item sprite. Represents a limited-use item, such as a flashlight,
	/// shield, health pack, adrenaline, etc.
	/// </summary>
	public class ItemSprite : FlixelSprite
	{
		#region Fields
		private ItemTypes _itemType = ItemTypes.None;
		private Int32 _totalUnits = 0;
		private Int32 _maxUnits = 0;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the item is used up.
		/// </summary>
		public event ItemEmptyEventHandler Empty;

		/// <summary>
		/// Occurs when the item is used once.
		/// </summary>
		public event ItemUsedEventHandler Used;

		/// <summary>
		/// Occurs when the item is replenished.
		/// </summary>
		public event ItemReplenishEventHandler Replenished;
		#endregion

		#region Constructors
		public ItemSprite(ItemTypes itemType, Int32 maxUnits)
			: base() {
			this._itemType = itemType;
			this._maxUnits = maxUnits;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the type of the item.
		/// </summary>
		public ItemTypes ItemType {
			get { return this._itemType; }
		}

		/// <summary>
		/// Gets or sets the total available units. Value must be less than or
		/// equal to <see cref="MaxUnits"/>. Adding units will raise the
		/// <see cref="Replenished"/>.
		/// </summary>
		public Int32 TotalUnits {
			get { return this._totalUnits; }
			set {
				if (this._totalUnits != value) {
					if (value > this._maxUnits) {
						throw new ArgumentOutOfRangeException("ItemSprite.TotalUnits",
						                                      "Value cannot be greater than MaxUnits.");
					}
					this._totalUnits = value;
					this.OnReplenished(new ItemReplenishEventArgs(this._totalUnits, this._maxUnits));
				}
			}
		}

		/// <summary>
		/// Gets the max units.
		/// </summary>
		public Int32 MaxUnits {
			get { return this._maxUnits; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the <see cref="Empty"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnEmpty(ItemEmptyEventArgs e) {
			if (this.Empty != null) {
				this.Empty(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="Used"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnUsed(ItemUsedEventArgs e) {
			if (this.Used != null) {
				this.Used(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="Replenished"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnReplenished(ItemReplenishEventArgs e) {
			if (this.Replenished != null) {
				this.Replenished(this, e);
			}
		}

		/// <summary>
		/// Initialize this item.
		/// </summary>
		public void Initialize() {
			this._totalUnits = this._maxUnits;
		}

		/// <summary>
		/// Sets the maximum available unit capacity.
		/// </summary>
		/// <param name="maxUnits">
		/// The maximum number of units.
		/// </param>
		public void SetMaxUnits(Int32 maxUnits) {
			if (maxUnits < 0) {
				maxUnits = 0;
			}
			this._maxUnits = maxUnits;
		}

		/// <summary>
		/// "Uses" this item.
		/// </summary>
		public void Use() {
			if (this._totalUnits > 0) {
				this._totalUnits--;
				this.OnUsed(new ItemUsedEventArgs(this._itemType, this._totalUnits));
				return;
			}
			this.OnEmpty(new ItemEmptyEventArgs(this._itemType));
		}

		/// <summary>
		/// Adds units to the item.
		/// </summary>
		/// <param name="units">
		/// The number of units to add.
		/// </param>
		public void AddUnits(Int32 units) {
			if ((units > 0) && (units <= this._maxUnits)) {
				this._totalUnits += units;
				if (this._totalUnits > this._maxUnits) {
					this._totalUnits = this._maxUnits;
				}
				this.OnReplenished(new ItemReplenishEventArgs(this._totalUnits, this._maxUnits));
			}
		}

		/// <summary>
		/// Replenish this item. This sets the total units to the maximum.
		/// </summary>
		public void Replenish() {
			if (this._totalUnits <= 0) {
				this._totalUnits = this._maxUnits;
				this.OnReplenished(new ItemReplenishEventArgs(this._totalUnits, this._maxUnits));
				return;
			}
		}
		#endregion
	}
}

