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

namespace BeverDrive.Gui.Controls
{
	/// <summary>
	/// Control for the Nübbles game
	/// </summary>
	public class NubblesControl : AGraphicsControl
	{
		private Point gameLocation;
		private int gameWidth;
		private int gameHeight;

		private int gridSize;
		private Brush blackBrush;
		private Brush foodBrush;
		private Brush wallBrush;
		private Font playerFont;
		private StringFormat leftFormat;
		private StringFormat rightFormat;

		public BeverDrive.Modules.Nubbles.GameState GameState { get; set; }

		public NubblesControl()
		{
			this.blackBrush = new SolidBrush(Color.Black);
			this.foodBrush = new SolidBrush(Color.HotPink);
			this.playerFont = BeverDrive.Gui.Styles.Fonts.GuiFont14;
			this.wallBrush = BeverDrive.Gui.Styles.Brushes.ForeBrush;
			this.GameState = new BeverDrive.Modules.Nubbles.GameState();

			leftFormat = new StringFormat();
			leftFormat.Alignment = StringAlignment.Near;
			leftFormat.LineAlignment = StringAlignment.Near;

			rightFormat = new StringFormat();
			rightFormat.Alignment = StringAlignment.Far;
			rightFormat.LineAlignment = StringAlignment.Near;
		}

		/// <summary>
		/// Calculate grid size and center playing field on screen
		/// </summary>
		public void Initialize()
		{
			int offsetLeft = 0;
			int offsetTop = 0;

			// Calculate grid size
			if ((this.Width / 80) > (this.Height / 50))
				gridSize = (this.Height / 50);
			else
				gridSize = (this.Width / 80);

			offsetLeft = (this.Width - gridSize * 80) / 2;
			offsetTop = (this.Height - gridSize * 50) - 46;

			//this.Width = gridSize * 80;
			//this.Height = gridSize * 50 + 46;
			//this.Location = new Point(offsetLeft, offsetTop);
			gameWidth = gridSize * 80;
			gameHeight = gridSize * 50 + 46;
			gameLocation = new Point(offsetLeft, offsetTop);
		}

		/// <summary>
		/// Render the current game frame
		/// </summary>
		/// <param name="graphic"></param>
		public override void PaintToBuffer(Graphics graphic)
		{
			if (this.gridSize == 0)
				this.Initialize();

			graphic.FillRectangle(this.blackBrush, this.ClientRectangle);
			this.RenderWalls(graphic, GameState.Walls);
			
			if (GameState.State == BeverDrive.Modules.Nubbles.State.Playing)
			{
				this.RenderPlayer(graphic, GameState.Player1, true);
				this.RenderPlayer(graphic, GameState.Player2, false);

				if (GameState.Food != new Point(0, 0))
					graphic.FillRectangle(this.foodBrush, this.PointToRectangle(GameState.Food));
			}

			if (!string.IsNullOrEmpty(GameState.Text))
				graphic.DrawString(
					GameState.Text,
					playerFont,
					new SolidBrush(BeverDrive.Gui.Styles.Colors.SelectedColor),
					new Point(gameWidth / 2 + 20, gameHeight),
					BeverDrive.Gui.Styles.Fonts.Centered);
		}

		private void RenderPlayer(Graphics graphic, BeverDrive.Modules.Nubbles.Player player, bool right)
		{
			if (player.Active)
			{
				Point p;
				
				// Render player text, to the left if player 2
				if (right)
				{
					p = new Point(gameWidth + 25, gameHeight - 12);
					graphic.DrawString(player.Text, playerFont, player.Brush, p, rightFormat);
				}
				else
				{
					p = new Point(gameLocation.X, gameHeight - 12);
					graphic.DrawString(player.Text, playerFont, player.Brush, p, leftFormat);
				}

				// Render player positions
				foreach (var pos in player.Positions)
					graphic.FillRectangle(player.Brush, this.PointToRectangle(pos));

				// Render player head separately to ease collision detecting
				// Render head only if it is alive
				if (player.Speed > 0)
					graphic.FillRectangle(player.Brush, this.PointToRectangle(new Point(player.HeadPositionX, player.HeadPositionY)));
			}
		}

		private void RenderWalls(Graphics graphic, List<Point> points)
		{
			foreach (var p in points)
				graphic.FillRectangle(wallBrush, this.PointToRectangle(p));
		}

		private Rectangle PointToRectangle(Point point)
		{
			return new Rectangle(point.X * gridSize + gameLocation.X, point.Y * gridSize + gameLocation.Y, gridSize, gridSize);
		}
	}
}
