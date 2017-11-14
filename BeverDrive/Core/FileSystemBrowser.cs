//
// Copyright 2012-2017 Sebastian Sjödin
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
using System.Threading;

namespace BeverDrive.Core
{
	public class FileSystemBrowser
	{
		private DirectoryInfo chrootPath;
		private bool showDirectories;
		private bool showFiles;

		public DirectoryInfo CurrentDirectory { get; private set; }
		public FileSystemItem CurrentItem { get; private set; }
		public List<DirectoryInfo> Directories { get; private set; }
		public List<FileInfo> Files { get; private set; }
		public List<FileSystemItem> Items { get; private set; }
		public bool ShowDirectories { get { return showDirectories; } set { showDirectories = value; this.ReadDirectory(); } }
		public bool ShowFiles { get { return showFiles; } set { showFiles = value; this.ReadDirectory(); } }

		public FileSystemBrowser(string startPath, bool chrootBehavior)
		{
			if (chrootBehavior && startPath != "> My computer")
				this.chrootPath = new DirectoryInfo(startPath);

			this.ShowDirectories = true;
			this.ShowFiles = true;
			this.ReadDirectory(startPath);
		}

		/// <summary>
		/// Returns index of an item, or -1 if the item is not found
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(string item)
		{
			for (int i = 0; i < this.Items.Count; i++)
				if (this.Items[i].Name == item)
					return i;

			return -1;
		}

		/// <summary>
		/// Refreshes the current bleh
		/// </summary>
		public void Refresh()
		{
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

			// Go to My computer if we should
			if (item.FileType == FileType.MyComputer)
			{
				this.ReadMyComputer();
				return item.Name;
			}

			if (item.FileType == FileType.Drive)
			{
				this.ReadDirectory(item.DriveName);
				return item.DriveName;
			}

			// Cd to subdirectory if we should
			if (item.Name.StartsWith(Path.DirectorySeparatorChar.ToString()))
			{

				this.ReadDirectory(this.CurrentDirectory + item.Name);
				//this.Cd(item.Name);
				return this.CurrentDirectory.Name;
			}

			// Seems to be a file, return it
			return item.Name;
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

		private void ReadDirectory()
		{
			if (this.CurrentDirectory != null)
				this.ReadDirectory(this.CurrentDirectory.FullName);
		}

		private void ReadDirectory(string path)
		{
			List<FileSystemItem> result = new List<FileSystemItem>();
			this.Directories = new List<DirectoryInfo>();
			this.Files = new List<FileInfo>();

			if (path == "> My computer")
			{
				DriveInfo.GetDrives().Any(x => { result.Add(new FileSystemItem(x)); return false; });
				this.CurrentItem = FileSystemItem.MyComputer();
			}
			else
			{
				DirectoryInfo pathDi = new DirectoryInfo(path);

				// Decide whether we should show .. or My computer or not
				if ((this.chrootPath == null) && (pathDi.Parent == null))
				{
					result.Add(FileSystemItem.MyComputer());
				}
				else
				{
					if ((this.chrootPath == null) || (pathDi.FullName != this.chrootPath.FullName))
						result.Add(new FileSystemItem(Path.DirectorySeparatorChar + "..", pathDi.FullName));
				}

				// This try/catch is for reading drives that may not be ready
				try
				{
					pathDi.GetDirectories().Any(x => { this.Directories.Add(x); return false; });
					pathDi.GetFiles("*.*").Any(x => { this.Files.Add(x); return false; });
					this.Directories.OrderBy(x => x.Name);
					this.Files.OrderBy(x => x.Name);
				}
				catch (Exception)
				{
				}

				if (this.ShowDirectories)
					this.Directories.Any(x => { result.Add(new FileSystemItem(Path.DirectorySeparatorChar + x.Name, x.FullName)); return false; });

				if (this.ShowFiles)
					this.Files.Any(x => { result.Add(new FileSystemItem(x.Name, x.FullName)); return false; });

				this.CurrentDirectory = pathDi;
				this.CurrentItem = new FileSystemItem(pathDi.Name, pathDi.FullName);
			}

			this.Items = result;
		}

		private void ReadMyComputer()
		{
			this.ReadDirectory("> My computer");
		}
	}
}
