//
//  FlixelList.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Data
{
	/// <summary>
	/// A tiny linked-list class. Useful for optimizing time-critical or highly
	/// repititive tasks.
	/// </summary>
	public class FlixelList
	{
		private const Int32 HASH_MULTIPLIER = 31;

		#region Fields
		private Guid _id = Guid.Empty;
		private CyFlixelObject _obj = null;
		private FlixelList _next = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelList"/>
		/// class. This is the default constructor.
		/// </summary>
		public FlixelList() {
			this._id = Guid.NewGuid();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>
		/// class with the object to put in the list as well as the next list
		/// to link to.
		/// </summary>
		/// <param name="obj">
		/// The object to put in the list.
		/// </param>
		/// <param name="next">
		/// The next list to link to.
		/// </param>
		public FlixelList(CyFlixelObject obj, FlixelList next) {
			this._id = Guid.NewGuid();
			this._obj = obj;
			this._next = next;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the flixel object.
		/// </summary>
		public CyFlixelObject FlixelObject {
			get { return this._obj; }
			set { this._obj = value; }
		}

		/// <summary>
		/// Gets or sets the next link in the list.
		/// </summary>
		public FlixelList Next {
			get { return this._next; }
			set { this._next = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Provides a hashcode identifier of this instance.
		/// </summary>
		/// <returns>
		/// The hashcode identifier for this instance.
		/// </returns>
		public override Int32 GetHashCode() {
			unchecked {
				Int32 hashCode = base.GetType().GetHashCode();
				return (hashCode * HASH_MULTIPLIER) ^ this._id.GetHashCode();
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>.
		/// </returns>
		public override string ToString() {
			if (this._obj != null) {
				return this._obj.ToString();
			}
			return String.Empty;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is
		/// equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="System.Object"/> is equal to the
		/// current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>;
		/// Otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj) {
			if (obj == null) {
				return false;
			}

			FlixelList fl = obj as FlixelList;
			if ((Object)fl == null) {
				return false;
			}

			return ((this._next == fl.Next) && (this._obj == fl.FlixelObject));
		}

		/// <summary>
		/// Determines whether the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>.
		/// </summary>
		/// <param name="fl">
		/// The <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>
		/// to compare with the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Data.FlixelList"/>;
		/// Otherwise, false.
		/// </returns>
		public Boolean Equals(FlixelList fl) {
			if (fl == null) {
				return false;
			}

			if ((Object)fl == null) {
				return false;
			}
			
			return ((this._next == fl.Next) && (this._obj == fl.FlixelObject));
		}
		#endregion
	}
}

