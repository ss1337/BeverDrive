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
	public enum State
	{
		StartOfGame,
		Playing,
		Paused,
		LevelStart,
		LevelEnd,
		PlayersDied
	}

	public class GameState
	{
		private Random rnd;
		private bool waitForClick;

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
		public int NumberOfPlayers { get; set; }

		/// <summary>
		/// Data for the current level
		/// </summary>
		public Level CurrentLevel { get; set; }

		/// <summary>
		/// Current foodplupp to be eaten
		/// </summary>
		public Point Food { get; set; }

		/// <summary>
		/// How many foods?
		/// </summary>
		public int FoodCount { get; set; }

		/// <summary>
		/// Which state is the game in
		/// </summary>
		public State State { get; set; }

		/// <summary>
		/// Text in the middle
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// All the walls to be rendered in the current level
		/// </summary>
		public List<Point> Walls { get; set; }

		public GameState()
		{
			rnd = new Random();
			this.Player1 = new Player();
			this.Player2 = new Player();
			this.Player1.Brush = new SolidBrush(Color.BlueViolet);
			this.Player2.Brush = new SolidBrush(Color.Green);
			this.State = State.StartOfGame;
			this.Walls = new List<Point>();
		}

		public void CheckCollision()
		{
			this.CheckCollision(Player1, Player2);
			this.CheckCollision(Player2, Player1);
		}

		public void CheckCollision(Player player, Player otherPlayer)
		{
			if (player.Active)
			{
				Point p = new Point(player.HeadPositionX, player.HeadPositionY);
				if (this.Walls.Any(x => x.Equals(p)) || player.Positions.Any(x => x.Equals(p)))
				{
					// Player hit a wall and died
					player.Speed = 0;
				}

				// Since we render the head separately we have to check for that too
				if (otherPlayer.Active && player.HeadPositionX == otherPlayer.HeadPositionX && player.HeadPositionY == otherPlayer.HeadPositionY)
				{
					player.Speed = 0;
				}

				// If other player is active, check for collisions there
				if (otherPlayer.Active && otherPlayer.Positions.Any(x => x.Equals(p)))
				{
					player.Speed = 0;
				}

				if (p == this.Food)
				{
					// Player ate a plupp
					player.AddLength = this.FoodCount;
					player.Score += this.FoodCount;
					player.UpdateText();
					this.FoodCount++;

					if (FoodCount < 11)
					{
						this.GenerateFood();
					}
					else
					{
						// Next level
						this.State = State.LevelEnd;
						WaitForClick("Level complete - click knob to continue");
					}
				}
			}
		}

		public bool Click()
		{
			if (waitForClick)
			{
				waitForClick = false;
				this.Text = "";

				switch (this.State)
				{
					case State.LevelStart:
						this.State = State.Playing;
						break;
					case State.LevelEnd:
						this.StartLevel(BeverDrive.Modules.Nubbles.LevelLibrary.GetNextLevel());
						break;
				}
			}
			else
			{
				if (Player1.Speed < 1)
				{
					if (Player1.Lives > 0)
					{
						Player1.Spawn(CurrentLevel.Player1Start, CurrentLevel.Player1Heading);
					}
					else
					{
						Player1.Reset("");
						Player1.Spawn(CurrentLevel.Player1Start, CurrentLevel.Player1Heading);
					}
				}
				else
					if (this.State == State.Playing)
					{
						this.State = State.Paused;
						return true;
					}
			}

			return false;
		}

		public void GenerateFood()
		{
			bool generated = false;
			Point p = new Point(0, 0);

			while (!generated)
			{
				p = new Point(rnd.Next(79), rnd.Next(49));
				if (!this.Walls.Any(x => x.Equals(p))
					&& !this.Player1.Positions.Any(x => x.Equals(p))
					&& !this.Player2.Positions.Any(x => x.Equals(p)))
					generated = true;
			}

			this.Food = p;
		}

		public void StartLevel(Level level)
		{
			this.CurrentLevel = level;
			this.Walls = level.Walls;
			this.Player1.Spawn(level.Player1Start, level.Player1Heading);
			this.Player2.Spawn(level.Player2Start, level.Player2Heading);
			this.FoodCount = 1;
			this.GenerateFood();

			Player1.Active = true;

			if (NumberOfPlayers == 2)
				Player2.Active = true;

			this.State = State.LevelStart;
			WaitForClick(level.Name + " - Click knob to start");
		}

		public void StartLevel1(int numPlayers)
		{
			Player1.Reset("Player 1");
			Player1.Reset("Player 2");
			NumberOfPlayers = numPlayers;
			this.StartLevel(BeverDrive.Modules.Nubbles.LevelLibrary.GetLevel9());
		}

		public void WaitForClick(string text)
		{
			this.Text = text;
			this.waitForClick = true;
		}

		public void Update()
		{
			if (!waitForClick)
			{
				this.Player1.Move();
				this.Player2.Move();
				this.CheckCollision();
			}
		}
	}
}
