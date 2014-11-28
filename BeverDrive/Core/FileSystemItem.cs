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
