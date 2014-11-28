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
	public class GraphicsPanelBackgroundImage
	{
		public Image Background { get; set; }
		public String Name { get; set; }

		public GraphicsPanelBackgroundImage(Image background, string name)
		{
			this.Background = background;
			this.Name = name;
		}
	}


	/// <summary>
	/// Control for drawing stuff with alpha channels.
	/// Overlayed controls such as VLC players etc seem to work
	/// </summary>
	public class GraphicsPanel : Control
	{
		private const int MAXFADE = 6;
		private int backgroundFade;
		private FileSystemItem backgroundImage;

		public new FileSystemItem BackgroundImage
		{
			get { return this.backgroundImage; }
			set
			{
				if (value == null || value.CoverImage == null)
				{
					// Fade out background image if it exists, from -8 to 0
					// and only if we are in fade in
					if (backgroundImage != null && backgroundFade > 0)
						backgroundFade = -MAXFADE;
				}
				else
				{
					if (backgroundImage == null || value.Name != backgroundImage.Name)
					{
						backgroundImage = value;
						backgroundFade = 0;
					}
				}
			}
		}

		/// <summary>
		/// Add AGraphicControls here is order to have them rendered nicely
		/// </summary>
		public IList<AGraphicsControl> GraphicControls { get; set; }

		private Bitmap buffer;

		public GraphicsPanel()
		{
			this.backgroundImage = new FileSystemItem("");
			this.GraphicControls = new List<AGraphicsControl>();
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
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

			using (var g = Graphics.FromImage(buffer))
			{
				g.Clear(this.BackColor);
				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

				if (backgroundImage != null && backgroundImage.CoverImage != null)
				{
					this.DrawBackgroundImage(g, this.backgroundImage.CoverImage, Math.Abs((float)this.backgroundFade / 10f));

					// Fade in/out background
					if (this.backgroundFade < MAXFADE)
					{
						this.backgroundFade += 1;
						if (this.backgroundFade == 0)
							this.backgroundImage = null;
					}
				}

				// Draw all the child controls using their respective PaintToBuffer function
				this.GraphicControls.Any(x => { if (x.Visible) { x.PaintToBuffer(g); }  return false; });
			}

			e.Graphics.DrawImageUnscaled(this.buffer, new Point(0, 0));
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			// Clearing buffer, the old one is of the wrong size
			this.buffer = null;
			base.OnSizeChanged(e);
		}

		private void DrawBackgroundImage(Graphics graphic, Image image, float fade)
		{
			float ratio = (float)((float)this.Width / (float)this.Height);
			int iWidth = image.Width;
			int iHeight = image.Height;
			int srcX = 0;
			int srcY = 0;
			int srcHeight = image.Height;
			int srcWidth = image.Width;
			var destRect = new Rectangle(0, 0, this.Width, this.Height);

			var srcRect = image.CalculateScaling(this.Width, this.Height);

			// Skip fading stuff if the image should be opaque
			if (fade == MAXFADE)
			{
				graphic.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel);
				return;
			}

			// Initialize the color matrix. 
			// Note the value 0.8 in row 4, column 4. 
			float[][] matrixItems ={ 
				new float[] {1, 0, 0, 0, 0},
				new float[] {0, 1, 0, 0, 0},
				new float[] {0, 0, 1, 0, 0},
				new float[] {0, 0, 0, fade, 0}, 
				new float[] {0, 0, 0, 0, 1}};
			ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

			// Create an ImageAttributes object and set its color matrix.
			ImageAttributes imageAtt = new ImageAttributes();
			imageAtt.SetColorMatrix(
				colorMatrix,
				ColorMatrixFlag.Default,
				ColorAdjustType.Bitmap);

			// Now draw the semitransparent bitmap image. 
			graphic.DrawImage(
			   image,
			   destRect,
			   srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height,
			   /*new Rectangle(0, 0, iWidth, iHeight),  // destination rectangle
			   0.0f,                          // source rectangle x 
			   0.0f,                          // source rectangle y
			   iWidth,                        // source rectangle width
			   iHeight,                       // source rectangle height*/
			   GraphicsUnit.Pixel,
			   imageAtt);
		}
	}
}
