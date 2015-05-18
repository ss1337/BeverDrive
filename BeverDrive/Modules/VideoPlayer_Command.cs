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
				case ModuleCommands.SelectLeft:
					this.SelectNext();
					break;
				case ModuleCommands.SelectRight:
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
			if (this.SetFullScreen(false))
				return;

			switch (this.ctrl_browser.SelectedIndex)
			{
				case -5:
					BeverDriveContext.SetActiveModule("");
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
					SetFullScreen(true);
					break;

				default:
					if (this.ctrl_browser.SelectedItem.StartsWith("\\"))
					{
						this.ctrl_browser.Select();
						vlcPopulated = false;
					}
					else
					{
						// Check if we are already playing this track, if so, fullscreen
						if (vlcPopulated)
						{
							if (playlist.CurrentItem.Filename == this.ctrl_browser.SelectedItem)
							{
								// Full screen
								SetFullScreen(true);
							}
						}

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
	}
}
