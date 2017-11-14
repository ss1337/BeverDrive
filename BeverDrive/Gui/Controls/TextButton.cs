//
// Copyright 2014-2017 Sebastian Sjödin
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
using System.Text;
using BeverDrive.Gui.Styles;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Gui.Controls
{
	public class TextButton : AGraphicsControl
	{
		private System.Drawing.StringFormat stringFormat;

		public TextButton()
		{
			this.stringFormat = new System.Drawing.StringFormat();
			this.stringFormat.Alignment = System.Drawing.StringAlignment.Center;
			this.stringFormat.LineAlignment = System.Drawing.StringAlignment.Center;
		}

		public override void PaintToBuffer(System.Drawing.Graphics graphic)
		{
			var rectangle = new System.Drawing.Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
			var brush = this.Selected ? Brushes.SelectedBrush : Brushes.ForeBrush;

			graphic.FillHollowRectangle(brush, rectangle, 4);
			graphic.DrawString(this.Text, this.Font, brush, rectangle, this.stringFormat);
		}
	}
}
