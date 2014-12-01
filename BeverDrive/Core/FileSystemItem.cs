//
// Copyright 2014 Sebastian Sjödin
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
		Video = 0xB4,
		Unknown = 0x9D
	}

	public class FileSystemItem
	{
		public System.Drawing.Image CoverImage { get; set; }
		public string Name { get; set; }
		public FileType FileType { get; set; }

		public FileSystemItem(string name)
			: this(name, string.Empty)
		{
		}

		public FileSystemItem(string name, string coverImageFile)
		{
			if (!string.IsNullOrEmpty(coverImageFile))
				this.CoverImage = System.Drawing.Image.FromFile(coverImageFile);

			this.Name = name;
			this.FileType = FileType.Unknown;

			if (name.StartsWith("\\"))
			{
				if (name.Equals("\\.."))
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
