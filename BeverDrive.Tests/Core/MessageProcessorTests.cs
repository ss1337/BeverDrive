//
// Copyright 2015-2016 Sebastian Sjödin
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
using BeverDrive.Core;
using Moq;
using NUnit.Framework;

namespace BeverDrive.Tests.Core
{
	[TestFixture]
	public class MessageProcessorTests
	{
		private const string cdxxx = "68 17 3B 23 62 30 20 07 20 43 44 20 34 2D 36 36 20 50 34 20 20 20 20 20 30";
		private const string fm_p4 = "68 17 3B 23 62 30 20 07 20 46 4D 44 20 08 53 52 20 50 34 20 20 20 20 20 30";
		private const string scan = "68 17 3B 23 62 30 20 20 07 20 20 20 20 20 08 20 20 53 43 41 4E 20 20 20 25";
		private const string tape = "68 17 3B 23 62 30 20 20 07 20 20 20 20 20 08 20 20 54 41 50 45 20 20 20 25";

		public MessageProcessorTests()
		{
		}

		[Test]
		public void Doesnt_crash_on_certain_messages()
		{
			var rtsEnable = false;
			var ibusMock = new Mock<BeverDrive.Ibus.IIbusContext>();
			ibusMock.SetupGet(p => p.RtsEnable).Returns(rtsEnable);
			ibusMock.SetupSet(p => p.RtsEnable = It.IsAny<bool>()).Callback<bool>(value => rtsEnable = value);
			BeverDriveContext.Initialize();
			BeverDriveContext.Ibus = ibusMock.Object;

			var module = new BeverDrive.Modules.MainMenuSimple();
			module.Settings = BeverDriveContext.Settings.ReadModuleSettings(module.GetType().FullName);
			module.Init();
			BeverDriveContext.LoadedModules.Add(module);
			BeverDriveContext.ActiveModule = module;

			// SCAN
			BeverDrive.Core.MessageProcessor.Process(scan);

			// FMD SR P4
			BeverDrive.Core.MessageProcessor.Process(fm_p4);

			// Broken CD
			BeverDrive.Core.MessageProcessor.Process("68 17 3B 23 62 30 20 20 07 20 20 20 20 20 43 44 20");
		}

		[Test]
		public void Sends_video_mode_when_switching_video()
		{
			List<string> sent = new List<string>();
			var rtsEnable = false;
			var ibusMock = new Mock<BeverDrive.Ibus.IIbusContext>();
			ibusMock.SetupGet(p => p.RtsEnable).Returns(rtsEnable);
			ibusMock.SetupSet(p => p.RtsEnable = It.IsAny<bool>()).Callback<bool>(value =>
			{
				rtsEnable = value;
				ibusMock.SetupGet(p => p.RtsEnable).Returns(rtsEnable);
			});

			BeverDriveContext.Initialize();
			BeverDriveContext.Ibus = ibusMock.Object;

			var module = new BeverDrive.Modules.MainMenuSimple();
			module.Settings = BeverDriveContext.Settings.ReadModuleSettings(module.GetType().FullName);
			module.Init();
			BeverDriveContext.LoadedModules.Add(module);
			BeverDriveContext.ActiveModule = module;

			Assert.False(rtsEnable);
			BeverDrive.Core.MessageProcessor.Process(cdxxx);
			ibusMock.Verify(x => x.Send(It.Is<string>(d => d == "ED 05 F0 4F 11 11 57")), Times.AtLeastOnce());
		}

		[Test]
		public void Sets_Rts_correctly()
		{
			var rtsEnable = false;
			var ibusMock = new Mock<BeverDrive.Ibus.IIbusContext>();
			ibusMock.SetupGet(p => p.RtsEnable).Returns(rtsEnable);
			ibusMock.SetupSet(p => p.RtsEnable = It.IsAny<bool>()).Callback<bool>( value => {
				rtsEnable = value;
				ibusMock.SetupGet(p => p.RtsEnable).Returns(rtsEnable);
			});
			BeverDriveContext.Initialize();
			BeverDriveContext.Ibus = ibusMock.Object;

			var module = new BeverDrive.Modules.MainMenuSimple();
			module.Settings = BeverDriveContext.Settings.ReadModuleSettings(module.GetType().FullName);
			module.Init();
			BeverDriveContext.LoadedModules.Add(module);
			BeverDriveContext.ActiveModule = module;

			Assert.False(rtsEnable);
			BeverDrive.Core.MessageProcessor.Process(cdxxx);
			Assert.True(rtsEnable);
			BeverDrive.Core.MessageProcessor.Process(fm_p4);
			Assert.False(rtsEnable);
			BeverDrive.Core.MessageProcessor.Process(scan);
			Assert.True(rtsEnable);
			BeverDrive.Core.MessageProcessor.Process(tape);
			Assert.False(rtsEnable);
		}
	}
}
