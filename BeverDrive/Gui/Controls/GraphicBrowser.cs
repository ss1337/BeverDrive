using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Core;
using BeverDrive.Core.Extensions;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Gui.Controls
{
	/// <summary>
	/// Control for a graphic file browser with cover support
	/// </summary>
	public class GraphicBrowser : AGraphicsControl
	{
		private FileSystemBrowser browser;
		private System.Drawing.RectangleF textRect;
		private int scrollCount;
		private int selectedIndex;

		public int SelectedIndex
		{
			get { return selectedIndex; }
			set {
				if (value > -1 && value < browser.Items.Count)
				{
					if ((value > selectedIndex) && (value > scrollCount * 5 + 9))
					{
						scrollCount++;
					}

					if ((value < selectedIndex) && (value < scrollCount * 5))
					{
						scrollCount--;
					}

					selectedIndex = value;

					if (browser.Items[value].CoverImage != null)
						BeverDriveContext.CurrentCoreGui.ModuleContainer.BackgroundImage = browser.Items[value];
					else
						BeverDriveContext.CurrentCoreGui.ModuleContainer.BackgroundImage = browser.CurrentItem;

					this.Invalidate();
				}
			}
		}

		public GraphicBrowser() : this("C:\\") { }

		public GraphicBrowser(string rootPath) : this(rootPath, false) { }

		public GraphicBrowser(string rootPath, bool chrootBehavior)
		{
			this.browser = new FileSystemBrowser(rootPath, chrootBehavior);
			this.textRect  = new System.Drawing.RectangleF();
			this.SelectedIndex = 0;
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Text = this.browser.CurrentDirectory.Name;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			textRect.Width = (float)(this.Width);
			textRect.Height = 64f;
			textRect.X = 0;
			textRect.Y = this.ClientRectangle.Bottom - 72f;
		}

		public new void Select()
		{
			if (this.SelectedIndex > -1 && this.SelectedIndex < this.browser.Items.Count)
			{
				var item = this.browser.Items[this.SelectedIndex];

				if (item.Name.StartsWith("\\"))
				{
					if (item.Name == "\\..")
					{
						this.browser.CdUp();
					}
					else
					{
						this.browser.Cd(item.Name);
					}

					BeverDriveContext.CurrentCoreGui.ModuleContainer.Text = this.browser.CurrentDirectory.Name;
					this.SelectedIndex = 0;
					this.Invalidate();
				}
			}
		}

		public override void PaintToBuffer(System.Drawing.Graphics graphic)
		{
			//graphic.FillRectangle(Brushes.SelectedBrush, this.ClientRectangle);

			if (this.SelectedIndex > -1)
			{
				graphic.DrawString(browser.Items[this.SelectedIndex].Name,
					Fonts.GuiFont24,
					Brushes.SelectedBrush,
					textRect,
					Fonts.Centered);
			}
			
			int start = scrollCount * 5;
			int stop = browser.Items.Count - start > 10 ? start + 10 : browser.Items.Count;
			int row = 0;
			int column = 0;
			int x = 0;
			int y = (int)this.ClientRectangle.Top;
			for(int i = start; i < stop; i++)
			{
				var item = browser.Items[i];
				var rect = new System.Drawing.Rectangle(10 + (150 * column) + (int)this.ClientRectangle.Left, 10 + (150 * row) + (int)this.ClientRectangle.Top, 140, 140);
				this.DrawQuadrant(item, graphic, rect, i.Equals(this.SelectedIndex));
				column++;

				if (column == 5)
				{
					column = 0;
					row++;
				}
			}
		}

		private void DrawQuadrant(FileSystemItem item, System.Drawing.Graphics graphic, System.Drawing.Rectangle rectangle, bool selected)
		{
			if (selected)
				graphic.FillHollowRectangle(Brushes.SelectedBrush, rectangle, 2);
			else
				graphic.FillHollowRectangle(Brushes.ForeBrush, rectangle, 2);

			rectangle.Inflate(-2, -2);

			if (item.CoverImage == null)
			{
				graphic.DrawString(Char.ConvertFromUtf32((int)item.FileType),
					BeverDrive.Gui.Styles.Fonts.WdFont64,
					BeverDrive.Gui.Styles.Brushes.ForeBrush, rectangle);
			}
			else
			{
				graphic.DrawImage(item.CoverImage, rectangle);
			}
		}
	}
}
