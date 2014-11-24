﻿//
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
using System.Drawing;

namespace BeverDrive.Gui.Controls
{
	public class Label : AGraphicsControl
	{
		public ContentAlignment TextAlign { get; set; }

		public Label()
		{
		}

		public override void PaintToBuffer(Graphics graphic)
		{
			StringFormat cFormat = new StringFormat();
			Int32 lNum = (Int32)Math.Log((Double)this.TextAlign, 2);
			cFormat.LineAlignment = (StringAlignment)(lNum / 4);
			cFormat.Alignment = (StringAlignment)(lNum % 4);
			//var rectf = new RectangleF((PointF)this.Location, (SizeF)this.Size);
			graphic.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, cFormat);
		}
	}
}