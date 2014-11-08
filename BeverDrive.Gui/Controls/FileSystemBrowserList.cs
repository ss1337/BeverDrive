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
using BeverDrive.Gui.Core;

namespace BeverDrive.Gui.Controls
{
	public class FileSystemBrowserList : MenuOptionList
	{
		private FileSystemBrowser browser;

		public string CurrentPath { get { return this.browser.CurrentDirectory.FullName; } }
		public List<DirectoryInfo> Directories { get { return this.browser.Directories; } }
		public List<FileInfo> Files { get { return this.browser.Files; } }
		public bool ShowDirectories { get { return this.browser.ShowDirectories; } set { this.browser.ShowDirectories = value; } }
		public bool ShowFiles { get { return this.browser.ShowFiles; } set { this.browser.ShowFiles = value; } }

		public FileSystemBrowserList() : this("C:\\") { }

		public FileSystemBrowserList(string rootPath) : this(rootPath, false) { }

		public FileSystemBrowserList(string rootPath, bool chrootBehavior)
		{
			this.browser = new FileSystemBrowser(rootPath, chrootBehavior);
			this.PopulateBrowser();
		}

		public new void Select()
		{
			if (this.SelectedIndex > -1 && this.SelectedIndex < this.Items.Count)
			{
				var item = this.browser.Items[this.SelectedIndex];

				if (item.StartsWith("\\"))
				{
					var childName = "\\" + browser.CurrentDirectory.Name;
					this.browser.Select(this.SelectedIndex);
					this.PopulateBrowser();
					this.Invalidate();

					if (item == "\\..") {
						int newIndex = this.Items.IndexOf(childName);
						this.ScrollToCenter(newIndex);
						this.SelectedIndex = newIndex;
					}
				}
			}
		}

		private void PopulateBrowser()
		{
			this.SelectedIndex = 0;
			this.Items.Clear();
			this.browser.Items.Any(x => { this.Items.Add(x); return false; });
		}
	}
}
