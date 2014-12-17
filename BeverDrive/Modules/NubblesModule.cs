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
using System.Linq;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Modules
{
	public enum MenuState
	{
		MainMenu,
		PlayingMenu,
		Playing
	}

	[BackButtonVisible(true)]
	public partial class NubblesModule : AModule
	{
		private int numPlayers;
		public MenuState MenuState { get; set; }

		public int SelectedIndex
		{
			get { return ctrlMenu.SelectedIndex; }
			set { ctrlMenu.SelectedIndex = value; }
		}

		public NubblesModule()
		{
			MenuState = MenuState.MainMenu;
			numPlayers = 1;
			InitControls();
			ShowMainMenu();
		}

		private void gameTimer_Tick(object sender, EventArgs e)
		{
			if (ctrlGame.Visible && MenuState == MenuState.Playing)
			{
				ctrlGame.GameState.Update();
				BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
			}
		}

		public override void Init()
		{
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			if (MenuState != MenuState.Playing && 
				(e.Command == ModuleCommands.SelectClick ||
				e.Command == ModuleCommands.SelectNext ||
				e.Command == ModuleCommands.SelectPrevious))
			{
				if (MenuState == MenuState.MainMenu)
					SwitchMainMenu(e.Command);

				if (MenuState == MenuState.PlayingMenu)
					SwitchPlayingMenu(e.Command);
			}
			else
				switch (e.Command)
				{
					case ModuleCommands.SelectClick:
						if (ctrlGame.GameState.Click() == true)
						{
							SelectedIndex = 0;
							ShowPlayingMenu();
							ctrlMenu.Visible = true;
						}
						break;

					case ModuleCommands.SelectPrevious:
						this.ctrlGame.GameState.Player1.TurnLeft();
						break;

					case ModuleCommands.SelectNext:
						this.ctrlGame.GameState.Player1.TurnRight();
						break;
					case ModuleCommands.Show:
						this.ShowControls();
						this.ctrlGame.Visible = true;
						this.SelectedIndex = 0;
						ctrlGame.GameState.Walls = BeverDrive.Modules.Nubbles.LevelLibrary.Walls();
						gameTimer.Start();
						break;

					case ModuleCommands.Hide:
						BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
						this.ctrlGame.Visible = false;
						gameTimer.Stop();
						break;
				}
		}

		public override void ProcessMessage(string message)
		{
		}

		public override void Update1Hz()
		{
		}

		private void ShowMainMenu()
		{
			MenuState = MenuState.MainMenu;
			ctrlMenu.Items.Clear();
			ctrlMenu.Items.Add("Number of players: " + numPlayers);
			ctrlMenu.Items.Add("Start");

			BeverDriveContext.CurrentCoreGui.BackButton.Selected = (this.SelectedIndex == -1) ? true : false;
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
		}

		private void ShowPlayingMenu()
		{
			MenuState = MenuState.PlayingMenu;
			ctrlMenu.Items.Clear();
			ctrlMenu.Items.Add("Resume");
			ctrlMenu.Items.Add("Exit to menu");

			BeverDriveContext.CurrentCoreGui.BackButton.Selected = (this.SelectedIndex == -1) ? true : false;
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
		}

		private void SwitchMainMenu(ModuleCommands command)
		{
			switch (command)
			{
				case ModuleCommands.SelectClick:
					if (this.SelectedIndex == -1)
					{
						BeverDriveContext.SetActiveModule("MainMenu");
					}

					if (this.SelectedIndex == 0)
					{
						if (numPlayers == 2)
							numPlayers = 1;
						else
							numPlayers = 2;

						ShowMainMenu();
					}

					if (this.SelectedIndex == 1)
					{
						// Start game
						ctrlTitle.Visible = false;
						ctrlMenu.Visible = false;
						ctrlGame.GameState.StartLevel1(numPlayers);
						MenuState = MenuState.Playing;
					}

					break;

				case ModuleCommands.SelectPrevious:
					if (this.SelectedIndex > -1)
						this.SelectedIndex--;

					ShowMainMenu();
					break;

				case ModuleCommands.SelectNext:
					if (this.SelectedIndex < 1)
						this.SelectedIndex++;

					ShowMainMenu();
					break;
			}
		}

		private void SwitchPlayingMenu(ModuleCommands command)
		{
			switch (command)
			{
				case ModuleCommands.SelectClick:
					if (this.SelectedIndex == -1)
					{
						BeverDriveContext.SetActiveModule("MainMenu");
					}

					if (this.SelectedIndex == 0)
					{
						ctrlMenu.Visible = false;
						ctrlGame.GameState.State = BeverDrive.Modules.Nubbles.State.Playing;
						MenuState = MenuState.Playing;
					}

					if (this.SelectedIndex == 1)
					{
						SelectedIndex = 0;
						ShowMainMenu();
					}

					break;

				case ModuleCommands.SelectPrevious:
					if (this.SelectedIndex > -1)
						this.SelectedIndex--;

					ShowPlayingMenu();
					break;

				case ModuleCommands.SelectNext:
					if (this.SelectedIndex < 1)
						this.SelectedIndex++;

					ShowPlayingMenu();
					break;
			}
		}
	}
}

