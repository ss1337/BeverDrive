//
// Copyright 2012-2016 Sebastian Sjödin
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

namespace BeverDrive.Modules
{
	public enum VlcMode
	{
		Normal = 0,
		FullScreen = 1,
		FullScreen_Zoom = 2
	}

	public partial class VideoPlayer : Module
	{
		private VlcMode vlcMode;

		/// <summary>
		/// Returns true or false if we are in or coming from fullscreen 
		/// mode and thus the keypress shouldn't be processed elsewhere
		/// </summary>
		/// <param name="switchTo"></param>
		/// <returns></returns>
		private bool SetFullScreen(bool switchTo)
		{
			// Fullscreen 0 = GUI visible
			// Fullscreen 1 = normal zoom on video
			// Fullscreen 2 = zoomed video
			if (vlcMode == VlcMode.FullScreen_Zoom)
			{
				// Set every control in ModuleContainer as visible
				foreach (var ctrl in BeverDriveContext.CurrentCoreGui.ModuleContainer.GraphicControls)
					ctrl.Visible = true;

				BeverDriveContext.CurrentCoreGui.ClockContainer.Visible = true;
				BeverDriveContext.CurrentMainForm.BackColor = BeverDrive.Gui.Styles.Colors.BackColor;

				// Un-fullscreen
				this.vlcMode = VlcMode.Normal;
				VlcContext.VideoPlayer.VideoScale = 0.0f;
				this.SetVlcControl();
				return true;
			}

			if (vlcMode == VlcMode.FullScreen)
			{
				this.vlcMode = VlcMode.FullScreen_Zoom;
				VlcContext.VideoPlayer.VideoScale = 1.6f;
				return true;
			}

			if (switchTo)
			{
				// Set every control except ctrl_vlc as not visible
				foreach (var ctrl in BeverDriveContext.CurrentCoreGui.ModuleContainer.GraphicControls)
					ctrl.Visible = false;

				BeverDriveContext.CurrentCoreGui.ClockContainer.Visible = false;
				BeverDriveContext.CurrentMainForm.BackColor = Color.Black;

				this.vlcMode = VlcMode.FullScreen;
				this.SetVlcControl();
				return true;
			}

			return false;
		}

		private void SetVlcControl()
		{
			var vlcx = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 210;

			switch(this.vlcMode)
			{
				case VlcMode.FullScreen:
					this.ctrl_vlc.BackColor = Color.Black;
					this.ctrl_vlc.Location = new System.Drawing.Point(0, 0);
					this.ctrl_vlc.Size = BeverDriveContext.CurrentCoreGui.ModuleAreaSize;
					break;

				case VlcMode.Normal:
					int height = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - ctrl_browser.Height - 10;
					this.ctrl_vlc.BackColor = Color.Black;
					this.ctrl_vlc.Location = new System.Drawing.Point(vlcx, 0);
					this.ctrl_vlc.Size = new System.Drawing.Size(420, height);
					BeverDriveContext.CurrentCoreGui.ModuleContainer.BackColor = BeverDriveContext.Settings.BackColor;
					BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
					break;
			}
		}
	}
}
