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
using NUnit.Framework;

namespace BeverDrive.Tests.Core
{
	[TestFixture]
	public class FileSystemBrowserTests
	{
		public FileSystemBrowserTests()
		{
			BeverDriveContext.Initialize();
			VlcContext.Initialize(BeverDriveContext.Settings.VlcPath);
		}

		[Test]
		public void Browser_doesnt_show_directories()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			browser.ShowDirectories = false;
			Assert.AreEqual(1, browser.Items.Where(x => x.Name.StartsWith("\\")).Count());
			Assert.Greater(browser.Items.Where(x => !x.Name.StartsWith("\\")).Count(), 1);
		}

		[Test]
		public void Chroot_works()
		{
			var browser = new FileSystemBrowser("C:\\Windows", true);
			Assert.AreNotEqual("\\..", browser.Items[0].Name);
			browser.Select(2);
			Assert.AreEqual("\\..", browser.Items[0].Name);
			browser.Select(0);
			Assert.AreNotEqual("\\..", browser.Items[0].Name);
		}

		[Test]
		public void My_computer_doesnt_also_show_dotdot()
		{
			var browser = new FileSystemBrowser("C:\\", false);
			Assert.AreEqual("> My computer", browser.Items[0].Name);
			Assert.AreNotEqual("\\..", browser.Items[1].Name);
		}

		[Test]
		public void Select_doesnt_break_on_invalid_indices()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			browser.Select(-1);
			browser.Select(-100);
			browser.Select(browser.Items.Count);
			browser.Select(browser.Items.Count + 10);
		}

		[Test]
		public void Starting_with_my_computer_shows_drives()
		{
			var browser = new FileSystemBrowser("> My computer", false);
			Assert.AreEqual("Local Disk (C:)", browser.Items[0].Name);
			Assert.AreEqual("Storage (D:)", browser.Items[1].Name);
		}
	}
}
