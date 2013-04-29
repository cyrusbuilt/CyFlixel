//
//  ItemCollection.cs
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
using System.Collections;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites
{
	/// <summary>
	/// A collection of item sprites.
	/// </summary>
	public class ItemCollection : CollectionBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.ItemCollection"/>
		/// class. This is the default constructor.
		/// </summary>
		public ItemCollection()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.ItemCollection"/>
		/// class with the collection capacity.
		/// </summary>
		/// <param name="capacity">
		/// The maximum capacity of this collection.
		/// </param>
		public ItemCollection(Int32 capacity)
			: base(capacity) {
		}
		#endregion

		#region Indexers
		/// <summary>
		/// Gets or sets the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.ItemSprite"/>
		/// at the specified index.
		/// </summary>
		/// <param name="index">
		/// The zero-based index at which to get or set the <see cref="ItemSprite"/>.
		/// </param>
		public ItemSprite this[Int32 index] {
			get { return base.List[index] as ItemSprite; }
			set { base.List[index] = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Add the specified <see cref="ItemSprite"/> to the collection.
		/// </summary>
		/// <param name="item">
		/// The <see cref="ItemSprite"/> to add to the collection.
		/// </param>
		/// <returns>
		/// If successful, the zero-based index at which the item was added;
		/// Otherwise, -1 if a duplicate item already exists in the collection.
		/// </returns>			
		public Int32 Add(ItemSprite item) {
			if (!base.List.Contains(item)) {
				return base.List.Add(item);
			}
			return -1;
		}

		/// <summary>
		/// Insert the specified <see cref="ItemSprite"/> at the specified index.
		/// </summary>
		/// <param name="index">
		/// The zero-based index at which to insert the item.
		/// </param>
		/// <param name="item">
		/// The item to insert.
		/// </param>
		/// <exception cref="DuplicateSpriteException">
		/// An identical <see cref="ItemSprite"/> instance already exists in
		/// this collection.
		/// </exception>
		public void Insert(Int32 index, ItemSprite item) {
			if (base.List.Contains(item)) {
				throw new DuplicateSpriteException(item.InstanceId);
			}
			base.List.Insert(index, item);
		}

		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">
		/// The <see cref="ItemSprite"/> to remove from the collection.
		/// </param>
		public void Remove(ItemSprite item) {
			if (base.List.Contains(item)) {
				base.List.Remove(item);
			}
		}

		/// <summary>
		/// Gets a flag indicate whether or not the specified <see cref="ItemSprite"/>
		/// exists in the collection.
		/// </summary>
		/// <param name="item">
		/// The <see cref="ItemSprite"/> to check for.
		/// </param>
		public Boolean Contains(ItemSprite item) {
			return base.List.Contains(item);
		}

		/// <summary>
		/// Gets the first index of the specified <see cref="ItemSprite"/>.
		/// </summary>
		/// <returns>
		/// If successful, the zero-based index of the specified <see cref="ItemSprite"/>;
		/// Otherwise, -1 if the item does not exist in the collection.
		/// </returns>
		/// <param name="item">
		/// The <see cref="ItemSprite"/> to check for.
		/// </param>
		public Int32 IndexOf(ItemSprite item) {
			return base.List.IndexOf(item);
		}

		/// <summary>
		/// Copies the contents of the specified array to this collection.
		/// </summary>
		/// <param name="array">
		/// The array of <see cref="ItemSprite"/> objects to copy to the
		/// collection.
		/// </param>
		/// <param name="index">
		/// The zero-based index at which to start copying.
		/// </param>
		public void CopyTo(ItemSprite[] array, Int32 index) {
			base.List.CopyTo(array, index);
		}

		/// <summary>
		/// Disposes the items in the collection.
		/// </summary>
		public void DisposeItems() {
			foreach (ItemSprite item in this) {
				item.Dispose();
			}
		}
 
		/// <summary>
		/// Determines if the specified collection is null or empty.
		/// </summary>
		/// <returns>
		/// true if the specifed collection is null or empty; otherwise, false<.
		/// </returns>
		/// <param name="collection">
		/// The collection to check.
		/// </param>
		public static Boolean IsNullOrEmpty(ItemCollection collection) {
			return ((collection == null) || (collection.Count == 0));
		}
		#endregion
	}
}

