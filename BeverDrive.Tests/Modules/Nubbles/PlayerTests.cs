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
	public class PlayerTests
	{
		Player player;

		public PlayerTests()
		{
		}

		public void SetupPlayer()
		{
			player = new Player();
			player.Active = true;
			player.AddLength = 0;
			player.Heading = 0;
			player.HeadPositionX = 5;
			player.HeadPositionY = 10;

			// Add some girth
			int x = 40;
			int y = 10;

			player.Length = 7;

			for (int i = 0; i < 7; i++)
				player.Positions.Enqueue(new Point(x, y + i));

		}

		[Test]
		public void Changes_heading_correctly()
		{
			this.SetupPlayer();
			this.player.TurnLeft();
			Assert.AreEqual(3, player.Heading);
			this.player.TurnLeft();
			Assert.AreEqual(2, player.Heading);
			this.player.TurnLeft();
			Assert.AreEqual(1, player.Heading);
			this.player.TurnLeft();
			Assert.AreEqual(0, player.Heading);
			this.player.TurnRight();
			Assert.AreEqual(1, player.Heading);
			this.player.TurnRight();
			Assert.AreEqual(2, player.Heading);
			this.player.TurnRight();
			Assert.AreEqual(3, player.Heading);
			this.player.TurnRight();
			Assert.AreEqual(0, player.Heading);
		}

		[Test]
		public void Dies_like_a_snake()
		{
			this.SetupPlayer();

			// Death
			// A X B X A X B
			this.player.Speed = 0;
			Assert.AreEqual(7, this.player.Positions.Count);
			this.player.Move();
			Assert.AreEqual(4, this.player.Positions.Count);
			this.player.Move();
			this.player.Move();
			this.player.Move();
			Assert.AreEqual(2, this.player.Positions.Count);
			this.player.Move();
			this.player.Move();
			this.player.Move();
			Assert.AreEqual(0, this.player.Positions.Count);
		}

		[Test]
		public void Reset_works()
		{
			this.player.Reset("Player 1");
			Assert.AreEqual(1, this.player.Length);
			Assert.AreEqual(3, this.player.Lives);
			Assert.AreEqual(0, this.player.Score);
		}

		[Test]
		public void Spawn_works()
		{
			var lives = this.player.Lives;
			this.player.Spawn(new Point(25, 35), 3);
			Assert.AreEqual(3, this.player.Heading);
			Assert.AreEqual(25, this.player.HeadPositionX);
			Assert.AreEqual(35, this.player.HeadPositionY);
			Assert.AreEqual(0, this.player.Positions.Count());
			Assert.AreEqual(1, this.player.Length);
			Assert.AreEqual(lives - 1, this.player.Lives);
			Assert.AreEqual(1, this.player.Speed);
		}
	}
}
