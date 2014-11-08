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

namespace BeverDrive.Ibus.Constants
{
	public class Messages
	{
		private static Dictionary<string, string> messageCollection;

		public static Dictionary<string, string> MessageCollection
		{
			get
			{
				if (messageCollection == null)
				{
					messageCollection = new Dictionary<string, string>();
					messageCollection.Add("Bordmonitor_Alternate1", "F0 04 68 48 91 45");
					messageCollection.Add("Bordmonitor_Alternate2", "F0 04 68 48 81 55");
					messageCollection.Add("Bordmonitor_Alternate3", "F0 04 68 48 92 46");
					messageCollection.Add("Bordmonitor_Alternate4", "F0 04 68 48 82 56");
					messageCollection.Add("Bordmonitor_Alternate5", "F0 04 68 48 93 47");
					messageCollection.Add("Bordmonitor_Alternate6", "F0 04 68 48 83 57");
					messageCollection.Add("Bordmonitor_AlternateLeft", "F0 04 3B 49 0X XX");
					messageCollection.Add("Bordmonitor_AlternateRight", "F0 04 3B 49 8X XX");
					messageCollection.Add("Bordmonitor_Button1", "F0 04 68 48 11 C5");
					messageCollection.Add("Bordmonitor_Button2", "F0 04 68 48 01 D5");
					messageCollection.Add("Bordmonitor_Button3", "F0 04 68 48 12 C6");
					messageCollection.Add("Bordmonitor_Button4", "F0 04 68 48 02 D6");
					messageCollection.Add("Bordmonitor_Button5", "F0 04 68 48 13 C7");
					messageCollection.Add("Bordmonitor_Button6", "F0 04 68 48 03 D7");

					messageCollection.Add("Bordmonitor_Eject", "F0 04 68 48 33 E7");
					messageCollection.Add("Bordmonitor_Menu", "F0 04 FF 48 34 77");
					messageCollection.Add("Bordmonitor_Mode", "F0 04 68 48 23 F7");

					messageCollection.Add("Bordmonitor_LeftKnobLeft", "F0 04 68 32 X0 XX");
					messageCollection.Add("Bordmonitor_LeftKnobRight", "F0 04 68 32 X1 XX");
					messageCollection.Add("Bordmonitor_LeftKnobPush", "F0 04 68 48 06 D2");
					messageCollection.Add("Bordmonitor_RightKnobLeft", "F0 04 68 49 0X XX");
					messageCollection.Add("Bordmonitor_RightKnobRight", "F0 04 68 49 8X XX");
					messageCollection.Add("Bordmonitor_RightKnobPush", "F0 04 3B 48 05 82");

					messageCollection.Add("Wheel_PrevTrack", "50 04 68 3B 08 0F");
					messageCollection.Add("Wheel_NextTrack", "50 04 68 3B 01 06");
					messageCollection.Add("Wheel_VolumeUp", "50 04 68 32 11 1F");
					messageCollection.Add("Wheel_VolumeDown", "50 04 68 32 10 1E");

					messageCollection.Add("CdChanger_Announce", "18 04 FF 02 01 E0");
					messageCollection.Add("CdChanger_PollResponse", "18 04 FF 02 00 E1");

				}

				return messageCollection;
			}
		}
	}
}
