//
//  GraphicsUtils.cs
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
using CyrusBuilt.CyFlixel.CyFlixelEngine.Events;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine
{
	/// <summary>
	/// Graphics utility functions.
	/// </summary>
	public static class GraphicsUtils
	{
		private static QuadTree _quadTree = null;

		/// <summary>
		/// Gets or sets the quad tree.
		/// </summary>
		public static QuadTree QuadTree {
			get { return _quadTree; }
			set { _quadTree = value; }
		}

		/// <summary>
		/// Rotates the specified point in 2D space around another point by the
		/// given angle.
		/// </summary>
		/// <returns>
		/// A point containing the coordinates of the rotated point.
		/// </returns>
		/// <param name="x">
		/// The X coordinate.
		/// </param>
		/// <param name="y">
		/// The Y coordinate.
		/// </param>
		/// <param name="pivotX">
		/// The X coordinate of the point you want to pivot around.
		/// </param>
		/// <param name="pivotY">
		/// The Y coordinate of the point you want to pivot around.
		/// </param>
		/// <param name="angle">
		/// The number of degrees to rotate.
		/// </param>
		public static Vector2 RotatePoint(float x, float y, float pivotX, float pivotY, float angle) {
			float radians = -angle / 180f * (float)Math.PI;
			float dx = (x - pivotX);
			float dy = (pivotY - y);
			float vx = (float)(pivotX + Math.Cos(radians) * dx - Math.Sin(radians) * dy);
			float vy = (float)(pivotY - (Math.Sin(radians) * dx + Math.Cos(radians) * dy));
			return new Vector2(vx, vy);
		}

		/// <summary>
		/// Calculate to angle between the point and the origin (0, 0).
		/// </summary>
		/// <returns>
		/// The angle in degrees.
		/// </returns>
		/// <param name="x">
		/// The X coordinate of the point.
		/// </param>
		/// <param name="y">
		/// The Y coordinate of the point.
		/// </param>
		public static float GetAngle(float x, float y) {
			return (float)(Math.Atan2(x, y) * 180f / Math.PI);
		}

		/// <summary>
		/// Generates a flash color from RGB components.
		/// </summary>
		/// <returns>
		/// The color as a <see cref="uint"/> code.
		/// </returns>
		/// <param name="red">
		/// The red component, between 0 and 255.
		/// </param>
		/// <param name="green">
		/// The green component, between 0 and 255.
		/// </param>
		/// <param name="blue">
		/// The blue component, between 0 and 255.
		/// </param>
		/// <param name="alpha">
		/// How opaque the color should be, either between 0 and 1 or 0 and 255.
		/// </param>
		public static uint GetColor(uint red, uint green, uint blue, float alpha) {
			return ((uint)((alpha > 1) ? alpha : (alpha * 255)) & 0xFF) << 24 | (red & 0xFF) << 16 | (green & 0xFF) << 8 | (blue & 0xFF);
		}

		/// <summary>
		/// Generates a flash color from RGB components.
		/// </summary>
		/// <returns>
		/// The color as a <see cref="uint"/> code.
		/// </returns>
		/// <param name="red">
		/// The red component, between 0 and 255.
		/// </param>
		/// <param name="green">
		/// The green component, between 0 and 255.
		/// </param>
		/// <param name="blue">
		/// The blue component, between 0 and 255.
		/// </param>
		public static uint GetColor(uint red, uint green, uint blue) {
			return GetColor(red, green, blue, 1.0f);
		}

		/// <summary>
		/// Generates a flash color from HSB components.
		/// </summary>
		/// <returns>
		/// The color as a <see cref="uint"/> code.
		/// </returns>
		/// <param name="hue">
		/// A number between 0 and 360, indicating position on a color wheel.
		/// </param>
		/// <param name="saturation">
		/// A number between 0 and 1, indicating how colorful or gray the color
		/// should be. 0 is gray, 1 is vibrant.
		/// </param>
		/// <param name="brightness">
		/// A number between 0 and 1, indicating how bright the color should be.
		/// 0 is black, 1 is full brightness.
		/// </param>
		/// <param name="alpha">
		/// How opaque the color should be, either between 0 and 1 or 0 and 255.
		/// </param>
		public static uint GetColorHSB(float hue, float saturation, float brightness, float alpha) {
			float red = 0f;
			float green = 0f;
			float blue = 0f;
			if (saturation == 0) {
				red = brightness;
				green = brightness;
				blue = brightness;
			}
			else {
				if (hue == 360) {
					hue = 0;
				}
				Int32 slice = ((Int32)hue / 60);
				float hf = (hue / 60 - slice);
				float aa = (brightness * (1 - saturation));
				float bb = (brightness * (1 - saturation * hf));
				float cc = (brightness * (1 - saturation * (1.0f - hf)));
				switch (slice) {
					case 0: red = brightness; green = cc; blue = aa; break;
					case 1: red = bb; green = brightness; blue = aa; break;
					case 2: red = aa; green = brightness; blue = cc; break;
					case 3: red = aa; green = bb; blue = brightness; break;
					case 4: red = cc; green = aa; blue = brightness; break;
					case 5: red = brightness; green = aa; blue = bb; break;
					default: red = 0; green = 0; blue = 0; break;
				}
			}
			return ((uint)((alpha > 1) ? alpha : (alpha * 255)) & 0xFF) << 24 | (uint)(red * 255) << 16 | (uint)(green * 255) << 8 | (uint)(blue * 255);
		}

		/// <summary>
		/// Generates a flash color from HSB components.
		/// </summary>
		/// <returns>
		/// The color as a <see cref="uint"/> code.
		/// </returns>
		/// <param name="hue">
		/// A number between 0 and 360, indicating position on a color wheel.
		/// </param>
		/// <param name="saturation">
		/// A number between 0 and 1, indicating how colorful or gray the color
		/// should be. 0 is gray, 1 is vibrant.
		/// </param>
		/// <param name="brightness">
		/// A number between 0 and 1, indicating how bright the color should be.
		/// 0 is black, 1 is full brightness.
		/// </param>
		public static uint GetColorHSB(float hue, float saturation, float brightness) {
			return GetColorHSB(hue, saturation, brightness, 1.0f);
		}

		/// <summary>
		/// Loads a list with the RGBA values of a flash color. RGB values are
		/// stored as 0 - 255. Alpha is stored as a floating point number
		/// between 0 and 1.
		/// </summary>
		/// <returns>
		/// A list containing red, green, blue and alpha values of the given
		/// color.
		/// </returns>
		/// <param name="color">
		/// The color you want to break into components.
		/// </param>
		/// <param name="results">
		/// An optional parameter, allows you to use a list that is already in
		/// memory to store the result.
		/// </param>
		public static List<Object> GetRGBA(uint color, List<Object> results) {
			if (results == null) {
				results = new List<Object>(4);
			}
			results[0] = (color >> 16) & 0xFF;
			results[1] = (color >> 8) & 0xFF;
			results[2] = color & 0xFF;
			results[3] = (float)((color >> 24) & 0xFF) / 255;
			return results;
		}

		// TODO deprecate all methods that return a list of objects containing
		// RGB or HSB values. Return a struct or class with real values instead.

		/// <summary>
		/// Loads a list with the HSB values of a flash color. Hue is a value
		/// between 0 and 360. Saturation, brightness and alpha are as floating
		/// point numbers between 0 and 1.
		/// </summary>
		/// <returns>
		/// A list object containg the hue, saturation, brightness, and alpha
		/// components.
		/// </returns>
		/// <param name="color">
		/// The color you want to break into components.
		/// </param>
		/// <param name="results">
		/// An optional parameter, allows you to use a list that already exists
		/// in memory to store the results.
		/// </param>
		public static List<Object> GetHSB(uint color, List<Object> results) {
			if (results == null) {
				results = new List<Object>(4);
			}

			float red = (float)((color >> 16) & 0xFF) / 255;
			float green = (float)((color >> 8) & 0xFF) / 255;
			float blue = (float)(color & 0xFF) / 255;
			float m = (red > green) ? red : green;
			float dmax = (m > blue) ? m : blue;
			m = (red > green) ? green : red;
			float dmin = (m > blue) ? blue : m;
			float range = (dmax - dmin);

			results[2] = dmax;
			results[1] = 0;
			results[0] = 0;

			if (dmax != 0) {
				results[1] = (range / dmax);
			}

			if ((float)results[1] != 0f) {
				if (red == dmax) {
					results[0] = (green - blue) / range;
				}
				else if (green == dmax) {
					results[0] = (2 + (blue - red) / range);
				}
				else if (blue == dmax) {
					results[0] = (4 + (red - green) / range);
					results[0] = (Int32)results[0] * 60;
				}

				if ((Int32)results[0] < 0) {
					results[0] = (Int32)results[0] + 360;
				}
			}

			results[3] = (float)((color >> 24) & 0xFF) / 255;
			return results;
		}

		/// <summary>
		/// A tween-like function that takes a starting velocity
		/// and some other factors and returns an altered velocity.
		/// </summary>
		/// <returns>
		/// The altered velocity value.
		/// </returns>
		/// <param name="velocity">
		/// Any component of a velocity (eg. 20).
		/// </param>
		/// <param name="acceleration">
		/// Rate at which the velocity is changing.
		/// </param>
		/// <param name="drag">
		/// A deceleration value, this is how much the velocity changes if
		/// acceleration is not set.
		/// </param>
		/// <param name="max">
		/// An absolute value cap for the velocity.
		/// </param>
		public static float ComputeVelocity(float velocity, float acceleration, float drag, float max) {
			if (acceleration != 0) {
				velocity += acceleration * EngineGlobal.Elapsed;
			}
			else if (drag != 0) {
				float d = drag * EngineGlobal.Elapsed;
				if ((velocity - d) > 0) {
					velocity -= d;
				}
				else if ((velocity + d) < 0) {
					velocity += d;
				}
				else {
					velocity = 0;
				}

				if ((velocity != 0) && (max != 10000)) {
					if (velocity > max) {
						velocity = max;
					}
					else if (velocity < -max) {
						velocity = -max;
					}
				}
			}
			return velocity;
		}

		/// <summary>
		/// Takes to objects and seperates them along their X-axis (if
		/// possible/reasonable). This is also used as a quadtree callback
		/// function.
		/// </summary>
		/// <param name="sender">
		/// The object sending the event call.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		/// <returns>
		/// true if the attacker and target hit each other on the X axis;
		/// otherwise, false.
		/// </returns>			
		public static Boolean SolveXCollision(Object sender, SpriteCollisionEventArgs e) {
			// Avoid fucked up collisions ahead of time.
			float o1 = e.Attacker.CollisionVector.X;
			float o2 = e.Target.CollisionVector.X;
			if (o1 == o2) {
				return false;
			}

			// Give the objects a heads up that we're about to resolve some collisions.
			e.Attacker.PreCollide(e.Target);
			e.Target.PreCollide(e.Attacker);

			// Basic resolution vars.
			Boolean f1 = false;
			Boolean f2 = false;
			Boolean hit = false;
			Boolean p1hn2 = false;
			float overlap = 0f;

			// Directional vars.
			Boolean attackerStopped = (o1 == 0);
			Boolean attackerMoveNeg = (o1 < 0);
			Boolean attackerMovePos = (o1 > 0);
			Boolean targetStopped = (o2 == 0);
			Boolean targetMoveNeg = (o2 < 0);
			Boolean targetMovePos = (o2 > 0);

			// Offset loop vars.
			Int32 i1 = 0;
			Int32 i2 = 0;
			CyFlixelRect attackerHull = e.Attacker.CollisionHullX;
			CyFlixelRect targetHull = e.Target.CollisionHullX;
			List<Vector2> col1 = e.Attacker.CollisionOffsets;
			List<Vector2> col2 = e.Target.CollisionOffsets;
			Int32 l1 = col1.Count;
			Int32 l2 = col2.Count;
			float ox1 = 0f;
			float oy1 = 0f;
			float ox2 = 0f;
			float oy2 = 0f;
			float r1 = 0f;
			float r2 = 0f;
			float sv1 = 0f;
			float sv2 = 0f;

			// Determine based on object's movement patterns if it was a
			// right-side or left-side collision.
			p1hn2 = (((attackerStopped) && (targetMoveNeg)) || ((attackerMovePos) && (targetStopped)) || // The obvious cases.
			         ((attackerMovePos) && (targetMoveNeg)) ||
			         ((attackerMoveNeg) && (targetMoveNeg) && (((o1 > 0) ? o1 : -o1) < ((o2 > 0) ? o2 : -o2))) || // both moving left, target overtakes attacker.
			         ((attackerMovePos) && (targetMovePos) && (((o1 > 0) ? o1 : -o1) > ((o2 > 0) ? o2 : -o2)))); // both moving right, attacker overtakes target.

			// Check to see if these objects allow these collisions.
			if (p1hn2 ? (!e.Attacker.CollideRight || !e.Target.CollideRight) : (!e.Attacker.CollideLeft || !e.Target.CollideRight)) {
				return false;
			}

			// This looks insane, but we're looping through collision offsets on each object.
			while (i1 < l1) {
				ox1 = col1[i1].X;
				oy1 = col1[i1].Y;
				attackerHull.X += ox1;
				attackerHull.Y += oy1;
				i2 = 0;
				while (i2 < l2) {
					ox2 = col2[i2].X;
					oy2 = col2[i2].Y;
					targetHull.X += ox2;
					targetHull.Y += oy2;

					// See if its actually a valid collision.
					if (((attackerHull.X + attackerHull.Width) < (targetHull.X + GameMath.RoundingError)) ||
					    ((attackerHull.X + GameMath.RoundingError) > (targetHull.X + targetHull.Width)) ||
					    ((attackerHull.Y + attackerHull.Height) < (targetHull.Y + GameMath.RoundingError)) ||
					    ((attackerHull.Y + GameMath.RoundingError) > (targetHull.Y + targetHull.Height))) {
						targetHull.X -= ox2;
						targetHull.Y -= oy2;
						i2++;
						continue;
					}

					// Calculate overlap between attacker and target.
					if (p1hn2) {
						if (attackerMoveNeg) {
							r1 = (attackerHull.X + e.Attacker.CollisionHullY.Width);
						}
						else {
							r1 = (attackerHull.X + attackerHull.Width);
						}
						
						if (targetMoveNeg) {
							r2 = targetHull.X;
						}
						else {
							r2 = (targetHull.X + targetHull.Width - e.Target.CollisionHullY.Width);
						}
					}
					else {
						if (targetMoveNeg) {
							r1 = (-targetHull.X - e.Target.CollisionHullY.Width);
						}
						else {
							r1 = (-targetHull.X - targetHull.Width);
						}
						
						if (attackerMoveNeg) {
							r2 = -targetHull.X;
						}
						else {
							r2 = (-attackerHull.X - attackerHull.Width + e.Attacker.CollisionHullY.Width);
						}
					}
					
					overlap = (r1 - r2);

					// Slightly smarter version of checking if objects are 'fixed' or not.
					f1 = e.Attacker.Fixed;
					f2 = e.Target.Fixed;
					if (f1 && f2) {
						f1 &= ((e.Attacker.CollisionVector.X == 0) && (o1 == 0));
						f2 &= ((e.Target.CollisionVector.X == 0) && (o2 == 0));
					}
					
					// Last chance to skip out on a bogus collision resolution.
					if ((overlap == 0) ||
					    (((!f1) && ((overlap > 0) ? overlap : -overlap) > (attackerHull.Width * 0.8))) ||
					    (((!f2) && ((overlap > 0) ? overlap : -overlap) > (targetHull.Width * 0.8)))) {
						targetHull.X -= ox2;
						targetHull.Y -= oy2;
						i2++;
						continue;
					}
					
					hit = true;

					// Adjust objects according to their flags n stuff.
					sv1 = e.Target.Velocity.X;
					sv2 = e.Attacker.Velocity.X;
					if ((!f1) && (f2)) {
						if (e.Attacker.IsGroup) {
							e.Attacker.Reset((e.Attacker.X - overlap), e.Attacker.Y);
						}
						else {
							e.Attacker.X -= overlap;
						}
					}
					else if ((f1) && (!f2)) {
						if (e.Target.IsGroup) {
							e.Target.Reset((e.Target.X - overlap), e.Target.Y);
						}
						else {
							e.Target.X += overlap;
						}
					}
					else if ((!f1) && (!f2)) {
						overlap /= 2;
						if (e.Attacker.IsGroup) {
							e.Attacker.Reset((e.Attacker.X - overlap), e.Attacker.Y);
						}
						else {
							e.Attacker.X -= overlap;
						}
						
						if (e.Target.IsGroup) {
							e.Target.Reset((e.Target.X + overlap), e.Target.Y);
						}
						else {
							e.Target.X += overlap;
						}
						
						sv1 *= 0.5f;
						sv2 *= 0.5f;
					}

					if (p1hn2) {
						e.Attacker.HitRight(e.Target, sv1);
						e.Target.HitLeft(e.Attacker, sv2);
					}
					else {
						e.Attacker.HitLeft(e.Target, sv1);
						e.Target.HitRight(e.Attacker, sv2);
					}

					// Adjust collision hulls, if necessary.
					if ((!f1) && (overlap != 0)) {
						if (p1hn2) {
							targetHull.X += overlap;
							targetHull.Width -= overlap;
						}
						else {
							targetHull.Width += overlap;
							e.Target.CollisionHullY.X += overlap;
						}
					}

					targetHull.X -= ox2;
					targetHull.Y -= oy2;
					i2++;
				}
				attackerHull.X -= ox1;
				attackerHull.Y -= oy2;
				i1++;
			}
			return hit;
		}

		/// <summary>
		/// Takes the attacker and target and separates them along their Y axis
		/// (if possible/reasonable). This is a quad tree callback function that
		/// can be used externally as well.
		/// </summary>
		/// <returns>
		/// true if the the attacker and target hit each other on the Y axis;
		/// otherwise, false.
		/// </returns>
		/// <param name="sender">
		/// The object sending the event call (if any).
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		public static Boolean SolveYCollision(Object sender, SpriteCollisionEventArgs e) {
			// Avoid fucked up collisions ahead of time.
			float o1 = e.Attacker.CollisionVector.Y;
			float o2 = e.Target.CollisionVector.Y;
			if (o1 == o2) {
				return false;
			}

			// Give the attacker and target a heads up that we're about to solve
			// some collisions.
			e.Attacker.PreCollide(e.Target);
			e.Target.PreCollide(e.Attacker);

			// Basic resolution variables.
			Boolean f1 = false;
			Boolean f2 = false;
			Boolean hit = false;
			Boolean p1hn2 = false;
			float overlap = 0f;

			// Directional variables.
			Boolean attackerStopped = (o1 == 0);
			Boolean attackerMoveNeg = (o1 < 0);
			Boolean attackerMovePos = (o1 > 0);
			Boolean targetStopped = (o2 == 0);
			Boolean targetMoveNeg = (o2 < 0);
			Boolean targetMovePos = (o2 > 0);

			// Offset loop variables.
			Int32 i1 = 0;
			Int32 i2 = 0;
			CyFlixelRect attackerHull = e.Attacker.CollisionHullY;
			CyFlixelRect targetHull = e.Target.CollisionHullY;
			List<Vector2> co1 = e.Attacker.CollisionOffsets;
			List<Vector2> co2 = e.Target.CollisionOffsets;
			Int32 l1 = co1.Count;
			Int32 l2 = co2.Count;
			float ox1 = 0f;
			float oy1 = 0f;
			float ox2 = 0f;
			float oy2 = 0f;
			float r1 = 0f;
			float r2 = 0f;
			float sv1 = 0f;
			float sv2 = 0f;

			// Determine based on the object's movement patterns if it was a
			// top or bottom collision.
			p1hn2 = (((attackerStopped) && (targetMoveNeg)) || ((attackerMovePos) && (targetStopped)) || // The obvious cases.
			         ((attackerMoveNeg) && (targetMoveNeg) && (((o1 > 0) ? o1 : -o1) < ((o2 > 0) ? o2 : -o2))) || // Both moving up, target overtakes attacker.
			         ((attackerMovePos) && (targetMovePos) && (((o1 > 0) ? o1 : -o1) > ((o2 > 0) ? o2 : -o2))));  // Both moving down, attacker overtaks target.

			// Check to see if these objects allow these collisions.
			if (p1hn2 ? ((!e.Attacker.CollideBottom) || (!e.Attacker.CollideTop)) : ((!e.Attacker.CollideTop) || (!e.Target.CollideBottom))) {
				return false;
			}

			// This looks insane, but we're just looping through collision offsets on each object.
			while (i1 < l1) {
				ox1 = co1[i1].X;
				oy1 = co1[i1].Y;
				attackerHull.X += ox1;
				attackerHull.Y += oy1;
				i2 = 0;
				while (i2 < l2) {
					ox2 = co2[i2].X;
					oy2 = co2[i2].Y;
					targetHull.X += ox2;
					targetHull.Y += oy2;

					// See if it's actually a valid collision.
					if (((attackerHull.X + attackerHull.Width) < (targetHull.X + GameMath.RoundingError)) ||
					    ((attackerHull.X + GameMath.RoundingError) > (targetHull.X + targetHull.Width)) ||
					    ((attackerHull.Y + attackerHull.Height) < (targetHull.Y + GameMath.RoundingError)) ||
					    ((attackerHull.Y + GameMath.RoundingError) > (targetHull.Y + targetHull.Height))) {
						targetHull.X -= ox2;
						targetHull.Y -= oy2;
						i2++;
						continue;
					}

					// Calculate the overlap between the objects.
					if (p1hn2) {
						if (attackerMoveNeg) {
							r1 = (attackerHull.Y + e.Attacker.CollisionHullX.Height);
						}
						else {
							r1 = (attackerHull.Y + attackerHull.Height);
						}

						if (targetMoveNeg) {
							r2 = targetHull.Y;
						}
						else {
							r2 = (targetHull.Y + targetHull.Height - e.Target.CollisionHullX.Height);
						}
					}
					else {
						if (targetMoveNeg) {
							r1 = (-targetHull.Y - e.Target.CollisionHullX.Height);
						}
						else {
							r1 = (-targetHull.Y - targetHull.Height);
						}

						if (attackerMoveNeg) {
							r2 = -attackerHull.Y;
						}
						else {
							r2 = (-attackerHull.Y - attackerHull.Height + e.Attacker.CollisionHullX.Height);
						}
					}
					overlap = (r1 - r2);

					// Slightly smarter version of checking if objects are
					// 'fixed' in space or not.
					f1 = e.Attacker.Fixed;
					f2 = e.Target.Fixed;
					if ((f1) && (f2)) {
						f1 &= (e.Attacker.CollisionVector.X == 0) && (o1 == 0);
						f2 &= (e.Target.CollisionVector.X == 0) && (o2 == 0);
					}

					// Last chance to skip out on a bogus collision resolution.
					if ((overlap == 0) ||
					    (((!f1) && ((overlap > 0) ? overlap : -overlap) > (attackerHull.Height * 0.8))) ||
					    (((!f2) && ((overlap > 0) ? overlap : -overlap) > (targetHull.Height * 0.8)))) {
						targetHull.X -= ox2;
						targetHull.Y -= oy2;
						i2++;
						continue;
					}
					hit = true;

					// Adjust the objects according to their flags and stuff.
					sv1 = e.Target.Velocity.Y;
					sv2 = e.Attacker.Velocity.Y;
					if ((!f1) && (f2)) {
						if (e.Attacker.IsGroup) {
							e.Attacker.Reset(e.Attacker.X, (e.Attacker.Y - overlap));
						}
						else {
							e.Attacker.Y -= overlap;
						}
					}
					else if ((f1) && (!f2)) {
						if (e.Target.IsGroup) {
							e.Target.Reset(e.Target.X, (e.Target.Y + overlap));
						}
						else {
							e.Target.Y += overlap;
						}
					}
					else if ((!f1) && (!f2)) {
						overlap /= 2;
						if (e.Attacker.IsGroup) {
							e.Attacker.Reset(e.Target.X, (e.Target.Y - overlap));
						}
						else {
							e.Attacker.Y -= overlap;
						}

						if (e.Target.IsGroup) {
							e.Target.Reset(e.Target.X, (e.Target.Y - overlap));
						}
						else {
							e.Target.Y += overlap;
						}

						sv1 *= 0.5f;
						sv2 *= 0.5f;
					}

					if (p1hn2) {
						e.Attacker.HitBottom(e.Target, sv1);
						e.Target.HitTop(e.Attacker, sv2);
					}
					else {
						e.Attacker.HitTop(e.Target, sv1);
						e.Target.HitBottom(e.Attacker, sv2);
					}

					// Adjust collision hulls if necessary.
					if ((!f1) && (overlap != 0)) {
						if (p1hn2) {
							attackerHull.Y -= overlap;

							// Help stuff ride horizontally moving objects.
							if ((f2) && (e.Target.CanMove)) {
								sv1 = e.Target.CollisionVector.X;
								e.Attacker.X += sv1;
								attackerHull.X += sv1;
								e.Attacker.CollisionHullX.X += sv1;
							}
						}
						else {
							attackerHull.Y -= overlap;
							attackerHull.Height += overlap;
						}
					}

					if ((!f2) && (overlap != 0)) {
						if (p1hn2) {
							targetHull.Y += overlap;
							targetHull.Height -= overlap;
						}
						else {
							targetHull.Height += overlap;

							// Help stuff ride horizontally moving platforms.
							if ((f1) && (e.Attacker.CanMove)) {
								sv2 = e.Attacker.CollisionVector.X;
								e.Target.X += sv2;
								targetHull.X += sv2;
								e.Target.CollisionHullX.X += sv2;
							}
						}
					}

					targetHull.X -= ox2;
					targetHull.Y -= oy2;
					i2++;
				}
				attackerHull.X -= ox1;
				attackerHull.Y -= oy1;
				i1++;
			}
			return hit;
		}

		/// <summary>
		/// Checks to see if one object collides with another. Can be called
		/// with one object and one group, or two groups, or two objects. It
		/// will put everything into a <see cref="QuadTree"/> and then check
		/// for collisions. For maximum performance try bundling a lot of
		/// objects together using a <see cref="FlixelGroup"/> (even bundling
		/// groups together!). Note: Does not take object's ScrollFactor into
		/// account.
		/// </summary>
		/// <param name="obj1">
		/// The first object or group to check.
		/// </param>
		/// <param name="obj2">
		/// The second object or group to check.
		/// </param>
		/// <returns>
		/// True if the two objects collide; Otherwise, false.
		/// </returns>			
		public static Boolean Collide(CyFlixelObject obj1, CyFlixelObject obj2) {
			if ((obj1 == null) || (!obj1.Exists) ||
				(obj2 == null) || (!obj2.Exists)) {
				return false;
			}
			float x = QuadTree.Bounds.X;
			float y = QuadTree.Bounds.Y;
			float w = QuadTree.Bounds.Width;
			float h = QuadTree.Bounds.Height;
			_quadTree = new QuadTree(x, y, w, h, null);
			_quadTree.Add(obj1, QuadTree.B_LIST);

			Boolean match = (obj1 == obj2);
			if (!match) {
				_quadTree.Add(obj2, QuadTree.B_LIST);
			}

			Boolean cx = _quadTree.Overlap(!match, SolveXCollision);
			Boolean cy = _quadTree.Overlap(!match, SolveYCollision);
			return ((cx) || (cy));
		}

		/// <summary>
		/// Checks to see if one object overlaps another. Can be called with
		/// one object and one group, or two groups, or two objects. It will
		/// put everything into a <see cref="QuadTree"/> and then check for
		/// overlaps. For maximum performance, try bundling a lot of objects
		/// together using a <see cref="FlixelGroup"/> (even bundling groups
		/// together!). Note: Does not take object's ScrollFactor into account.
		/// </summary>
		/// <param name="obj1">
		/// The first object or group to check.
		/// </param>
		/// <param name="obj2">
		/// The second object or gorup to check. If it is the same as the first,
		/// CyFlixel knows to just do a comparison with a group.
		/// </param>
		/// <param name="e">
		/// The event arguments containing a callback function. If no function
		/// is provided, <see cref="QuadTree"/> will call <see cref="CyFlixelObject.Kill()"/>
		/// on both objects.
		/// </param>
		/// <returns>
		/// true if the objects overlap; Otherwise, false.
		/// </returns>			
		public static Boolean Overlap(CyFlixelObject obj1, CyFlixelObject obj2, SpriteCollisionEventHandler e) {
			if ((obj1 == null) || (!obj1.Exists) || 
			    (obj2 == null) || (!obj2.Exists)) {
				return false;
			}

			float x = QuadTree.Bounds.X;
			float y = QuadTree.Bounds.Y;
			float w = QuadTree.Bounds.Width;
			float h = QuadTree.Bounds.Height;
			_quadTree = new QuadTree(x, y, w, h, null);
			_quadTree.Add(obj1, QuadTree.A_LIST);
			if (obj1 == obj2) {
				return _quadTree.Overlap(false, e);
			}

			_quadTree.Add(obj2, QuadTree.B_LIST);
			return _quadTree.Overlap(true, e);
		}

		/// <summary>
		/// Specifies a more efficient boundary for your game world. This boundary
		/// is used by <see cref="Overlap()"/> and <see cref="Collide()"/>, so
		/// it can't hurt to have it be the right size. CyFlixel with invent a
		/// size for you, but its pretty huge - 256x the size of the screen,
		/// whatever that may be. Leave width and height empty if you want to
		/// just update the game world's position.
		/// </summary>
		/// <param name="x">
		/// The x coordinate of the left side of the game world.
		/// </param>
		/// <param name="y">
		/// The y coordinate of the top of the game world.
		/// </param>
		/// <param name="width">
		/// The desired width of the game world.
		/// </param>
		/// <param name="height">
		/// The desired height of the game world.
		/// </param>
		/// <param name="divisions">
		/// Should be a non-zero value to set <see cref="QuadTree.Divisions"/>.
		/// </param>
		public static void SetWorldBounds(float x, float y, float width, float height, uint divisions) {
			QuadTree.Bounds.X = x;
			QuadTree.Bounds.Y = y;
			if (width > 0) {
				QuadTree.Bounds.Width = width;
			}

			if (height > 0) {
				QuadTree.Bounds.Height = height;
			}

			if (divisions > 0) {
				QuadTree.Divisions = divisions;
			}
		}

		/// <summary>
		/// Specifies a more efficient boundary for your game world. This boundary
		/// is used by <see cref="Overlap()"/> and <see cref="Collide()"/>, so
		/// it can't hurt to have it be the right size. CyFlixel with invent a
		/// size for you, but its pretty huge - 256x the size of the screen,
		/// whatever that may be. Leave width and height empty if you want to
		/// just update the game world's position.
		/// </summary>
		/// <param name="x">
		/// The x coordinate of the left side of the game world.
		/// </param>
		/// <param name="y">
		/// The y coordinate of the top of the game world.
		/// </param>
		/// <param name="width">
		/// The desired width of the game world.
		/// </param>
		/// <param name="height">
		/// The desired height of the game world.
		/// </param>
		/// <remarks>
		/// Defaults <see cref="QuadTree.Divisions"/> to 3.
		/// </remarks>			
		public static void SetWorldBounds(float x, float y, float width, float height) {
			SetWorldBounds(x, y, width, height, 3);
		}
	}
}

