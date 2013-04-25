//
//  SaveData.cs
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
using System.IO;
using System.Xml.Serialization;

namespace CyrusBuilt.CyFlixel.CyFlixelEngine.Storage
{
#if !WINDOWS_PHONE
	/// <summary>
	/// A container for storing savegame data.
	/// </summary>
	public sealed class SaveData
	{
		private Dictionary<String, String> _data = null;

		#region Constructors.
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.SaveData"/>
		/// class. This is the default constructor.
		/// </summary>
		public SaveData() {
			this._data = new Dictionary<String, String>();
		}
		#endregion

		#region Indexers
		/// <summary>
		/// Gets or sets the <see cref="CyrusBuilt.CyFlixel.CyFlixelEngine.SaveData"/>
		/// with the specified key.
		/// </summary>
		/// <param name="key">
		/// The key name.
		/// </param>
		public String this[String key] {
			get {
				if (this._data.ContainsKey(key)) {
					return this._data[key];
				}
				return null;
			}
			set {
				if (this._data.ContainsKey(key)) {
					this._data[key] = value;
				}
				else {
					this._data.Add(key, value);
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Serialize the data entries.
		/// </summary>
		/// <param name="writer">
		/// The stream used to write the serialized data with.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="writer"/> cannot be null.
		/// </exception>
		public void Serialize(Stream writer) {
			if (writer == null) {
				throw new ArgumentNullException("writer");
			}

			List<SaveDataEntry> entries = new List<SaveDataEntry>(this._data.Count);
			foreach (String key in this._data.Keys) {
				entries.Add(new SaveDataEntry(key, this._data[key]));
			}

			XmlSerializer serializer = new XmlSerializer(typeof(List<SaveDataEntry>));
			serializer.Serialize(writer, entries);
		}

		/// <summary>
		/// Deserialize the data entries.
		/// </summary>
		/// <param name="reader">
		/// The stream that contains the data to deserialize.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="reader"/> cannot be null.
		/// </exception>
		public void Deserialize(Stream reader) {
			if (reader == null) {
				throw new ArgumentNullException("reader");
			}

			this._data.Clear();
			XmlSerializer serializer = new XmlSerializer(typeof(List<SaveDataEntry>));
			List<SaveDataEntry> list = (List<SaveDataEntry>)serializer.Deserialize(reader);
			foreach (SaveDataEntry entry in list) {
				this._data[entry.Key] = entry.Value;
			}
		}
		#endregion
	}
#endif
}

