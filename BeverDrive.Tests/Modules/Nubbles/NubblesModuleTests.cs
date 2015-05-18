//
// Copyright 2014-2015 Sebastian Sjödin
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

namespace BeverDrive.Tests.Modules.Nubbles
{
	[TestFixture]
	public class NubblesModuleTests
	{
		public NubblesModuleTests()
		{
			BeverDriveContext.Initialize();
		}

		[Test]
		public void InGameMenu_works()
		{
			var module = new NubblesModule();
			BeverDriveContext.LoadedModules.Clear();
			BeverDriveContext.LoadedModules.Add(module);

			module.SelectedIndex = 0;
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectLeft });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(MenuState.PlayingMenu, module.MenuState);

			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(MenuState.Playing, module.MenuState);

			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectLeft });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(MenuState.MainMenu, module.MenuState);
		}

		[Test]
		public void MainMenu_works()
		{
			var module = new NubblesModule();
			BeverDriveContext.LoadedModules.Clear();
			BeverDriveContext.LoadedModules.Add(module);

			module.SelectedIndex = 0;
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectLeft });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });
			Assert.AreEqual(1, module.SelectedIndex);
			Assert.AreEqual(MenuState.Playing, module.MenuState);
		}
	}
}
