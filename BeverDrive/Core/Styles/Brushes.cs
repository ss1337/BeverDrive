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
using System.Drawing;
using BeverDrive.Core;

namespace BeverDrive.Core.Styles
{
	public struct Brushes
	{
		private static Brush backBrush;
		private static Brush foreBrush;
		private static Brush selectedBrush;

		public static Brush BackBrush { get { if (backBrush == null) { backBrush = new SolidBrush(Colors.BackColor); } return backBrush; } }
		public static Brush ForeBrush { get { if (foreBrush == null) { foreBrush = new SolidBrush(Colors.ForeColor); } return foreBrush; } }
		public static Brush SelectedBrush { get { if (selectedBrush == null) { selectedBrush = new SolidBrush(Colors.SelectedColor); } return selectedBrush; } }
	}
}
