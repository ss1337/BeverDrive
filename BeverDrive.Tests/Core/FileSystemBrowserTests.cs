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
using NUnit.Framework;

namespace BeverDrive.Tests.Core
{
	[TestFixture]
	public class FileSystemBrowserTests
	{
		public FileSystemBrowserTests()
		{
		}

		[Test]
		public void Browser_stops_when_chroot()
		{
			var browser = new FileSystemBrowser("C:\\", false);
			browser.CdUp();
			Assert.AreEqual("C:\\", browser.CurrentDirectory.Name);
		}

		[Test]
		public void Browser_cdups()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			browser.CdUp();
			Assert.AreEqual("C:\\", browser.CurrentDirectory.Name);
		}

		[Test]
		public void Browser_cds_with_index()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			Assert.AreEqual("Windows", browser.CurrentDirectory.Name);
			browser.Cd(1);
			Assert.AreNotEqual("Windows", browser.CurrentDirectory.Name);
		}

		[Test]
		public void Browser_cds_with_name()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			Assert.AreEqual("Windows", browser.CurrentDirectory.Name);
			browser.Cd("system32");
			Assert.AreEqual("system32", browser.CurrentDirectory.Name);
		}

		[Test]
		public void Browser_doesnt_show_directories()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			browser.ShowDirectories = false;
			Assert.AreEqual(1, browser.Items.Where(x => x.StartsWith("\\")).Count());
			Assert.Greater(browser.Items.Where(x => !x.StartsWith("\\")).Count(), 1);
		}

		[Test]
		public void Select_doesnt_break_on_invalid_indices()
		{
			var browser = new FileSystemBrowser("C:\\Windows", false);
			browser.Select(-1);
			browser.Select(browser.Items.Count);
			browser.Select(browser.Items.Count + 10);
		}
	}
}
