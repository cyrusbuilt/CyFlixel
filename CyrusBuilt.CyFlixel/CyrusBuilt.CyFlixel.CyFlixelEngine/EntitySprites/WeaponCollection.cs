//
//  WeaponCollection.cs
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
	/// A collection of weapon sprites.
	/// </summary>
	public class WeaponCollection : CollectionBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.WeaponCollection"/>
		/// class. This is the default constructor.
		/// </summary>
		public WeaponCollection()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.WeaponCollection"/>
		/// class with the maximum number of weapons this collection can hold.
		/// </summary>
		/// <param name="capacity">
		/// The maximum capacity of the collection.
		/// </param>
		public WeaponCollection(Int32 capacity)
			: base(capacity) {
		}
		#endregion

		#region Indexers
		/// <summary>
		/// Gets or sets the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.EntitySprites.WeaponSprite"/>
		/// at the specified index.
		/// </summary>
		/// <param name="index">
		/// The zero-based index at which to get or set the <see cref="WeaponSprite"/>.
		/// </param>
		public WeaponSprite this[Int32 index] {
			get { return base.List[index] as WeaponSprite; }
			set { base.List[index] = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Add the specified <see cref="WeaponSprite"/> to the collection.
		/// </summary>
		/// <param name="weapon">
		/// The <see cref="WeaponSprite"/> to add to the collection.
		/// </param>
		/// <returns>
		/// If successful, the zero-based index at which the weapon was added;
		/// Otherwise, -1 if a duplicate weapon already exists in the collection.
		/// </returns>			
		public Int32 Add(WeaponSprite weapon) {
			if (!base.List.Contains(weapon)) {
				return base.List.Add(weapon);
			}
			return -1;
		}

		/// <summary>
		/// Insert the specified <see cref="WeaponSprite"/> at the specified index.
		/// </summary>
		/// <param name="index">
		/// The zero-based index at which to insert the weapon.
		/// </param>
		/// <param name="weapon">
		/// The weapon to insert.
		/// </param>
		/// <exception cref="DuplicateSpriteException">
		/// An identical <see cref="WeaponSprite"/> instance already exists in
		/// this collection.
		/// </exception>
		public void Insert(Int32 index, WeaponSprite weapon) {
			if (base.List.Contains(weapon)) {
				throw new DuplicateSpriteException(weapon.InstanceId);
			}
			base.List.Insert(index, weapon);
		}

		/// <summary>
		/// Remove the specified weapon.
		/// </summary>
		/// <param name="weapon">
		/// The <see cref="WeaponSprite"/> to remove from the collection.
		/// </param>
		public void Remove(WeaponSprite weapon) {
			if (base.List.Contains(weapon)) {
				base.List.Remove(weapon);
			}
		}

		/// <summary>
		/// Gets a flag indicate whether or not the specified <see cref="WeaponSprite"/>
		/// exists in the collection.
		/// </summary>
		/// <param name="weapon">
		/// The <see cref="WeaponSprite"/> to check for.
		/// </param>
		public Boolean Contains(WeaponSprite weapon) {
			return base.List.Contains(weapon);
		}

		/// <summary>
		/// Gets the first index of the specified <see cref="WeaponSprite"/>.
		/// </summary>
		/// <returns>
		/// If successful, the zero-based index of the specified <see cref="WeaponSprite"/>;
		/// Otherwise, -1 if the item does not exist in the collection.
		/// </returns>
		/// <param name="weapon">
		/// The <see cref="WeaponSprite"/> to check for.
		/// </param>
		public Int32 IndexOf(WeaponSprite weapon) {
			return base.List.IndexOf(weapon);
		}

		/// <summary>
		/// Copies the contents of the specified array to this collection.
		/// </summary>
		/// <param name="array">
		/// The array of <see cref="WeaponSprite"/> objects to copy to the
		/// collection.
		/// </param>
		/// <param name="index">
		/// The zero-based index at which to start copying.
		/// </param>
		public void CopyTo(WeaponSprite[] array, Int32 index) {
			base.List.CopyTo(array, index);
		}

		/// <summary>
		/// Disposes the weapons in the collection.
		/// </summary>
		public void DisposeWeapons() {
			foreach (WeaponSprite ws in this) {
				ws.Dispose();
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
		public static Boolean IsNullOrEmpty(WeaponCollection collection) {
			return ((collection == null) || (collection.Count == 0));
		}
		#endregion
	}
}

