//
// Copyright 2015 Sebastian Sjödin
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
using BeverDrive.Core;
using NUnit.Framework;

namespace BeverDrive.Tests.Core
{
	[TestFixture]
	public class ImageProcessorTests
	{
		public ImageProcessorTests()
		{
		}

		[Test]
		public void Blur_and_fade_works_correctly()
		{
			var img = new Bitmap(800, 600);
			var imgproc = new ImageProcessor(img);
			Assert.False(imgproc.ProcessComplete);
			imgproc.BlurAndFade(5, 0.0f, 0.8f, 5, Color.Aqua);
			while (!imgproc.ProcessComplete) { }
			Assert.True(imgproc.ProcessComplete);
			Assert.AreEqual(5, imgproc.ProcessedImages.Count);
		}

		[Test]
		public void Fade_works_correctly()
		{
			var img = new Bitmap(800, 600);
			var imgproc = new ImageProcessor(img);
			Assert.False(imgproc.ProcessComplete);
			imgproc.Fade(0.0f, 0.8f, 5, Color.Aqua);
			while (!imgproc.ProcessComplete) { }
			Assert.True(imgproc.ProcessComplete);
			Assert.AreEqual(5, imgproc.ProcessedImages.Count);
		}
	}
}
