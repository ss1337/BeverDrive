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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BeverDrive.Gui.Controls
{
	public delegate void ControlEventHandler(object sender, EventArgs e);

	public class AGraphicsControl
	{
		public int Index { get; set; }
		public string Name { get; set; }

		public virtual Color BackColor { get; set; }
		public virtual Color ForeColor { get; set; }
		public virtual Font Font { get; set; }
		public ContentAlignment TextAlign { get; set; }

		public Point Location { get; set; }
		public Size Size
		{
			get { return new Size(this.Width, this.Height); }
			set { this.Width = value.Width; this.Height = value.Height; }
		}
		public virtual int Width { get; set; }
		public virtual int Height { get; set; }

		public virtual bool Selected { get; set; }

		public virtual string Text { get; set; }

		public virtual bool Visible { get; set; }

		public event ControlEventHandler Click;

		public event ControlEventHandler Hover;

		public RectangleF ClientRectangle
		{
			get { return new RectangleF((PointF)this.Location, (SizeF)this.Size); }
		}

		public AGraphicsControl()
		{
			this.Visible = true;
			var c = new System.Windows.Forms.Label();
		}

		/// <summary>
		/// Draws the control to a buffer
		/// </summary>
		/// <param name="graphic"></param>
		public virtual void PaintToBuffer(Graphics graphic)
		{
		}

		/// <summary>
		/// Raises the click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void RaiseClick(object sender, EventArgs e)
		{
			if (this.Click != null)
				this.Click(sender, e);
		}

		/// <summary>
		/// Raises the hover event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void RaiseHover(object sender, EventArgs e)
		{
			if (this.Hover != null)
				this.Hover(sender, e);
		}
	}
}
