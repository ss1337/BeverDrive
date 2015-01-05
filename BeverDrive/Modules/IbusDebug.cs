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
	public class IbusDebug : AModule
	{
		public bool Logging;
		public bool Rts;

		public Label title;
		public Label log;
		public TextButton button1;
		public TextButton button2;

		public int SelectedIndex { get; set; }

		public IbusDebug()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			var height = BeverDriveContext.CurrentCoreGui.ModuleContainer.Height;

			this.title = new Label();
			this.title.Font = Fonts.GuiFont36;
			this.title.ForeColor = Colors.SelectedColor;
			this.title.Location = new System.Drawing.Point(0, 16);
			this.title.Size = new System.Drawing.Size(width, 50);
			this.title.Text = "Ibus debugging";
			this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.log = new Label();
			this.log.Font = Fonts.GuiFont14;
			this.log.ForeColor = Colors.ForeColor;
			this.log.Location = new System.Drawing.Point(0, 130);
			this.log.Size = new System.Drawing.Size(width, height - 130);
			this.log.Text = "";
			this.log.TextAlign = System.Drawing.ContentAlignment.TopLeft;

			this.button1 = new TextButton();
			this.button1.Font = Fonts.GuiFont14;
			this.button1.ForeColor = Colors.ForeColor;
			this.button1.Location = new System.Drawing.Point(16, 90);
			this.button1.Size = new System.Drawing.Size(200, 24);
			this.button1.Text = "Start/stop Logging";

			this.button2 = new TextButton();
			this.button2.Font = Fonts.GuiFont14;
			this.button2.ForeColor = Colors.ForeColor;
			this.button2.Location = new System.Drawing.Point(232, 90);
			this.button2.Size = new System.Drawing.Size(150, 24);
			this.button2.Text = "RTS on/off";
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
					this.SelectClick();
					break;
				case ModuleCommands.SelectNext:
					if (this.SelectedIndex < 1)
						this.SelectedIndex++;
					break;
				case ModuleCommands.SelectPrevious:
					if (this.SelectedIndex > -1)
						this.SelectedIndex--;
					break;
				default:
					break;
			}

			switch (this.SelectedIndex)
			{
				case -1:
					BeverDriveContext.CurrentCoreGui.BackButton.Selected = true;
					this.button1.Selected = false;
					break;

				case 0:
					BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
					this.button1.Selected = true;
					this.button2.Selected = false;
					break;
				case 1:
					this.button1.Selected = false;
					this.button2.Selected = true;
					break;
			}

			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		public override void ProcessMessage(string message)
		{
			if (this.Logging)
			{
				this.log.Text += message + Environment.NewLine;
			}
		}

		public override void Update1Hz()
		{
			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		private void SelectClick()
		{
			switch (this.SelectedIndex)
			{
				case -1:
					BeverDriveContext.SetActiveModule("MainMenu");
					break;

				case 0:
					this.Logging = !this.Logging;

					// Clear log on start of Logging
					if (this.Logging)
						this.log.Text = "";

					break;
				case 1:
					BeverDriveContext.Ibus.RtsEnable = !BeverDriveContext.Ibus.RtsEnable;
					break;
				default:
					break;
			}
		}

		private void Show()
		{
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			this.button1.Selected = true;
			this.Logging = false;
			this.Rts = false;
			this.SelectedIndex = 0;
			this.ShowControls();
		}

		private void Hide()
		{
			BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
			this.Logging = false;
		}
	}
}
