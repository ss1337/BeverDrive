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
using System.Windows.Forms;
using BeverDrive.Core;
using BeverDrive.Modules;
using BeverDrive.Ibus;
using BeverDrive.Ibus.Extensions;

namespace BeverDrive
{
	public partial class MainForm : Form
	{
		public Timer Timer1Hz;
		public Timer Timer50Hz;

		public MainForm()
		{
			InitializeComponent();
			
			// Check if ShowSplashScreen returned an error, if so, don't do anything
			if (this.ShowSplashScreen() == -1)
				return;

			BeverDriveContext.Initialize();
			VlcContext.Initialize(BeverDriveContext.Settings.VlcPath);
			BeverDriveContext.CurrentMainForm = this;
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Modules.CoreGui());
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Modules.MainMenu());
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Modules.Mp3Player());
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Modules.VideoPlayer());

			if (BeverDriveContext.Settings.EnableBluetooth)
				BeverDriveContext.LoadedModules.Add(new BeverDrive.Modules.Bluetooth());

			// Init ibus
			if (BeverDriveContext.Settings.EnableIbus)
			{
				BeverDriveContext.Ibus.OnValidMessage += new BeverDrive.Ibus.ValidMessageEventHandler(Ibus_OnValidMessage);
				BeverDriveContext.Ibus.Send(BeverDrive.Ibus.Messages.Other.Cdc_Announce);
			}

			if (BeverDriveContext.Settings.EnableIbusDebug)
				BeverDriveContext.LoadedModules.Add(new BeverDrive.Modules.IbusDebug());

			BeverDriveContext.CurrentCoreGui.ClockContainer.Time = DateTime.Now.ToShortTimeString();
			BeverDriveContext.CurrentCoreGui.ClockContainer.Date = DateTime.Now.ToString("yyyy-MM-dd");
			BeverDriveContext.CurrentCoreGui.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });
			BeverDriveContext.SetActiveModule("MainMenu");

			this.Timer1Hz = new Timer();
			this.Timer1Hz.Interval = 1000;
			this.Timer1Hz.Tick += new EventHandler(Timer1Hz_Tick);
			this.Timer1Hz.Start();

			/*this.Timer50Hz = new Timer();
			this.Timer50Hz.Interval = 20;
			this.Timer50Hz.Tick += new EventHandler(Timer50Hz_Tick);
			this.Timer50Hz.Start();*/

			if (BeverDriveContext.Settings.HideCursor)
				Cursor.Hide();
		}

		protected void Ibus_OnValidMessage(object sender, BeverDrive.Ibus.ValidMessageRecievedEventArgs e)
		{
			if (this.InvokeRequired)
				this.Invoke(new Action<string>(ProcessMessage), new object[] { e.Message });
			else
				this.ProcessMessage(e.Message);
		}

		protected void Timer1Hz_Tick(object sender, EventArgs e)
		{
			if (BeverDriveContext.ActiveModule != null)
				BeverDriveContext.ActiveModule.Update1Hz();

			BeverDriveContext.CurrentCoreGui.Update1Hz();
		}

		protected void Timer50Hz_Tick(object sender, EventArgs e)
		{
			BeverDriveContext.CurrentCoreGui.Update50Hz();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// UNLOAD ALL THE THINGS!
			if (VlcContext.AudioPlayer != null)
				VlcContext.AudioPlayer.Dispose();

			if (VlcContext.VideoPlayer != null)
				VlcContext.VizPlayer.Dispose();

			if (VlcContext.VizPlayer != null)
				VlcContext.VideoPlayer.Dispose();
		}

		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
		{
			if (keyData == Keys.Left)
				BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectPrevious });

			if (keyData == Keys.Right)
				BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectNext });

			if (keyData == Keys.Space)
				BeverDriveContext.ActiveModule.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.SelectClick });

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}