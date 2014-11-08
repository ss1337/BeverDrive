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
using System.Drawing;
using System.Linq;
using System.Text;

namespace BeverDrive.Gui.Core.Styles
{
	public struct Colors
	{
		public static Color BackColor { get { return BeverDriveContext.Settings.BackColor; } }
		public static Color ForeColor { get { return BeverDriveContext.Settings.ForeColor; } }
		public static Color SelectedColor { get { return BeverDriveContext.Settings.SelectedColor; } }
		public static Color ClockBackgroundColor { get { return BeverDriveContext.Settings.ClockBackgroundColor; } }
		public static Color ClockForegroundColor { get { return BeverDriveContext.Settings.ClockForegroundColor; } }
	}
}
