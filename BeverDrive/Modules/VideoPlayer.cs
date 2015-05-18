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
	[BackButtonVisible(true)]
	[PlaybackModule]
	public partial class VideoPlayer : AModule
	{
		private int fullScreen;
		private FileSystemBrowserListControl ctrl_browser;
		private WebdingsButton ctrl_prev;
		private WebdingsButton ctrl_play;
		private WebdingsButton ctrl_next;
		private WebdingsButton ctrl_full;
		private Panel ctrl_vlc;

		private Playlist playlist;
		private bool vlcPopulated;

		public VideoPlayer()
		{
			int browserHeight = 7;
			if (BeverDriveContext.Settings.VideoMode == VideoMode.Mode_169)
				browserHeight = 5;

			this.ctrl_browser = new FileSystemBrowserListControl(BeverDriveContext.Settings.VideoRoot);
			this.ctrl_browser.HeightInItems = browserHeight;
			this.ctrl_browser.Name = "ctrl_browser";
			this.ctrl_browser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.ctrl_browser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.ctrl_browser.Height);

			var btnx = (BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 338) / 2;

			this.ctrl_prev = new WebdingsButton(0x39);
			this.ctrl_prev.Location = new System.Drawing.Point(70, 20);

			this.ctrl_play = new WebdingsButton(0x34);
			this.ctrl_play.Location = new System.Drawing.Point(70, 70);

			this.ctrl_next = new WebdingsButton(0x3A);
			this.ctrl_next.Location = new System.Drawing.Point(70, 120);

			this.ctrl_full = new WebdingsButton(0x32);
			this.ctrl_full.Location = new System.Drawing.Point(70, 170);

			this.ctrl_vlc = new Panel();
			this.ctrl_vlc.Name = "ctrl_vlc";

			VlcContext.VideoPlayer.WindowHandle = this.ctrl_vlc.Handle;
			VlcContext.VideoPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
			playlist = new Playlist();
		}

		protected void Events_MediaEnded(object sender, EventArgs e)
		{
			playlist.CurrentIndex++;
			this.PlayTrack();
		}

		private void AddControls()
		{
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			this.ShowControls();
			this.SetVlcControl();
		}
	}
}
