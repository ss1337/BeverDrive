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
using System.Drawing.Imaging;

namespace BeverDrive.Core.Extensions
{
	public static class GraphicsExtensions
	{
		/// <summary>
		/// Scales and crops an image
		/// </summary>
		/// <param name="image"></param>
		/// <param name="scaleToWidth"></param>
		/// <param name="scaleToHeight"></param>
		/// <returns></returns>
		public static Rectangle CalculateScaling(this Image image, int scaleToWidth, int scaleToHeight)
		{
			float ratio = (float)((float)scaleToWidth / (float)scaleToHeight);
			int iWidth = image.Width;
			int iHeight = image.Height;
			int crop = 0;
			var result = new Rectangle(0, 0, iWidth, iHeight);

			// Check that image is ratio croppable
			if (iHeight / ratio < iWidth)
			{
				if (iHeight < scaleToHeight && iWidth >= scaleToWidth)
					crop = 2;
				else
					crop = 1;
			}

			if (iHeight / ratio > iWidth)
			{
				if (iWidth > scaleToWidth)
					crop = 2;
				else
					crop = 1;
			}

			if (crop == 1)
			{
				// Crop top and bottom
				result.Height = (int)((float)iWidth / ratio);
				result.Width = iWidth;
				result.X = 0;
				result.Y = Math.Abs((iHeight - result.Height) / 2);
			}

			if (crop == 2)
			{
				// Crop sides
				result.Height = iHeight;
				result.Width = (int)((float)iHeight * ratio);
				result.X = Math.Abs((iWidth - result.Width) / 2);
				result.Y = 0;
			}

			return result;
		}

		/// <summary>
		/// Draws an image faded with an alpha value
		/// </summary>
		/// <param name="graphic"></param>
		/// <param name="dest"></param>
		/// <param name="source"></param>
		/// <param name="alpha">Alpha value, from 0f to 1f</param>
		public static void DrawImageAlphaFadedAsdf(this Graphics graphic, Image image, Rectangle dest, Rectangle source, float alpha)
		{
			// Initialize the color matrix. 
			float[][] matrixItems ={ 
				new float[] {1, 0, 0, 0, 0},
				new float[] {0, 1, 0, 0, 0},
				new float[] {0, 0, 1, 0, 0},
				new float[] {0, 0, 0, alpha, 0}, 
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
				dest,
				source.X, source.Y, source.Width, source.Height,
				GraphicsUnit.Pixel,
				imageAtt);
		}

		/// <summary>
		/// Draws a hollow rectangle
		/// </summary>
		/// <param name="graphic"></param>
		/// <param name="brush"></param>
		/// <param name="rectangle"></param>
		/// <param name="borderWidth"></param>
		public static void FillHollowRectangle(this Graphics graphic, Brush brush, Rectangle rectangle, int borderWidth)
		{
			if (rectangle.Height < borderWidth * 2 || rectangle.Width < borderWidth * 2)
			{
				// Draw a simple solid rectangle
				graphic.FillRectangle(brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			}
			else
			{
				// Draw top border
				graphic.FillRectangle(brush, rectangle.X, rectangle.Y, rectangle.Width, borderWidth);

				// Draw right border
				graphic.FillRectangle(brush, rectangle.X + rectangle.Width - borderWidth, rectangle.Y + borderWidth, borderWidth, rectangle.Height - borderWidth * 2);

				// Draw bottom border
				graphic.FillRectangle(brush, rectangle.X, rectangle.Y + rectangle.Height - borderWidth, rectangle.Width, borderWidth);

				// Draw left border
				graphic.FillRectangle(brush, rectangle.X, rectangle.Y + borderWidth, borderWidth, rectangle.Height - borderWidth * 2);
			}
		}
	}
}
