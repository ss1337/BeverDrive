//
// Copyright 2012-2017 Sebastian Sjödin
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
	[BackButtonVisible(true)]
	[MenuText("Video player")]
	[PlaybackModule]
	public partial class VideoPlayer : Module
	{
		private FileSystemListControl ctrl_browser;
		private MetroidButton ctrl_prev;
		private MetroidButton ctrl_play;
		private MetroidButton ctrl_next;
		private MetroidButton ctrl_full;
		private Panel ctrl_vlc;

		private Playlist playlist;
		private bool vlcPopulated;
		private bool playing;

		public VideoPlayer()
		{
		}

		public override void Back()
		{
			BeverDriveContext.SetActiveModule("");
		}

		public override void Init()
		{
			this.CreateControls();
		}

		protected void Events_MediaEnded(object sender, EventArgs e)
		{
			playlist.CurrentIndex++;
			this.PlayTrack();
		}

		private void CreateControls()
		{
			int browserHeight = 7;
			if (BeverDriveContext.Settings.VideoMode == VideoMode.Mode_169)
				browserHeight = 5;

			this.ctrl_browser = new FileSystemListControl(BeverDriveContext.Settings.VideoRoot);
			this.ctrl_browser.HeightInItems = browserHeight;
			this.ctrl_browser.Name = "ctrl_browser";
			this.ctrl_browser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.ctrl_browser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.ctrl_browser.Height);

			var btnx = (BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 338) / 2;

			this.ctrl_prev = new MetroidButton("core_prev.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.ctrl_prev.Index = -4;
			this.ctrl_prev.Location = new System.Drawing.Point(70, 20);
			this.ctrl_prev.Click += (sender, e) => { };
			this.ctrl_prev.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Previous"; };

			this.ctrl_play = new MetroidButton("core_play.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.ctrl_play.Index = -3;
			this.ctrl_play.Location = new System.Drawing.Point(70, 70);
			this.ctrl_play.Click += (sender, e) => { this.TogglePlayback(); };
			this.ctrl_play.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Set fullscreen"; };

			this.ctrl_next = new MetroidButton("core_next.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.ctrl_next.Index = -2;
			this.ctrl_next.Location = new System.Drawing.Point(70, 120);
			this.ctrl_next.Click += (sender, e) => { };
			this.ctrl_next.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Next"; };

			this.ctrl_full = new MetroidButton("core_full.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.ctrl_full.Index = -1;
			this.ctrl_full.Location = new System.Drawing.Point(70, 170);
			this.ctrl_full.Click += (sender, e) => { this.SetFullScreen(true); };
			this.ctrl_full.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Set fullscreen"; };

			this.ctrl_vlc = new Panel();
			this.ctrl_vlc.Name = "ctrl_vlc";
			this.ctrl_vlc.Visible = false;

			VlcContext.VideoPlayer.WindowHandle = this.ctrl_vlc.Handle;
			VlcContext.VideoPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
			playlist = new Playlist();

			base.Controls.Add(ctrl_browser);
			base.Controls.Add(ctrl_full);
			base.Controls.Add(ctrl_next);
			base.Controls.Add(ctrl_play);
			base.Controls.Add(ctrl_prev);
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Controls.Add(ctrl_vlc);
		}
	}
}
