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
using System.Windows.Forms;
using System.Drawing;

namespace BeverDrive.Gui.Controls
{
	public class MetroidButton : AGraphicsControl
	{
		public int GridLeft { get { return gridLeft; } set { this.gridLeft = value; this.Left = 16 + value * 37; } }
		public int GridTop { get { return gridTop; } set { this.gridTop = value; this.Top = 24 + value * 37; } }
		public bool Selected { get; set; }

		private int gridLeft;
		private int gridTop;
		private Image icon;
		private Image selectedIcon;

		public MetroidButton(string iconFile, string selectedIconFile)
		{
			icon = Bitmap.FromFile(iconFile);
			selectedIcon = Bitmap.FromFile(selectedIconFile);

			this.Font = new Font("Arial", 16f);
			this.Width = icon.Width;
			this.Height = icon.Height;
		}

		public void PerformClick()
		{
			OnClick(EventArgs.Empty);
		}

		public override void PaintToBuffer(Graphics graphic)
		{
			var clientRectangle = new Rectangle(this.Location, this.Size);
			graphic.DrawImage(this.Selected ? selectedIcon : icon, clientRectangle);

#if DEBUG
			if (!string.IsNullOrEmpty(this.Text))
			{
				graphic.DrawString(this.Text, this.Font, Brushes.Black, clientRectangle.Left + 2, clientRectangle.Bottom - 28);
			}
#endif
		}
	}
}
