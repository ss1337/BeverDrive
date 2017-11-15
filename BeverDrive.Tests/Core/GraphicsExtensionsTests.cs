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
using System.Linq;
using System.Text;
using BeverDrive.Core.Extensions;
using NUnit.Framework;

namespace BeverDrive.Tests.Core
{
	[TestFixture(Ignore=true)]
	public class GraphicsExtensionsTests
	{
		public GraphicsExtensionsTests()
		{
		}

		[Test]
		public void Calculates_an_image_with_greater_height_correctly()
		{
			var bmp = new System.Drawing.Bitmap(600, 600);
			var result = bmp.CalculateScaling(760, 560);
			Assert.AreEqual(600, result.Width);
			Assert.AreEqual(442, result.Height);
			Assert.AreEqual(0, result.X);
			Assert.AreEqual(79, result.Y);
		}

		[Test]
		public void Calculates_an_image_with_greater_size_correctly()
		{
			var bmp = new System.Drawing.Bitmap(1024, 1024);
			var result = bmp.CalculateScaling(760, 560);
			Assert.AreEqual(1024, result.Width);
			Assert.AreEqual(754, result.Height);
			Assert.AreEqual(0, result.X);
			Assert.AreEqual(135, result.Y);
		}

		[Test]
		public void Calculates_a_quirky_image_correctly()
		{
			var bmp = new System.Drawing.Bitmap(500, 1024);
			var result = bmp.CalculateScaling(760, 560);
			Assert.AreEqual(500, result.Width);
			Assert.AreEqual(368, result.Height);
			Assert.AreEqual(0, result.X);
			Assert.AreEqual(328, result.Y);
		}

		[Test]
		public void Calculates_another_quirky_image_correctly()
		{
			var bmp = new System.Drawing.Bitmap(1024, 500);
			var result = bmp.CalculateScaling(760, 560);
			Assert.AreEqual(678, result.Width);
			Assert.AreEqual(500, result.Height);
			Assert.AreEqual(173, result.X);
			Assert.AreEqual(0, result.Y);
		}

		[Test]
		public void Calculates_yet_another_quirky_image_correctly()
		{
			var bmp = new System.Drawing.Bitmap(399, 400);
			var result = bmp.CalculateScaling(760, 560);
			Assert.AreEqual(399, result.Width);
			Assert.AreEqual(294, result.Height);
			Assert.AreEqual(0, result.X);
			Assert.AreEqual(53, result.Y);
		}
	}
}
