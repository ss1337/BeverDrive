﻿//
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
	public class Level
	{
		public string Name { get; set; }
		public List<Point> Walls { get; set; }
		public int Player1Heading { get; set; }
		public int Player2Heading { get; set; }
		public Point Player1Start { get; set; }
		public Point Player2Start { get; set; }

		public Level()
		{
		}
	}
}
