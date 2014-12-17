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
	public class GameStateTests
	{
		GameState gamestate;

		public GameStateTests()
		{
		}

		[Test]
		public void CheckCollision_detects_headon_collision()
		{
			gamestate = new GameState();
			gamestate.NumberOfPlayers = 2;
			gamestate.StartLevel(LevelLibrary.GetLevel1());

			Assert.Greater(gamestate.Player1.Speed, 0);
			Assert.Greater(gamestate.Player2.Speed, 0);
			gamestate.Player1.Positions.Clear();
			for (int i = 30; i < 35; i++)
				gamestate.Player1.Positions.Enqueue(new Point(i, 25));

			gamestate.Player1.HeadPositionX = 29;
			gamestate.Player1.HeadPositionY = 25;

			gamestate.Player2.Positions.Clear();
			for (int i = 24; i < 29; i++)
				gamestate.Player1.Positions.Enqueue(new Point(i, 25));

			gamestate.Player2.HeadPositionX = 29;
			gamestate.Player2.HeadPositionY = 25;

			gamestate.CheckCollision();
			Assert.AreEqual(0, gamestate.Player1.Speed);
			Assert.AreEqual(0, gamestate.Player2.Speed);
		}

		[Test]
		public void CheckCollision_detects_player_in_player()
		{
			gamestate = new GameState();
			gamestate.NumberOfPlayers = 2;
			gamestate.StartLevel(LevelLibrary.GetLevel1());

			Assert.Greater(gamestate.Player1.Speed, 0);
			Assert.Greater(gamestate.Player2.Speed, 0);
			gamestate.Player1.Positions.Clear();
			for (int i = 30; i < 35; i++)
				gamestate.Player1.Positions.Enqueue(new Point(i, 25));

			gamestate.Player1.HeadPositionX = 29;
			gamestate.Player1.HeadPositionY = 25;

			gamestate.Player2.HeadPositionX = 32;
			gamestate.Player2.HeadPositionY = 25;
			gamestate.CheckCollision();
			Assert.Greater(gamestate.Player1.Speed, 0);
			Assert.AreEqual(0, gamestate.Player2.Speed);
		}

		[Test]
		public void CheckCollision_detects_player_in_wall()
		{
			gamestate = new GameState();
			gamestate.NumberOfPlayers = 2;
			gamestate.StartLevel(LevelLibrary.GetLevel1());

			Assert.Greater(gamestate.Player1.Speed, 0);
			Assert.Greater(gamestate.Player2.Speed, 0);
			gamestate.Player1.HeadPositionX = 79;
			gamestate.Player1.HeadPositionY = 30;
			gamestate.Player2.HeadPositionX = 79;
			gamestate.Player2.HeadPositionY = 10;
			gamestate.CheckCollision();
			Assert.AreEqual(0, gamestate.Player1.Speed);
			Assert.AreEqual(0, gamestate.Player2.Speed);
		}
	}
}
