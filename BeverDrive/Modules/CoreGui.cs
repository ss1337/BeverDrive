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
using System.Text;
using System.Windows.Forms;
using BeverDrive.Controls;
using BeverDrive.Core;
using BeverDrive.Core.Styles;

namespace BeverDrive.Modules
{
	public class CoreGui : AModule
	{
		public Panel BaseContainer;								// Adjust this to fit everything to screen
		public Label BaseClock;									// Shows current time
		public Label BaseDate;									// Shows current date
		public Label BaseText;									// Shows text in the lower white portion
		public MetroidButton BackButton;						// Ambigous back button
		public Panel ClockContainer;
		public Panel ModuleContainer;							// All module controls goes into here

		public Size ModuleAreaSize { get { return this.ModuleContainer.Size; } }

		public CoreGui()
		{
			this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch(e.Command)
			{
				case ModuleCommands.Hide:
					this.BaseContainer.Controls.Clear();
					break;
				case ModuleCommands.Show:
					BeverDriveContext.CurrentMainForm.Controls.Add(this.BaseContainer);
					this.ClockContainer.Controls.Add(this.BaseClock);
					this.ClockContainer.Controls.Add(this.BaseDate);
					this.ClockContainer.Controls.Add(this.BaseText);
					this.BaseContainer.Controls.Add(this.ClockContainer);
					this.BaseContainer.Controls.Add(this.ModuleContainer);
					break;
				case ModuleCommands.NextTrack:
				case ModuleCommands.PreviousTrack:
					if (BeverDriveContext.PlaybackModule != null)
						BeverDriveContext.PlaybackModule.OnCommand(e);
					break;
			}
		}

		/// <summary>
		/// Adds ctrl to module container
		/// </summary>
		/// <param name="ctrl"></param>
		public void AddControl(Control ctrl)
		{
			this.ModuleContainer.Controls.Add(ctrl);
		}

		/// <summary>
		/// Clears base container of all controls. Useful for full screen modules
		/// </summary>
		public void ClearBaseContainer()
		{
			this.BaseContainer.Controls.Clear();
		}

		/// <summary>
		/// Clears the module container of all controls, do this when showing a module
		/// </summary>
		public void ClearModuleContainer()
		{
			this.ModuleContainer.Controls.Clear();
		}

		private void CreateControls()
		{
			int width = 800 - BeverDriveContext.Settings.OffsetLeft - BeverDriveContext.Settings.OffsetRight;
			int height = 600 - BeverDriveContext.Settings.OffsetTop - BeverDriveContext.Settings.OffsetBottom;

			// Adjust this to fit to screen
			this.BaseContainer = new Panel();
			this.BaseContainer.BackColor = Colors.BackColor;
			//this.BaseContainer.BackgroundImage = System.Drawing.Image.FromFile("Resources\\background.png");
			this.BaseContainer.Location = new System.Drawing.Point(BeverDriveContext.Settings.OffsetLeft, BeverDriveContext.Settings.OffsetTop);
			this.BaseContainer.Name = "BaseContainer";
			this.BaseContainer.Size = new System.Drawing.Size(width, height);
			this.BaseContainer.TabIndex = 0;

			this.ClockContainer = new Panel();
			this.ClockContainer.BackColor = Colors.ClockBackgroundColor;
			this.ClockContainer.Location = new System.Drawing.Point(0, this.BaseContainer.Size.Height - 48);
			this.ClockContainer.Name = "ClockContainer";
			this.ClockContainer.Size = new System.Drawing.Size(this.BaseContainer.Size.Width, 48);
			this.ClockContainer.TabIndex = 0;

			int clkBottomY = this.ClockContainer.Size.Height - 40;
			int clkHeight = 37;

			this.BaseClock = new System.Windows.Forms.Label();
			this.BaseClock.AutoSize = false;
			this.BaseClock.BackColor = System.Drawing.Color.Transparent;
			this.BaseClock.Font = Fonts.GuiFont24;
			this.BaseClock.ForeColor = Colors.ClockForegroundColor;
			this.BaseClock.Location = new System.Drawing.Point(this.ClockContainer.Size.Width - 106, clkBottomY);
			this.BaseClock.Name = "BaseClock";
			this.BaseClock.Size = new System.Drawing.Size(106, clkHeight);
			this.BaseClock.TabIndex = 0;
			this.BaseClock.Text = "";
			this.BaseClock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

			this.BaseDate = new System.Windows.Forms.Label();
			this.BaseDate.AutoSize = false;
			this.BaseDate.BackColor = System.Drawing.Color.Transparent;
			this.BaseDate.Font = Fonts.GuiFont24;
			this.BaseDate.ForeColor = Colors.ClockForegroundColor;
			this.BaseDate.Location = new System.Drawing.Point(5, clkBottomY);
			this.BaseDate.Name = "BaseDate";
			this.BaseDate.Size = new System.Drawing.Size(184, clkHeight);
			this.BaseDate.TabIndex = 0;
			this.BaseDate.Text = "";
			this.BaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

			int baseTextWidth = this.ClockContainer.Size.Width - this.BaseClock.Size.Width - this.BaseDate.Size.Width;
			int baseTextX = this.BaseDate.Size.Width;

			this.BaseText = new System.Windows.Forms.Label();
			this.BaseText.AutoSize = false;
			this.BaseText.BackColor = System.Drawing.Color.Transparent;
			this.BaseText.Font = Fonts.GuiFont24;
			this.BaseText.ForeColor = Colors.SelectedColor;
			this.BaseText.Location = new System.Drawing.Point(baseTextX, clkBottomY);
			this.BaseText.Name = "BaseText";
			this.BaseText.Size = new System.Drawing.Size(baseTextWidth, clkHeight);
			this.BaseText.TabIndex = 0;
			this.BaseText.Text = "";
			this.BaseText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.BackButton = new MetroidButton("Resources\\back.png", "Resources\\back_s.png");
			this.BackButton.Name = "BackButton";
			this.BackButton.Location = new System.Drawing.Point(5, 5);
			this.BackButton.TabIndex = 0;
			this.BackButton.Visible = false;

			// This panel is were everything ends up
			this.ModuleContainer = new Panel();
			this.ModuleContainer.BackColor = Colors.BackColor;
			this.ModuleContainer.Location = new System.Drawing.Point(0, 0);
			this.ModuleContainer.Name = "ModuleContainer";
			this.ModuleContainer.Size = new System.Drawing.Size(this.BaseContainer.Size.Width, this.BaseContainer.Size.Height - 40);
			this.ModuleContainer.TabIndex = 0;

#if DEBUG
			//this.BaseContainer.BackColor = System.Drawing.Color.Chartreuse;
			//this.ModuleContainer.BackColor = System.Drawing.Color.PaleVioletRed;
#endif
		}

		public override void Update1Hz()
		{
			BeverDriveContext.CurrentCoreGui.BaseClock.Text = DateTime.Now.ToShortTimeString();
			BeverDriveContext.CurrentCoreGui.BaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
		}
	}
}
