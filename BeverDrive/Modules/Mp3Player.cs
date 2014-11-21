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
using System.Text;
using System.Windows.Forms;
using BeverDrive.Controls;
using BeverDrive.Core;
using BeverDrive.Core.Styles;
using nVlc.LibVlcWrapper.Declarations;
using nVlc.LibVlcWrapper.Declarations.Events;
using nVlc.LibVlcWrapper.Declarations.Media;
using nVlc.LibVlcWrapper.Declarations.Players;
using nVlc.LibVlcWrapper.Implementation;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	[PlaybackModule]
	public class Mp3Player : AModule
	{
		/// <summary>
		/// Controls...
		/// </summary>
		private FileSystemBrowserList ctrl_browser;
		private Label ctrl_album;
		private Label ctrl_filename;
		private Label ctrl_title;
		private BeverDrive.Controls.ProgressBar ctrl_pb;
		private MetroidButton ctrl_shuffle;

		private Playlist playlist;
		private bool vlcPopulated;
		private bool shuffle;

		public Mp3Player()
		{
			this.CreateControls();
			VlcContext.AudioPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
			playlist = new Playlist();
		}

		protected void Events_MediaEnded(object sender, EventArgs e)
		{
			playlist.CurrentIndex++;
			this.PlayTrack();
		}

		#region IModule methods
		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch (e.Command)
			{
				case ModuleCommands.Hide:
					this.Hide();
					break;
				case ModuleCommands.SelectClick:
					this.SelectClick();
					break;
				case ModuleCommands.SelectNext:
					this.SelectNext();
					break;
				case ModuleCommands.SelectPrevious:
					this.SelectPrevious();
					break;
				case ModuleCommands.Show:
					this.Show();
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
		}

		public override void Update1Hz()
		{
			if (VlcContext.AudioPlayer.IsPlaying)
			{
				var d = VlcContext.AudioPlayer.Length;
				var t = VlcContext.AudioPlayer.Time;
				this.ctrl_pb.Maximum = (int)d;
				this.ctrl_pb.Value = (int)t;
				this.ctrl_pb.Invalidate();
			}
		}
		#endregion

		#region Command methods
		private void Hide()
		{
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Controls.Clear();
		}

		private void SelectClick()
		{
			if (this.ctrl_browser.SelectedIndex == -2)
			{
				BeverDriveContext.SetActiveModule("MainMenu");
				return;
			}
			else if (this.ctrl_browser.SelectedIndex == -1)
			{
				// Enable/disable shuffle
				if (this.shuffle)
				{
					this.shuffle = false;
					playlist.Unshuffle();
				}
				else
				{
					this.shuffle = true;
					playlist.Shuffle();
				}
			}
			else
			{
				this.ctrl_browser.Select();

				if (this.ctrl_browser.SelectedItem.StartsWith("\\"))
				{
					vlcPopulated = false;
					shuffle = false;
					ctrl_shuffle.Selected = false;
				}
				else
				{
					if (!vlcPopulated)
					{
						this.playlist.Clear();

						// Add stuff to list
						foreach (var f in ctrl_browser.Files)
							playlist.AddFile(ctrl_browser.CurrentPath + "\\" + f.Name);
					}

					vlcPopulated = true;

					playlist.CurrentIndex = this.ctrl_browser.SelectedIndex - this.ctrl_browser.Directories.Count - 1;
					this.PlayTrack();
				}

				ctrl_shuffle.Invalidate();
			}
		}

		private void SelectNext()
		{
			if (this.ctrl_browser.SelectedIndex == -2)
			{
				BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
				BeverDriveContext.CurrentCoreGui.BackButton.Invalidate();
				this.ctrl_shuffle.Selected = true;
				this.ctrl_shuffle.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex == -1)
			{
				this.ctrl_shuffle.Selected = this.shuffle;
				this.ctrl_shuffle.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex != this.ctrl_browser.Items.Count - 1)
			{
				this.ctrl_browser.SelectedIndex++;
				this.ctrl_browser.Invalidate();
			}
		}

		private void SelectPrevious()
		{
			if (this.ctrl_browser.SelectedIndex > -2)
				this.ctrl_browser.SelectedIndex--;

			if (this.ctrl_browser.SelectedIndex == -2)
			{
				BeverDriveContext.CurrentCoreGui.BackButton.Selected = true;
				BeverDriveContext.CurrentCoreGui.BackButton.Invalidate();
				this.ctrl_shuffle.Selected = this.shuffle;
				this.ctrl_shuffle.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex == -1)
			{
				this.ctrl_shuffle.Selected = true;
				this.ctrl_shuffle.Invalidate();
			}

			this.ctrl_browser.Invalidate();
		}

		private void Show()
		{
			this.ctrl_browser.SelectedIndex = 0;
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_browser);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_album);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_filename);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_title);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_pb);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_shuffle);
		}

		private void StartPlayback()
		{
			VlcContext.AudioPlayer.Play();
		}

		private void StopPlayback()
		{
			VlcContext.AudioPlayer.Pause();
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

		#endregion

		#region Event handlers
		// TODO: Används dessa?
		void BaseVlc_MediaChanged(/*Vlc.DotNet.Forms.VlcControl sender, VlcEventArgs<EventArgs> e*/)
		{
		}

		void ctrl_browser_ItemSelected(object sender, BeverDrive.Controls.ItemSelectedEventArgs e)
		{
			//throw new NotImplementedException();
		}
		#endregion

		private void PlayTrack()
		{
			VlcContext.AudioPlayer.Stop();
			VlcContext.AudioPlayer.Open(playlist.CurrentItem.VlcMedia);
			VlcContext.AudioPlayer.Play();

			VlcContext.CurrentTrack = playlist.CurrentIndex + 1;
			BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(VlcContext.CurrentDisc, VlcContext.CurrentTrack));

			if (BeverDriveContext.CurrentMainForm.InvokeRequired)
				BeverDriveContext.CurrentMainForm.Invoke(new Action(() => this.PopulateGui()));
			else
				this.PopulateGui();
		}

		private void PopulateGui()
		{
			this.ctrl_pb.Reset();
			this.ctrl_title.Text = playlist.CurrentItem.Artist + " - " + playlist.CurrentItem.Title;
			this.ctrl_album.Text = playlist.CurrentItem.Album;
			this.ctrl_filename.Text = playlist.CurrentItem.Filename;
			this.ctrl_title.Invalidate();
			this.ctrl_album.Invalidate();
			this.ctrl_filename.Invalidate();
		}

		private void CreateControls()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;

			this.ctrl_browser = new FileSystemBrowserList(BeverDriveContext.Settings.MusicRoot);
			this.ctrl_browser.HeightInItems = 7;
			this.ctrl_browser.Name = "list1";
			this.ctrl_browser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.ctrl_browser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.ctrl_browser.Height);
			this.ctrl_browser.TabIndex = 0;
			this.ctrl_browser.ItemSelected += new MenuOptionList.ItemSelectedEventHandler(ctrl_browser_ItemSelected);

			this.ctrl_title = new Label();
			this.ctrl_title.AutoSize = false;
			this.ctrl_title.Font = Fonts.GuiFont32;
			this.ctrl_title.BackColor = System.Drawing.Color.Transparent;
			this.ctrl_title.ForeColor = Colors.ForeColor;
			this.ctrl_title.Location = new System.Drawing.Point(42, 16);
			this.ctrl_title.Size = new System.Drawing.Size(width - 84, 50);
			this.ctrl_title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.ctrl_title.Text = "";

			this.ctrl_album = new Label();
			this.ctrl_album.AutoSize = false;
			this.ctrl_album.Font = Fonts.GuiFont24;
			this.ctrl_album.BackColor = System.Drawing.Color.Transparent;
			this.ctrl_album.ForeColor = Colors.ForeColor;
			this.ctrl_album.Location = new System.Drawing.Point(0, 66);
			this.ctrl_album.Size = new System.Drawing.Size(width, 38);
			this.ctrl_album.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.ctrl_album.Text = "";

			this.ctrl_filename = new Label();
			this.ctrl_filename.AutoSize = false;
			this.ctrl_filename.Font = Fonts.GuiFont14;
			this.ctrl_filename.BackColor = System.Drawing.Color.Transparent;
			this.ctrl_filename.ForeColor = Colors.ForeColor;
			this.ctrl_filename.Location = new System.Drawing.Point(0, 104);
			this.ctrl_filename.Size = new System.Drawing.Size(width, 26);
			this.ctrl_filename.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.ctrl_filename.Text = "";

			this.ctrl_pb = new BeverDrive.Controls.ProgressBar();
			this.ctrl_pb.BackColor = Colors.BackColor;
			this.ctrl_pb.Location = new System.Drawing.Point(15, 140);
			this.ctrl_pb.Height = 25;
			this.ctrl_pb.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width - 30;
			this.ctrl_pb.Maximum = 100;
			this.ctrl_pb.Value = 0;

			var btnx = (BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 338) / 2;

			this.ctrl_shuffle = new MetroidButton("Resources\\shuffle.png", "Resources\\shuffle_s.png");
			this.ctrl_shuffle.Name = "ctrl_shuffle";
			this.ctrl_shuffle.Location = new System.Drawing.Point(15, 185);
			this.ctrl_shuffle.TabIndex = 0;
		}
	}
}
