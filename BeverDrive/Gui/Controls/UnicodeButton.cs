//
// Copyright 2016 Sebastian Sjödin
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
using System.Text;
using BeverDrive.Gui.Styles;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Gui.Controls
{
	public class UnicodeButton : AGraphicsControl
	{
		public bool Selected { get; set; }
		public new int Width { get { return 50; } }
		public new int Height { get { return 38; } }

		private string character;
		//private System.Drawing.Font font;

		public UnicodeButton (string unicodeChar)
		{
			this.character = unicodeChar;
		}

		public override void PaintToBuffer(System.Drawing.Graphics graphic)
		{
			var rectangle = new System.Drawing.Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
			var brush = this.Selected ? Brushes.SelectedBrush : Brushes.ForeBrush;

			graphic.FillHollowRectangle(brush, rectangle, 2);
			graphic.DrawString(this.character, this.Font, brush, rectangle);
		}
	}
}

