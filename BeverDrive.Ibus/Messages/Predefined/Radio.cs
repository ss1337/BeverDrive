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

	public class Radio
	{
		public static Message Radio2Amp_CenterBalance()
		{
		// Balans Mitten: 68 04 6A 36 40 70
			return new Message(Devices.Radio, Devices.Amplifier, new byte[2] { 0x36, 0x40 });
		}

		public static Message Radio2Amp_CenterBass()
		{
			// Bas Mitten: 68 04 6A 36 60 50
			return new Message(Devices.Radio, Devices.Amplifier, new byte[2] { 0x36, 0x60 });
		}

		public static Message Radio2Amp_CenterFader()
		{
			// Fädning Mitten: 68 04 6A 36 80 B0
			return new Message(Devices.Radio, Devices.Amplifier, new byte[2] { 0x36, 0x80 });
		}

		public static Message Radio2Amp_CenterTreble()
		{
			// Diskant Mitten: 68 04 6A 36 C0 F0
			return new Message(Devices.Radio, Devices.Amplifier, new byte[2] { 0x36, 0xC0 });
		}

		public static Message Radio2Cd_ChangeCd(Int32 cdno)
		{
			var data = new byte[3] { 0x38, 0x06, 0x00 };
			data[2] = (byte)cdno;
			return new Message(Devices.Radio, Devices.CdChanger, data);
		}

		public static Message Radio2Cd_Play()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x03, 0x00 } );
		}

		public static Message Radio2Cd_Poll()
		{
			// Cd-changer poll: 68 03 18 01 72 (104 3 24 1 114)
			return new Message(Devices.Radio, Devices.CdChanger, new byte[1] { 0x01 });
		}

		public static Message Radio2Cd_ReqTrack()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x00, 0x00 });
		}

		public static Message Radio2Cd_ScanBackward()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x04, 0x01 });
		}

		public static Message Radio2Cd_ScanForward()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x04, 0x01 });
		}

		public static Message Radio2Cd_SetRandom(bool randomOn)
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x04, 0x01 });
		}

		public static Message Radio2Cd_Stop()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x01, 0x00 });
		}


		public static Message Radio2Cd_TrackNext()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x0A, 0x00 });
		}

		public static Message Radio2Cd_TrackPrev()
		{
			return new Message(Devices.Radio, Devices.CdChanger, new byte[3] { 0x38, 0x0A, 0x00 });
		}

		//68 XX " + DisplayTarget + " 23 62 30 ?" & MIDmsg & "?"
		public static Message Radio2Broad_TextMessage(String text1)
		{
			String text = "";

			if (text1.Length > 15)
				text = text1.Substring(0, 15);
			else
				text = text1;

			var data = new byte[text.Length + 3];
			data[0] = 0x23;
			data[1] = 0x62;
			data[2] = 0x30;

			for (Int32 i = 0; i < text.Length; i++)
				data[3 + i] = (byte)text[i];

			return new Message(Devices.Radio, Devices.Broadcast, data);
		}
	}
}
