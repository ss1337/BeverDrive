//
// Copyright 2012-2015 Sebastian Sjödin
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
using nVlc.LibVlcWrapper.Declarations.Media;

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
				// Un-fullscreen
				this.fullScreen = 0;
				BeverDriveContext.FullScreen = false;
				VlcContext.VideoPlayer.VideoScale = 0.0f;
				this.AddControls();
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
				this.fullScreen = 1;
				BeverDriveContext.FullScreen = true;
				// TODO: Set background black here, doesn't work yet for some reason
				BeverDriveContext.CurrentCoreGui.BaseContainer.BackColor = Color.Black;
				this.SetVlcControl();
				return true;
			}

			return false;
		}

		private void SetVlcControl()
		{
			var vlcx = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 210;

			/*if (fullScreen == 2)
			{
				var vlcSize = BeverDriveContext.CurrentCoreGui.BaseContainer.Size;
				vlcSize.Height += 180;
				vlcSize.Height += 320;

				this.ctrl_vlc.BackColor = System.Drawing.Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(-160, -90);
				this.ctrl_vlc.Size = vlcSize;
			}*/

			if (this.fullScreen > 0)
			{
				this.ctrl_vlc.BackColor = System.Drawing.Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(0, 0);
				this.ctrl_vlc.Size = BeverDriveContext.CurrentCoreGui.BaseContainer.Size;
				BeverDriveContext.CurrentCoreGui.BaseContainer.Controls.Add(this.ctrl_vlc);
			}

			if (fullScreen == 0)
			{
				int height = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - ctrl_browser.Height - 10;
				this.ctrl_vlc.BackColor = System.Drawing.Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(vlcx, 0);
				this.ctrl_vlc.Size = new System.Drawing.Size(420, height);
				BeverDriveContext.CurrentCoreGui.ModuleContainer.Controls.Add(ctrl_vlc);
			}

			this.ctrl_vlc.Visible = true;
		}
	}
}
