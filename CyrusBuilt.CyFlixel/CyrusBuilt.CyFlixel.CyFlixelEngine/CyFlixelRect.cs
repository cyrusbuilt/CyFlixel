//
//  CyFlixelRect.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// A CyFlixel rectangle.
	/// </summary>
	public class CyFlixelRect
	{
		private const Int32 HASH_MULTIPLIER = 31;

		#region Fields
		private Guid _id = Guid.Empty;
		private float _x = 0f;
		private float _y = 0f;
		private float _width = 0f;
		private float _height = 0f;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>
		/// class with the x and y coordinates as well as the width and height.
		/// </summary>
		/// <param name="x">
		/// The x coordinate.
		/// </param>
		/// <param name="y">
		/// The y coordinate.
		/// </param>
		/// <param name="width">
		/// The width of the rectangle.
		/// </param>
		/// <param name="height">
		/// The height of the rectangle.
		/// </param>
		public CyFlixelRect(float x, float y, float width, float height) {
			this._id = Guid.NewGuid();
			this._x = x;
			this._y = y;
			this._width = width;
			this._height = height;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the X coordinate.
		/// </summary>
		public float X {
			get { return this._x; }
			set { this._x = value; }
		}

		/// <summary>
		/// Gets or sets the Y coordinate.
		/// </summary>
		public float Y {
			get { return this._y; }
			set { this._y = value; }
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width {
			get { return this._width; }
			set { this._width = value; }
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height {
			get { return this._height; }
			set { this._height = value; }
		}

		/// <summary>
		/// Gets the empty rectangle instance.
		/// </summary>
		public static CyFlixelRect Empty {
			get { return new CyFlixelRect(0f, 0f, 0f, 0f); }
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
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>.
		/// </returns>
		public override String ToString() {
			return String.Format("[CyFlixelRect: X={0}, Y={1}, Width={2}, Height={3}]", X, Y, Width, Height);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is
		/// equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="System.Object"/> is equal
		/// to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>; 
		/// Otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj) {
			if (obj == null) {
				return false;
			}

			CyFlixelRect rect = obj as CyFlixelRect;
			if ((Object)rect == null) {
				return false;
			}

			return ((this._x == rect.X) && (this._y == rect.Y) &&
			        (this._width == rect.Width) && (this._height == rect.Height));
		}

		/// <summary>
		/// Determines whether the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>.
		/// </summary>
		/// <param name="rect">
		/// The <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>
		/// to compare with the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>
		/// is equal to the current <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.CyFlixelRect"/>;
		/// Otherwise, false.
		/// </returns>
		public Boolean Equals(CyFlixelRect rect) {
			if (rect == null) {
				return false;
			}

			if ((Object)rect == null) {
				return false;
			}
			
			return ((this._x == rect.X) && (this._y == rect.Y) &&
			        (this._width == rect.Width) && (this._height == rect.Height));
		}
		#endregion
	}
}

