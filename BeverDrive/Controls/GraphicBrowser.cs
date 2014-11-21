using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Core;
using BeverDrive.Core.Styles;
using BeverDrive.Extensions;

namespace BeverDrive.Controls
{
	/// <summary>
	/// Control for a graphic file browser with cover support
	/// </summary>
	public class GraphicBrowser : UserControl
	{
		private FileSystemBrowser browser;
		private System.Drawing.Bitmap background;
		private System.Drawing.Bitmap buffer;
		private System.Drawing.RectangleF rectText;
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
					this.Invalidate();
				}
			}
		}

		public GraphicBrowser() : this("C:\\") { }

		public GraphicBrowser(string rootPath) : this(rootPath, false) { }

		public GraphicBrowser(string rootPath, bool chrootBehavior)
		{
			//this.background = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile("D:\\BeverDrive\\Mp3\\Panda\\cover.jpg");
			this.browser = new FileSystemBrowser(rootPath, chrootBehavior);
			this.rectText = new System.Drawing.RectangleF();

			this.BackColor = System.Drawing.Color.Transparent;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			BeverDriveContext.CurrentCoreGui.BaseText.Text = this.browser.CurrentDirectory.Name;
			//BeverDriveContext.CurrentCoreGui.ModuleContainer.BackgroundImage = System.Drawing.Bitmap.FromFile("D:\\BeverDrive\\Mp3\\Panda\\cover.jpg");
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// TODO: Is there a better way to clear buffer?
			if (buffer != null)
				buffer.Dispose();

			buffer = new System.Drawing.Bitmap(this.Width, this.Height);

			using (var g = System.Drawing.Graphics.FromImage(buffer))
			{
				this.PaintToBuffer(g, this.ClientRectangle);
			}

			e.Graphics.DrawImage(buffer, this.ClientRectangle);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (buffer != null)
				buffer.Dispose();

			buffer = new System.Drawing.Bitmap(this.Size.Width, this.Size.Height);
			rectText.Width = (float)(this.Width);
			rectText.Height = 64f;
			rectText.X = 0;
			rectText.Y = (float)(this.Size.Height - 68);
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

					BeverDriveContext.CurrentCoreGui.BaseText.Text = this.browser.CurrentDirectory.Name;
					this.SelectedIndex = 0;
					this.Invalidate();
				}
			}
		}

		private void PaintToBuffer(System.Drawing.Graphics graphic, System.Drawing.Rectangle clientRectangle)
		{
			if (this.SelectedIndex > -1)
			{
				graphic.DrawString(browser.Items[this.SelectedIndex].Name,
					Fonts.GuiFont24,
					Brushes.SelectedBrush,
					rectText,
					Fonts.Centered);
			}
			
			int start = scrollCount * 5;
			int stop = browser.Items.Count - start > 10 ? start + 10 : browser.Items.Count;
			int x = 0;
			int y = 0;
			for(int i = start; i < stop; i++)
			{
				var item = browser.Items[i];
				var rect = new System.Drawing.Rectangle(10 + (150 * x), 10 + (150 * y), 140, 140);
				this.DrawQuadrant(item, graphic, rect, i.Equals(this.SelectedIndex));
				x++;

				if (x == 5)
				{
					x = 0;
					y++;
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
					BeverDrive.Core.Styles.Fonts.WdFont64,
					BeverDrive.Core.Styles.Brushes.ForeBrush, rectangle);
			}
			else
			{
				graphic.DrawImage(item.CoverImage, rectangle);
			}
		}
	}
}
