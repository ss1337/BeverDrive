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
/*using BeverDrive.Core;
using BeverDrive.Core.Extensions;
using BeverDrive.Gui.Styles;*/

namespace BeverDrive.Gui.Controls
{
	/// <summary>
	/// Control for displaying an overlayed menu
	/// </summary>
	public class OverlayedMenu : AGraphicsControl
	{
		public List<string> Items { get; set; }

		public Color SelectedColor { get; set; }

		public int SelectedIndex { get; set; }

		public ContentAlignment TextAlign { get; set; }

		public OverlayedMenu()
		{
			this.Items = new List<string>();
		}

		public override void PaintToBuffer(Graphics graphic)
		{
			int center = this.Width / 2;
			int y = this.Location.Y;

			StringFormat cFormat = new StringFormat();
			Int32 lNum = (Int32)Math.Log((Double)this.TextAlign, 2);
			cFormat.LineAlignment = (StringAlignment)(lNum / 4);
			cFormat.Alignment = (StringAlignment)(lNum % 4);

			/*using (var g = Graphics.FromImage(buffer))
			{*/
				Brush b;

				for (int i = 0; i < this.Items.Count; i++ )
				{
					if (i == this.SelectedIndex)
						b = new SolidBrush(this.SelectedColor);
					else
						b = new SolidBrush(this.ForeColor);

					graphic.DrawString(this.Items[i], this.Font, b, new Point(center, y), cFormat);
					y += (int)this.Font.GetHeight() + 20;
				}
			/*}

			graphic.DrawImage(buffer, this.ClientRectangle);*/

		}
	}
}
