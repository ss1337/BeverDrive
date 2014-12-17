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
	public class Player
	{
		private string playerText;
		/*private int respawnX;
		private int respawnY;*/
		private int respawnLength = 2;

		/// <summary>
		/// Is this player active?
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// Length to be added, if > 0
		/// </summary>
		public int AddLength { get; set; }

		/// <summary>
		/// Brush to paint the player with
		/// </summary>
		public Brush Brush { get; set; }

		/// <summary>
		/// 0 is north, 1 is east, 2 is south, 3 is west
		/// </summary>
		public int Heading { get; set; }

		/// <summary>
		/// Current length of snaaake
		/// </summary>
		public int Length { get; set; }

		/// <summary>
		/// Number of lives left
		/// </summary>
		public int Lives { get; set; }

		/// <summary>
		/// Position of the head
		/// </summary>
		public int HeadPositionX { get; set; }
		public int HeadPositionY { get; set; }

		/// <summary>
		/// Collection of all the positions the snake occupies
		/// </summary>
		public Queue<Point> Positions { get; set; }

		/// <summary>
		/// Current score of player
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// Current speed of player, if speed < 1 then the player has died
		/// </summary>
		public int Speed { get; set; }

		/// <summary>
		/// Text correlated with player
		/// </summary>
		public string Text { get; set; }

		public Player()
		{
			this.Positions = new Queue<Point>();
			this.Speed = 1;
		}

		public void Move()
		{
			if (this.Active)
			{
				if (this.Speed < 1)
				{
					if (this.Speed == -3)
					{
						// Dead and gone
					}
					else
					{
						ExecuteDeath();
					}
				}
				else
				{
					this.Positions.Enqueue(new Point(this.HeadPositionX, this.HeadPositionY));

					if (this.AddLength > 0)
						this.AddLength--;
					else
						this.Positions.Dequeue();

					// Move player
					switch (this.Heading)
					{
						case 0:
							this.HeadPositionY--;
							break;
						case 1:
							this.HeadPositionX++;
							break;
						case 2:
							this.HeadPositionY++;
							break;
						case 3:
							this.HeadPositionX--;
							break;
					}
				}
			}
		}

		public void Reset(string playerText)
		{
			this.Length = 1;
			this.Lives = 3;
			this.Score = 0;
			
			if (!string.IsNullOrEmpty(playerText))
				this.playerText = playerText;
			
			this.UpdateText();
		}

		public void Spawn(Point start, int heading)
		{
			this.AddLength = respawnLength;
			this.Heading = heading;
			this.HeadPositionX = start.X;
			this.HeadPositionY = start.Y;
			this.Positions.Clear();
			this.Length = 1;
			this.Lives -= 1;
			this.Speed = 1;
			this.UpdateText();
		}

		public void TurnLeft()
		{
			if (this.Active)
			{
				if (this.Heading == 0)
					this.Heading = 3;
				else
					this.Heading--;
			}
		}

		public void TurnRight()
		{
			if (this.Active)
			{
				if (this.Heading == 3)
					this.Heading = 0;
				else
					this.Heading++;
			}
		}

		public void ExecuteDeath()
		{
			if (this.Speed == -2)
				this.Speed = 0;

			if (this.Speed == -1)
				this.Speed = -2;

			if (this.Speed == 0)
			{
				if (this.Positions.Count < 2)
				{
					this.Positions.Clear();
					this.Speed = -3;

					if (this.Lives > 0)
						this.Text = playerText + "\nClick knob to respawn";
					else
						this.Text = playerText + "\nClick knob to continue";
				}
				else
				{
					// Player hit a wall and died
					var tmpList = this.Positions.Where((p, i) => i % 2 == 0).ToList();
					this.Positions.Clear();
					tmpList.Any(p => { this.Positions.Enqueue(p); return false; });
					this.Speed = -1;
				}
			}
		}

		public void UpdateText()
		{
			this.Text = string.Format("{0}\nScore: {1}, Lives: {2}", playerText, Score, Lives);
		}
	}
}
