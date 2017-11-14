//
// Copyright 2012-2017 Sebastian Sjödin
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
using BeverDrive.Ibus;
using BeverDrive.Modules;

namespace BeverDrive.Core
{
	public static class BeverDriveContext
	{
		private static bool fullScreen;

		public static BeverDrive.Gui.ICoreGui CurrentCoreGui { get; set; }
		public static System.Windows.Forms.Form CurrentMainForm { get; set; }
		public static bool RtsEnabled { get; set; }
		public static BeverDriveSettings Settings { get; set; }
		public static Module ActiveModule { get; set; }
		public static Module PlaybackModule { get; set; }
		public static List<Module> LoadedModules { get; set; }
		public static IIbusContext Ibus { get; set; }

		static BeverDriveContext()
		{
		}

		public static void Initialize()
		{
			LoadedModules = new List<Module>();
			Settings = new BeverDriveSettings();
			if (Settings.EnableIbus)
				Ibus = new BeverDrive.Ibus.IbusContext(Settings.ComPort);
			else
				Ibus = new BeverDrive.Ibus.IbusContext("");

			CurrentCoreGui = new BeverDrive.Gui.CoreGui();
		}

		public static bool FullScreen
		{
			get { return fullScreen; }
			set
			{
				fullScreen = value;

				if (fullScreen)
					BeverDriveContext.CurrentCoreGui.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Hide });
				else
					BeverDriveContext.CurrentCoreGui.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });
			}
		}

		public static void LoadModules()
		{
			if (BeverDriveContext.Settings == null)
				throw new Exception("Run Initialize() first");

			// Load modules according to the ones defined in Config.xml
			foreach (var modName in BeverDriveContext.Settings.Modules)
			{
				Type modType = Type.GetType(modName);

				if (modType == null)
				{
					// Try loading module...
					/*Assembly assembly = Assembly.LoadFrom("MyNice.dll");
					Type type = assembly.GetType("MyType");
					object instanceOfMyType = Activator.CreateInstance(type);*/
				}

				if (modType != null)
				{
					Module mod = (Module)Activator.CreateInstance(modType);
					mod.Settings = BeverDriveContext.Settings.ReadModuleSettings(modName);
					mod.Init();
					BeverDriveContext.LoadedModules.Add(mod);
				}
			}
		}

		/// <summary>
		/// Sets active module, if moduleName is empty, the first module loaded
		/// is set active
		/// </summary>
		/// <param name="moduleName"></param>
		public static void SetActiveModule(string moduleName)
		{
			bool playbackModule = false;
			Module module = null;

			if (string.IsNullOrEmpty(moduleName))
			{
				// Find first module, the main menu module
				module = BeverDriveContext.LoadedModules[0];
			}
			else
			{
				// Find module by name
				module = BeverDriveContext.LoadedModules.Find(x => { return x.GetType().Name.Equals(moduleName); });
			}

			if (module == null)
				return;

			// Figure out the new module's attributes
			// Check for attributes on that module
			foreach (object attrib in module.GetType().GetCustomAttributes(false))
			{
				if (attrib is PlaybackModuleAttribute)
					playbackModule = true;
			}

			if (BeverDriveContext.ActiveModule != null)
			{
				BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Hide });
				BeverDriveContext.ActiveModule.Visible = false;
			}

			BeverDriveContext.ActiveModule = module;
			BeverDriveContext.ActiveModule.Visible = true;
			BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });

			// If this module controls playback (ie should react to for example steering wheel buttons
			// set PlaybackModule here
			if (playbackModule)
				BeverDriveContext.PlaybackModule = module;

		}
	}
}
