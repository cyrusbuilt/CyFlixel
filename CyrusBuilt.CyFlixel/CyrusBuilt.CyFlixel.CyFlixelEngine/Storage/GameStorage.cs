//
//  GameStorage.cs
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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
#if !WINDOWS_PHONE
using Microsoft.Xna.Framework.Storage;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Storage
{
	/// <summary>
	/// Helps automate and simplify save game functionality.
	/// </summary>
	public class GameStorage : GameComponent
	{
		#region Fields
		private const String SAVE_FILE = "cyflixelsavedata.dat";
		private Boolean _wantsDevice = false;
		private IAsyncResult _storageasync = null;
		private StorageDevice _device = null;
		private SaveData _savedata = null;
		private String _name = String.Empty;
		protected StorageContainer _so = null;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// class. This is the default constructor.
		/// </summary>
		public GameStorage()
			: base(EngineGlobal.GameReference) {
			EngineGlobal.GameReference.Components.Add(this);
			this.GetStorageDevice();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// class with the storage device that will persist the savegame data.
		/// </summary>
		/// <param name="device">
		/// The storage device to persist the savegame data to.
		/// </param>
		public GameStorage(StorageDevice device)
			: base(EngineGlobal.GameReference) {
			EngineGlobal.GameReference.Components.Add(this);
			this._device = device;
			this._name = String.Empty;
			this._so = null;
			this._savedata = new SaveData();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if the underlying storage resources should be disposed.
		/// </param>
		protected override void Dispose(bool disposing) {
			if (this._so != null) {
				this.ForceSave();
			}

			if (disposing) {
				if (this._storageasync != null) {
					if (this._storageasync.AsyncWaitHandle != null) {
						this._storageasync.AsyncWaitHandle.Dispose();
					}
					this._storageasync = null;
				}

				if (this._so != null) {
					this._so.Dispose();
					this._so = null;
				}

				this._device = null;
				this._savedata = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// so the garbage collector can reclaim the memory that the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// was occupying.
		/// </remarks>
		new public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations
		/// before the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~GameStorage() {
			this.Dispose(false);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the save game data.
		/// </summary>
		public SaveData Data {
			get { return this._savedata; }
		}

		/// <summary>
		/// Gets or sets the name of the save game.
		/// </summary>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.Storage.GameStorage"/>
		/// is waiting on device selector.
		/// </summary>
		/// <value>
		/// true if waiting on device selector; otherwise, false.
		/// </value>
		public Boolean WaitingOnDeviceSelector {
			get { return this._wantsDevice; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance can save to the
		/// storage device.
		/// </summary>
		/// <value>
		/// true if this instance can save; otherwise, false.
		/// </value>
		public Boolean CanSave {
			get { return ((this._device != null) && (this._device.IsConnected)); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the storage device.
		/// </summary>
		private void GetStorageDevice() {
			if ((this._wantsDevice) || (this._storageasync != null)) {
				return;
			}
			this._device = null;
			this._wantsDevice = true;
		}

		/// <summary>
		/// Writes the local shared object to disk immediately.
		/// </summary>
		/// <returns>
		/// true if the flush was successful; Otherwise, false.
		/// </returns>
		/// <param name="minFileSize">
		/// If a specific amount of space is needed for the save,
		/// specify the value in bytes here.
		/// </param>
		public Boolean ForceSave(uint minFileSize) {
			if ((this._so == null) || (String.IsNullOrEmpty(this._name))) {
				// TODO Call logging function.
				return false;
			}

			try {
				// Open a storage container. Then wait or the waithandle to
				// become signaled. And finally, close the wait handle.
				IAsyncResult result = this._device.BeginOpenContainer(this._name, null, null);
				result.AsyncWaitHandle.WaitOne();
				this._so = this._device.EndOpenContainer(result);
				result.AsyncWaitHandle.Close();

				// IF the file already exists, delete it so we can create
				// a fresh one.
				if (this._so.FileExists(SAVE_FILE)) {
					this._so.DeleteFile(SAVE_FILE);
				}

				// Create the file, then convert the object into XML data and
				// put it into the stream.
				Stream s = this._so.CreateFile(SAVE_FILE);
				this._savedata.Serialize(s);

				// Close the file and then dispose the container to commit the
				// changes.
				s.Close();
				this._so.Dispose();
			}
			catch {
				// TODO Call logging function.
				return false;
			}
			return true;
		}

		/// <summary>
		/// Writes the local shared object to disk immediately.
		/// </summary>
		/// <returns>
		/// true if the flush was successful; Otherwise, false.
		/// </returns>
		public Boolean ForceSave() {
			return this.ForceSave(0);
		}

		/// <summary>
		/// Bind the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public Boolean Bind(String name) {
			if (String.IsNullOrEmpty(name)) {
				return false;
			}

			this._so = null;
			this._name = name;
			try {
				IAsyncResult result = this._device.BeginOpenContainer(this._name, null, null);
				result.AsyncWaitHandle.WaitOne();
				this._so = this._device.EndOpenContainer(result);

				if (!this._so.FileExists(SAVE_FILE)) {
					this._so.Dispose();
					this._savedata = new SaveData();
					return true;
				}

				Stream s = this._so.OpenFile(SAVE_FILE, FileMode.Open);
				this._savedata.Deserialize(s);

				s.Close();
				this._so.Dispose();
			}
			catch {
				// TODO call logging function.
				this._name = String.Empty;
				if (this._so != null) {
					this._so.Dispose();
				}

				this._savedata = null;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Writes to the data object directly.
		/// </summary>
		/// <param name="fieldName">
		/// The name of the data field you want to create or overwrite.
		/// </param>
		/// <param name="fieldValue">
		/// The data you want to store.
		/// </param>
		/// <param name="minFileSize">
		/// If you need a specific amount of space for your save data, specify
		/// it here. You can also specify 0 to use as much as needed.
		/// </param>
		/// <returns>
		/// Whether or not the save and flush was successful.
        /// </returns>			
		public Boolean Write(String fieldName, String fieldValue, uint minFileSize) {
			if (this._so == null) {
				// TODO call log function.
				return false;
			}
			this._savedata[fieldName] = fieldValue;
			return this.ForceSave(minFileSize);
		}

		/// <summary>
		/// Writes to the data object directly.
		/// </summary>
		/// <param name="fieldName">
		/// The name of the data field you want to create or overwrite.
		/// </param>
		/// <param name="fieldValue">
		/// The data you want to store.
		/// </param>
		/// <returns>
		/// Whether or not the save and flush was successful.
		/// </returns>
		public Boolean Write(String fieldName, String fieldValue) {
			return this.Write(fieldName, fieldValue, 0);
		}

		/// <summary>
		/// Read directly from the underlying data object.
		/// </summary>
		/// <param name="fieldName">
		/// The name of the data field you want to read.
		/// </param>
		/// <returns>
		/// The value of the data field being read, or null if it does not exist.
        /// </returns>			
		public String Read(String fieldName) {
			if (this._so == null) {
				// TODO call log function.
				return null;
			}
			return this._savedata[fieldName];
		}

		/// <summary>
		/// Erases everything in the local shared data object.
		/// </summary>
		/// <param name="minFileSize">
		/// The size of the field data to clear.
		/// </param>
		/// <returns>
		/// Whether or not the clear and flush was successful.
        /// </returns>
		public Boolean Erase(uint minFileSize) {
			if (this._so == null) {
				// TODO call log function.
				return false;
			}
			this._savedata = null;
			Boolean success = this.ForceSave(minFileSize);
			this._savedata = new SaveData();
			return success;
		}

		/// <summary>
		/// Erases everything in the local shared data object.
		/// </summary>
		/// <returns>
		/// Whether or not the clear and flush was successful.
		/// </returns>
		public Boolean Erase() {
			return this.Erase(0);
		}

		/// <summary>
		/// Called when <b>GameStorage</b> needs to be updated.
		/// </summary>
		/// <param name="gameTime">
		/// Time since the last call to <b>Update</b>.
		/// </param>
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (this._wantsDevice) {
				if (!Guide.IsVisible) {
					try {
						this._storageasync = StorageDevice.BeginShowSelector(null, null);
					}
					catch {
						// Silent fail.
						this._storageasync = null;
						this._wantsDevice = false;
					}
				}
			}
			else if (this._storageasync.IsCompleted) {
				try {
					this._device = StorageDevice.EndShowSelector(this._storageasync);
				}
				catch {
					// Silent fail.
				}
				finally {
					this._storageasync = null;
					this._wantsDevice = false;
				}
			}
		}
		#endregion
	}
}
#endif

