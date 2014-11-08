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
using BeverDrive.Gui.Core;

namespace BeverDrive.Gui.Modules
{
	public abstract class AOverlayedModule : IModule
	{
		public MainForm ParentForm { get { return BeverDriveContext.CurrentMainForm; } }
		public bool Visible { get; set; }

		public abstract void Paint(System.Drawing.Graphics graphic);

		public virtual void Init() { }

		public virtual void OnCommand(ModuleCommandEventArgs e) { }

		public virtual void Update1Hz() { }

	}
}
