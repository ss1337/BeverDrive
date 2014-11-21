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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Core.Styles;
using BeverDrive.Extensions;

namespace BeverDrive.Controls
{
	public class ProgressBar : APaintControl
	{
		private Bitmap backBuffer;

		public int BorderWidth { get; set; }
		public int Maximum { get; set; }
		public int Minimum { get; set; }
		public int Step { get; set; }
		public int Value { get; set; }

		public ProgressBar()
		{
			var pb = new System.Windows.Forms.ProgressBar();
			this.Resize += new EventHandler(ProgressBar_Resize);
			this.BorderWidth = 2;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
		}

		public void PerformStep()
		{
			this.Value += this.Step;
		}

		public void Reset()
		{
			this.backBuffer = null;
		}

		protected void ProgressBar_Resize(object sender, EventArgs e)
		{
			this.backBuffer = null;
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			this.OnPaintToBuffer(e.Graphics, this.ClientRectangle);
		}

		protected override void OnPaintToBuffer(Graphics graphic, Rectangle clientRectangle)
		{
			if (this.backBuffer == null)
				this.backBuffer = new Bitmap(this.Width, this.Height);

			var oRect = clientRectangle;
			var iRect = clientRectangle;
			iRect.Inflate(new Size(-this.BorderWidth, -this.BorderWidth));
			int valueWidth = (int)(iRect.Width * ((double)this.Value / this.Maximum));

			// Draw core progress bar
			using (var g = Graphics.FromImage(this.backBuffer))
			{
				// Draw borders
				g.FillHollowRectangle(BeverDrive.Core.Styles.Brushes.ForeBrush, oRect, this.BorderWidth);
				g.FillRectangle(BeverDrive.Core.Styles.Brushes.SelectedBrush, iRect.Left, iRect.Top, valueWidth, iRect.Height);
			}

			graphic.DrawImageUnscaled(this.backBuffer, this.ClientRectangle);
		}
	}
}
