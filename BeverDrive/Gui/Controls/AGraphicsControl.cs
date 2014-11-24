//
// Copyright 2014 Sebastian Sjödin
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
using System.Windows.Forms;
using System.Drawing;

namespace BeverDrive.Gui.Controls
{
	public abstract class AGraphicsControl : Control
	{
		public new RectangleF ClientRectangle
		{
			get { return new RectangleF((PointF)this.Location, (SizeF)this.Size); }
		}

		public AGraphicsControl()
		{
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Dont draw here
			//base.OnPaintBackground(pevent);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// Dont draw here
			//base.OnPaint(e);
		}

		public abstract void PaintToBuffer(Graphics graphic);
	}
}
