//
//  FlixelGroup.cs
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
using Microsoft.Xna.Framework.Graphics;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// Represens a CyFlixel object containing other CyFlixel obejcts (group).
	/// </summary>
	public class FlixelGroup : CyFlixelObject
	{
		#region Fields
		private List<CyFlixelObject> _members = null;
		protected Vector2 _last = Vector2.Zero;
		protected Boolean _first = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelGroup"/>
		/// class. This is the default constructor.
		/// </summary>
		public FlixelGroup()
			: base() {
			base.IsGroup = true;
			base.Solid = false;
			this._members = new List<CyFlixelObject>();
			this._first = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the group members.
		/// </summary>
		public List<CyFlixelObject> Members {
			get { return this._members; }
			set { this._members = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Add the specified object to the list of children.
		/// </summary>
		/// <param name="obj">
		/// The object to add to the list.
		/// </param>
		/// <param name="shareScroll">
		/// Set true if the specified object should sync up with this layer's
		/// scroll factor.
		/// </param>
		/// <returns>
		/// The same object that was passed in.
		/// </returns>			
		public CyFlixelObject Add(CyFlixelObject obj, Boolean shareScroll) {
			// TODO Is it really necessary to return a value if we're only
			// returning what we're given?? What does that accomplish?
			// Seems like this should be a subroutine instead of a function...
			if (!this._members.Contains(obj)) {
				this._members.Add(obj);
				if (shareScroll) {
					obj.ScrollFactor = base.ScrollFactor;
				}
			}
			return obj;
		}

		/// <summary>
		/// Add the specified object to the list of children.
		/// </summary>
		/// <param name="obj">
		/// The object to add to the list.
		/// </param>
		/// <returns>
		/// The same object that was passed in.
		/// </returns>	
		public CyFlixelObject Add(CyFlixelObject obj) {
			return this.Add(obj, false);
		}

		/// <summary>
		/// Replace an existing object with a new one.
		/// </summary>
		/// <param name="oldObj">
		/// The existing object to replace.
		/// </param>
		/// <param name="newObj">
		/// The object to replace the existing one with.
		/// </param>
		/// <returns>
		/// If successful, the new object; Otherwise, null.
		/// </returns>			
		public CyFlixelObject Replace(CyFlixelObject oldObj, CyFlixelObject newObj) {
			Int32 index = this._members.IndexOf(oldObj);
			if ((index < 0) || (index >= this._members.Count)) {
				return null;
			}
			this._members[index] = newObj;
			return newObj;
		}

		/// <summary>
		/// Removes an object from the group.
		/// </summary>
		/// <param name="obj">
		/// The object to remove.
		/// </param>
		/// <param name="splice">
		/// Set true if the object should be cut from the group entirely, or
		/// just nullified.
		/// </param>
		/// <returns>
		/// If successful, the removed object; Otherwise, null.
		/// </returns>			
		public CyFlixelObject Remove(CyFlixelObject obj, Boolean splice) {
			Int32 index = this._members.IndexOf(obj);
			if ((index < 0) || (index >= this._members.Count)) {
				return null;
			}

			if (splice) {
				this._members.RemoveAt(index);
			}
			else {
				this._members[index] = null;
			}
			return obj;
		}

		/// <summary>
		/// Removes an object from the group.
		/// </summary>
		/// <param name="obj">
		/// The object to remove.
		/// </param>
		/// <returns>
		/// If successful, the removed object; Otherwise, null.
		/// </returns>
		public CyFlixelObject Remove(CyFlixelObject obj) {
			return this.Remove(obj, false);
		}

		/// <summary>
		/// Sort the group according to the specified value and order. You must
		/// implement your own IComparer interface for each sorting operation
		/// you want to perform. For example, to sort game objects for Zelda-style
		/// overlaps, you might call Sort by an object's "Y" member at the bottom
		/// of your Update() override. To sort all existing objects after a big
		/// explosion or bomb attack, you might sort by Exists.
		/// </summary>
		/// <param name="sorter">
		/// The object that will receive the sorting comparisons.
		/// </param>
		public void Sort(IComparer<CyFlixelObject> sorter) {
			this._members.Sort(sorter);
		}

		/// <summary>
		/// Retrieves the first object flagged as being non-existent in the
		/// group. This is handy for recycling in general (eg. respawning
		/// enemies).
		/// </summary>
		/// <returns>
		/// The first object in the collection flagged as non-existent.
		/// </returns>
		public CyFlixelObject GetFirstAvailable() {
			CyFlixelObject o = null;
			foreach (CyFlixelObject cfo in this._members) {
				if (!cfo.Exists) {
					o = cfo;
					break;
				}
			}
			return o;
		}

		/// <summary>
		/// Gets the index of the first null slot.
		/// </summary>
		/// <returns>
		/// The index of the first null slot in the group, or -1 if no null
		/// slots are found.
		/// </returns>
		public Int32 GetFirstNull() {
			Int32 i = 0;
			Int32 ml = this._members.Count;
			while (i < ml) {
				if (this._members[i] == null) {
					return i;
				}
				else {
					i++;
				}
			}
			return -1;
		}

		/// <summary>
		/// Resets the first object in the group flagged as non-existent and
		/// resets it.
		/// </summary>
		/// <returns>
		/// true if a suitable object was found and reset; Otherwise, false.
		/// </returns>
		/// <param name="x">
		/// The new X coordinate of this object.
		/// </param>
		/// <param name="y">
		/// The new Y coordinate of this object.
		/// </param>
		public Boolean ResetFirstAvailable(Int32 x, Int32 y) {
			CyFlixelObject o = this.GetFirstAvailable();
			if (o == null) {
				return false;
			}

			o.Reset(x, y);
			return true;
		}

		/// <summary>
		/// Gets the first object in the group flagged as existing. This is
		/// handy for checking to see if everything is wiped out, or choosing
		/// a squad leader, etc.
		/// </summary>
		/// <returns>
		/// The first object flagged as existing; Otherwise, null.
		/// </returns>
		public CyFlixelObject GetFirstExtant() {
			CyFlixelObject o = null;
			foreach (CyFlixelObject cfo in this._members) {
				if (cfo.Exists) {
					o = cfo;
					break;
				}
			}
			return o;
		}

		/// <summary>
		/// Gets the first object in the group flagged as not being dead.
		/// </summary>
		/// <returns>
		/// The first object in the group flagged as being not dead; Otherwise,
		/// null (no live objects).
		/// </returns>
		public CyFlixelObject GetFirstAlive() {
			CyFlixelObject o = null;
			foreach (CyFlixelObject cfo in this._members) {
				if ((cfo.Exists) && (!cfo.IsDead)) {
					o = cfo;
					break;
				}
			}
			return o;
		}

		/// <summary>
		/// Gets the first dead object in the group
		/// </summary>
		/// <returns>
		/// The first dead object in the group; Otherwise, null.
		/// </returns>
		public CyFlixelObject GetFirstDead() {
			CyFlixelObject o = null;
			foreach (CyFlixelObject cfo in this._members) {
				if (cfo.IsDead) {
					o = cfo;
					break;
				}
			}
			return o;
		}

		/// <summary>
		/// Determines how many objects in the group are alive.
		/// </summary>
		/// <returns>
		/// The number of objects in the group that are not dead; Otherwise, -1
		/// if the group is empty.
		/// </returns>
		public Int32 CountLiving() {
			Int32 count = 0;
			foreach (CyFlixelObject o in this._members) {
				if ((o.Exists) && (!o.IsDead)) {
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// Determines the number of objects in the group that are not alive.
		/// </summary>
		/// <returns>
		/// The number of objects in the group that are dead; or -1 if the
		/// group is empty.
		/// </returns>
		public Int32 CountDead() {
			Int32 count = 0;
			foreach (CyFlixelObject o in this._members) {
				if (o.IsDead) {
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// Gets a count of how many objects in this group are on-screen right
		/// now.
		/// </summary>
		/// <returns>
		/// The number of objects in the group that are currently on-screen;
		/// Otherwise, -1 if this group is empty.
		/// </returns>
		public Int32 CountOnScreen() {
			Int32 count = 0;
			foreach (CyFlixelObject o in this._members) {
				if (o.IsOnScreen()) {
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// Gets an object at random from this group.
		/// </summary>
		/// <returns>
		/// A random object from this group.
		/// </returns>
		public CyFlixelObject GetRandom() {
			Int32 c = 0;
			Int32 l = this._members.Count;
			Int32 i = (Int32)(GameMath.Random() * l);
			CyFlixelObject o = null;
			while ((o == null) && (c < l)) {
				o = this._members[(i++) % l] as CyFlixelObject;
				c++;
			}
			return o;
		}

		/// <summary>
		/// Internal helper method, helps with the moving/updating of group
		/// members.
		/// </summary>
		protected void SaveOldPosition() {
			if (this._first) {
				this._first = false;
				this._last.X = 0;
				this._last.Y = 0;
				return;
			}
			this._last.X = base.X;
			this._last.Y = base.Y;
		}

		/// <summary>
		/// Updates all the group members. Depends on <see cref="SaveOldPosition()"/>
		/// to set up the correct values in the last vector in order to work
		/// properly.
		/// </summary>
		protected virtual void UpdateMembers() {
			float mx = 0;
			float my = 0;
			Boolean moved = false;
			if ((base.X != this._last.X) || (base.Y != this._last.Y)) {
				moved = true;
				mx = (base.X - this._last.X);
				my = (base.Y - this._last.Y);
			}

			float x = 0;
			float y = 0;
			foreach (CyFlixelObject o in this._members) {
				if (o.Exists) {
					if (moved) {
						if (o.IsGroup) {
							o.Reset((o.X + mx), (o.Y + my));
						}
						else {
							o.X += mx;
							o.Y += my;
						}
					}
					
					if (o.Active) {
						o.Update();
					}
					
					if ((moved) && (o.Solid)) {
						o.CollisionHullX.Width += ((mx > 0) ? mx : -mx);
						if (mx < 0) {
							o.CollisionHullX.X += mx;
						}
						
						o.CollisionHullY.X = base.X;
						o.CollisionHullY.Height += ((my > 0) ? my : -my);
						if (my < 0) {
							o.CollisionHullY.Y += my;
						}

						x = o.CollisionVector.X;
						x += mx;
						y = o.CollisionVector.Y;
						y += my;
						o.CollisionVector = new Vector2(x, y);
					}
				}
			}
		}

		/// <summary>
		/// Automatically goes through and calls update on everything you added.
		/// Override this function to handle custom input and perform collisions.
		/// </summary>
		public override void Update() {
			this.SaveOldPosition();
			base.UpdateMotion();
			this.UpdateMembers();
			base.UpdateFlickering();
		}

		/// <summary>
		/// Loops through and renders all members.
		/// </summary>
		/// <param name="sb">
		/// The group of sprites to render.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="sb"/> cannot be null.
		/// </exception>
		protected void RenderMembers(SpriteBatch sb) {
			if (sb == null) {
				throw new ArgumentNullException("sb");
			}

			foreach (CyFlixelObject o in this._members) {
				if ((o.Exists) && (o.Visible)) {
					o.Render(sb);
				}
			}
		}

		/// <summary>
		/// Automatically goes through and renders everything you added.
		/// Override this loop to control render order manually.
		/// </summary>
		/// <param name="spriteBatch">
		/// A group of sprites to be drawn using the same settings.
		/// </param>
		public override void Render(SpriteBatch spriteBatch) {
			this.RenderMembers(spriteBatch);
		}

		/// <summary>
		/// Kills all the members.
		/// </summary>
		protected void KillMembers() {
			foreach (CyFlixelObject o in this._members) {
				o.Kill();
			}
		}

		/// <summary>
		/// Kill this group and all its members.
		/// </summary>
		public override void Kill() {
			this.KillMembers();
			base.Kill();
		}

		/// <summary>
		/// Loops through and disposes all the members.
		/// </summary>
		protected void DisposeMembers() {
			foreach (CyFlixelObject o in this._members) {
				o.Dispose();
			}
			this._members.Clear();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelGroup"/>
		/// object. Override this method to handle deleting or "shutdown" type
		/// operations you might need, such as removing traditional Flash
		/// childrend like Sprite objects.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelGroup"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelGroup"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelGroup"/>
		/// so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.FlixelGroup"/>
		/// was occupying.
		/// </remarks>
		public override void Dispose() {
			this.DisposeMembers();
			base.Dispose();
		}

		/// <summary>
		/// If the group's position is reset, then will reset all its members too.
		/// </summary>
		/// <param name="x">
		/// The new X coordinate of this object.
		/// </param>
		/// <param name="y">
		/// The new Y coordinate of this object.
		/// </param>
		public override void Reset(float x, float y) {
			this.SaveOldPosition();
			base.Reset(x, y);
			float mx = 0;
			float my = 0;
			Boolean moved = false;
			if ((base.X != this._last.X) || (base.Y != this._last.Y)) {
				moved = true;
				mx = (base.X - this._last.X);
				my = (base.Y - this._last.Y);
			}

			float vx = 0;
			float vy = 0;
			foreach (CyFlixelObject o in this._members) {
				if (o.Exists) {
					if (moved) {
						if (o.IsGroup) {
							o.Reset((o.X + mx), (o.Y + my));
						}
					}
					else {
						o.X += mx;
						o.Y += my;
						if (this._solid) {
							o.CollisionHullX.Width += ((mx > 0) ? mx : -mx);
							if (mx < 0) {
								o.CollisionHullX.X += mx;
							}

							o.CollisionHullY.X = base.X;
							o.CollisionHullY.Height += ((my > 0) ? my : -my);
							if (my < 0) {
								o.CollisionHullY.Y += my;
							}

							vx = o.CollisionVector.X;
							vx += mx;
							vy = o.CollisionVector.Y;
							vy += my;
							o.CollisionVector = new Vector2(vx, vy);
						}
					}
				}
			}
		}
 		#endregion
	}
}

