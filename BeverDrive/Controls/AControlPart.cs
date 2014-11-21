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
using System.Drawing;
using System.Windows.Forms;

namespace BeverDrive.Controls
{
	public class AControlPart
	{
		public virtual Color BackColor { get; set; }
		public virtual Font Font { get; set; }
		public virtual Color ForeColor { get; set; }
		public virtual int Height { get; set; }
		public virtual Point Location { get; set; }
		public virtual Color SelectedColor { get; set; }
		public virtual int Width { get; set; }

		public Control Parent { get; set; }
		public void OnPaintToBuffer(Graphics graphic) { }
	}
}
