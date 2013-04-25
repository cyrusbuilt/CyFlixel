//
//  ItemEmptyEventArgs.cs
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
	/// Item empty event arguments class.
	/// </summary>
	public class ItemEmptyEventArgs : EventArgs
	{
		private ItemTypes _type = ItemTypes.None;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.ItemEmptyEventArgs"/>
		/// class with the item type.
		/// </summary>
		/// <param name="itemType">Item type.</param>
		public ItemEmptyEventArgs(ItemTypes itemType)
			: base() {
			this._type = itemType;
		}

		/// <summary>
		/// Gets the type of the item.
		/// </summary>
		public ItemTypes ItemType {
			get { return this._type; }
		}
	}
}

