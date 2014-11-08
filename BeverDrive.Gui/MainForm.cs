//
// Copyright 2012-2014 Sebastian Sj�din
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
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Core;
using BeverDrive.Gui.Modules;
using BeverDrive.Ibus;
using BeverDrive.Ibus.Extensions;

namespace BeverDrive.Gui
{
	public partial class MainForm : Form
	{
		public Timer Timer1hz;
		public IbusContext IbusInstance;

		public MainForm()
		{
			InitializeComponent();
			BeverDriveContext.CurrentMainForm = this;
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Gui.Modules.CoreGui());
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Gui.Modules.MainMenu());
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Gui.Modules.Mp3Player());
			BeverDriveContext.LoadedModules.Add(new BeverDrive.Gui.Modules.VideoPlayer());

			if (BeverDriveContext.Settings.EnableBluetooth)
				BeverDriveContext.LoadedModules.Add(new BeverDrive.Gui.Modules.Bluetooth());

			// Init ibus
			this.IbusInstance = new BeverDrive.Ibus.IbusContext(BeverDriveContext.Settings.ComPort);
			this.IbusInstance.OnValidMessage += new BeverDrive.Ibus.ValidMessageEventHandler(Ibus_OnValidMessage);
			this.IbusInstance.Send(BeverDrive.Ibus.Messages.Other.Cdc_Announce);

			BeverDriveContext.CurrentCoreGui.BaseClock.Text = DateTime.Now.ToShortTimeString();
			BeverDriveContext.CurrentCoreGui.BaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			BeverDriveContext.CurrentCoreGui.OnCommand(new ModuleCommandEventArgs { Command = ModuleCommands.Show });
			BeverDriveContext.SetActiveModule("MainMenu");

			this.Timer1hz = new Timer();
			this.Timer1hz.Interval = 1000;
			this.Timer1hz.Tick += new EventHandler(Timer1hz_Tick);
			this.Timer1hz.Start();

			if (BeverDriveContext.Settings.HideCursor)
				Cursor.Hide();
		}

		//public void SetActiveModule(string moduleName)
		//{
		//    BeverDriveContext.SetActiveModule(moduleName);
		//}

		// Does not work with VLC
		//protected override CreateParams CreateParams
		//{
		//    get
		//    {
		//        CreateParams cp = base.CreateParams;
		//        cp.ExStyle |= 0x02000000;
		//        return cp;
		//    }
		//}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Paint any AOverlayModules visible
			BeverDriveContext.LoadedModules.OfType<AOverlayedModule>().Any(x => { if (x.Visible) { x.Paint(e.Graphics); } return false; });
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
		}

		protected void Ibus_OnValidMessage(object sender, BeverDrive.Ibus.ValidMessageRecievedEventArgs e)
		{
			if (this.InvokeRequired)
				this.Invoke(new Action<string>(ProcessMessage), new object[] { e.Message });
			else
				this.ProcessMessage(e.Message);
		}

		protected void Timer1hz_Tick(object sender, EventArgs e)
		{
			if (BeverDriveContext.ActiveModule != null)
				BeverDriveContext.ActiveModule.Update1Hz();

			BeverDriveContext.CurrentCoreGui.Update1Hz();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// UNLOAD ALL THE THINGS!
			VlcContext.AudioPlayer.Dispose();
			VlcContext.VizPlayer.Dispose();
			VlcContext.VideoPlayer.Dispose();
		}
	}
}