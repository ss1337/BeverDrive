//
// Copyright 2011-2014 Sebastian Sjödin
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
namespace BeverDrive.Ibus.Messages.Predefined
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class CdChanger
	{
		public static Message Cd2Broad_Announce()
		{
			// Cd-changer announce: 18 04 FF 02 01 E0 (24 4 255 2 1 224)
			return new Message(Devices.CdChanger, Devices.Broadcast, new byte[2] { 0x02, 0x01 });
		}

		public static Message Cd2Broad_PollResponse()
		{
			// Cd-changer poll response: 18 04 FF 02 00 E1 (24 4 255 2 0 225)
			return new Message(Devices.CdChanger, Devices.Broadcast, new byte[2] { 0x02, 0x00 });
		}

		public static Message Cd2Radio_StatusNotPlaying(Int32 disc, Int32 track)
		{
			var data = new byte[8] { 0x39, 0x00, 0x02, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}

		public static Message Cd2Radio_StatusPlaying(Int32 disc, Int32 track)
		{
			// 18 0A 68 39 02 09 00 3F 00 01 00 76
			var data = new byte[8] { 0x39, 0x00, 0x09, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}

		public static Message Cd2Radio_TrackStart(Int32 disc, Int32 track)
		{
			// TODO: Implementera this everywhere
			if (disc > 6)
				disc = 6;

			if (track > 99)
				track = track - 100;

			var data = new byte[8] { 0x39, 0x02, 0x09, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}

		public static Message Cd2Radio_ScanForward(Int32 disc, Int32 track)
		{
			var data = new byte[8] { 0x39, 0x03, 0x09, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}

		public static Message Cd2Radio_ScanBackward(Int32 disc, Int32 track)
		{
			var data= new byte[8] { 0x39, 0x04, 0x09, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}

		public static Message Cd2Radio_Seeking(Int32 disc, Int32 track)
		{
			var data = new byte[8] { 0x39, 0x08, 0x09, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}

		public static Message Cd2Radio_TrackEnd(Int32 disc, Int32 track)
		{
			var data = new byte[8] { 0x39, 0x07, 0x09, 0x00, 0x3F, 0x00, 0x00, 0x00 };
			data[6] = (byte)disc;
			data[7] = byte.Parse(track.ToString(), System.Globalization.NumberStyles.HexNumber);
			return new Message(Devices.CdChanger, Devices.Radio, data);
		}
	}
}
