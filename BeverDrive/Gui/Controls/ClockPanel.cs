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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeverDrive.Gui.Controls
{
	public class ClockPanel : Control
	{
		private StringFormat dateFormat;
		private StringFormat timeFormat;
		private StringFormat textFormat;
		private Bitmap buffer;

		public Color TextColor { get; set; }

		public string Date { get; set; }
		public string Time { get; set; }

		public ClockPanel()
		{
			this.dateFormat = new StringFormat();
			this.dateFormat.Alignment = StringAlignment.Near;
			this.textFormat = new StringFormat();
			this.textFormat.Alignment = StringAlignment.Center;
			this.timeFormat = new StringFormat();
			this.timeFormat.Alignment = StringAlignment.Far;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Don't draw a background, its drawn in the OnPaint function
			//base.OnPaintBackground(pevent);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.buffer == null)
				this.buffer = new Bitmap(this.Width, this.Height);

			using (var g = Graphics.FromImage(buffer))
			{
				g.Clear(this.BackColor);

				if (!string.IsNullOrEmpty(this.Date))
					g.DrawString(this.Date, this.Font, new SolidBrush(this.ForeColor), new Point(5, 1), this.dateFormat);

				if (!string.IsNullOrEmpty(this.Text))
					g.DrawString(this.Text, this.Font, new SolidBrush(this.TextColor), new Point((this.Width / 2) + 42, 1), this.textFormat);

				if (!string.IsNullOrEmpty(this.Time))
					g.DrawString(this.Time, this.Font, new SolidBrush(this.ForeColor), new Point(this.Width - 5, 1), this.timeFormat);

			}

			e.Graphics.DrawImageUnscaled(this.buffer, new Point(0, 0));
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			// Clearing buffer, the old one is of the wrong size
			this.buffer = null;
			base.OnSizeChanged(e);
		}
	}
}
