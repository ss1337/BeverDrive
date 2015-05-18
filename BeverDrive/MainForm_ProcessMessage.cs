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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Core;
using BeverDrive.Ibus;
using BeverDrive.Ibus.Extensions;
using BeverDrive.Modules;

namespace BeverDrive
{
	public partial class MainForm : Form
	{
		private void ProcessMessage(string message)
		{
			bool rtsEnable = BeverDriveContext.Ibus.RtsEnable;
			int disc = VlcContext.CurrentDisc;
			int track = VlcContext.CurrentTrack;

			if (message.IsMessage(BeverDrive.Ibus.Messages.Other.Cdc_PollCd))
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Other.Cdc_PollResponse);

			if (message.IsMessage(BeverDrive.Ibus.Messages.Other.AnteKrök))
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_StatusPlaying(disc, track));

			if (message.IsMessage(BeverDrive.Ibus.Messages.Other.Wheel_NextTrack))
			{
				// TODO: Make this point to CurrentVlc and update current module
				// TODO: Check if sending TrackStart avoids urspårning
				BeverDriveContext.CurrentCoreGui.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.NextTrack });
				
				// Should it really be a cd -> radio message???
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(disc, track));
			}

			if (message.IsMessage(BeverDrive.Ibus.Messages.Other.Wheel_PrevTrack))
			{
				// TODO: Make this point to CurrentVlc and update current module
				// TODO: Check if sending TrackStart avoids urspårning
				BeverDriveContext.CurrentCoreGui.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.PreviousTrack });

				// Should it really be a cd -> radio message???
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(disc, track));
			}

			if (message.IsMessage(BeverDrive.Ibus.Messages.Radio.ReqTrack) /* || message.IsMessage("68 05 18 38 06 0X XX")*/)
			{
				if (VlcContext.IsPlaying)
					BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_StatusPlaying(disc, track));
				else
					BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_StatusNotPlaying(disc, track));
			}

			if (message.IsMessage(BeverDrive.Ibus.Messages.Radio.PlayCd))
			{
				BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.StartPlayback });
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(disc, track));
			}

			if (message.IsMessage(BeverDrive.Ibus.Messages.Radio.StopCd))
			{
				// Stop playback
				BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.StopPlayback });
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_StatusNotPlaying(disc, track));
			}

			if (message.IsMessage(BeverDrive.Ibus.Messages.Radio.Menu))
				rtsEnable = false;

			if (rtsEnable)
			{
				if (message.IsMessage(BeverDrive.Ibus.Messages.BordMonitor.RightKnobLeft))
					BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectLeft });

				if (message.IsMessage(BeverDrive.Ibus.Messages.BordMonitor.RightKnobRight))
					BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectRight });

				if (message.IsMessage(BeverDrive.Ibus.Messages.BordMonitor.RightKnobPush))
					BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			}

			// Check if we should enable or disable RTS
			// This checks whether the message is a Write large text message
			if (message.IsMessage("68 XX 3B 23 62 30", true))
			{
				rtsEnable = false;

				// Simpler check, check for CD X-XX only
				var index = message.IndexOf("43 44 20");
				if (message.Substring(index, 20).IsMessage("43 44 20 3X 2D XX XX"))
					rtsEnable = true;

				// Check for SCAN
				if (message.Contains("53 43 41 4E"))
					rtsEnable = true;

				// TAPE
				// We dont want stuff in tape mode
				if (message.Contains("54 41 50 45"))
					rtsEnable = false;
			}

			if (message.IsMessage(BeverDrive.Ibus.Messages.BordMonitor.Menu) || message.IsMessage(BeverDrive.Ibus.Messages.BordMonitor.Tone))
			{
				rtsEnable = false;
			}

			// Something IKE -> LCM, PDC?
			if (message.IsMessage("80 04 BF 11 00 2A") || message.StartsWith("3B 06 68 46 02"))
			{
				rtsEnable = false;
			}

			if (rtsEnable)
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Predefined.LightWipers.SetTvMode(BeverDriveContext.Settings.TvMode));

			BeverDriveContext.Ibus.RtsEnable = rtsEnable;
			BeverDriveContext.ActiveModule.ProcessMessage(message);
		}
	}
}
