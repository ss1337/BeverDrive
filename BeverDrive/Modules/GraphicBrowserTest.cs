//
// Copyright 2014-2017 Sebastian Sjödin
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
using BeverDrive.Gui.Styles;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	[MenuText("Graphic test")]
	public class GraphicBrowserTest : Module
	{
		private Label ctrl_title;
		private GraphicBrowser browser;

		public GraphicBrowserTest()
		{
		}

		public override void Back()
		{
			BeverDriveContext.SetActiveModule("");
		}

		public override void Init()
		{
			this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Graphic browser test";
			base.OnCommand(e);

			if (this.SelectedIndex == this.browser.Items.Count)
				this.SelectedIndex--;

			this.browser.SelectedIndex = this.SelectedIndex;

			if (e.Command == ModuleCommands.SelectClick)
			{
				this.browser.Select();
				this.SelectedIndex = this.browser.SelectedIndex;
			}
		}

		public override void Update1Hz()
		{
			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		private void CreateControls()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			var height = BeverDriveContext.CurrentCoreGui.ModuleContainer.Height;

			this.ctrl_title = new Label();
			this.ctrl_title.Font = Fonts.GuiFont36;
			this.ctrl_title.ForeColor = Colors.SelectedColor;
			this.ctrl_title.Location = new System.Drawing.Point(0, 16);
			this.ctrl_title.Size = new System.Drawing.Size(width, 50);
			this.ctrl_title.Text = "GraphicBrowser Test";
			this.ctrl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.browser = new GraphicBrowser("D:\\BeverDrive");
			this.browser.Index = 0;
			this.browser.Location = new System.Drawing.Point(0, 180);
			this.browser.Size = new System.Drawing.Size(width, 380);

			base.Controls.Add(this.browser);
			base.Controls.Add(this.ctrl_title);
		}
	}
}
