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
using BeverDrive.Core.Styles;
using BeverDrive.Extensions;

namespace BeverDrive.Controls
{
	public class ItemSelectedEventArgs : EventArgs
	{
		public int SelectedIndex { get; set; }
	}

	public class MenuOptionList : APaintControl
	{
		private ListControlPart listPart;
		private Bitmap backBuffer;

		public override Font Font { get { return base.Font; } set { base.Font = value; this.listPart.Font = value; } }
		public int HeightInItems { get { return this.listPart.HeightInItems; } set { this.listPart.HeightInItems = value; this.Height = this.listPart.Height; } }
		public List<string> Items { get { return this.listPart.Items; } set { this.listPart.Items = value; } }
		public int SelectedIndex { get { return this.listPart.SelectedIndex; } set { this.listPart.SelectedIndex = value; } }
		public string SelectedItem { get { return this.listPart.Items[this.SelectedIndex]; } }
		public new int Width { get { return this.listPart.Width; } set { this.listPart.Width = value; base.Width = value; } }

		public MenuOptionList()
		{
			this.listPart = new ListControlPart();
			this.listPart.HeightInItems = 7;
			this.listPart.Location = new Point(0, 0);
			this.listPart.Width = this.Width;

			this.Font = Fonts.GuiFont26;
			this.Items = new List<string>();
			this.SelectedIndex = -1;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
		}

		public delegate void ItemSelectedEventHandler(object sender, ItemSelectedEventArgs e);

		public event ItemSelectedEventHandler ItemSelected;

		protected override void OnClick(EventArgs e)
		{
			var m = (MouseEventArgs)e;
			this.SelectedIndex = (m.Y / this.Font.Height);
			this.Invalidate();
			this.ItemSelected(this, new ItemSelectedEventArgs { SelectedIndex = this.SelectedIndex });
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Do nothing in order to avoid flickering
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//if (backBuffer == null)
				backBuffer = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			using(var graphic = Graphics.FromImage(backBuffer))
			{
				this.listPart.OnPaintToBuffer(graphic);
			}

			e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
			backBuffer.Dispose();
		}

		public void ScrollToCenter(int index)
		{
			this.listPart.ScrollToCenter(index);
		}
	}
}
