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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Core;
using BeverDrive.Gui.Core.Styles;

namespace BeverDrive.Gui.Modules
{
	[BackButtonVisible(false)]
	public class MainMenu : AModule
	{
		private bool bluetoothEnabled;
		private Label lbl_title;
		private MetroidButton mb1;
		private MetroidButton mb2;
		private MetroidButton mb3;
		private int selectedIndex;

		public MainMenu()
		{
			this.bluetoothEnabled = BeverDriveContext.Settings.EnableBluetooth;
			this.selectedIndex = 0;
			this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch(e.Command)
			{
				case ModuleCommands.SelectClick:
					this.SelectClick();
					break;
				case ModuleCommands.SelectNext:
					this.SelectNext();
					break;
				case ModuleCommands.SelectPrevious:
					this.SelectPrevious();
					break;
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					this.Hide();
					break;
			}
		}

		private void SelectClick()
		{
			switch (selectedIndex)
			{
				case 0:
					mb1.PerformClick();
					break;
				case 1:
					mb2.PerformClick();
					break;
				case 2:
					mb3.PerformClick();
					break;
				default:
					break;
			}
		}

		private void SelectNext()
		{
			int maxIndex = 1;

			if (bluetoothEnabled)
				maxIndex = 2;

			if (selectedIndex == maxIndex)
				selectedIndex = maxIndex;
			else
				selectedIndex++;

			this.Update();
		}

		private void SelectPrevious()
		{
			if (selectedIndex == 0)
				selectedIndex = 0;
			else
				selectedIndex--;

			this.Update();
		}

		private void Show()
		{
			BeverDriveContext.CurrentCoreGui.AddControl(this.lbl_title);
			BeverDriveContext.CurrentCoreGui.AddControl(this.mb1);
			BeverDriveContext.CurrentCoreGui.AddControl(this.mb2);
			
			if (bluetoothEnabled)
				BeverDriveContext.CurrentCoreGui.AddControl(this.mb3);

			this.Update();
		}

		private void Hide()
		{
			BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
		}

		private void Update()
		{
			switch (selectedIndex)
			{
				case 0:
					mb1.Selected = true;
					mb2.Selected = false;
					mb3.Selected = false;
					break;
				case 1:
					mb1.Selected = false;
					mb2.Selected = true;
					mb3.Selected = false;
					break;
				case 2:
					mb1.Selected = false;
					mb2.Selected = false;
					mb3.Selected = true;
					break;
				default:
					break;
			}

			mb1.Invalidate();
			mb2.Invalidate();
			mb3.Invalidate();
		}

		private void CreateControls()
		{
			this.lbl_title = new Label();
			this.lbl_title.AutoSize = false;
			this.lbl_title.BackColor = Color.Transparent;
			this.lbl_title.Font = Fonts.GuiFont28;
			this.lbl_title.ForeColor = Colors.SelectedColor;
			this.lbl_title.Location = new System.Drawing.Point(0, 16);
			this.lbl_title.Name = "Title";
			this.lbl_title.Size = new System.Drawing.Size(BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width, 50);
			this.lbl_title.TabIndex = 0;
			this.lbl_title.Text = "BeverDrive";
			this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.mb1 = new MetroidButton("Resources\\music.png", "Resources\\music_s.png");
			this.mb1.Name = "mm_mb1";
			this.mb1.GridLeft = 1;
			this.mb1.GridTop = 2;
			this.mb1.TabIndex = 0;
			this.mb1.Text = "Music";
			this.mb1.Click += new EventHandler(mb1_Click);

			this.mb2 = new MetroidButton("Resources\\video.png", "Resources\\video_s.png");
			this.mb2.Name = "mm_mb2";
			this.mb2.GridLeft = 7;
			this.mb2.GridTop = 2;
			this.mb2.TabIndex = 0;
			this.mb2.Text = "Video";
			this.mb2.Click += new EventHandler(mb2_Click);

			this.mb3 = new MetroidButton("Resources\\bluetooth.png", "Resources\\bluetooth_s.png");
			this.mb3.Name = "mm_mb3";
			this.mb3.GridLeft = 13;
			this.mb3.GridTop = 2;
			this.mb3.TabIndex = 0;
			this.mb3.Text = "Bluetooth";
			this.mb3.Click += new EventHandler(mb3_Click);
		}

		protected void mb1_Click(object sender, EventArgs e)
		{
			BeverDriveContext.SetActiveModule("Mp3Player");
		}

		protected void mb2_Click(object sender, EventArgs e)
		{
			BeverDriveContext.SetActiveModule("VideoPlayer");
		}

		protected void mb3_Click(object sender, EventArgs e)
		{
			BeverDriveContext.SetActiveModule("Bluetooth");
		}
	}
}
