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
using System;
using System.Collections.Generic;
using System.Text;

namespace BeverDrive.Ibus.Messages.Predefined
{
	public class BordMonitor
	{
		// BMBT --> RAD : BM Button: Preset_1_pressed
		// F0 04 68 48 11 C5
		public static Message Button1()
		{
			return new Message(Devices.BordMonitor, Devices.Radio, new byte[] { 0x48, 0x11 });
		}

		public static Message Button2()
		{
			return new Message(Devices.BordMonitor, Devices.Radio, new byte[] { 0x48, 0x01 });
		}

		public static Message LeftKnobLeft(int speed)
		{
			return new Message(Devices.BordMonitor, Devices.Radio, new byte[] { 0x32, (byte)(speed * 16) });
		}

		public static Message LeftKnobRight(int speed)
		{
			return new Message(Devices.BordMonitor, Devices.Radio, new byte[] { 0x32, (byte)(speed * 16 + 1) });
		}

		public static Message LeftKnobPush()
		{
			return new Message(Devices.BordMonitor, Devices.Radio, new byte[] { 0x48, 0x06 });
		}

		public static Message RightKnobLeft(int speed)
		{
			return new Message(Devices.BordMonitor, Devices.Nav, new byte[] { 0x49, (byte)(speed) });
		}

		public static Message RightKnobRight(int speed)
		{
			return new Message(Devices.BordMonitor, Devices.Nav, new byte[] { 0x49, (byte)(0x80 + speed) });
		}

		public static Message RightKnobPush()
		{
			return new Message(Devices.BordMonitor, Devices.Nav, new byte[] { 0x48, 0x05 });
		}
	}
}
