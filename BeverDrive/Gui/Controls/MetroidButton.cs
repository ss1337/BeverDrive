//
// Copyright 2012-2016 Sebastian Sjödin
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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

		public MetroidButton(string iconFile, Color normal, Color selected)
		{
			Bitmap tmpBmp = (Bitmap)Bitmap.FromFile("Resources" + Path.DirectorySeparatorChar + iconFile);
			int width = tmpBmp.Width;
			int height = tmpBmp.Height;

			icon = new Bitmap(width, height, tmpBmp.PixelFormat);
			selectedIcon = new Bitmap(width, height, tmpBmp.PixelFormat);

			// Create normal and selected icons depending on colors
			BitmapData tmpData = tmpBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, tmpBmp.PixelFormat);
			BitmapData icon1Data = ((Bitmap)icon).LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, tmpBmp.PixelFormat);
			BitmapData icon2Data = ((Bitmap)selectedIcon).LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, tmpBmp.PixelFormat);

			unsafe
			{
				uint* p_tmp = (uint*)tmpData.Scan0;
				uint* p_icon1 = (uint*)icon1Data.Scan0;
				uint* p_icon2 = (uint*)icon2Data.Scan0;

				uint pixelValue = 0;
				uint icon1Color = (uint)normal.ToArgb() & 0xffffff;
				uint icon2Color = (uint)selected.ToArgb() & 0xffffff;

				for (uint i = 0; i < width * height; i++)
				{
					pixelValue = (uint)*(p_tmp + i);
					if (pixelValue > 0)
					{
						// Alpha from original image + color
						*(p_icon1 + i) = ((pixelValue >> 24) << 24) + icon1Color;
						*(p_icon2 + i) = ((pixelValue >> 24) << 24) + icon2Color;
					}
					else
					{
						*(p_icon1 + i) = 0x00000000;
						*(p_icon2 + i) = 0x00000000;
					}
				}
			}

			((Bitmap)selectedIcon).UnlockBits(icon2Data);
			((Bitmap)icon).UnlockBits(icon1Data);
			((Bitmap)tmpBmp).UnlockBits(tmpData);
			tmpBmp.Dispose();

			this.Font = new Font("Arial", 16f);
			this.Width = width;
			this.Height = height;
		}

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
