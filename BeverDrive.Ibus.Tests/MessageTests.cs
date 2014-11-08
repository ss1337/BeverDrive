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
	public class MessageTests
	{
		[Test]
		public void Creating_works()
		{
			var msg = new Message(Devices.CdChanger, Devices.Broadcast, new byte[] { 0x02, 0x01 });
			Assert.AreEqual((byte)0xE0, msg.Checksum);
			Assert.AreEqual(Devices.CdChanger, msg.Source);
			Assert.AreEqual(Devices.Broadcast, msg.Destination);
			Assert.AreEqual(4, msg.Length);
		}

		[Test]
		public void Creating_message_from_byte_array_works()
		{
			var msg = new Message(new byte[] { 0x18, 0x04, 0xFF, 0x02, 0x01, 0xE0 });
			Assert.AreEqual((byte)0xE0, msg.Checksum);
			Assert.AreEqual(Devices.CdChanger, msg.Source);
			Assert.AreEqual(Devices.Broadcast, msg.Destination);
			Assert.AreEqual(4, msg.Length);
		}

		[Test]
		public void Equals_works_correctly_on_strings()
		{
			var msg = BeverDrive.Ibus.Messages.Predefined.CdChanger.Cd2Broad_Announce();
			Assert.True(msg.Equals("18 04 FF 02 01 E0"));
			Assert.False(msg.Equals("18 04 FF 02 01 E1"));
		}

		[Test]
		public void Equals_works_on_strings_with_wildcards()
		{
			var msg = BeverDrive.Ibus.Messages.Predefined.BordMonitor.LeftKnobLeft(3);
			Assert.True(msg.Equals(BeverDrive.Ibus.Messages.BordMonitor.LeftKnobLeft));
		}

		[Test]
		public void ToString_works_correctly()
		{
			var msg = new Message(new byte[] { 0x18, 0x04, 0xFF, 0x02, 0x01, 0xE0 });
			Assert.True(msg.ToString() == "18 04 FF 02 01 E0");
			Assert.False(msg.ToString() == "18 04 FF 02 01 E1");
		}

		[Test]
		public void Contains_works_correctly()
		{
			var msg = new Message(new byte[] { 0x18, 0x04, 0xFF, 0x02, 0x01, 0xE0 });
			Assert.True(msg.Contains("FF 02 01"));
			Assert.False(msg.Contains("FF 02 03"));
			Assert.True(msg.Contains("FF 0X 01"));
			Assert.False(msg.Contains("FF X0 01"));
		}

		[Test]
		public void StartsWith_works_correctly()
		{
			var msg = new Message(new byte[] { 0x18, 0x04, 0xFF, 0x02, 0x01, 0xE0 });
			Assert.True(msg.StartsWith("18 04 FF"));
			Assert.False(msg.StartsWith("18 04 FE"));
			Assert.True(msg.StartsWith("18 0X FF"));
			Assert.False(msg.StartsWith("18 X0 FE"));
		}
	}
}
