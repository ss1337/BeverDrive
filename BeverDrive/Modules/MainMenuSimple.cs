//
// Copyright 2012-2017 Sebastian Sjödin
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
	[HandlesSelection(true)]
	public class MainMenuSimple : Module
	{
		private int x;
		private int y;

		private Label lbl_title;
		private List<Label> buttons;

		private int firstRightIdx = 0;
		private int lastLeftIdx = 0;
		private int lastIdx = 0;

		public MainMenuSimple()
		{
			this.buttons = new List<Label>();
		}

		public override void Back()
		{
			throw new NotImplementedException();
		}

		public override void Init()
		{
			this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			base.OnCommand(e);

			switch(e.Command)
			{
				case ModuleCommands.Hide:
					this.lastIdx = this.SelectedIndex;
					break;

				case ModuleCommands.Show:
					this.SelectedIndex = this.lastIdx;
					this.Update();
					break;

				case ModuleCommands.SelectRight:
					if (RightSide(this.buttons[SelectedIndex]))
					{
						if (SelectedIndex == this.buttons.Count - 1)
						{
							// Last item on the left side
							SelectedIndex = lastLeftIdx;
						}
						else
						{
							SelectedIndex++;
						}

						this.Update();
						break;
					}

					if (!RightSide(this.buttons[SelectedIndex]))
					{
						if (SelectedIndex == 0)
						{
							// Find the first item on the right side, if it exists
							if (firstRightIdx == -1)
								SelectedIndex = this.buttons.Count - 1;
							else
								SelectedIndex = firstRightIdx;
						}
						else
						{
							SelectedIndex--;
						}

						this.Update();
						break;
					}
					break;
				case ModuleCommands.SelectLeft:
					if (RightSide(this.buttons[SelectedIndex]))
					{
						if (SelectedIndex == firstRightIdx)
							SelectedIndex = 0;
						else
							SelectedIndex--;

						this.Update();
						break;
					}

					if (!RightSide(this.buttons[SelectedIndex]))
					{
						if (SelectedIndex == lastLeftIdx)
						{
							if (firstRightIdx != -1)
								SelectedIndex = this.buttons.Count - 1;
							else
								SelectedIndex = 0;
						}
						else
						{
							SelectedIndex++;
						}

						this.Update();
						break;
					}

					break;
			}
		}

		private void Update()
		{
			for (int i = 0; i < this.buttons.Count; i++)
			{
				if (i == SelectedIndex)
					this.buttons[i].ForeColor = Colors.SelectedColor;
				else
					this.buttons[i].ForeColor = Colors.ForeColor;
			}

			BeverDriveContext.CurrentCoreGui.ClockContainer.Text = this.buttons[this.SelectedIndex].Text;
			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		private void CreateButton(string text, Type moduleType, int index, int x, int y)
		{
			// Create and add menu choices
			var lb = new Label();
			lb.Font = Fonts.GuiFont28;
			lb.ForeColor = Colors.ForeColor;
			lb.Index = index;
			lb.Location = new System.Drawing.Point(x, y);
			lb.Size = new System.Drawing.Size(400, 50);
			lb.Text = text;
			lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			lb.Click += (sender, e) => { BeverDriveContext.SetActiveModule(moduleType.Name); };
			lb.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = text; };

			this.buttons.Add(lb);
		}

		private void CreateControls()
		{
			int i = 0;
			int x = 20;
			int y = 140;

			foreach (var kvp in this.Settings)
			{
				if (kvp.Key.StartsWith("MenuItem"))
				{
					string name = "";
					Type t = Type.GetType(kvp.Value);

					// Check if the type actually exists
					if (t != null)
					{
						name = t.Name;

						foreach (object attrib in t.GetCustomAttributes(false))
						{
							if (attrib is MenuTextAttribute)
							{
								name = ((MenuTextAttribute)attrib).MenuText;
							}
						}
					}

					this.CreateButton(name, t, i, x, y);

					// Increment x/y, i
					y += 80;
					i++;

					if (y > 310)
					{
						y = 140;
						x = 460;
					}
				}
			}

			// Set indexes
			firstRightIdx = this.buttons.FindIndex(a => RightSide(a));
			lastLeftIdx = this.buttons.FindLastIndex(a => !RightSide(a));

			this.lbl_title = new Label();
			this.lbl_title.Font = Fonts.GuiFont36;
			this.lbl_title.ForeColor = Colors.SelectedColor;
			this.lbl_title.Location = new System.Drawing.Point(BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width / 2 - 200, 16);
			this.lbl_title.Size = new System.Drawing.Size(400, 50);
			this.lbl_title.Text = "BeverDrive";
			this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.buttons.ForEach(b => base.Controls.Add(b));
			base.Controls.Add(lbl_title);
		}

		private bool RightSide(Label label)
		{
			return (label.Location.X > 400);
		}
	}
}
