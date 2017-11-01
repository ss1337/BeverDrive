//
// Copyright 2014-2017 Sebastian Sjödin
//
// This file is part of BeverDrive.
//
// BeverDrive is free software: you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option)
// any later version.
//
// BeverDrive is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// BeverDrive. If not, see http://www.gnu.org/licenses/.
//
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BeverDrive.Core
{
	/// <summary>
	/// Character codes for various symbols in WebDings
	/// </summary>
	public enum FileType
	{
		Directory = 0xCC,
		DotDot = 0x31,
		Drive = 0xC0,
		Music = 0xB2,
		MyComputer = 0xA5, // TODO: Fix this
		Video = 0xB4,
		Unknown = 0x9D
	}

	public class FileSystemItem
	{
		public System.Drawing.Image CoverImage { get; set; }
		public string DriveName { get; set; }
		public string Name { get; set; }
		public FileType FileType { get; set; }

		public static FileSystemItem MyComputer()
		{
			var result = new FileSystemItem("> My computer");
			result.FileType = FileType.MyComputer;
			return result;
		}

		public FileSystemItem(string name)
			: this(name, string.Empty)
		{
		}

		public FileSystemItem(DriveInfo di)
		{
			string label = "";

			if (di.IsReady && !string.IsNullOrEmpty(di.VolumeLabel))
			{
				label = di.VolumeLabel;
			}
			else
			{
				switch (di.DriveType)
				{
					case DriveType.CDRom:
						label = "CD-ROM";
						break;
					case DriveType.Fixed:
						label = "Local Disk";
						break;
					case DriveType.Network:
						label = "Network Drive";
						break;
					case DriveType.Removable:
						label = "Removable Disk";
						break;
					default:
						label = "Unknown";
						break;
				}
			}

			this.Name = string.Format("{0} ({1})", label, di.Name.TrimEnd( new char[] { '\\' } ));
			this.FileType = FileType.Drive;
			this.DriveName = di.Name;
		}

		public FileSystemItem(string name, string coverImageFile)
		{
			if (!string.IsNullOrEmpty(coverImageFile))
				this.CoverImage = System.Drawing.Image.FromFile(coverImageFile);

			this.Name = name;
			this.FileType = FileType.Unknown;

			if (name.StartsWith(Path.DirectorySeparatorChar.ToString()))
			{
				if (name.Equals(Path.DirectorySeparatorChar))
					this.FileType = FileType.DotDot;
				else
					this.FileType = FileType.Directory;
			}
			else
			{
				int index = this.Name.LastIndexOf(".");
				if (index > 0)
				{
					string extension = this.Name.Substring(index + 1);
					if (BeverDriveContext.Settings.MusicFileTypes.Contains(extension))
						this.FileType = FileType.Music;

					if (BeverDriveContext.Settings.VideoFileTypes.Contains(extension))
						this.FileType = FileType.Video;
				}
			}
		}
	}
}
