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
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using NUnit.Framework;

namespace BeverDrive.Tests.Controls
{
	[TestFixture]
	public class FileSystemBrowserListTests
	{
		private FileSystemListControl control;

		public FileSystemBrowserListTests()
		{
			BeverDriveContext.Initialize();
			this.control = new FileSystemListControl();
			this.control.HeightInItems = 1;
		}

		[Test]
		public void Control_populates()
		{
			Assert.True(this.control.Items.Count > 0);
		}

		[Test]
		public void Select_doesnt_break_on_invalid_indices()
		{
			this.control.SelectedIndex = -2;
			this.control.Select();

			this.control.SelectedIndex = this.control.Items.Count;
			this.control.Select();
		}

		[Test]
		public void Select_in_my_computer_works()
		{
			var ctrl = new FileSystemListControl("> My computer");
			ctrl.SelectedIndex = 1;
			ctrl.Select();
			Assert.True(this.control.Items.Count > 1);
		}
	}
}
