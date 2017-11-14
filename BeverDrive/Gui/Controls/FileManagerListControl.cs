//
// Copyright 2017 Sebastian Sjödin
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
using System.IO;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Core.Extensions;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Gui.Controls
{
	public class FileManagerListControl : AGraphicsControl
	{
		private FileSystemBrowser browser;

		// Scroll index may be a maximum of Items.Count - HeightInItems
		private int heightInItems;
		private int scrollIndex;
		private int selectedIndex;
		private System.Drawing.Brush tickBrush;
		private System.Drawing.Font tickFont;

		public new int Height { get { return this.HeightInItems * this.ItemHeight; } }
		public int HeightInItems { get { return heightInItems; } set { heightInItems = value; } }
		public int ItemHeight { get { return this.Font.Height; } }

		/// <summary>
		/// Holds a list of items in the list box, and wheter they are selected or not
		/// </summary>
		public List<FileSystemItem> Items { get { return browser.Items; } }
		public DirectoryInfo CurrentDirectory { get { return browser.CurrentDirectory; } }
		public FileSystemItem CurrentItem { get { return this.browser.CurrentItem; } }
		
		public int SelectedIndex
		{
			get { return selectedIndex; }
			set {
				if (value < Items.Count)
				{
					selectedIndex = value;
					if (value == 0) { scrollIndex = 0; }
				}
			}
		}

		public string SelectedItem { get { return this.Items[this.SelectedIndex].Name; } }

		public FileManagerListControl()
		{
			this.browser = new FileSystemBrowser("> My computer", false);
			this.Font = Fonts.GuiFont26;
			this.tickBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
			this.tickFont = Fonts.WdFont26;
		}

		#region List control stuff
		public override void PaintToBuffer(System.Drawing.Graphics graphic)
		{
			graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), new System.Drawing.Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height));

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

		private void PaintItem(System.Drawing.Graphics g, int itemIndex)
		{
			int y = itemIndex * this.ItemHeight;

			// Calculate rectangles and stuff...
			string text = this.Items[itemIndex + scrollIndex].Name;
			var outerRect = new System.Drawing.Rectangle(this.Location.X, this.Location.Y + y, this.Width, this.ItemHeight);

			// Draw selected box if this is selected
			if (itemIndex + scrollIndex == this.SelectedIndex)
				g.FillHollowRectangle(BeverDrive.Gui.Styles.Brushes.SelectedBrush, outerRect, 3);

			// Vertical centering
			int height = outerRect.Height;
			int fontHeight = this.Font.Height;
			int offset = (height - fontHeight) / 2;

			// Draw a tick if it's selected
			if (this.Items[itemIndex + scrollIndex].Selected)
				g.DrawString("a", this.tickFont, this.tickBrush, outerRect.X - 7, outerRect.Y + offset);

			g.DrawString(text, this.Font, BeverDrive.Gui.Styles.Brushes.ForeBrush, outerRect.X + 20, outerRect.Y + offset);
		}
		#endregion

		#region File browser
		public void Refresh()
		{
			this.browser.Refresh();
		}

		public new void Select()
		{
			if (this.SelectedIndex > -1 && this.SelectedIndex < this.Items.Count)
			{
				var item = this.browser.Items[this.SelectedIndex];

				if (!this.SelectedItemIsFile())
				{
					string currentDir = "";
					if (browser.CurrentDirectory != null)
						currentDir = Path.DirectorySeparatorChar + browser.CurrentDirectory.Name;

					this.browser.Select(this.SelectedIndex);
					this.SelectedIndex = 0;

					if (item.Name == Path.DirectorySeparatorChar + "..")
					{
						for (int i = 0; i < this.Items.Count; i++)
						{
							if (this.Items[i].Name == currentDir)
								this.ScrollToCenter(i);
						}
					}
				}
				else
				{
					this.Items[this.SelectedIndex].Selected = !this.Items[this.SelectedIndex].Selected;
				}
			}
		}

		public bool SelectedItemIsFile()
		{
			if (this.SelectedItem.StartsWith(Path.DirectorySeparatorChar.ToString()))
				return false;

			if (this.browser.Items[this.SelectedIndex].FileType == FileType.MyComputer)
				return false;

			if (this.browser.Items[this.SelectedIndex].FileType == FileType.Drive)
				return false;

			return true;
		}
		#endregion
	}
}
