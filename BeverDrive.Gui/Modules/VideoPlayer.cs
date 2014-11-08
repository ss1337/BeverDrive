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
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Core;
using BeverDrive.Gui.Core.Styles;
using nVlc.LibVlcWrapper.Declarations.Media;

namespace BeverDrive.Gui.Modules
{
	[BackButtonVisible(true)]
	//[PlaybackModule] // Nexting track in main menu causes a reboot
	public class VideoPlayer : AModule
	{
		private bool fullScreen;
		private FileSystemBrowserList ctrl_browser;
		private MetroidButton ctrl_full;
		private MetroidButton ctrl_play;
		private Panel ctrl_vlc;

		private Playlist playlist;
		private bool vlcPopulated;

		public VideoPlayer()
		{
			this.ctrl_browser = new BeverDrive.Gui.Controls.FileSystemBrowserList(BeverDriveContext.Settings.VideoRoot);
			this.ctrl_browser.HeightInItems = 7;
			this.ctrl_browser.Name = "ctrl_browser";
			this.ctrl_browser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.ctrl_browser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.ctrl_browser.Height);
			this.ctrl_browser.TabIndex = 0;

			var btnx = (BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 338) / 2;
			this.ctrl_play = new BeverDrive.Gui.Controls.MetroidButton("Resources\\play.png", "Resources\\play_s.png");
			this.ctrl_play.Name = "ctrl_play";
			this.ctrl_play.Location = new System.Drawing.Point(btnx, 65);
			this.ctrl_play.TabIndex = 0;

			this.ctrl_full = new BeverDrive.Gui.Controls.MetroidButton("Resources\\fullscreen.png", "Resources\\fullscreen_s.png");
			this.ctrl_full.Name = "ctrl_full";
			this.ctrl_full.Location = new System.Drawing.Point(btnx, 155);
			this.ctrl_full.TabIndex = 0;

			this.ctrl_vlc = new Panel();
			this.ctrl_vlc.Name = "ctrl_vlc";
			this.ctrl_vlc.TabIndex = 0;

			VlcContext.VideoPlayer.WindowHandle = this.ctrl_vlc.Handle;
			VlcContext.VideoPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
			playlist = new Playlist();
		}

