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
using System.IO;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;
using nVlc.LibVlcWrapper.Declarations.Media;

namespace BeverDrive.Modules
{
	public partial class VideoPlayer : Module
	{
		public override void OnCommand(ModuleCommandEventArgs e)
		{
			if (this.vlcMode != VlcMode.Normal)
			{
				if (e.Command == ModuleCommands.SelectClick)
					SetFullScreen(false);

				return;
			}


			base.OnCommand(e);

			this.ctrl_play.Selected = this.ctrl_play.Selected || this.playing;

			if (this.SelectedIndex == this.ctrl_browser.Items.Count)
				this.SelectedIndex--;

			this.ctrl_browser.SelectedIndex = this.SelectedIndex;

			switch(e.Command)
			{
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					this.Hide();
					break;
				case ModuleCommands.StartPlayback:
					this.StartPlayback();
					break;
				case ModuleCommands.StopPlayback:
					this.StopPlayback();
					break;
			}

			 if (e.Command == ModuleCommands.SelectClick && this.SelectedIndex > -1)
			{
				this.ctrl_browser.Select();
				this.SelectedIndex = this.ctrl_browser.SelectedIndex;

				if (this.ctrl_browser.SelectedItemIsFile())
				{
					if (vlcPopulated)
					{
						// Check if we are already playing this track, if so, fullscreen
						if (playlist.CurrentItem.Filename == this.ctrl_browser.SelectedItem)
						{
							// Play if paused
							VlcContext.VideoPlayer.Play();

							// Full screen
							SetFullScreen(true);
						}
						else
						{
							// .. else play new video
							playlist.CurrentIndex = this.ctrl_browser.SelectedIndex - this.ctrl_browser.Directories.Count - 1;
							this.PlayTrack();
							this.SetFullScreen(true);
						}

					}
					else
					{
						this.playlist.Clear();

						// Add stuff to list
						foreach (var f in ctrl_browser.Files)
							playlist.AddFile(ctrl_browser.CurrentPath + Path.DirectorySeparatorChar + f.Name);
						
						vlcPopulated = true;

						playlist.CurrentIndex = this.ctrl_browser.SelectedIndex - this.ctrl_browser.Directories.Count - 1;
						this.PlayTrack();
						this.SetFullScreen(true);
					}

					ctrl_play.Selected = this.playing = true;
				}
				else
				{
					vlcPopulated = false;
				}
			}
		}

		#region Show/hide
		private void Show()
		{
			if (VlcContext.AudioPlayer.IsPlaying)
				VlcContext.AudioPlayer.Pause();

			if (VlcContext.VizPlayer.IsPlaying)
				VlcContext.VizPlayer.Pause();

			// Set vlc control to the
			VlcContext.VideoPlayer.WindowHandle = this.ctrl_vlc.Handle;
			this.ctrl_vlc.Visible = true;
			this.ctrl_browser.SelectedIndex = 0;
			this.SetVlcControl();
		}

		private void Hide()
		{
			this.ctrl_vlc.Visible = false;
			VlcContext.VideoPlayer.WindowHandle = IntPtr.Zero; // Window handle must be null, othervise vlc will try to draw to a control that doesn't exist
			BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
		}
		#endregion

		#region Playback n stuffs
		private void StartPlayback()
		{
			VlcContext.VideoPlayer.Play();
		}

		private void StopPlayback()
		{
			VlcContext.VideoPlayer.Pause();
		}

		private void TogglePlayback()
		{
			this.playing = !this.playing;

			if (this.playing)
				this.StartPlayback();
			else
				this.StopPlayback();
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
			this.playing = true;
			VlcContext.VideoPlayer.Open(playlist.CurrentItem.VlcMedia);
			VlcContext.VideoPlayer.Play();
			VlcContext.CurrentTrack = playlist.CurrentIndex + 1;
			BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(VlcContext.CurrentDisc, VlcContext.CurrentTrack));
		}
		#endregion
	}
}
