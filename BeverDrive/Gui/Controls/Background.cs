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
using System.Drawing.Imaging;
using BeverDrive.Core.Extensions;
using BeverDrive.Core;

namespace BeverDrive.Gui.Controls
{
	public class BackgroundImage
	{
		public string Name { get; set; }
		public Image Image { get; set; }
		public BackgroundImage(string name, Image image)
		{
			this.Name = name;
			this.Image = image;
		}
	}

	/// <summary>
	/// Class for the background, with support for superslow fading of images
	/// </summary>
	public class Background
	{
		private const int STEPS = 4;

		private ImageProcessor imgProc;
		private string name;

		public int Alpha { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public Background()
		{
			imgProc = new ImageProcessor();
		}

		/// <summary>
		/// Draws the background, with or without background image
		/// </summary>
		/// <param name="graphic"></param>
		/// <param name="backColor"></param>
		public void Draw(Graphics graphic, Color backColor)
		{
			graphic.Clear(backColor);
			// TODO: Remove this to have superslow stupid backgrounds
			return;
			
			if (imgProc.Original != null && imgProc.ProcessComplete)
				graphic.DrawImageUnscaled(imgProc.ProcessedImages[0], new Point(0, 0));

			return;
		}

		/// <summary>
		/// Use this to set a background image, if the name is the same as the previous images name
		/// nothing happens to prevent stupid fades of the same image
		/// </summary>
		/// <param name="backgroundName"></param>
		/// <param name="backgroundImage"></param>
		public void SetBackgroundImage(string backgroundName, Image backgroundImage)
		{
			if (backgroundImage != null && this.name != backgroundName)
			{
				imgProc.NewOriginal(backgroundImage);
				imgProc.CropToSize(this.Width, this.Height);
				imgProc.BlurAndFade(5, 0.6f, 0.6f, 1, BeverDriveContext.Settings.BackColor);
				name = backgroundName;
			}
		}
	}
}
