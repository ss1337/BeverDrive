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

namespace BeverDrive.Extensions
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
	}
}
