//
//  ItemChangeEventArgs.cs
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
	/// Item change event arguments class.
	/// </summary>
	public class ItemChangeEventArgs : EventArgs
	{
		#region Fields
		private Int32 _selectedIndex = -1;
		private Int32 _lastIndex = -1;
		private ItemSprite _selectedItem = null;
		private ItemSprite _lastSelected = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.ItemChangeEventArgs"/>
		/// class with the selected item and its index in the item inventory.
		/// </summary>
		/// <param name="itemIndex">
		/// The zero-based index of the selected item.
		/// </param>
		/// <param name="selected">
		/// A reference to the selected item instance.
		/// </param>
		public ItemChangeEventArgs(Int32 itemIndex, ItemSprite selected)
			: base() {
			this._selectedIndex = itemIndex;
			this._selectedItem = selected;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Events.ItemChangeEventArgs"/>
		/// class with the selected item and its index in the item inventory
		/// along with the index and instance reference to the previous item
		/// selection.
		/// </summary>
		/// <param name="itemIndex">
		/// The zero-based index of the selected item.
		/// </param>
		/// <param name="selected">
		/// A reference to the selected item instance.
		/// </param>
		/// <param name="lastIndex">
		/// The zero-based index of the previous item selection.
		/// </param>
		/// <param name="lastItem">
		/// A reference to the previous item selection instance.
		/// </param>
		public ItemChangeEventArgs(Int32 itemIndex, ItemSprite selected, Int32 lastIndex, ItemSprite lastItem)
			: base() {
			this._selectedIndex = itemIndex;
			this._selectedItem = selected;
			this._lastIndex = lastIndex;
			this._lastSelected = lastItem;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the zero-based index of the currently selected item.
		/// </summary>
		public Int32 ItemIndex {
			get { return this._selectedIndex; }
		}

		/// <summary>
		/// Gets the selected item instance.
		/// </summary>
		public ItemSprite SelectedItem {
			get { return this._selectedItem; }
		}

		/// <summary>
		/// Gets the zero-based index of the previous item selection.
		/// </summary>
		public Int32 LastIndex {
			get { return this._lastIndex; }
		}

		/// <summary>
		/// Gets the last selected item instance.
		/// </summary>
		public ItemSprite LastItem {
			get { return this._lastSelected; }
		}
		#endregion
	}
}

