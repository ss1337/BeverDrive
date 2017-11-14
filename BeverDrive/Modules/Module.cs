//
// Copyright 2017 Sebastian Sjödin
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
using BeverDrive.Core;
using BeverDrive.Gui.Controls;

namespace BeverDrive.Modules
{
	public class Module
	{
		private bool handlesSelection;
		private int firstIndex;
		private int lastIndex;

		/// <summary>
		/// This is a list containing all the controls of the current module
		/// </summary>
		public List<AGraphicsControl> Controls { get; set; }

		/// <summary>
		/// Selected index for the current module
		/// </summary>
		public int SelectedIndex { get; set; }

		/// <summary>
		/// Settings for this module
		/// </summary>
		public IEnumerable<KeyValuePair<string, string>> Settings { get; set; }

		/// <summary>
		/// Is set if the module is visible
		/// </summary>
		public bool Visible { get; set; }

		public Module()
		{
			this.Controls = new List<AGraphicsControl>();

			// Check if this module handles selection on its own
			foreach (object attrib in this.GetType().GetCustomAttributes(false))
			{
				if (attrib is HandlesSelectionAttribute)
					this.handlesSelection = ((HandlesSelectionAttribute)attrib).HandlesSelection;
			}
		}

		/// <summary>
		/// This is executed when the back button is pressed
		/// </summary>
		public virtual void Back() { }

		/// <summary>
		/// A module's constructor is executed before the settings for that module are loaded, so
		/// never have any code that is dependent on settings in the constructor, instead have that
		/// in Init, since it is executed after the settings are loaded
		/// </summary>
		public virtual void Init() { }

		/// <summary>
		/// Process messages from the Ibus directly
		/// </summary>
		/// <param name="message"></param>
		public virtual void ProcessMessage(string message) { }

		/// <summary>
		/// Command event handler
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnCommand(ModuleCommandEventArgs e)
		{
			switch (e.Command)
			{
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					BeverDriveContext.CurrentCoreGui.ClearModuleContainer();
					break;
				case ModuleCommands.SelectClick:
					this.Click();
					break;
				case ModuleCommands.SelectLeft:
					if (!this.handlesSelection)
					{
						this.SelectedIndex++;
						this.UpdateSelectedIndex();
					}
					break;
				case ModuleCommands.SelectRight:
					if (!this.handlesSelection)
					{
						this.SelectedIndex--;
						this.UpdateSelectedIndex();
					}
					break;
			}

			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		/// <summary>
		/// Is executed every tick of the 1Hz timer
		/// </summary>
		public virtual void Update1Hz() { }

		/// <summary>
		/// Is executed every tick of the 50Hz timer
		/// </summary>
		public virtual void Update50Hz() { }

		private void Click()
		{
			// Invoke click handler on the currently selected control
			AGraphicsControl ctrl = BeverDriveContext.CurrentCoreGui.ModuleContainer.GraphicControls.FirstOrDefault(c =>
			{
				return (c.Index == this.SelectedIndex);
			});

			if (BeverDriveContext.CurrentCoreGui.BackButton.Selected)
				this.Back();

			if (ctrl != null)
				ctrl.RaiseClick(this, new EventArgs());
		}

		private void Show()
		{
			this.firstIndex = 0;

			// Add all the controls to the GUI
			foreach (var ctrl in this.Controls)
			{
				if (ctrl.Index < this.firstIndex)
					this.firstIndex = ctrl.Index;

				if (ctrl.Index > this.lastIndex)
					this.lastIndex = ctrl.Index;

				ctrl.Selected = false;
				BeverDriveContext.CurrentCoreGui.AddControl(ctrl);
			}

			// Reflection to show backbutton
			foreach (object attrib in this.GetType().GetCustomAttributes(false))
			{
				if (attrib is BackButtonVisibleAttribute)
					if (((BackButtonVisibleAttribute)attrib).BackButtonVisible)
					{
						BeverDriveContext.CurrentCoreGui.BackButton.Index = --this.firstIndex;
						BeverDriveContext.CurrentCoreGui.AddControl(BeverDriveContext.CurrentCoreGui.BackButton);
					}
			}

			this.SelectedIndex = 0;
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
		}

		private void UpdateSelectedIndex()
		{
			// Don't scroll past the back button
			if (this.SelectedIndex < this.firstIndex)
				this.SelectedIndex = this.firstIndex;

			// Select and unselect stuffs
			foreach(AGraphicsControl c in BeverDriveContext.CurrentCoreGui.ModuleContainer.GraphicControls)
			{
				c.Selected = false;
				if (c.Index == this.SelectedIndex)
				{
					c.Selected = true;
					c.RaiseHover(this, new EventArgs());
				}
			}

			// Update stuffs
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = (BeverDriveContext.CurrentCoreGui.BackButton.Index == this.SelectedIndex);
			if (BeverDriveContext.CurrentCoreGui.BackButton.Selected)
				BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Back...";

			BeverDriveContext.CurrentCoreGui.Invalidate();
		}
	}
}
