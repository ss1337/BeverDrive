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

namespace BeverDrive.Modules.Nubbles
{
	public class LevelLibrary
	{
		public static int LevelCount = 1;
		public static Func<Level> GetNextLevel;

		public static List<Point> Line(int x1, int y1, int x2, int y2)
		{
			List<Point> res = new List<Point>();

			if (x1 == x2)
				for (int i = y1; i < y2 + 1; i++)
					res.Add(new Point(x1, i));

			if (y1 == y2)
				for (int i = x1; i < x2 + 1; i++)
					res.Add(new Point(i, y1));

			return res;
		}

		public static List<Point> Walls()
		{
			// Generate border list
			var border = new List<Point>();
			for (int i = 0; i < 80; i++)
			{
				border.Add(new Point(i, 0));
				border.Add(new Point(i, 49));
			}

			for (int i = 0; i < 50; i++)
			{
				border.Add(new Point(0, i));
				border.Add(new Point(79, i));
			}

			return border;
		}

		public static Level GetLevel1()
		{
			var result = new Level();
			result.Name = "Level 1";
			result.Walls = Walls();
			result.Player1Heading = 3;
			result.Player1Start = new Point(60, 20);
			result.Player2Heading = 1;
			result.Player2Start = new Point(20, 30);
			GetNextLevel = GetLevel2;
			return result;
		}

		public static Level GetLevel2()
		{
			var result = new Level();
			result.Name = "Level 2";
			result.Walls = Walls();

			var line = Line(20, 24, 60, 24);
			line.Any(p => { result.Walls.Add(p); return false; });

			result.Player1Heading = 3;
			result.Player1Start = new Point(60, 15);
			result.Player2Heading = 1;
			result.Player2Start = new Point(20, 35);
			GetNextLevel = GetLevel3;
			return result;
		}

		public static Level GetLevel3()
		{
			var result = new Level();
			result.Name = "Level 3";
			result.Walls = Walls();

			for (int i = 11; i < 41; i++)
			{
				result.Walls.Add(new Point(20, i));
				result.Walls.Add(new Point(60, i));
			}

			result.Player1Heading = 0;
			result.Player1Start = new Point(50, 24);
			result.Player2Heading = 2;
			result.Player2Start = new Point(30, 24);
			GetNextLevel = GetLevel4;
			return result;
		}

		public static Level GetLevel4()
		{
			var result = new Level();
			result.Name = "Level 4";
			result.Walls = Walls();

			for (int i = 1; i < 30; i++)
			{
				result.Walls.Add(new Point(i, 15));
				result.Walls.Add(new Point(79 - i, 34));
			}

			for (int i = 1; i < 20; i++)
			{
				result.Walls.Add(new Point(64, i));
				result.Walls.Add(new Point(15, 49 - i));
			}

			result.Player1Heading = 3;
			result.Player1Start = new Point(60, 7);
			result.Player2Heading = 1;
			result.Player2Start = new Point(20, 43);
			GetNextLevel = GetLevel5;
			return result;
		}

		public static Level GetLevel5()
		{
			var result = new Level();
			result.Name = "Level 5";
			result.Walls = Walls();

			for (int i = 12; i < 39; i++)
			{
				result.Walls.Add(new Point(20, i));
				result.Walls.Add(new Point(58, i));
			}

			for (int i = 22; i < 57; i++)
			{
				result.Walls.Add(new Point(i, 10));
				result.Walls.Add(new Point(i, 40));
			}

			result.Player1Heading = 0;
			result.Player1Start = new Point(50, 24);
			result.Player2Heading = 2;
			result.Player2Start = new Point(30, 24);
			GetNextLevel = GetLevel6;
			return result;
		}

		public static Level GetLevel6()
		{
			var result = new Level();
			result.Name = "Level 6";
			result.Walls = Walls();

			for (int i = 1; i < 49; i++)
			{
				if (i > 29 || i < 22)
				{
					result.Walls.Add(new Point(9, i));
					result.Walls.Add(new Point(19, i));
					result.Walls.Add(new Point(29, i));
					result.Walls.Add(new Point(39, i));
					result.Walls.Add(new Point(49, i));
					result.Walls.Add(new Point(59, i));
					result.Walls.Add(new Point(69, i));
				}
			}

			result.Player1Heading = 2;
			result.Player1Start = new Point(64, 6);
			result.Player2Heading = 0;
			result.Player2Start = new Point(14, 42);
			GetNextLevel = GetLevel7;
			return result;
		}

		public static Level GetLevel7()
		{
			var result = new Level();
			result.Name = "Level 7";
			result.Walls = Walls();

			for (int i = 2; i < 49; i+=2)
				result.Walls.Add(new Point(40, i));

			result.Player1Heading = 2;
			result.Player1Start = new Point(64, 6);
			result.Player2Heading = 0;
			result.Player2Start = new Point(14, 42);
			GetNextLevel = GetLevel8;
			return result;
		}

		public static Level GetLevel8()
		{
			var result = new Level();
			result.Name = "Level 8";
			result.Walls = Walls();

			for (int i = 4; i < 30; i++)
			{
				result.Walls.Add(new Point(9, i));
				result.Walls.Add(new Point(52 -i, 19));
				result.Walls.Add(new Point(29, i));
				result.Walls.Add(new Point(52 - i, 39));
				result.Walls.Add(new Point(49, i));
				result.Walls.Add(new Point(52 - i, 59));
				result.Walls.Add(new Point(69, i));
			}

			result.Player1Heading = 2;
			result.Player1Start = new Point(64, 6);
			result.Player2Heading = 0;
			result.Player2Start = new Point(14, 42);
			GetNextLevel = GetLevel9;
			return result;
		}

		public static Level GetLevel9()
		{
			var result = new Level();
			result.Name = "Level 9";
			result.Walls = Walls();
			/*
			 * FOR i = 6 TO 47
				Set i, i, colorTable(3)
				Set i, i + 28, colorTable(3)
			NEXT i
			 * */
			for (int i = 5; i < 46; i++)
			{
				result.Walls.Add(new Point(i, i));
				result.Walls.Add(new Point(i + 28, i));
			}

			result.Player1Heading = 2;
			result.Player1Start = new Point(64, 6);
			result.Player2Heading = 0;
			result.Player2Start = new Point(14, 42);
			GetNextLevel = GetLevel1;
			return result;
		}

	}
}
