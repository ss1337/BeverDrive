//
// Copyright 2012-2014 Sebastian Sjödin
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
using NUnit.Framework;

namespace BeverDrive.Ibus.Tests
{
	[TestFixture]
	public class PredefinedMessagesTests
	{
		[Test]
		public void TrackStart_with_track_100_works()
		{
			var msg = BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Radio_TrackStart(6, 101);
			Assert.AreEqual("18 0A 68 39 02 09 00 3F 00 06 01 70", msg.GetMessageAsString());
		}
	}
}
