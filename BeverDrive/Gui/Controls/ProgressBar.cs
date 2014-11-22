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
using BeverDrive.Gui.Styles;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Gui.Controls
{
	public class ProgressBar : AGraphicsControl
	{
		public int BorderWidth { get; set; }
		public int Maximum { get; set; }
		public int Minimum { get; set; }
		public int Step { get; set; }
		public int Value { get; set; }

		public ProgressBar()
		{
			this.BorderWidth = 2;
		}

		public void PerformStep()
		{
			this.Value += this.Step;
		}

		public void Reset()
		{
			this.Value = 0;
		}

		public override void PaintToBuffer(System.Drawing.Graphics graphic)
		{
			var oRect = new System.Drawing.Rectangle(this.Location, this.Size);
			var iRect = new System.Drawing.Rectangle(this.Location, this.Size);
			iRect.Inflate(new System.Drawing.Size(-this.BorderWidth, -this.BorderWidth));
			int valueWidth = (int)(iRect.Width * ((double)this.Value / this.Maximum));

			// Draw core progress bar
			// Draw borders
			graphic.FillHollowRectangle(Brushes.ForeBrush, oRect, this.BorderWidth);
			graphic.FillRectangle(Brushes.SelectedBrush, iRect.Left, iRect.Top, valueWidth, iRect.Height);
		}
	}
}
