//
// Copyright 2012-2014 Sebastian Sjödin
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
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Modules
{
	[BackButtonVisible(false)]
	public class MainMenu : AModule
	{
		private int gridLeft = 1;
		private int gridTop = 2;
		private int selectedIndex = 0;

		private Label lbl_title;
		private List<MetroidButton> buttons;
		private List<string> buttonTypes;

		public MainMenu()
		{
			this.buttons = new List<MetroidButton>();
			this.buttonTypes = new List<string>();
		}

		public override void Init()
		{
			// Check if there are any buttons loaded
			if (this.buttons.Count == 0)
				this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch(e.Command)
			{
				case ModuleCommands.SelectClick:
					BeverDriveContext.SetActiveModule(this.buttonTypes[this.selectedIndex]);
					break;
				case ModuleCommands.SelectNext:
					if (selectedIndex < this.buttons.Count - 1)
						selectedIndex++;

					this.Update();
					break;
				case ModuleCommands.SelectPrevious:
					if (selectedIndex > 0)
						selectedIndex--;

					this.Update();
					break;
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					this.Hide();
					break;
			}
		}

		private void Show()
		{
			// Add controls to the module container
			this.buttons.Any(x => { BeverDriveContext.CurrentCoreGui.AddControl(x); return false; });
			this.ShowControls();
			this.Update();
		}

		private void Hide()
		{
			BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
		}

		private void Update()
		{
			this.buttons.Any(x => { x.Selected = false; return false; });
			this.buttons[this.selectedIndex].Selected = true;
			BeverDriveContext.CurrentCoreGui.ClockContainer.Text = this.buttons[this.selectedIndex].Text;
			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		private void CreateButton(string text, Type moduleType, string icon, string selectedIcon)
		{
			// Create and add button
			var mb = new MetroidButton(icon, selectedIcon);
			mb.GridLeft = gridLeft;
			mb.GridTop = gridTop;
			mb.Text = text;
			this.buttons.Add(mb);
			this.buttonTypes.Add(moduleType.Name);

			// Increment grid
			this.gridLeft += 6;
			if (gridLeft > 13)
			{
				gridLeft = 1;
				gridTop += 6;
			}
		}

		private void CreateControls()
		{
			foreach (var kvp in this.Settings)
			{
				if (kvp.Key.StartsWith("MenuItem"))
					switch(kvp.Value)
					{
						case "BeverDrive.Modules.Bluetooth":
							this.CreateButton("Bluetooth", typeof(Bluetooth), "Resources\\bluetooth.png", "Resources\\bluetooth_s.png");
							break;

						case "BeverDrive.Modules.Mp3Player":
							this.CreateButton("Music player", typeof(Mp3Player), "Resources\\music.png", "Resources\\music_s.png");
							break;

						case "BeverDrive.Modules.NubblesModule":
							this.CreateButton("Nubbles", typeof(NubblesModule), "Resources\\nubbles.png", "Resources\\nubbles_s.png");
							break;

						case "BeverDrive.Modules.VideoPlayer":
							this.CreateButton("Video player", typeof(VideoPlayer), "Resources\\video.png", "Resources\\video_s.png");
							break;

						default:
							Type t = Type.GetType(kvp.Value);
							this.CreateButton(t.Name, t, "Resources\\settings.png", "Resources\\settings_s.png");
							break;
					}
			}

			this.lbl_title = new Label();
			this.lbl_title.Font = Fonts.GuiFont36;
			this.lbl_title.ForeColor = Colors.SelectedColor;
			this.lbl_title.Location = new System.Drawing.Point(BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 200, 16);
			this.lbl_title.Size = new System.Drawing.Size(400, 50);
			this.lbl_title.Text = "BeverDrive";
			this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		}
	}
}
