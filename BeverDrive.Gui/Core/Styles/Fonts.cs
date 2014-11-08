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
using System.Text;

namespace BeverDrive.Gui.Core.Styles
{
	public struct Fonts
	{
		private static Font guiFont14 = new Font("Arial", 14f, FontStyle.Bold);
		private static Font guiFont18 = new Font("Arial", 18f, FontStyle.Bold);
		private static Font guiFont24 = new Font("Arial", 24f, FontStyle.Bold);
		private static Font guiFont26 = new Font("Arial", 26f, FontStyle.Bold);
		private static Font guiFont28 = new Font("Arial", 28f, FontStyle.Bold);
		private static Font guiFont32 = new Font("Arial", 32f, FontStyle.Bold);
		private static Font guiFont36 = new Font("Arial", 36f, FontStyle.Bold);

		public static Font GuiFont14 { get { return guiFont14; } }
		public static Font GuiFont18 { get { return guiFont18; } }
		public static Font GuiFont24 { get { return guiFont24; } }
		public static Font GuiFont26 { get { return guiFont26; } }
		public static Font GuiFont28 { get { return guiFont28; } }
		public static Font GuiFont32 { get { return guiFont32; } }
		public static Font GuiFont36 { get { return guiFont36; } }
	}
}
