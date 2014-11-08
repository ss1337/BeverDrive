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
using System.Linq;
using System.Text;

namespace BeverDrive.Ibus.Messages
{
	// Compare received messages to these
	public class Other
	{
		public const string AnteKrök = "C0 06 68 31 43 02 0A D4";
		public const string Cdc_Announce = "18 04 FF 02 01 E0";
		public const string Cdc_PollCd = "68 03 18 01 72";
		public const string Cdc_PollResponse = "18 04 FF 02 00 E1";
		public const string Wheel_PrevTrack = "50 04 68 3B 08 0F";
		public const string Wheel_NextTrack = "50 04 68 3B 01 06";
		public const string Wheel_VolumeUp = "50 04 68 32 11 1F";
		public const string Wheel_VolumeDown = "50 04 68 32 10 1E";
	}
}
