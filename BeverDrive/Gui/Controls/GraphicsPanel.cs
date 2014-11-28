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
using System.Drawing.Imaging;
using BeverDrive.Core;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Gui.Controls
{
	/// <summary>
	/// Control for drawing stuff with alpha channels.
	/// Overlayed controls such as VLC players etc seem to work
	/// </summary>
	public class GraphicsPanel : Control
	{
		/// <summary>
		/// Add AGraphicControls here is order to have them rendered nicely
		/// </summary>
		public IList<AGraphicsControl> GraphicControls { get; set; }

		private Bitmap buffer;
		private Background background;

#if DEBUG
		// Frame timer
		double lastFrame;
#endif

		public GraphicsPanel()
		{
			this.background = new Background();
			this.GraphicControls = new List<AGraphicsControl>();
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		public void SetBackgroundImage(string backgroundName, Image backgroundImage)
		{
			this.background.SetBackgroundImage(backgroundName, backgroundImage);
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

#if DEBUG
			System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
#endif

			using (var gr = Graphics.FromImage(buffer))
			{
				background.Draw(gr, this.BackColor);

				// Draw all the child controls using their respective PaintToBuffer function
				this.GraphicControls.Any(x => { if (x.Visible) { x.PaintToBuffer(gr); }  return false; });
			}

			e.Graphics.DrawImageUnscaled(this.buffer, new Point(0, 0));

#if DEBUG
			// Draw frame timer
			e.Graphics.DrawString(lastFrame.ToString(), BeverDrive.Gui.Styles.Fonts.GuiFont18, BeverDrive.Gui.Styles.Brushes.SelectedBrush, 650f, 15f);
			sw.Stop();
			lastFrame = sw.Elapsed.TotalMilliseconds;
#endif
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			// Clearing buffer, the old one is of the wrong size
			this.buffer = null;
			this.background.Width = this.Width;
			this.background.Height = this.Height;
			base.OnSizeChanged(e);
		}
	}
}
