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
using BeverDrive.Ibus.Messages.Predefined;
using NUnit.Framework;

namespace BeverDrive.Ibus.Tests
{
	[TestFixture]
	public class PredefinedMessagesTests
	{
		[Test]
		public void CdChanger_TrackStart_with_track_100_works()
		{
			var msg = CdChanger.Cd2Radio_TrackStart(6, 101);
			Assert.AreEqual("18 0A 68 39 02 09 00 3F 00 06 01 70", msg.GetMessageAsString());
		}

		[Test]
		public void BordMonitor_LedsUpperRight_works()
		{
			var msg1 = BordMonitor.LedsUpperRight(0, 0, 0);
			var msg2 = BordMonitor.LedsUpperRight(1, 0, 1);
			var msg3 = BordMonitor.LedsUpperRight(2, 1, 2);
			Assert.AreEqual("C8 04 E7 2B 00 00", msg1.GetMessageAsString());
			Assert.AreEqual("C8 04 E7 2B 05 05", msg2.GetMessageAsString());
			Assert.AreEqual("C8 04 E7 2B 1F 1F", msg3.GetMessageAsString());
		}

		[Test]
		public void LightWipers_SetTvMode_works()
		{
			var msg1 = LightWipers.SetTvMode(LightWipers.TvMode.Mode_169_50Hz);
			var msg2 = LightWipers.SetTvMode(LightWipers.TvMode.Mode_169Zoom_50Hz);
			var msg3 = LightWipers.SetTvMode(LightWipers.TvMode.Mode_43_50Hz);
			Assert.AreEqual("ED 05 F0 4F 11 12 54", msg1.GetMessageAsString());
			Assert.AreEqual("ED 05 F0 4F 11 32 74", msg2.GetMessageAsString());
			Assert.AreEqual("ED 05 F0 4F 11 02 44", msg3.GetMessageAsString());
		}

		[Test]
		public void ObcTextbar_SetUrgentText_works()
		{
			var msg = ObcTextbar.SetUrgentText("  CHECK CONTROL OK  ");
			Assert.AreEqual("30 19 80 1A 35 00 20 20 43 48 45 43 4B 20 43 4F 4E 54 52 4F 4C 20 4F 4B 20 20 83", msg.GetMessageAsString());
		}
	}
}
