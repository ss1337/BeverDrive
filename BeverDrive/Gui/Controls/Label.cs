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
using System.Drawing;

namespace BeverDrive.Gui.Controls
{
	/// <summary>
	/// Works like the regular Label control except it draws itself to a buffer in order to speed up things
	/// </summary>
	public class Label : AGraphicsControl
	{
		private Image buffer;

		public ContentAlignment TextAlign { get; set; }

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;

				// Reset buffer then text is changed
				this.buffer = null;
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;

				// Reset buffer when text is changed
				buffer = null;
			}
		}

		public Label()
		{
		}

		public override void PaintToBuffer(Graphics graphic)
		{
			// Draw string to a buffer at the first time to speed up stuff
			if (buffer == null)
			{
				var rect = new RectangleF(0, 0, this.Width, this.Height);
				buffer = new Bitmap(this.Width, this.Height);
				StringFormat cFormat = new StringFormat();
				Int32 lNum = (Int32)Math.Log((Double)this.TextAlign, 2);
				cFormat.LineAlignment = (StringAlignment)(lNum / 4);
				cFormat.Alignment = (StringAlignment)(lNum % 4);

				using (var g = Graphics.FromImage(buffer))
				{
					g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), rect, cFormat);
				}
			}

			graphic.DrawImage(buffer, this.ClientRectangle);
		}
	}
}
