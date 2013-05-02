//
//  ItemAddedEventArgs.cs
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
	/// Item added event arguments class..
	/// </summary>
	public class ItemAddedEventArgs : EventArgs
	{
		#region Fields
		private Int32 _itemIndex = -1;
		private Int32 _itemCount = 0;
		private ItemTypes _type = ItemTypes.None;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.ItemAddedEventArgs"/>
		/// class with the index of the item in inventory and the item type.
		/// </summary>
		/// <param name="itemIndex">
		/// The zero-based index of the item in inventory.
		/// </param>
		/// <param name="type">
		/// The type of item added.
		/// </param>
		public ItemAddedEventArgs(Int32 itemIndex, ItemTypes type)
			: base() {
			this._itemIndex = itemIndex;
			this._type = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.ItemAddedEventArgs"/>
		/// class with the index of the item in inventory, the item type,
		/// and the count of items in inventory.
		/// </summary>
		/// <param name="itemIndex">
		/// The zero-based index of the item in inventory.
		/// </param>
		/// <param name="type">
		/// The type of item added.
		/// </param>
		/// <param name="itemCount">
		/// The count of items in inventory.
		/// </param>
		public ItemAddedEventArgs(Int32 itemIndex, ItemTypes type, Int32 itemCount)
			: base() {
			this._itemIndex = itemIndex;
			this._itemCount = itemCount;
			this._type = type;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the index of the item in inventory.
		/// </summary>
		public Int32 ItemIndex {
			get { return this._itemIndex; }
		}

		/// <summary>
		/// Gets the count of items in inventory.
		/// </summary>
		public Int32 Count {
			get { return this._itemCount; }
		}

		/// <summary>
		/// Gets the type of item added.
		/// </summary>
		public ItemTypes ItemType {
			get { return this._type; }
		}
		#endregion
	}
}

