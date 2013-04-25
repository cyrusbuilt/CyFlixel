//
//  QuadTree.cs
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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Data;
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// 
	/// </summary>
	public class QuadTree : CyFlixelRect
	{
		#region Constants
		/// <summary>
		/// Flag for specifying that you want to add an object to the A list.
		/// </summary>
		public const uint A_LIST = 0;

		/// <summary>
		/// Flaf for specifying that you want to add an object to the B list.
		/// </summary>
		public const uint B_LIST = 1;
		#endregion

		#region Static Fields
		private static QuadTree _tree = null;
		private static CyFlixelRect _bounds = new CyFlixelRect(0, 0, 0, 0);
		private static uint _divisions = 0;
		protected static uint _min = 0;
		protected static CyFlixelObject _o = null;
		protected static float _ol = 0;
		protected static float _ot = 0;
		protected static float _or = 0;
		protected static float _ob = 0;
		protected static float _oa = 0;
		protected static SpriteCollisionEventHandler _oc = null;
		#endregion

		#region Protected Fields
		protected Boolean _canSubDivide = false;
		protected FlixelList _headA = null;
		protected FlixelList _tailA = null;
		protected FlixelList _headB = null;
		protected FlixelList _tailB = null;
		protected QuadTree _nw = null;
		protected QuadTree _ne = null;
		protected QuadTree _se = null;
		protected QuadTree _sw = null;
		protected float _l = 0;
		protected float _r = 0;
		protected float _t = 0;
		protected float _b = 0;
		protected float _hw = 0;
		protected float _hh = 0;
		protected float _mx = 0;
		protected float _my = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.QuadTree"/>
		/// class with the node dimensions and the parent branch or node.
		/// </summary>
		/// <param name="x">
		/// The X coordinate of the point in space.
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the point in space.
		/// </param>
		/// <param name="width">
		/// Desired width of this node.
		/// </param>
		/// <param name="height">
		/// Desired height of this node.
		/// </param>
		/// <param name="parent">
		/// The parent branch or node. Set null to create a root node.
		/// </param>
		public QuadTree(float x, float y, float width, float height, QuadTree parent)
			: base(x, y, width, height) {
			this._headA = this._tailA = new FlixelList();
			this._headB = this._tailB = new FlixelList();

			// Copy the parent's children, if there are any.
			if (parent != null) {
				FlixelList itr = null;
				FlixelList ot = null;
				if (parent._headA.FlixelObject != null) {
					itr = parent._headA;
					while (itr != null) {
						if (this._tailA.FlixelObject != null) {
							ot = this._tailA;
							this._tailA = new FlixelList();
							ot.Next = this._tailB;
						}
						this._tailB.FlixelObject = itr.FlixelObject;
						itr = itr.Next;
					}
				}
			}
			else {
				_min = (uint)(base.Width + base.Height) / (2 * _divisions);
				this._canSubDivide = (base.Width > _min) || (base.Height > _min);

				// Setup comparison/sort helpers.
				this._l = base.X;
				this._r = (base.X + base.Width);
				this._hw = (base.Width / 2);
				this._mx = (this._l + this._hw);
				this._t = base.Y;
				this._b = (base.Y + base.Height);
				this._hh = (base.Height / 2);
				this._my = (this._t + this._hh);
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the left position.
		/// </summary>
		public float Left {
			get { return base.X; }
		}

		/// <summary>
		/// Gets the top position.
		/// </summary>
		public float Top {
			get { return base.Y; }
		}

		/// <summary>
		/// Gets the right position.
		/// </summary>
		public float Right {
			get { return base.X + base.Width; }
		}

		/// <summary>
		/// Gets the bottom position.
		/// </summary>
		public float Bottom {
			get { return base.Y + base.Height; }
		}

		/// <summary>
		/// Gets or sets the engine quad tree. Set null to force a refresh on
		/// the next collision.
		/// </summary>
		public static QuadTree EngineQuadTree {
			get { return _tree; }
			set { _tree = value; }
		}

		/// <summary>
		/// Gets or sets the dimensions of the root of the quad tree.
		/// This is eligible game collision space.
		/// </summary>
		public static CyFlixelRect Bounds {
			get { return _bounds; }
			set { _bounds = value; }
		}

		/// <summary>
		/// Gets or sets the divisions. Controls the granularity of the quad
		/// tree. Default is 3 (decent performance on large and small worlds).
		/// </summary>
		public static uint Divisions {
			get { return _divisions; }
			set { _divisions = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Recursively adds objects to leaf lists.
		/// </summary>
		protected void AddToList() {
			FlixelList ot = null;
			if (_oa == A_LIST) {
				if (this._tailA.FlixelObject != null) {
					ot = this._tailA;
					this._tailA = new FlixelList();
					ot.Next = this._tailA;
				}
				this._tailA.FlixelObject = _o;
			}
			else {
				if (this._tailB.FlixelObject != null) {
					ot = this._tailB;
					this._tailB = new FlixelList();
					ot.Next = this._tailB;
				}
				this._tailB.FlixelObject = _o;
			}

			if (!this._canSubDivide) {
				return;
			}

			if (this._nw != null) {
				this._nw.AddToList();
			}

			if (this._ne != null) {
				this._ne.AddToList();
			}

			if (this._se != null) {
				this._se.AddToList();
			}

			if (this._sw != null) {
				this._sw.AddToList();
			}
		}

		/// <summary>
		/// Recursively navigates and creates the tree while adding objects
		/// to the appropriate nodes.
		/// </summary>
		protected void AddObject() {
			// If this quadtree (not its children) lies entirely inside this
			// object, add it here.
			if ((!this._canSubDivide) || 
			    ((this._l >= _ol) && (this._r <= _or) && (this._t >= _ot) && (this._b <= _ob))) {
				this.AddToList();
				return;
			}

			// See if the selected object fits completely inside any of the
			// quadrants.
			if ((_ol > this._l) && (_or < this._mx)) {
				if ((_ot > this._t) && (_ob < this._my)) {
					if (this._nw == null) {
						this._nw = new QuadTree((Int32)this._l, (Int32)this._t, (Int32)this._hw, (Int32)this._hh, this);
						this._nw.AddObject();
						return;
					}
				}

				if ((_ot > this._my) && (_ob < this._b)) {
					if (this._sw == null) {
						this._sw = new QuadTree((Int32)this._l, (Int32)this._my, (Int32)this._hw, (Int32)this._hh, this);
						this._sw.AddObject();
						return;
					}
				}
			}

			// If it wasn't completely contained we have to check out the
			// partial overlaps.
			if ((_or > this._l) && (_ol < this._mx) &&
			    (_ob > this._t) && (_ot < this._my)) {
				if (this._nw == null) {
					this._nw = new QuadTree((Int32)this._l, (Int32)this._t, (Int32)this._hw, (Int32)this._hh, this);
					this._nw.AddObject();
				}
			}

			if ((_or > this._mx) && (_ol < this._r) &&
			    (_ob > this._t) && (_ot < this._my)) {
				if (this._ne == null) {
					this._ne = new QuadTree((Int32)this._mx, (Int32)this._t, (Int32)this._hw, (Int32)this._hh, this);
					this._ne.AddObject();
				}
			}

			if ((_or > this._mx) && (_ol < this._r) &&
			    (_ob > this._my) && (_ot < this._b)) {
				if (this._se == null) {
					this._se = new QuadTree((Int32)this._mx, (Int32)this._my, (Int32)this._hw, (Int32)this._hh, this);
					this._se.AddObject();
				}
			}

			if ((_or > this._l) && (_ol < this._mx) &&
			    (_ob > this._my) && (_ot < this._b)) {
				if (this._sw == null) {
					this._sw = new QuadTree((Int32)this._l, (Int32)this._my, (Int32)this._hw, (Int32)this._hh, this);
					this.AddObject();
				}
			}
		}

		/// <summary>
		/// Adds the specified object to the root of the tree. This method will
		/// recursively add all group members, but not the groups themselves.
		/// </summary>
		/// <param name="obj">
		/// The object to add. This object will be recursed and the applicable
		/// members will be added automatically.
		/// </param>
		/// <param name="list">
		/// A flag indicating the list to which you want to add the objects.
		/// Options are <see cref="A_LIST"/> or <see cref="B_LIST"/>. 
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="obj"/> cannot be null.
		/// </exception>
		public void Add(CyFlixelObject obj, uint list) {
			if (obj == null) {
				throw new ArgumentNullException("obj");
			}

			_oa = list;
			if (obj.IsGroup) {
				Int32 i = 0;
				CyFlixelObject m = null;
				List<CyFlixelObject> members = (obj as FlixelGroup).Members;
				Int32 l = members.Count;
				while (i < l) {
					m = members[i++] as CyFlixelObject;
					if ((m != null) && (m.Exists)) {
						if (m.IsGroup) {
							this.Add(m, list);
						}
						else if (m.Solid) {
							_o = m;
							_ol = _o.X;
							_ot = _o.Y;
							_or = (_o.X + _o.Width);
							_ob = (_o.Y + _o.Height);
							this.AddObject();
						}
					}
				}
			}

			if (obj.Solid) {
				_o = obj;
				_ol = _o.X;
				_ot = _o.Y;
				_or = (_o.X + _o.Width);
				_ob = (_o.Y + _o.Height);
				this.AddObject();
			}
		}

		/// <summary>
		/// An internal function for comparing an object against the contents
		/// of a node.
		/// </summary>
		/// <returns>
		/// true if any overlaps were found; Otherwise, false.
		/// </returns>
		/// <param name="iterator">
		/// A pointer to a linked list entry (for comparing against itself).
		/// </param>
		protected Boolean OverlapNode(FlixelList iterator) {
			Boolean c = false;
			CyFlixelObject co = null;
			FlixelList itr = iterator;
			if (itr == null) {
				if (_oa == A_LIST) {
					itr = this._headA;
				}
				else {
					itr = this._headB;
				}
			}

			if (itr.FlixelObject != null) {
				while (itr != null) {
					co = itr.FlixelObject;
					if ((_o == co) || (!co.Exists) || (!_o.Exists) || (!co.Solid) || (!_o.Solid) ||
					    ((_o.X + _o.Width) < (co.X + GameMath.RoundingError)) ||
					    ((_o.X + GameMath.RoundingError) > (co.X + co.Width)) ||
					    ((_o.Y + _o.Height) < (co.Y + GameMath.RoundingError)) ||
					    ((_o.Y + GameMath.RoundingError) > (co.Y + co.Height))) {
						itr = itr.Next;
						continue;
					}

					if (_oc == null) {
						_o.Kill();
						co.Kill();
						c = true;
					}
					else if (_oc(this, new SpriteCollisionEventArgs(_o, co))) {
						c = true;	
					}
					itr = itr.Next;
				}
			}
			return c;
		}

		/// <summary>
		/// An internal function for comparing an object against the contents
		/// of a node.
		/// </summary>
		/// <returns>
		/// true if any overlaps were found; Otherwise, false.
		/// </returns>
		protected Boolean OverlapNode() {
			return this.OverlapNode(null);
		}

		/// <summary>
		/// This method should be called after call <see cref="Add()"/> to
		/// compare the objects that were loaded.
		/// </summary>
		/// <param name="bothLists">
		/// Set true to do an A - B list comparison or false if you want to
		/// compare A_LIST against itself.
		/// </param>
		/// <param name="e">
		/// The event arguments containing a function the checks for overlaps.
		/// If the arguments do not contain a callback function, then Kill()
		/// will be called on both objects.
		/// </param>
		/// <returns>
		/// true if any overlaps were detected; Otherwise, false.
		/// </returns>			
		public Boolean Overlap(Boolean bothLists, SpriteCollisionEventHandler e) {
			_oc = e;
			Boolean c = false;
			FlixelList itr = null;
			if (bothLists) {
				// An A - B list comparison.
				_oa = B_LIST;
				if (this._headA.FlixelObject != null) {
					itr = this._headA;
					while (itr != null) {
						_o = itr.FlixelObject;
						if ((_o.Exists) && (_o.Solid) && (this.OverlapNode())) {
							c = true;
						}
						itr = itr.Next;
					}
				}

				_oa = A_LIST;
				if (this._headB.FlixelObject != null) {
					itr = this._headB;
					while (itr != null) {
						_o = itr.FlixelObject;
						if ((_o.Exists) && (_o.Solid)) {
							if ((this._nw != null) && (this._nw.OverlapNode())) {
								c = true;
							}

							if ((this._ne != null) && (this._ne.OverlapNode())) {
								c = true;
							}

							if ((this._se != null) && (this._se.OverlapNode())) {
								c = true;
							}

							if ((this._sw != null) && (this._sw.OverlapNode())) {
								c = true;
							}
						}
						itr = itr.Next;
					}
				}
			}
			else {
				// Just checking the A list against itself.
				if (this._headA.FlixelObject != null) {
					itr = this._headA;
					while (itr != null) {
						_o = itr.FlixelObject;
						if ((_o.Exists) && (_o.Solid) && (this.OverlapNode(itr.Next))) {
							c = true;
						}
						itr = itr.Next;
					}
				}
			}

			// Advance through the tree by recursing on each child.
			if ((this._nw != null) && (this._nw.Overlap(bothLists, _oc))) {
				c = true;
			}

			if ((this._ne != null) && (this._ne.Overlap(bothLists, _oc))) {
				c = true;
			}

			if ((this._se != null) && (this._se.Overlap(bothLists, _oc))) {
				c = true;
			}

			if ((this._sw != null) && (this._sw.Overlap(bothLists, _oc))) {
				c = true;
			}
			return c;
 		}
		#endregion
	}
}

