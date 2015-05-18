﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Modules;
using NUnit.Framework;

namespace BeverDrive.Tests.Modules
{
	[TestFixture]
	public class MainMenuSimpleTests
	{
		public MainMenuSimpleTests()
		{
			BeverDriveContext.Initialize();
		}

		[Test]
		public void Creates_correct_number_of_options()
		{
			BeverDriveContext.LoadedModules.Clear();

			var module = new MainMenuSimple();
			module.Settings = BeverDriveContext.Settings.ReadModuleSettings(module.GetType().FullName);
			module.Init();
			BeverDriveContext.LoadedModules.Add(module);

			var t = module.GetType();
			Console.WriteLine(string.Format("{0}", t.Name));
			var p = t.GetField("buttons", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			var buttons = (List<Label>)p.GetValue(module);

			Assert.AreEqual(4, buttons.Count);
		}

		[Test]
		public void Selection_works()
		{
			BeverDriveContext.LoadedModules.Clear();

			var module = new MainMenuSimple();
			module.Settings = BeverDriveContext.Settings.ReadModuleSettings(module.GetType().FullName);
			module.Init();
			BeverDriveContext.LoadedModules.Add(module);

			module.SelectedIndex = 0;
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectLeft });
			Assert.AreEqual(1, module.SelectedIndex);
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectRight });
			module.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectRight });
			Assert.AreEqual(3, module.SelectedIndex);
		}
	}
}
