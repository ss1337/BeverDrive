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
using System.Drawing;
using BeverDrive.Gui.Styles;
using BeverDrive.Core.Extensions;

namespace BeverDrive.Gui.Controls
{
	/// <summary>
	/// This control draws itself from bottom left and up
	/// </summary>
	public class ListControl : AGraphicsControl
	{
		// Scroll index may be a maximum of Items.Count - HeightInItems
		private int scrollIndex;
		private int selectedIndex;
		private int heightInItems;

		public new int Height { get { return this.HeightInItems * this.ItemHeight; } }
		public int HeightInItems { get { return heightInItems; } set { heightInItems = value; } }
		public int ItemHeight { get { return this.Font.Height; } }
		public List<string> Items { get; set; }
		
		public int SelectedIndex
		{
			get { return selectedIndex; }
			set {
				if (value < Items.Count())
				{
					selectedIndex = value;
					if (value == 0) { scrollIndex = 0; }
				}
			}
		}

		public string SelectedItem { get { return this.Items[this.SelectedIndex]; } }

		public ListControl()
		{
			this.Items = new List<string>();
			this.SelectedIndex = -1;
		}

		public override void PaintToBuffer(Graphics graphic)
		{
			graphic.FillRectangle(new SolidBrush(Color.Black), new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height));

			// Calculate start index
			if (this.SelectedIndex + 1 > this.HeightInItems + this.scrollIndex && this.SelectedIndex < this.Items.Count)
				this.scrollIndex++;

			if (this.SelectedIndex < this.scrollIndex && this.SelectedIndex > -1)
				this.scrollIndex--;

			int count = this.Items.Count < this.HeightInItems ? this.Items.Count : this.HeightInItems;

			for (int i = 0; i < count; i++)
				PaintItem(graphic, i);
		}

		public void ScrollToCenter(int index)
		{
			if (index > this.HeightInItems / 2) 
				scrollIndex = index - (this.HeightInItems / 2);

			// Om det inte finns någon scroll, sätt scrollindex till 0
			if (this.HeightInItems >= this.Items.Count)
				scrollIndex = 0;
			else
				if (scrollIndex > this.Items.Count - this.HeightInItems)
					scrollIndex = this.Items.Count - this.HeightInItems;

			this.selectedIndex = index;
		}

		private void PaintItem(Graphics g, int itemIndex)
		{
			int y = itemIndex * this.ItemHeight;

			// Calculate rectangles and stuff...
			string text = this.Items[itemIndex + scrollIndex].ToString();
			var outerRect = new Rectangle(this.Location.X, this.Location.Y + y, this.Width, this.ItemHeight);

			// Draw selected box if this is selected
			if (itemIndex + scrollIndex == this.SelectedIndex)
			{
				g.FillHollowRectangle(BeverDrive.Gui.Styles.Brushes.SelectedBrush, outerRect, 3);
			}

			// Vertical centering
			int height = outerRect.Height;
			int fontHeight = this.Font.Height;
			int offset = (height - fontHeight) / 2;
			g.DrawString(text, this.Font, BeverDrive.Gui.Styles.Brushes.ForeBrush, outerRect.X, outerRect.Y + offset);
		}
	}
}
