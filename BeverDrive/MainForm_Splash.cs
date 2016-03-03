//
// Copyright 2014-2016 Sebastian Sjödin
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
using System.Windows.Forms;
using BeverDrive.Core;
using InTheHand.Net.Sockets;
using BeverDrive.Gui.Controls;

namespace BeverDrive
{
	/// <summary>
	/// Code for the splash screen part
	/// </summary>
	public partial class MainForm
	{
		public bool IsRunningMono { get { return Type.GetType ("Mono.Runtime") != null; } }

		private TeletypeLabel lblSplash;

		/// <summary>
		/// Shows the splash screen with startup messages
		/// </summary>
		/// <returns>0 on success, -1 on failure</returns>
		private int ShowSplashScreen()
		{
			this.lblSplash = new TeletypeLabel();
			this.lblSplash.AutoSize = true;
			this.lblSplash.Location = new System.Drawing.Point(38, 37);
			this.lblSplash.ForeColor = Color.FromArgb(211, 211, 211);
			this.lblSplash.Font = new Font("Arial", 14f, FontStyle.Bold);
			this.lblSplash.Name = "label1";
			this.lblSplash.Size = new System.Drawing.Size(0, 13);
			this.lblSplash.TabIndex = 0;
			this.lblSplash.Visible = true;
			this.Controls.Add(this.lblSplash);
			this.BackColor = Color.FromArgb(62, 67, 130);

			bool fail = false;

			BeverDriveSettings bs = null;

			// Check that config exists
			if (!fail && !System.IO.File.Exists("Config.xml"))
			{
				lblSplash.Text += "Config.xml doesn't exist... exiting\n";
				fail = true;
				QuitWithError();
			}
			else
			{
				lblSplash.Text += "Config.xml exists...\n";
			}

			// Parse config...
			if (!fail)
			{
				try
				{
					lblSplash.Text += "Parsing Config.xml... ";
					bs = new BeverDrive.Core.BeverDriveSettings();
				}
				catch (Exception ex)
				{
					lblSplash.Text += string.Format("failed ({0})... exiting\n", ex.Message);
					fail = true;
					QuitWithError();
				}
			}

			// Check com port
			if (!fail)
			{
				lblSplash.Text += "done\n";

				// Try to initialize com port
				try
				{
					lblSplash.Text += string.Format("Initializing com port {0}... ", bs.ComPort);
				}
				catch (Exception ex)
				{
					lblSplash.Text += string.Format("failed ({0})... exiting\n", ex.Message);
					fail = true;
					QuitWithError();
				}
			}

			// Check path to vlc dlls, only if we are running in Windows
			if (!fail && !this.IsRunningMono)
			{
				lblSplash.Text += "done\nChecking VLC path... ";

				if (!System.IO.File.Exists(bs.VlcPath + "\\libvlc.dll") || !System.IO.File.Exists(bs.VlcPath + "\\libvlccore.dll"))
				{
					lblSplash.Text += string.Format("can't find libvlc.dll and libvlccore.dll in \n   {0}\n\nExiting...", bs.VlcPath);
					fail = true;
					QuitWithError();
				}
			}

			// Check if music path exists
			if (!fail)
			{
				lblSplash.Text += "done\nChecking music path... ";

				if (!System.IO.Directory.Exists(bs.MusicRoot))
				{
					lblSplash.Text += string.Format("can't find music root {0}\n\nExiting...", bs.MusicRoot);
					fail = true;
					QuitWithError();
				}
			}

			// Check if video path exists
			if (!fail)
			{
				lblSplash.Text += "done\nChecking video path... ";

				if (!System.IO.Directory.Exists(bs.VideoRoot))
				{
					lblSplash.Text += string.Format("can't find video root {0}\n\nExiting...", bs.VideoRoot);
					fail = true;
					QuitWithError();
				}
			}

			// TODO: Check modules loaded here...

			// Everything A-OK
			if (fail)
				return -1;
			else
				this.Controls.Remove(lblSplash);

			return 0;
		}

		private void QuitWithError()
		{
			var t1 = new Timer();
			t1.Interval = 4000;
			t1.Tick += new EventHandler(t1_Tick);
			t1.Start();

			var t2 = new Timer();
			t2.Interval = 50;
			t2.Tick += new EventHandler(t2_Tick);
			t2.Start();
		}

		private void t1_Tick(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void t2_Tick(object sender, EventArgs e)
		{
			lblSplash.Refresh();
		}
	}
}
