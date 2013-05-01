//
//  PlayerSprite.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites
{
	/// <summary>
	/// Player sprite. Represents one of the players in the game.
	/// </summary>
	public class PlayerSprite : FlixelSprite
	{
		#region Fields
		private ItemCollection _itemInventory = null;
		private WeaponCollection _weaponInventory = null;
		private WeaponSprite _selectedWeapon = null;
		private ItemSprite _selectedItem = null;
		private PlayerIndex _index = PlayerIndex.One;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when weapon selection changes.
		/// </summary>
		public event WeaponChangeEventHandler WeaponSelectionChanged;

		/// <summary>
		/// Occurs when item selection changes.
		/// </summary>
		public event ItemChangeEventHandler ItemSelectionChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>
		/// class. This is the default constructor.
		/// </summary>
		public PlayerSprite()
			: base() {
			this._itemInventory = new ItemCollection();
			this._weaponInventory = new WeaponCollection();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>
		/// class with the player index.
		/// </summary>
		/// <param name="index">
		/// The index (player number) in the game.
		/// </param>
		public PlayerSprite(PlayerIndex index)
			: base() {
			this._index = index;
			this._itemInventory = new ItemCollection();
			this._weaponInventory = new WeaponCollection();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the weapon inventory.
		/// </summary>
		public WeaponCollection WeaponInventory {
			get { return this._weaponInventory; }
		}

		/// <summary>
		/// Gets the item inventory.
		/// </summary>
		public ItemCollection ItemInventory {
			get { return this._itemInventory; }
		}

		/// <summary>
		/// Gets the selected weapon.
		/// </summary>
		public WeaponSprite SelectedWeapon {
			get { return this._selectedWeapon; }
		}

		/// <summary>
		/// Gets the selected item.
		/// </summary>
		public ItemSprite SelectedItem {
			get { return this._selectedItem; }
		}

		/// <summary>
		/// Gets the index of the player.
		/// </summary>
		public PlayerIndex PlayerIndex {
			get { return this._index; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the weapon selection changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnWeaponSelectionChanged(WeaponChangeEventArgs e) {
			if (this.WeaponSelectionChanged != null) {
				this.WeaponSelectionChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the item selection changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnItemSelectionChanged(ItemChangeEventArgs e) {
			if (this.ItemSelectionChanged != null) {
				this.ItemSelectionChanged(this, e);
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>
		/// object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>.
		/// The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.PlayerSprite"/>
		/// was occupying.
		/// </remarks>
		public override void Dispose() {
			this._weaponInventory.DisposeWeapons();
			this._weaponInventory.Clear();
			this._weaponInventory = null;
			this._itemInventory.DisposeItems();
			this._itemInventory.Clear();
			this._itemInventory = null;
			base.Dispose();
		}

		/// <summary>
		/// Selects the specified weapon.
		/// </summary>
		/// <param name="weaponIndex">
		/// The zero-based index of the weapon to select from inventory.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="weaponIndex"/> cannot be less than zero or greater
		/// than the last index in the inventory.
		/// </exception>
		public void SelectWeapon(Int32 weaponIndex) {
			if ((weaponIndex < 0) || (weaponIndex > (this._weaponInventory.Count - 1))) {
				throw new IndexOutOfRangeException();
			}

			// Find the specified weapon.
			if (!WeaponCollection.IsNullOrEmpty(this._weaponInventory)) {
				WeaponSprite ws = this._weaponInventory[weaponIndex];
				if ((ws != null) && (!ws.Equals(this._selectedWeapon))) {
					// The weapon was found in inventory. Save off the current
					// weapon selection (if we have one).
					Int32 lastIndex = -1;
					WeaponSprite lastWeapon = null;
					if (this._selectedItem != null) {
						lastWeapon = this._selectedWeapon;
						lastIndex = this._weaponInventory.IndexOf(lastWeapon);
					}

					// Set selection and fire notification.
					this._selectedWeapon = ws;
					this.OnWeaponSelectionChanged(new WeaponChangeEventArgs(weaponIndex, ws, lastIndex, lastWeapon));
				}
			}
		}

		/// <summary>
		/// Selects the specified item.
		/// </summary>
		/// <param name="itemIndex">
		/// The zero-based index of the item to select from inventory.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="itemIndex"/> cannot be less than zero or greater
		/// than the last index in the inventory.
		/// </exception>
		public void SelectItem(Int32 itemIndex) {
			if ((itemIndex < 0) || (itemIndex > (this._itemInventory.Count - 1))) {
				throw new IndexOutOfRangeException();
			}

			// Find the specified item in inventory.
			if (!ItemCollection.IsNullOrEmpty(this._itemInventory)) {
				ItemSprite item = this._itemInventory[itemIndex];
				if ((item != null) && (!itemIndex.Equals(this._selectedItem))) {
					// The item was found in inventory. Save off the current
					// item selection (if we have one).
					Int32 lastIndex = -1;
					ItemSprite lastItem = null;
					if (this._selectedItem != null) {
						lastItem = this._selectedItem;
						lastIndex = this._itemInventory.IndexOf(lastItem);
					}

					// Set the selection and fire notification.
					this._selectedItem = item;
					this.OnItemSelectionChanged(new ItemChangeEventArgs(itemIndex, item, lastIndex, lastItem));
				}
			}
		}

		/// <summary>
		/// Gives the player a weapon if the specified weapon does not already
		/// exist in the player's inventory.
		/// </summary>
		/// <param name="weapon">
		/// The weapon to give to the player.
		/// </param>
		/// <param name="autoSelect">
		/// If set to true automatically select once added select.
		/// </param>
		public void GiveWeapon(WeaponSprite weapon, Boolean autoSelect) {
			Int32 index = this._weaponInventory.Add(weapon);
			if (index != -1) {
				// TODO fire weapon added event.
				if (autoSelect) {
					this.SelectWeapon(index);
				}
				return;
			}
			// TODO if the weapon already exists, take what ammo we can.
		}

		/// <summary>
		/// Gives the player an item if the specified item does not already
		/// exist in the player's inventory.
		/// </summary>
		/// <param name="item">
		/// The item to give to the player.
		/// </param>
		/// <param name="autoSelect">
		/// If set to true, then automatically select the item after adding it.
		/// </param>
		public void GiveItem(ItemSprite item, Boolean autoSelect) {
			Int32 index = this._itemInventory.Add(item);
			if (index != -1) {
				// TODO fire item added event.
				if (autoSelect) {
					this.SelectItem(index);
				}
			}
			// TODO if item already exists and is not full, take what units we can.
		}

		/// <summary>
		/// Drops the currently selected weapon.
		/// </summary>
		public void DropWeapon() {
			if ((this._selectedWeapon != null) &&
			    (!WeaponCollection.IsNullOrEmpty(this._weaponInventory))) {
				this._weaponInventory.Remove(this._selectedWeapon);
				this.SelectWeapon(0);
				// TODO fire weapon drop event.
			}
		}

		/// <summary>
		/// Fires the currently selected weapon.
		/// </summary>
		public void FireWeapon() {
			if (this._selectedWeapon != null) {
				this._selectedWeapon.Fire();
			}
		}

		/// <summary>
		/// Uses the selected item.
		/// </summary>
		public void UseItem() {
			if (this._selectedItem != null) {
				this._selectedItem.Use();
			}
		}

		/// <summary>
		/// Drops the currently selected item.
		/// </summary>
		public void DropItem() {
			if ((this._selectedItem != null) &&
			    (!ItemCollection.IsNullOrEmpty(this._itemInventory))) {
				this._itemInventory.Remove(this._selectedItem);
				this.SelectItem(0);
				// TODO fire item drop event.
			}
		}
		#endregion
	}
}

