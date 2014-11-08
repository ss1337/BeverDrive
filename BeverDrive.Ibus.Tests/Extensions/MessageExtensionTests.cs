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
using BeverDrive.Ibus.Extensions;
using NUnit.Framework;

namespace BeverDrive.Ibus.Tests.Extensions
{
	[TestFixture]
	public class MessageExtensionTests
	{
		[Test]
		public void ToHexBytes_works_correctly()
		{
			byte[] expected = new byte[] { 0x4E, 0x4F, 0x20, 0x44, 0x49, 0x53, 0x43 };
			string msg = "4E 4F 20 44 49 53 43";
			byte[] ba = msg.ToHexBytes();

			Assert.AreEqual(expected.Length, ba.Length);

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], ba[i]);
			}
		}

		[Test]
		public void IsMessage_works_correctly()
		{
			string msg = "4E 4X 20 44 49 53 43";
			Assert.True(msg.IsMessage("4E 4E 20 44 49 53 43"));
			Assert.True(msg.IsMessage("4E 4F 20 44 49 53 43"));
			Assert.False(msg.IsMessage("4E 4E 20 44 49 53 44"));
			
			msg = "68 17 3B 23 62 30 20 07 20 20 20 20 20 08 56 49 41 50 4C 41 59 20 20 20 40";
			Assert.True(msg.IsMessage("68 XX 3B 23 62 30 20 07", true));
		}
	}
}
