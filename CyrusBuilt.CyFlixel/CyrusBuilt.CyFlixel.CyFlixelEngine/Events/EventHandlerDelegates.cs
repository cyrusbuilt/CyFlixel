//
//  EventHandlerDelegates.cs
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

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Events
{
	/// <summary>
	/// Handler delegate for sprite collision events.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	/// <returns>
	/// true if the event was handled.
	/// </returns>		
	public delegate Boolean SpriteCollisionEventHandler(Object sender, SpriteCollisionEventArgs e);

	/// <summary>
	/// Animation callback.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void AnimationCallback(Object sender, AnimationEventArgs e);

	/// <summary>
	/// Handler delegate for weapon reload events.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void WeaponReloadEventHandler(Object sender, WeaponReloadEventArgs e);

	/// <summary>
	/// Handler delegate for the weapon empty event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void WeaponEmptyEventHandler(Object sender, WeaponEmptyEventArgs e);

	/// <summary>
	/// Handler delegate for the the weapon fire event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void WeaponFireEventHandler(Object sender, WeaponFireEventArgs e);

	/// <summary>
	/// Handler delegate for the item empty event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void ItemEmptyEventHandler(Object sender, ItemEmptyEventArgs e);

	/// <summary>
	/// Handler delegate for the item used event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void ItemUsedEventHandler(Object sender, ItemUsedEventArgs e);

	/// <summary>
	/// Handler delegate for the item replenish event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void ItemReplenishEventHandler(Object sender, ItemReplenishEventArgs e);

	/// <summary>
	/// Handler delegate for the weapon change (select) event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void WeaponChangeEventHandler(Object sender, WeaponChangeEventArgs e);

	/// <summary>
	/// Handler delegate for the item change (select) event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void ItemChangeEventHandler(Object sender, ItemChangeEventArgs e);

	/// <summary>
	/// Handler delegate for the weapon added event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void WeaponAddedEventHandler(Object sender, WeaponAddedEventArgs e);

	/// <summary>
	/// Handler delegate for the item added event.
	/// </summary>
	/// <param name="sender">
	/// The object sending the event call.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void ItemAddedEventHandler(Object sender, ItemAddedEventArgs e);
}

