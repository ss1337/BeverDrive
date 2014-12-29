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
using System.Reflection;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;

namespace BeverDrive.Modules
{
	public class AModule
	{
		/// <summary>
		/// Settings for this module
		/// </summary>
		public IEnumerable<KeyValuePair<string, string>> Settings { get; set; }

		/// <summary>
		/// A module's constructor is executed before the settings for that module are loaded, so
		/// never have any code that is dependent on settings in the constructor, instead have that
		/// in Init, since it is executed after the settings are loaded
		/// </summary>
		public virtual void Init() { }

		/// <summary>
		/// Command event handler
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnCommand(ModuleCommandEventArgs e) { }

		/// <summary>
		/// Shows all AGraphicControl
		/// </summary>
		public void ShowControls()
		{
			// Reflection to show all the controls, oh yes
			FieldInfo[] fieldInfos;
			Type t = this.GetType();
			fieldInfos = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (var fi in fieldInfos)
			{
				if (fi.FieldType.IsSubclassOf(typeof(AGraphicsControl)))
				{
					var field = t.GetField(fi.Name, BindingFlags.NonPublic | BindingFlags.Instance);
					var ctrl = (AGraphicsControl)field.GetValue(this);
					if (ctrl != null)
						BeverDriveContext.CurrentCoreGui.AddControl(ctrl);
				}
			}

			// Reflection to show backbutton
			foreach (object attrib in this.GetType().GetCustomAttributes(false))
			{
				if (attrib is BackButtonVisibleAttribute)
					if (((BackButtonVisibleAttribute)attrib).BackButtonVisible)
						BeverDriveContext.CurrentCoreGui.AddControl(BeverDriveContext.CurrentCoreGui.BackButton);
			}
		}

		public virtual void ProcessMessage(string message) { }

		/// <summary>
		/// Is executed every tick of the 1Hz timer
		/// </summary>
		public virtual void Update1Hz() { }

		/// <summary>
		/// Is executed every tick of the 50Hz timer
		/// </summary>
		public virtual void Update50Hz() { }
	}
}
