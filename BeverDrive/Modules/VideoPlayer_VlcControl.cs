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
	public partial class VideoPlayer : AModule
	{
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
			if (fullScreen == 2)
			{
				// Set every control in ModuleContainer as visible
				foreach (Control ctrl in BeverDriveContext.CurrentCoreGui.ModuleContainer.Controls)
					ctrl.Visible = true;

				BeverDriveContext.CurrentCoreGui.ClockContainer.Visible = true;

				// Un-fullscreen
				this.fullScreen = 0;
				VlcContext.VideoPlayer.VideoScale = 0.0f;
				this.SetVlcControl();
				return true;
			}

			if (fullScreen == 1)
			{
				this.fullScreen = 2;
				VlcContext.VideoPlayer.VideoScale = 1.6f;
				return true;
			}

			if (switchTo)
			{
				// Set every control except ctrl_vlc as not visible
				foreach (Control ctrl in BeverDriveContext.CurrentCoreGui.ModuleContainer.Controls)
					if (ctrl != ctrl_vlc)
						ctrl.Visible = false;

				BeverDriveContext.CurrentCoreGui.ClockContainer.Visible = false;

				this.fullScreen = 1;
				this.SetVlcControl();
				return true;
			}

			return false;
		}

		private void SetVlcControl()
		{
			var vlcx = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 210;

			if (this.fullScreen == 1)
			{
				this.ctrl_vlc.BackColor = Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(0, 0);
				this.ctrl_vlc.Size = BeverDriveContext.CurrentCoreGui.ModuleAreaSize;
			}

			if (fullScreen == 0)
			{
				int height = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - ctrl_browser.Height - 10;
				this.ctrl_vlc.BackColor = Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(vlcx, 0);
				this.ctrl_vlc.Size = new System.Drawing.Size(420, height);
				BeverDriveContext.CurrentCoreGui.ModuleContainer.BackColor = BeverDriveContext.Settings.BackColor;
				BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
			}
		}
	}
}
