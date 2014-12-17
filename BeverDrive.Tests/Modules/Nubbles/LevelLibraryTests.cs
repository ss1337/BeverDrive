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
using BeverDrive.Modules.Nubbles;
using NUnit.Framework;

namespace BeverDrive.Tests.Modules.Nubbles
{
	[TestFixture]
	public class LevelLibraryTests
	{
		public LevelLibraryTests()
		{
		}

		[Test]
		public void Levels_are_in_correct_order()
		{
			Level lvl = LevelLibrary.GetLevel1();
			for (int i = 1; i < 10; i++)
			{
				Assert.AreEqual("Level " + i, lvl.Name);
				lvl = LevelLibrary.GetNextLevel();

				if (i == 9)
					Assert.AreEqual("Level 1", lvl.Name);
			}
		}

		[Test]
		public void Line_draws_a_line_between_points()
		{
			var p1 = new Point(5, 5);
			var p2 = new Point(5, 15);
			var line = LevelLibrary.Line(p1.X, p1.Y, p2.X, p2.Y);
			Assert.AreEqual(11, line.Count());
			
			int i = 5;
			int j = 5;
			for(i = 0; i < 11; i++)
			{
				Assert.AreEqual(5, line[i].X);
				Assert.AreEqual(j, line[i].Y);
				j++;
			}
		}
	}
}
