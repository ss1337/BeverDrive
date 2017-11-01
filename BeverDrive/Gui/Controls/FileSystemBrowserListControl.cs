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
using System.IO;
using System.Linq;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Gui.Controls
{
	public class FileSystemBrowserListControl : ListControl
	{
		private FileSystemBrowser browser;

		public FileSystemItem CurrentItem { get { return this.browser.CurrentItem; } }
		public string CurrentPath { get { return this.browser.CurrentDirectory.FullName; } }
		public List<DirectoryInfo> Directories { get { return this.browser.Directories; } }
		public List<FileInfo> Files { get { return this.browser.Files; } }
		public bool ShowDirectories { get { return this.browser.ShowDirectories; } set { this.browser.ShowDirectories = value; } }
		public bool ShowFiles { get { return this.browser.ShowFiles; } set { this.browser.ShowFiles = value; } }

		public FileSystemBrowserListControl() : this("C:\\") { }

		public FileSystemBrowserListControl(string rootPath) : this(rootPath, false) { }

		public FileSystemBrowserListControl(string rootPath, bool chrootBehavior)
		{
			this.browser = new FileSystemBrowser(rootPath, chrootBehavior);
			this.Font = Fonts.GuiFont26;
			this.PopulateBrowser();
		}

		public new void Select()
		{
			if (this.SelectedIndex > -1 && this.SelectedIndex < this.Items.Count)
			{
				var item = this.browser.Items[this.SelectedIndex];

				if (!this.SelectedItemIsFile())
				{
					var childName = Path.DirectorySeparatorChar + browser.CurrentDirectory.Name;
					this.browser.Select(this.SelectedIndex);
					this.PopulateBrowser();
					this.Invalidate();

					if (item.Name == Path.DirectorySeparatorChar + "..") {
						int newIndex = this.Items.IndexOf(childName);
						this.ScrollToCenter(newIndex);
						this.SelectedIndex = newIndex;
					}
				}
			}
		}

		public bool SelectedItemIsFile()
		{
			if (this.SelectedItem.StartsWith(Path.DirectorySeparatorChar.ToString()))
				return false;

			if (this.browser.Items[this.SelectedIndex].FileType == FileType.MyComputer)
				return false;

			if (this.browser.Items[this.SelectedIndex].FileType == FileType.Drive)
				return false;

			return true;
		}

		private void PopulateBrowser()
		{
			this.SelectedIndex = 0;
			this.Items.Clear();
			this.browser.Items.Any(x => { this.Items.Add(x.Name); return false; });
		}
	}
}
