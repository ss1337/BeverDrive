//
// Copyright 2014 Sebastian Sjödin
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
using BeverDrive.Modules;
using NUnit.Framework;

namespace BeverDrive.Tests.Modules
{
	[TestFixture]
	public class IbusDebugTests
	{
		public IbusDebugTests()
		{
			BeverDriveContext.Initialize();
		}

		[Test]
		public void Logging_works()
		{
			var msg = "bmw";

			BeverDriveContext.LoadedModules.Clear();
			BeverDriveContext.LoadedModules.Add(new CoreGui());
			var module = new IbusDebug();
			BeverDriveContext.LoadedModules.Add(module);

			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });
			Assert.AreEqual(false, module.Logging);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(true, module.Logging);

			// Logging on should add msg to module.log
			Assert.AreEqual(string.Empty, module.log.Text);
			module.ProcessMessage(msg);
			Assert.AreEqual(msg + Environment.NewLine, module.log.Text);

			// Logging off should not add to module.log
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			module.ProcessMessage(msg);
			Assert.AreEqual(msg + Environment.NewLine, module.log.Text);
		}

		[Test]
		public void Logging_onoff_works()
		{
			BeverDriveContext.LoadedModules.Clear();
			BeverDriveContext.LoadedModules.Add(new CoreGui());
			var module = new IbusDebug();
			BeverDriveContext.LoadedModules.Add(module);

			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });
			Assert.AreEqual(false, module.Logging);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(true, module.Logging);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(false, module.Logging);
		}

		[Test]
		public void Menu_works()
		{
			BeverDriveContext.LoadedModules.Clear();
			BeverDriveContext.LoadedModules.Add(new CoreGui());
			var module = new IbusDebug();
			BeverDriveContext.LoadedModules.Add(module);

			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });
			Assert.AreEqual(0, module.SelectedIndex);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectNext });
			Assert.AreEqual(1, module.SelectedIndex);
			
			// SelectedIndex should never exceed 1
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectNext });
			Assert.AreEqual(1, module.SelectedIndex);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectPrevious });
			Assert.AreEqual(0, module.SelectedIndex);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectPrevious });
			Assert.AreEqual(-1, module.SelectedIndex);

			// SelectedIndex should never be less than -1
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectPrevious });
			Assert.AreEqual(-1, module.SelectedIndex);
		}
	}
}
