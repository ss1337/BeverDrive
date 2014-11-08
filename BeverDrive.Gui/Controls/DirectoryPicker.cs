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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Gui.Core;
using BeverDrive.Gui.Core.Styles;
using BeverDrive.Gui.Extensions;
using BeverDrive.Gui.Modules;

namespace BeverDrive.Gui.Controls
{
	public enum DirectoryPickerStatus
	{
		OK = 1,
		Cancel = 2
	}

	public class DirectoryPicker : AModuleControl
	{
		private Bitmap backBuffer;
		private FileSystemBrowser browser;
		private ListControlPart listPart;
		private int width;

		public int BorderWidth { get; set; }

		public DirectoryInfo CurrentDirectory
		{
			get { return this.browser.CurrentDirectory; }
		}

		public DirectoryPickerStatus Status { get; private set; }

		public int SelectedIndex
		{
			get { return this.listPart.SelectedIndex; }
			set { 
				if (value > -3 && value < this.listPart.Items.Count) {
					this.listPart.SelectedIndex = value;
					this.Invalidate();
				}
			}
		}

		public new int Width
		{
			get { return width; }
			set {
				width = value;
				this.listPart.Width = value;
				base.Width = value;
			}
		}

		public DirectoryPicker()
		{
			base.Font = Fonts.GuiFont24;
			this.browser = new FileSystemBrowser("C:\\", false);
			this.listPart = new ListControlPart();
			this.listPart.Font = Fonts.GuiFont24;
			this.listPart.HeightInItems = 8;
			this.listPart.Location = new Point(0, 44);
			this.listPart.Width = this.Width;
			this.BorderWidth = 3;
			this.PopulateBrowser();
			this.Height = this.listPart.Height + 44;
			this.SelectedIndex = -1;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			if (!this.Visible)
				return;

			switch(e.Command)
			{
				case ModuleCommands.SelectClick:
					this.SelectClick();
					break;

				case ModuleCommands.SelectNext:
					this.SelectedIndex++;
					break;

				case ModuleCommands.SelectPrevious:
					this.SelectedIndex--;
					break;
			}
		}

		//protected override void OnPaintBackground(PaintEventArgs e)
		//{
		//    // Do nothing in order to avoid flickering
		//}

		protected override void OnPaint(PaintEventArgs e)
		{
			backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			Graphics g = Graphics.FromImage(backBuffer);
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			
			// Draw border
			g.FillHollowRectangle(BeverDrive.Gui.Core.Styles.Brushes.ForeBrush, this.ClientRectangle, 3);

			// Draw current directory name
			g.DrawString(this.browser.CurrentDirectory.Name, this.Font, BeverDrive.Gui.Core.Styles.Brushes.ForeBrush, 0, 4);

			// Draw select button
			g.DrawString("Select", this.Font, BeverDrive.Gui.Core.Styles.Brushes.ForeBrush, this.Width - 230, 4);
			
			if (this.SelectedIndex == -1)
				g.FillHollowRectangle(BeverDrive.Gui.Core.Styles.Brushes.SelectedBrush, new Rectangle(this.Width - 230, 4, 110, 36), 3);

			// Draw cancel button with selected box if we should
			g.DrawString("Cancel", this.Font, BeverDrive.Gui.Core.Styles.Brushes.ForeBrush, this.Width - 120, 4);
			
			if (this.SelectedIndex == -2)
				g.FillHollowRectangle(BeverDrive.Gui.Core.Styles.Brushes.SelectedBrush, new Rectangle(this.Width - 120, 4, 118, 36), 3);

			// Draw separator
			g.FillRectangle(BeverDrive.Gui.Core.Styles.Brushes.ForeBrush, 0, 40, this.Width, this.BorderWidth);

			// Draw list
			this.listPart.SelectedIndex = this.SelectedIndex;
			this.listPart.OnPaintToBuffer(g);
			g.Dispose();
			e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
			backBuffer.Dispose();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			this.Center();
			this.SelectedIndex = 0;
			base.OnVisibleChanged(e);
		}

		private void Center()
		{
			if (this.ParentForm != null)
			{
				var x = (this.ParentForm.Width - this.Width)/2;
				var y = (this.ParentForm.Height - this.Height)/2;
				this.Location = new Point(x, y);
			}
		}

		private void PopulateBrowser()
		{
			this.browser.Items.Any(x => { this.listPart.Items.Add(x); return false; });
		}

		private void SelectClick()
		{
			if (this.SelectedIndex == -1) {
				this.Visible = false;
				this.Status = DirectoryPickerStatus.OK;
			}

			if (this.SelectedIndex == -2) {
				this.Visible = false;
				this.Status = DirectoryPickerStatus.Cancel;
			}

			this.browser.Select(this.SelectedIndex);
		}
	}
}
