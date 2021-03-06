﻿//
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
using System.Text;
using System.Drawing;

namespace BeverDrive.Core.Extensions
{
	public static class RectangleExtensions
	{
		public static Rectangle Shrink(this Rectangle rectangle, int amount)
		{
			var newRect = rectangle;
			newRect.Width = rectangle.Width - amount * 2;
			newRect.Height = rectangle.Height - amount * 2;
			newRect.X = rectangle.X + amount;
			newRect.Y = rectangle.Y + amount;
			return newRect;
		}
	}
}
