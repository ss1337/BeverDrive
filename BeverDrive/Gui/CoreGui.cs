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
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;
using BeverDrive.Modules;

namespace BeverDrive.Gui
{
	public class CoreGui : ICoreGui
	{
		public Panel BaseContainer { get; set; }								// Adjust this to fit everything to screen
		public ClockPanel ClockContainer { get; set; }						// Contains lower portion with date/time and maybe text
		public GraphicsPanel ModuleContainer { get; set; }					// All module controls goes into here

		public BackButton BackButton { get; set; }

		public Size BaseAreaSize { get { return this.BaseContainer.Size; } }

		public Size ModuleAreaSize { get { return this.ModuleContainer.Size; } }

		public CoreGui()
		{
			this.CreateControls();
		}

		public void OnCommand(ModuleCommandEventArgs e)
		{
			switch(e.Command)
			{
				case ModuleCommands.Hide:
					this.BaseContainer.Controls.Clear();
					break;
				case ModuleCommands.Show:
					BeverDriveContext.CurrentMainForm.Controls.Add(this.BaseContainer);
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
		public void AddControl(AGraphicsControl ctrl)
		{
			this.ModuleContainer.GraphicControls.Add((AGraphicsControl)ctrl);
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
			this.ModuleContainer.GraphicControls.Clear();
		}

		/// <summary>
		/// Trigger redraw
		/// </summary>
		public void Invalidate()
		{
			this.ClockContainer.Invalidate();
			this.ModuleContainer.Invalidate();
		}

		private void CreateControls()
		{
			int width = 800 - BeverDriveContext.Settings.OffsetLeft - BeverDriveContext.Settings.OffsetRight;
			int height = 600 - BeverDriveContext.Settings.OffsetTop - BeverDriveContext.Settings.OffsetBottom;

			this.BackButton = new BackButton();
			this.BackButton.Location = new System.Drawing.Point(0, 5);

			// Adjust this to fit to screen
			this.BaseContainer = new Panel();
			this.BaseContainer.BackColor = Colors.BackColor;
			this.BaseContainer.Location = new System.Drawing.Point(BeverDriveContext.Settings.OffsetLeft, BeverDriveContext.Settings.OffsetTop);
			this.BaseContainer.Name = "BaseContainer";
			this.BaseContainer.Size = new System.Drawing.Size(width, height);
			this.BaseContainer.TabIndex = 0;

			this.ClockContainer = new ClockPanel();
			this.ClockContainer.BackColor = Colors.ClockBackgroundColor;
			this.ClockContainer.ForeColor = Colors.ClockForegroundColor;
			this.ClockContainer.TextColor = Colors.SelectedColor;
			this.ClockContainer.Font = Fonts.GuiFont24;
			this.ClockContainer.Location = new System.Drawing.Point(0, this.BaseContainer.Size.Height - 40);
			this.ClockContainer.Size = new System.Drawing.Size(this.BaseContainer.Size.Width, 40);
			this.ClockContainer.Name = "ClockContainer";
			this.ClockContainer.TabIndex = 0;

			// This panel is were everything ends up
			this.ModuleContainer = new GraphicsPanel();
			this.ModuleContainer.BackColor = Colors.BackColor;
			this.ModuleContainer.Location = new System.Drawing.Point(0, 0);
			this.ModuleContainer.Name = "ModuleContainer";
			this.ModuleContainer.Size = new System.Drawing.Size(this.BaseContainer.Size.Width, this.BaseContainer.Size.Height - 40);
			this.ModuleContainer.TabIndex = 0;

#if DEBUG
			//this.BaseContainer.BackColor = System.Drawing.Color.Green;
			//this.ModuleContainer.BackColor = System.Drawing.Color.PaleVioletRed;
#endif
		}

		public void Update1Hz()
		{
			BeverDriveContext.CurrentCoreGui.ClockContainer.Time = DateTime.Now.ToShortTimeString();
			BeverDriveContext.CurrentCoreGui.ClockContainer.Date = DateTime.Now.ToString("yyyy-MM-dd");
			BeverDriveContext.CurrentCoreGui.ClockContainer.Invalidate();
		}

		public void Update50Hz()
		{
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
		}
	}
}
