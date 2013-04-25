//
//  ItemUsedEventArguments.cs
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
	/// Item used event arguments class.
	/// </summary>
	public class ItemUsedEventArgs : EventArgs
	{
		#region Fields
		private ItemTypes _type = ItemTypes.None;
		private Int32 _unitCount = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.ItemUsedEventArgs"/>
		/// class with the item type and unit count.
		/// </summary>
		/// <param name="itemType">
		/// The item type.
		/// </param>
		/// <param name="unitCount">
		/// The unit count.
		/// </param>
		public ItemUsedEventArgs(ItemTypes itemType, Int32 unitCount)
			: base() {
			this._type = ItemTypes.None;
			this._unitCount = unitCount;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the type of the item.
		/// </summary>
		public ItemTypes ItemType {
			get { return this._type; }
		}

		/// <summary>
		/// Gets the unit count.
		/// </summary>
		public Int32 UnitCount {
			get { return this._unitCount; }
		}
		#endregion
	}
}

