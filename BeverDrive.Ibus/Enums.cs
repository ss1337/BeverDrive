//
// Copyright 2011-2015 Sebastian Sjödin
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
		BroadCast00 = 0x00,
		SHD = 0x08,
		CdChanger = 0x18,

		HKM = 0x24,
		FUM = 0x28,

		CCM = 0x30,
		Nav = 0x3B,				// Navigation, video module
		DIA = 0x3F,

		FBZV = 0x40,
		MenuScreen = 0x43,
		EWS = 0x44,
		CID = 0x46,
		FMBT = 0x47,

		SteeringWheel = 0x50,	// Multi Functional Steering Wheel Buttons
		MML = 0x51,
		IHK = 0x5B,

		PDC = 0x60,				// Park Distance Control
		CDCD = 0x66,
		Radio = 0x68,
		Amplifier = 0x6A,

		RDC = 0x70,
		SM = 0x72,
		SDRS = 0x73,
		// CDCD = 0x76,
		NAVE = 0x7F,

		IKE = 0x80,				// Instrument Kombi Electronics

		MMR = 0x9B,
		CVM = 0x9C,

		FMID = 0xA0,
		ACM = 0xA4,
		FHK = 0xA7,
		NAVC = 0xA8,
		EHC = 0xAC,

		SES = 0xB0,
		TvModule = 0xBB,		// TV, NAVJ
		LCM = 0xBF,				// Light Control Module

		Mid = 0xC0,				// Multi Information Display
		Telephone = 0xC8,

		LKM = 0xD0,				// Navigation Location, LKM
		SMAD = 0xDA,

		IRIS = 0xE0,			// IRIS
		ObcTextBar = 0xE7,		// OBC Textbar, ANZV
		ISP = 0xE8,
		LightWipers = 0xED,		// Lights, wipers, seat memory, TV

		BordMonitor = 0xF0,		// Bordmonitor buttons
		CSU = 0xF5,
		Broadcast = 0xFF,
	}

	public enum Mode : int
	{
		Radio = 0,
		CdChanger = 1,
		Tape = 2,
	}
}
