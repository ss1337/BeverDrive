﻿//
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
using System.Drawing;
using System.Linq;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	public partial class Bluetooth : AModule
	{
		private Label lbl_title;
		private Label lbl_bt1;
		private Label lbl_bt2;
		private bool isActive;
		private int selectedIndex;

		public Bluetooth()
		{
			this.selectedIndex = 0;
			this.CreateControls();

			try
			{
				this.btClient = new InTheHand.Net.Sockets.BluetoothClient();
			}
			catch (Exception ex)
			{
				lbl_bt1.Text = ex.Message;
				lbl_bt2.Text = "";
			}
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch (e.Command)
			{
				case ModuleCommands.SelectClick:
					this.SelectClick();
					break;
				case ModuleCommands.SelectLeft:
					this.SelectNext();
					break;
				case ModuleCommands.SelectRight:
					this.SelectPrevious();
					break;
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					this.Hide();
					break;
			}

			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		private void SelectClick()
		{
			switch (selectedIndex)
			{
				case -1:
					BeverDriveContext.SetActiveModule("MainMenu");
					break;
				default:
					break;
			}
		}

		private void SelectNext()
		{
			if (selectedIndex == 0)
				selectedIndex = 0;
			else
			{
				selectedIndex++;

				if (selectedIndex == 0)
					BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			}

			this.Update();
		}

		private void SelectPrevious()
		{
			if (selectedIndex == -1)
				selectedIndex = -1;
			else
			{
				selectedIndex--;

				if (selectedIndex == -1)
					BeverDriveContext.CurrentCoreGui.BackButton.Selected = true;
			}

			this.Update();
		}

		private void Show()
		{
			VlcContext.AudioPlayer.Stop();
			VlcContext.VideoPlayer.Stop();
			isActive = true;
			selectedIndex = 0;
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			this.ShowControls();
			this.Update();
		}

		private void Hide()
		{
			BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
			isActive = false;
		}

		private void Update()
		{
		}

		private void CreateControls()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			int labelWidth = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2;

			this.lbl_title = new Label();
			this.lbl_title.Font = Fonts.GuiFont36;
			this.lbl_title.ForeColor = Colors.SelectedColor;
			this.lbl_title.Location = new System.Drawing.Point(BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 150, 16);
			this.lbl_title.Size = new System.Drawing.Size(300, 50);
			this.lbl_title.Text = "Bluetooth";
			this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.lbl_bt1 = new Label();
			this.lbl_bt1.Font = Fonts.GuiFont18;
			this.lbl_bt1.ForeColor = Colors.ForeColor;
			this.lbl_bt1.Location = new System.Drawing.Point(16, 100);
			this.lbl_bt1.Size = new System.Drawing.Size(500, 36);
			this.lbl_bt1.Text = "Connected device: ";
			this.lbl_bt1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

			this.lbl_bt2 = new Label();
			this.lbl_bt2.Font = Fonts.GuiFont18;
			this.lbl_bt2.ForeColor = Colors.ForeColor;
			this.lbl_bt2.Location = new System.Drawing.Point(BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 150, 336);
			this.lbl_bt2.Size = new System.Drawing.Size(300, 36);
			this.lbl_bt2.Text = "Connect device now";
			this.lbl_bt2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		}
	}
}