		protected void Events_MediaEnded(object sender, EventArgs e)
		{
			playlist.CurrentIndex++;
			this.PlayTrack();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch(e.Command)
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

		#region Command methods
		private void Hide()
		{
			this.ctrl_vlc.Visible = false;
			VlcContext.VideoPlayer.WindowHandle = IntPtr.Zero; // Window handle must be null, othervise vlc will try to draw to a control that doesn't exist
			BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
		}

		private void StartPlayback()
		{
			VlcContext.VideoPlayer.Play();
		}

		private void StopPlayback()
		{
			VlcContext.VideoPlayer.Pause();
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

		private void PlayTrack()
		{
			VlcContext.VideoPlayer.Open(playlist.CurrentItem.VlcMedia);
			VlcContext.VideoPlayer.Play();

			VlcContext.CurrentTrack = playlist.CurrentIndex + 1;
			this.ParentForm.IbusInstance.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(VlcContext.CurrentDisc, VlcContext.CurrentTrack));

			//var m = VlcContext.Factory.CreateMedia<IMedia>(this.ctrl_browser.CurrentPath + "\\" + this.ctrl_browser.SelectedItem);
			////VlcContext.AudioPlayer.Open(playlist.CurrentItem.VlcMedia);
			////VlcContext.AudioPlayer.Open(m);
			////VlcContext.AudioPlayer.Play();
			//VlcContext.VideoPlayer.Open(m);
			//VlcContext.VideoPlayer.Play();
		}

		private void SelectClick()
		{
			if (fullScreen)
			{
				// Un-fullscreen
				this.fullScreen = false;
				BeverDriveContext.FullScreen = this.fullScreen;
				this.AddControls();
				return;
			}

			if (this.ctrl_browser.SelectedIndex == -3)
			{
				BeverDriveContext.SetActiveModule("MainMenu");
				return;
			}
			else if (this.ctrl_browser.SelectedIndex == -2)
			{
				// Play
				if (VlcContext.VideoPlayer.IsPlaying)
					VlcContext.VideoPlayer.Pause();
				else
					VlcContext.VideoPlayer.Play();
				return;
			}
			else if (this.ctrl_browser.SelectedIndex == -1)
			{
				// Full screen
				this.fullScreen = !this.fullScreen;
				BeverDriveContext.FullScreen = this.fullScreen;
				this.SetVlcControl();
				return;
			}
			else
			{
				if (this.ctrl_browser.SelectedItem.StartsWith("\\"))
				{
					this.ctrl_browser.Select();
					vlcPopulated = false;
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
			}
		}

		private void SelectNext()
		{
			// Do nothing if we are full screened
			if (fullScreen)
				return;

			if (this.ctrl_browser.SelectedIndex != this.ctrl_browser.Items.Count - 1)
				this.ctrl_browser.SelectedIndex++;

			if (this.ctrl_browser.SelectedIndex == -2)
			{
				BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
				BeverDriveContext.CurrentCoreGui.BackButton.Invalidate();
				this.ctrl_play.Selected = true;
				this.ctrl_play.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex == -1)
			{
				this.ctrl_play.Selected = false;
				this.ctrl_play.Invalidate();
				this.ctrl_full.Selected = true;
				this.ctrl_full.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex == 0)
			{
				this.ctrl_full.Selected = false;
				this.ctrl_full.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex > -1)
				this.ctrl_browser.Invalidate();
		}

		private void SelectPrevious()
		{
			// Do nothing if we are full screened
			if (fullScreen)
				return;

			if (this.ctrl_browser.SelectedIndex > -3)
				this.ctrl_browser.SelectedIndex--;

			if (this.ctrl_browser.SelectedIndex == -1)
			{
				this.ctrl_full.Selected = true;
				this.ctrl_full.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex == -2)
			{
				this.ctrl_full.Selected = false;
				this.ctrl_full.Invalidate();
				this.ctrl_play.Selected = true;
				this.ctrl_play.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex == -3)
			{
				BeverDriveContext.CurrentCoreGui.BackButton.Selected = true;
				BeverDriveContext.CurrentCoreGui.BackButton.Invalidate();
				this.ctrl_play.Selected = false;
				this.ctrl_play.Invalidate();
			}

			if (this.ctrl_browser.SelectedIndex > -2)
				this.ctrl_browser.Invalidate();
		}

		private void Show()
		{
			if (VlcContext.AudioPlayer.IsPlaying)
				VlcContext.AudioPlayer.Pause();

			if (VlcContext.VizPlayer.IsPlaying)
				VlcContext.VizPlayer.Pause();

			// Set vlc control to the
			VlcContext.VideoPlayer.WindowHandle = this.ctrl_vlc.Handle;
			this.ctrl_browser.SelectedIndex = 0;
			this.AddControls();
		}
		#endregion

		#region Usercontrol methods
		private void AddControls()
		{
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_browser);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_full);
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_play);
			this.SetVlcControl();
		}

		private void SetVlcControl()
		{
			var vlcx = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 210;

			if (this.fullScreen)
			{
				this.ctrl_vlc.BackColor = System.Drawing.Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(0, 0);
				this.ctrl_vlc.Size = BeverDriveContext.CurrentCoreGui.BaseContainer.Size;
				BeverDriveContext.CurrentCoreGui.BaseContainer.Controls.Add(this.ctrl_vlc);
			}
			else
			{
				this.ctrl_vlc.BackColor = System.Drawing.Color.Black;
				this.ctrl_vlc.Location = new System.Drawing.Point(vlcx, 0);
				this.ctrl_vlc.Size = new System.Drawing.Size(420, 240);
				BeverDriveContext.CurrentCoreGui.AddControl(ctrl_vlc);
			}

			this.ctrl_vlc.Visible = true;
		}
		#endregion
	}
}
