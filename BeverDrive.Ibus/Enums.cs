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

namespace BeverDrive.Ibus
{
	public enum Devices : byte
	{
		CdChanger = 0x18,
		Nav = 0x3B,
		MenuScreen = 0x48,
		SteeringWheel = 0x50,
		Radio = 0x68,
		Amplifier = 0x6A,
		IKE = 0x80,
		LCM = 0xBF,
		Mid = 0xC0,
		ObcTextBar = 0xE7,
		LightWipers = 0xED,
		BordMonitor = 0xF0,
		Broadcast = 0xFF
	}

	public enum Mode : int
	{
		Radio = 0,
		CdChanger = 1,
		Tape = 2,
	}
}
