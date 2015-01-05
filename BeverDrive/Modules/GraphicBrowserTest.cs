//
// Copyright 2014 Sebastian Sjödin
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
	public class GraphicBrowserTest : AModule
	{
		private Label ctrl_title;
		private GraphicBrowser br;

		public GraphicBrowserTest()
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

			this.br = new GraphicBrowser("D:\\BeverDrive");
			this.br.Location = new System.Drawing.Point(0, 180);
			this.br.Size = new System.Drawing.Size(width, 380);
		}

		public override void Init()
		{
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch (e.Command)
			{
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					this.Hide();
					break;
				case ModuleCommands.SelectClick:
					this.br.Select();
					break;
				case ModuleCommands.SelectNext:
					this.br.SelectedIndex++;
					break;
				case ModuleCommands.SelectPrevious:
					this.br.SelectedIndex--;
					break;
				default:
					break;
			}

			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		public override void Update1Hz()
		{
			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		private void Show()
		{
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			this.ShowControls();
		}

		private void Hide()
		{
		}
	}
}
