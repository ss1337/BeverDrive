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
using System.Windows.Forms;
using System.Drawing;
using BeverDrive.Gui.Core.Styles;

namespace BeverDrive.Gui.Controls
{
	public class APaintControl : UserControl
	{
		public override Color BackColor { get { return Colors.BackColor; } }
		public override Color ForeColor { get { return Colors.ForeColor; } }

		protected virtual void OnPaintToBuffer(Graphics graphic, Rectangle clientRectangle) { }
	}
}
