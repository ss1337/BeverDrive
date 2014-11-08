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
	public class BordMonitor
	{
		// Unknown messages
		// C8 06 E7 23 01 00 20 2B - Phone -> ODC textbar (audio on)
		// C8 04 E7 2C 35 32 - Phone -> ODC textbar (audio off)

		// Messages for switching to komputar
		// 43 == Menuscreen
		// 43 44 20 31 2D
		// 43 44 20 32 2D
		// 43 44 20 33 2D
		// 43 44 20 34 2D
		// 43 44 20 35 2D
		// 43 44 20 36 2D
		// 43 44 20 20 2D
		// 43 44 43
		// 54 52 20 39 39
		// 4E 4F 20 44 49 53 43

		// Message for switching off komputar
		// 80 04 BF 11 00 2A - IKE -> LCM something something
		// F0 04 68 48 04 D0 - BM -> Radio (Tone pressed)
		// F0 04 68 48 06 D2 - BM -> Left knob pressed
		// XX XX XX 46 02
		// XX XX XX 46 01
		public const string Alternate1 = "F0 04 68 48 91 45";
		public const string Alternate2 = "F0 04 68 48 81 55";
		public const string Alternate3 = "F0 04 68 48 92 46";
		public const string Alternate4 = "F0 04 68 48 82 56";
		public const string Alternate5 = "F0 04 68 48 93 47";
		public const string Alternate6 = "F0 04 68 48 83 57";
		public const string AlternateLeft = "F0 04 3B 49 0X XX";
		public const string AlternateRight = "F0 04 3B 49 8X XX";

		public const string Button1 = "F0 04 68 48 11 C5";
		public const string Button2 = "F0 04 68 48 01 D5";
		public const string Button3 = "F0 04 68 48 12 C6";
		public const string Button4 = "F0 04 68 48 02 D6";
		public const string Button5 = "F0 04 68 48 13 C7";
		public const string Button6 = "F0 04 68 48 03 D7";

		public const string Eject = "F0 04 68 48 33 E7";
		public const string Menu = "F0 04 FF 48 34 77";
		public const string Mode = "F0 04 68 48 23 F7";
		public const string Tone = "F0 04 68 48 04 D0";

		public const string LeftKnobLeft = "F0 04 68 32 X0 XX";
		public const string LeftKnobRight = "F0 04 68 32 X1 XX";
		public const string LeftKnobPush = "F0 04 68 48 06 D2";
		public const string RightKnobLeft = "F0 04 3B 49 0X XX";
		public const string RightKnobRight = "F0 04 3B 49 8X XX";
		public const string RightKnobPush = "F0 04 3B 48 05 82";
	}
}
