//
// Copyright 2015 Sebastian Sjödin
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

namespace BeverDrive.Ibus.Messages.Predefined
{
	public class LightWipers
	{
		/*
		 * Q: Hello, can someone tell me the I-Bus code (TV-module -to-> BM widescreen) for changing 
		 * the screenmode (4:3 –> 16:9 –> letterbox..) while the TV-Mode is on.
		 *
		 * ED 05 F0 4F 11 32 74
		 * TV –> BMBT: RGB control: LCD_on TV 50Hz Byte2_Bit4 Byte2_Bit5
		 *
		 * Its either data Byte2_Bit4 or Byte2_Bit5 to set widescreen, I haven’t determined which yet.
		 * 
		 * 
		 * Steuerung des 16:9 Widescreen Bordmonitor
		 *
		 * Nachrichten zur Änderung des Anzeigemodus im Bordmonitor: Adresse F0 Hex (Sender ED Hex)
		 *
		 * <ED 05 F0> 4F 11 <Option> <XOR>
		 *
		 * Optionen
		 *
		 * 31 Setze 16:9/Zoom 60Hz (Navianzeige)
		 * 32 Setze 16:9/Zoom 50Hz (TV Mode)

		 * 11 Setze 16:9 60Hz (Navianzeige)
		 * 12 Setze 16:9 50Hz (TV Mode)

		 * 01 Setze 4:3 60Hz (Navianzeige)
		 * 02 Setze 4:3 50Hz (TV Mode)
		*/

		public enum TvMode : byte
		{
			Mode_169Zoom_60Hz = 0x31,	// Nav
			Mode_169Zoom_50Hz = 0x32,	// TV
			Mode_169_60Hz = 0x11,		// Nav
			Mode_169_50Hz = 0x12,		// TV
			Mode_43_60Hz = 0x01,		// Nav
			Mode_43_50Hz = 0x02			// TV
		}

		public static Message SetTvMode(TvMode mode)
		{
			byte[] data = new byte [] { 0x4F, 0x11, (byte)mode};
			return new Message(Devices.LightWipers, Devices.BordMonitor, data);
		}
	}
}
