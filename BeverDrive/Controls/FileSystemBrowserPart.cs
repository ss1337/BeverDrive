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
using System.IO;
using System.Linq;
using System.Text;
using BeverDrive.Core;

namespace BeverDrive.Controls
{
	public class FileSystemBrowserPart : ListControlPart
	{
		public string CurrentPath { get { return this.browser.CurrentDirectory.FullName; } }
		public List<DirectoryInfo> Directories { get { return this.browser.Directories; } }
		public List<FileInfo> Files { get { return this.browser.Files; } }
		public bool ShowDirectories { get { return this.browser.ShowDirectories; } set { this.browser.ShowDirectories = value; } }
		public bool ShowFiles { get { return this.browser.ShowFiles; } set { this.browser.ShowFiles = value; } }

		private FileSystemBrowser browser;

		public FileSystemBrowserPart() : this("C:\\") { }

		public FileSystemBrowserPart(string rootPath) : this(rootPath, false) { }

		public FileSystemBrowserPart(string rootPath, bool chrootBehavior)
		{
			this.browser = new FileSystemBrowser(rootPath, chrootBehavior);
			this.PopulateBrowser();
		}

		private void PopulateBrowser()
		{
			this.Items.Clear();

			//if (this.browser.

			this.Items.Add("\\..");
			this.SelectedIndex = 0;

			foreach (var f in this.browser.Directories)
				this.Items.Add("\\" + f.Name);

			foreach (var f in this.browser.Files)
			{
				this.Items.Add(f.Name);
			}
		}
	}
}
