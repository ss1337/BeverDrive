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
using System.Windows.Forms;
using BeverDrive.Gui.Core.Styles;
using BeverDrive.Gui.Extensions;

namespace BeverDrive.Gui.Controls
{
	public class MenuOption : UserControl
	{
		public override Font Font { get { return base.Font;	} }
		protected override Size DefaultSize { get { return new System.Drawing.Size(500, 60); } }
		public bool Selected { get; set; }

		public MenuOption()
		{
			base.Font = Fonts.GuiFont36;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// Decreate width and height in order for rectangle to draw correctly
			var rect = e.ClipRectangle;
			var innerRect = rect.Shrink(3);

			Graphics g = e.Graphics;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			if (this.Selected)
			{
				g.FillRectangle(BeverDrive.Gui.Core.Styles.Brushes.SelectedBrush, rect);
				g.FillRectangle(BeverDrive.Gui.Core.Styles.Brushes.BackBrush, innerRect);
			}
			else
			{
				g.Clear(BeverDrive.Gui.Core.Styles.Colors.BackColor);
			}

			g.DrawString(this.Text, this.Font, BeverDrive.Gui.Core.Styles.Brushes.ForeBrush, rect.Shrink(3));
			g.Dispose();
		}
	}
}
