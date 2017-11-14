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
using System.IO;
using System.Reflection;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;
using nVlc.LibVlcWrapper.Declarations;
using nVlc.LibVlcWrapper.Declarations.Events;
using nVlc.LibVlcWrapper.Declarations.Media;
using nVlc.LibVlcWrapper.Declarations.Players;
using nVlc.LibVlcWrapper.Implementation;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	[MenuText("Music player")]
	[PlaybackModule]
	public class Mp3Player : Module
	{
		/// <summary>
		/// Controls...
		/// </summary>
		private FileSystemListControl ctrlBrowser;
		private Label lblAlbum;
		private Label lblFilename;
		private Label lblTitle;
		private ProgressBar ctrlPb;
		private MetroidButton mbPrev;
		private MetroidButton mbPlay;
		private MetroidButton mbNext;
		private MetroidButton mbShuffle;

		private Playlist playlist;
		private bool vlcPopulated;
		private bool playing;
		private bool shuffle;

		public Mp3Player()
		{
		}

		protected void Events_MediaEnded(object sender, EventArgs e)
		{
			playlist.CurrentIndex++;
			this.PlayTrack();
		}

		#region Module methods
		public override void Back()
		{
			BeverDriveContext.SetActiveModule("");
		}

		public override void Init()
		{
			this.CreateControls();
			VlcContext.AudioPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
			playlist = new Playlist();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Music player";
			base.OnCommand(e);

			if (this.SelectedIndex == this.ctrlBrowser.Items.Count)
				this.SelectedIndex--;

			switch (e.Command)
			{
				case ModuleCommands.Show:
					if (VlcContext.VideoPlayer.IsPlaying)
						VlcContext.VideoPlayer.Pause();

					break;
				case ModuleCommands.StartPlayback:
					this.StartPlayback();
					break;
				case ModuleCommands.StopPlayback:
					this.StopPlayback();
					break;
				case ModuleCommands.NextTrack:
					this.NextTrack();
					break;
				case ModuleCommands.PreviousTrack:
					this.PreviousTrack();
					break;
			}

			if (e.Command == ModuleCommands.SelectClick && this.SelectedIndex > -1)
			{
				this.ctrlBrowser.Select();

				if (this.ctrlBrowser.SelectedItemIsFile())
				{
					if (!vlcPopulated)
					{
						this.playlist.Clear();

						// Add stuff to list
						foreach (var f in ctrlBrowser.Files)
							playlist.AddFile(ctrlBrowser.CurrentPath + Path.DirectorySeparatorChar + f.Name);

						// TODO: Add cover image
						/*if (ctrl_browser.CurrentItem.CoverImage != null)
						{
							BeverDriveContext.CurrentCoreGui.ModuleContainer.SetBackgroundImage(ctrl_browser.CurrentItem.Name, ctrl_browser.CurrentItem.CoverImage);
							BeverDriveContext.CurrentCoreGui.Invalidate();
						}
						else
						{
							BeverDriveContext.CurrentCoreGui.ModuleContainer.SetBackgroundImage("", null);
						}*/
					}

					vlcPopulated = true;

					playlist.CurrentIndex = this.ctrlBrowser.SelectedIndex - this.ctrlBrowser.Directories.Count - 1;
					this.PlayTrack();
				}
				else
				{
					vlcPopulated = false;
					shuffle = false;
					this.SelectedIndex = this.ctrlBrowser.SelectedIndex;
				}
			}

			this.ctrlBrowser.SelectedIndex = this.SelectedIndex;
			this.mbShuffle.Selected = this.mbShuffle.Selected || this.shuffle;
			this.mbPlay.Selected = this.mbPlay.Selected || this.playing;
			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		public override void Update1Hz()
		{
			if (VlcContext.AudioPlayer.IsPlaying)
			{
				var d = VlcContext.AudioPlayer.Length;
				var t = VlcContext.AudioPlayer.Time;
				this.ctrlPb.Maximum = (int)d;
				this.ctrlPb.Value = (int)t;
				BeverDriveContext.CurrentCoreGui.Invalidate();
			}
		}
		#endregion

		#region Playback
		private void StartPlayback()
		{
			VlcContext.PlayAudio();
			this.playing = true;
		}

		private void StopPlayback()
		{
			VlcContext.AudioPlayer.Pause();
			this.playing = false;
		}

		private void NextTrack()
		{
			this.StopPlayback();
			playlist.CurrentIndex++;
			this.PlayTrack();
		}

		private void PreviousTrack()
		{
			this.StopPlayback();
			playlist.CurrentIndex--;
			this.PlayTrack();
		}

		private void TogglePlayback()
		{
			if (VlcContext.AudioPlayer.IsPlaying)
				this.StopPlayback();
			else
				this.StartPlayback();
		}

		private void ToggleShuffle()
		{
			if (this.shuffle)
			{
				playlist.Unshuffle();
				this.shuffle = false;
			}
			else
			{
				playlist.Shuffle();
				this.shuffle = true;
			}
		}

		private void PlayTrack()
		{
			VlcContext.AudioPlayer.Stop();
			VlcContext.AudioPlayer.Open(playlist.CurrentItem.VlcMedia);
			VlcContext.PlayAudio();
			this.playing = true;

			VlcContext.CurrentTrack = playlist.CurrentIndex + 1;
			BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(VlcContext.CurrentDisc, VlcContext.CurrentTrack));

			if (BeverDriveContext.CurrentMainForm.InvokeRequired)
				BeverDriveContext.CurrentMainForm.Invoke(new Action(() => this.RefreshGui()));
			else
				this.RefreshGui();
		}
		#endregion

		private void RefreshGui()
		{
			this.ctrlPb.Reset();
			this.lblTitle.Text = playlist.CurrentItem.Artist + " - " + playlist.CurrentItem.Title;
			this.lblAlbum.Text = playlist.CurrentItem.Album;
			this.lblFilename.Text = playlist.CurrentItem.Filename;
		}

		private void CreateControls()
		{
			int browserHeight = 7;
			if (BeverDriveContext.Settings.VideoMode == VideoMode.Mode_169)
				browserHeight = 5;

			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;

			this.ctrlBrowser = new FileSystemListControl(BeverDriveContext.Settings.MusicRoot);
			this.ctrlBrowser.HeightInItems = browserHeight;
			this.ctrlBrowser.Index = 0;
			this.ctrlBrowser.Name = "list1";
			this.ctrlBrowser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.ctrlBrowser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.ctrlBrowser.Height);

			this.lblTitle = new Label();
			this.lblTitle.Font = Fonts.GuiFont32;
			this.lblTitle.ForeColor = Colors.ForeColor;
			this.lblTitle.Location = new System.Drawing.Point(42, 16);
			this.lblTitle.Size = new System.Drawing.Size(width - 84, 50);
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblTitle.Text = "";

			this.lblAlbum = new Label();
			this.lblAlbum.Font = Fonts.GuiFont24;
			this.lblAlbum.ForeColor = Colors.ForeColor;
			this.lblAlbum.Location = new System.Drawing.Point(19, 66);
			this.lblAlbum.Size = new System.Drawing.Size(width, 38);
			this.lblAlbum.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblAlbum.Text = "";

			this.lblFilename = new Label();
			this.lblFilename.Font = Fonts.GuiFont14;
			this.lblFilename.ForeColor = Colors.ForeColor;
			this.lblFilename.Location = new System.Drawing.Point(13, 104);
			this.lblFilename.Size = new System.Drawing.Size(width, 26);
			this.lblFilename.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblFilename.Text = "";

			this.ctrlPb = new ProgressBar();
			this.ctrlPb.BackColor = Colors.BackColor;
			this.ctrlPb.Location = new System.Drawing.Point(15, 138);
			this.ctrlPb.Height = 25;
			this.ctrlPb.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width - 30;
			this.ctrlPb.Maximum = 100;
			this.ctrlPb.Value = 0;

			var x = width / 2 - 120;

			this.mbPrev = new MetroidButton("core_prev.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.mbPrev.Index = -4;
			this.mbPrev.Location = new System.Drawing.Point(x, 172);
			this.mbPrev.Click += (sender, e) => { this.PreviousTrack(); };
			this.mbPrev.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Previous track"; };

			this.mbPlay = new MetroidButton("core_play.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.mbPlay.Index = -3;
			this.mbPlay.Location = new System.Drawing.Point(x + 60, 172);
			this.mbPlay.Click += (sender, e) => { this.TogglePlayback(); };
			this.mbPlay.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Play"; };

			this.mbNext = new MetroidButton("core_next.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.mbNext.Index = -2;
			this.mbNext.Location = new System.Drawing.Point(x + 120, 172);
			this.mbNext.Click += (sender, e) => { this.NextTrack(); };
			this.mbNext.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Next track"; };

			this.mbShuffle = new MetroidButton("core_shuffle.png", BeverDriveContext.Settings.ForeColor, BeverDriveContext.Settings.SelectedColor);
			this.mbShuffle.Index = -1;
			this.mbShuffle.Location = new System.Drawing.Point(x + 180, 172);
			this.mbShuffle.Click += (sender, e) => { this.ToggleShuffle(); };
			this.mbShuffle.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Shuffle on/off"; };

			base.Controls.Add(ctrlBrowser);
			base.Controls.Add(ctrlPb);
			base.Controls.Add(mbNext);
			base.Controls.Add(mbPlay);
			base.Controls.Add(mbPrev);
			base.Controls.Add(mbShuffle);
			base.Controls.Add(lblAlbum);
			base.Controls.Add(lblFilename);
			base.Controls.Add(lblTitle);
		}
	}
}
