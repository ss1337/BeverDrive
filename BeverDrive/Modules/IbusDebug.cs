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
	public class IbusDebug : Module
	{
		public bool Logging;

		private Label title;
		private Label log;
		private TextButton button1;
		private TextButton button2;

		public IbusDebug()
		{
		}

		public override void Back()
		{
			this.Logging = false;
			BeverDriveContext.SetActiveModule("");
		}

		public override void Init()
		{
			this.CreateControls();
			this.Logging = false;
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			base.OnCommand(e);

			if (this.SelectedIndex < -2)
				this.SelectedIndex = -2;

			if (this.SelectedIndex > 0)
				this.SelectedIndex = 0;

			if (this.SelectedIndex == 0)
				this.button2.Selected = true;

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

		private void CreateControls()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			var height = BeverDriveContext.CurrentCoreGui.ModuleContainer.Height;

			this.title = new Label();
			this.title.Font = Fonts.GuiFont36;
			this.title.ForeColor = Colors.ForeColor;
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
			this.button1.Index = -1;
			this.button1.Location = new System.Drawing.Point(16, 90);
			this.button1.Size = new System.Drawing.Size(200, 24);
			this.button1.Text = "Start/stop Logging";
			this.button1.Click += (sender, e) =>
			{
				this.Logging = !this.Logging;
				if (this.Logging)
				{
					this.log.Text = "";
				}
			};
			this.button1.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Start/stop logging"; };

			this.button2 = new TextButton();
			this.button2.Font = Fonts.GuiFont14;
			this.button2.ForeColor = Colors.ForeColor;
			this.button2.Index = 0;
			this.button2.Location = new System.Drawing.Point(232, 90);
			this.button2.Size = new System.Drawing.Size(150, 24);
			this.button2.Text = "RTS on/off";
			this.button2.Click += (sender, e) => { BeverDriveContext.Ibus.RtsEnable = !BeverDriveContext.Ibus.RtsEnable; };
			this.button2.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "RTS on/off"; };

			base.Controls.Add(button1);
			base.Controls.Add(button2);
			base.Controls.Add(log);
			base.Controls.Add(title);
		}
	}
}
