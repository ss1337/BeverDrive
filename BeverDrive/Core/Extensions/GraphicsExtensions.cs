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

namespace BeverDrive.Core.Extensions
{
	public static class GraphicsExtensions
	{
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

		/*public static void DrawImageScaled()
		{
		}*/
	}
}
