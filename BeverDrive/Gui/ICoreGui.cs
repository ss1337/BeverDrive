//
// Copyright 2012-2016 Sebastian Sjödin
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Gui.Controls;
using BeverDrive.Modules;

namespace BeverDrive.Gui
{
	public interface ICoreGui
	{
		MetroidButton BackButton { get; set; }
		Size ModuleAreaSize { get; }

		Panel BaseContainer { get; set; }							// Adjust this to fit everything to screen
		ClockPanel ClockContainer { get; set; }						// Contains lower portion with date/time and maybe text
		GraphicsPanel ModuleContainer { get; set; }					// All module controls goes into here


		void AddControl(AGraphicsControl ctrl);
		void ClearBaseContainer();
		void ClearModuleContainer();
		void Invalidate();
		void OnCommand(ModuleCommandEventArgs e);
		void Update1Hz();
		void Update50Hz();
	}
}
