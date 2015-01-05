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
using nVlc.LibVlcWrapper.Declarations.Media;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	//[PlaybackModule] // Nexting track in main menu causes a reboot
	public class VideoPlayer : AModule
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
			this.ctrl_browser = new FileSystemBrowserListControl(BeverDriveContext.Settings.VideoRoot);
			this.ctrl_browser.HeightInItems = 7;
			this.ctrl_browser.Name = "ctrl_browser";
			this.ctrl_browser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.ctrl_browser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.ctrl_browser.Height);

			var btnx = (BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 338) / 2;

			this.ctrl_prev = new WebdingsButton(0x39);
			this.ctrl_prev.Location = new System.Drawing.Point(70, 30);

			this.ctrl_play = new WebdingsButton(0x34);
			this.ctrl_play.Location = new System.Drawing.Point(70, 80);

			this.ctrl_next = new WebdingsButton(0x3A);
			this.ctrl_next.Location = new System.Drawing.Point(70, 130);

			this.ctrl_full = new WebdingsButton(0x32);
			this.ctrl_full.Location = new System.Drawing.Point(70, 180);

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

			BeverDriveContext.CurrentCoreGui.Invalidate();
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
			this.ctrl_play.Selected = true;
			VlcContext.VideoPlayer.Open(playlist.CurrentItem.VlcMedia);
			VlcContext.VideoPlayer.Play();
			VlcContext.CurrentTrack = playlist.CurrentIndex + 1;
			BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(VlcContext.CurrentDisc, VlcContext.CurrentTrack));
		}

		private void SelectClick()
		{
			// Fullscreen 0 = GUI visible
			// Fullscreen 1 = normal zoom on video
			// Fullscreen 2 = zoomed video
			if (fullScreen == 1)
			{
				this.fullScreen = 2;
				VlcContext.VideoPlayer.CropGeometry.CropArea = new Rectangle(0, 0, ctrl_vlc.Size.Width, ctrl_vlc.Size.Height);
				VlcContext.VideoPlayer.CropGeometry.Enabled = true;
				return;
			}

			if (fullScreen == 2) 
			{
				// Un-fullscreen
				this.fullScreen = 0;
				BeverDriveContext.FullScreen = false;
				this.AddControls();
				return;
			}

			switch (this.ctrl_browser.SelectedIndex)
			{
				case -5:
					BeverDriveContext.SetActiveModule("MainMenu");
					break;
				
				case -4:
					// TODO: Play previous video...
					break;
				
				case -3:
					// Play
					if (VlcContext.VideoPlayer.IsPlaying)
						VlcContext.VideoPlayer.Pause();
					else
						VlcContext.VideoPlayer.Play();
					break;

				case -2:
					// TODO: Play next video...
					break;

				case -1:
					// Full screen
					this.fullScreen = 1;
					BeverDriveContext.FullScreen = true;
					VlcContext.VideoPlayer.CropGeometry.Enabled = false;
					this.SetVlcControl();
					break;

				default:
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
					break;
			}
		}

		private void SelectNext()
		{
			// Do nothing if we are full screened
			if (fullScreen > 0)
				return;

			if (this.ctrl_browser.SelectedIndex != this.ctrl_browser.Items.Count - 1)
				this.ctrl_browser.SelectedIndex++;

			// Do stuff depending on position
			switch (this.ctrl_browser.SelectedIndex)
			{
				case -4:
					BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
					this.ctrl_prev.Selected = true;
					break;

				case -3:
					this.ctrl_prev.Selected = false;
					this.ctrl_play.Selected = true;
					break;

				case -2:
					this.ctrl_play.Selected = VlcContext.VideoPlayer.IsPlaying;
					this.ctrl_next.Selected = true;
					break;

				case -1:
					this.ctrl_next.Selected = false;
					this.ctrl_full.Selected = true;
					break;

				case 0:
					this.ctrl_full.Selected = false;
					break;

				default:
					break;
			}
		}

		private void SelectPrevious()
		{
			// Do nothing if we are full screened
			if (fullScreen > 0)
				return;

			if (this.ctrl_browser.SelectedIndex > -5)
				this.ctrl_browser.SelectedIndex--;

			// Do stuff depending on position
			switch (this.ctrl_browser.SelectedIndex)
			{
				case -5:
					BeverDriveContext.CurrentCoreGui.BackButton.Selected = true;
					this.ctrl_prev.Selected = false;
					break;

				case -4:
					this.ctrl_prev.Selected = true;
					this.ctrl_play.Selected = VlcContext.VideoPlayer.IsPlaying;
					break;

				case -3:
					this.ctrl_play.Selected = true;
					this.ctrl_next.Selected = false;
					break;

				case -2:
					this.ctrl_next.Selected = true;
					this.ctrl_full.Selected = false;
					break;

				case -1:
					this.ctrl_full.Selected = true;
					break;

				default:
					break;
			}

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
			this.ShowControls();
			this.SetVlcControl();
		}

		private void SetVlcControl()
		{
			var vlcx = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 210;

			if (this.fullScreen > 0)
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
				BeverDriveContext.CurrentCoreGui.ModuleContainer.Controls.Add(ctrl_vlc);
			}

			this.ctrl_vlc.Visible = true;
		}
		#endregion
	}
}
