//
// Copyright 2012-2014 Sebastian Sjödin
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
using System.IO;

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

		public FileSystemItem(string name) : this(name, string.Empty)
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

	public class FileSystemBrowser
	{
		private bool chrootBehavior;
		private DirectoryInfo chrootPath;
		private bool showDirectories;
		private bool showFiles;

		public DirectoryInfo CurrentDirectory { get; private set; }
		public List<DirectoryInfo> Directories { get; private set; }
		public List<FileInfo> Files { get; private set; }
		public List<FileSystemItem> Items { get; private set; }
		public bool ShowDirectories { get { return showDirectories; } set { showDirectories = value; this.ReadDirectory(); } }
		public bool ShowFiles { get { return showFiles; } set { showFiles = value; this.ReadDirectory(); } }

		public FileSystemBrowser(string startPath, bool chrootBehavior)
		{
			this.chrootBehavior = chrootBehavior;
			this.chrootPath = new DirectoryInfo(startPath);
			this.CurrentDirectory = new DirectoryInfo(startPath);
			this.Directories = new List<DirectoryInfo>();
			this.Files = new List<FileInfo>();
			this.Items = new List<FileSystemItem>();
			this.ShowDirectories = true;
			this.ShowFiles = true;
			this.ReadDirectory();
		}

		/// <summary>
		/// Change directory to a subdirectory, \.. is omitted
		/// </summary>
		/// <param name="index"></param>
		public void Cd(int index)
		{
			this.CurrentDirectory = this.Directories[index];
			this.ReadDirectory();
		}

		/// <summary>
		/// Change directory to a subdirectory
		/// </summary>
		/// <param name="name">Name of subdirectory, can be prepended with \ for simplicity</param>
		public void Cd(string name)
		{
			if (!name.StartsWith("\\"))
				name = "\\" + name;

			this.CurrentDirectory = new DirectoryInfo(this.CurrentDirectory.FullName + name);
			this.ReadDirectory();
		}

		/// <summary>
		/// Changes directory to the parent of current directory if possible
		/// </summary>
		public void CdUp()
		{
			if (((this.chrootBehavior) && this.CurrentDirectory.FullName == this.chrootPath.FullName) || this.CurrentDirectory.Parent == null)
				return;

			this.CurrentDirectory = this.CurrentDirectory.Parent;
			this.ReadDirectory();
		}

		/// <summary>
		/// Selects an index from items and behaves accordingly
		/// </summary>
		/// <param name="index"></param>
		public string Select(int index)
		{
			if (index < 0 || index > this.Items.Count - 1)
				return string.Empty;

			var item = this.Items[index];

			// Cd up if we should
			if (item.Name == "\\..")
			{
				this.CdUp();
				return this.CurrentDirectory.Name;
			}

			// Cd to subdirectory if we should
			if (item.Name.StartsWith("\\"))
			{
				this.Cd(item.Name);
				return this.CurrentDirectory.Name;
			}

			// Seems to be a file, return it
			return item.Name;
		}

		private void ReadDirectory()
		{
			this.Directories.Clear();
			this.Files.Clear();
			this.Items.Clear();

			// Decide whether we should show .. or not
			if (!((this.chrootBehavior) && this.CurrentDirectory.FullName == this.chrootPath.FullName) || (this.CurrentDirectory.Parent != null))
				this.Items.Add(new FileSystemItem("\\.."));

			// TODO: We are on top, enumerate drives instead...
			if (this.CurrentDirectory.Parent != null)
			{
			}

			this.CurrentDirectory.GetDirectories().Any(x => { this.Directories.Add(x); return false; });
			this.CurrentDirectory.GetFiles("*.*").Any(x => { this.Files.Add(x); return false; });
			this.Directories.OrderBy(x => x.Name);
			this.Files.OrderBy(x => x.Name);

			if (this.ShowDirectories)
				this.Directories.Any(x => {
					string cover = this.FindCoverImage(this.CurrentDirectory.FullName + "\\" + x.Name);
					this.Items.Add(new FileSystemItem("\\" + x.Name, cover)); return false; 
				});

			if (this.ShowFiles)
				this.Files.Any(x => { this.Items.Add(new FileSystemItem(x.Name)); return false; });

		}

		private string FindCoverImage(string path)
		{
			var di = new System.IO.DirectoryInfo(path);
			FileInfo[] fi;
			
			try { fi = di.GetFiles("*.png"); }
			catch(UnauthorizedAccessException) { return string.Empty; }

			if (fi.Count() > 0)
				return fi[0].FullName;

			fi = di.GetFiles("*.jpg");
			if (fi.Count() > 0)
				return fi[0].FullName;

			return string.Empty;
		}
	}
}
