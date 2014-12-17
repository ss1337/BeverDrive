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
	public partial class NubblesModule
	{
		private NubblesControl ctrlGame;
		private Label ctrlTitle;
		private OverlayedMenu ctrlMenu;
		private System.Windows.Forms.Timer gameTimer;

		private void InitControls()
		{
			ctrlGame = new NubblesControl();
			ctrlGame.Location = new System.Drawing.Point(0, 0);
			ctrlGame.Size = BeverDriveContext.CurrentCoreGui.ModuleAreaSize;
			ctrlGame.Initialize();

			ctrlMenu = new OverlayedMenu();
			ctrlMenu.Font = Fonts.GuiFont18;
			ctrlMenu.ForeColor = Colors.ForeColor;
			ctrlMenu.SelectedColor = Colors.SelectedColor;
			ctrlMenu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			ctrlMenu.Location = new System.Drawing.Point(0, 200);
			ctrlMenu.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			ctrlMenu.Height = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - 200;

			ctrlTitle = new Label();
			ctrlTitle.Font = Fonts.GuiFont36;
			ctrlTitle.ForeColor = Colors.SelectedColor;
			ctrlTitle.Location = new System.Drawing.Point(BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 200, 48);
			ctrlTitle.Size = new System.Drawing.Size(400, 50);
			ctrlTitle.Text = "Nübbles";
			ctrlTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			gameTimer = new System.Windows.Forms.Timer();
			gameTimer.Interval = 40;
			gameTimer.Tick += new EventHandler(gameTimer_Tick);
		}
	}
}
