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
using System.Windows.Forms;
using BeverDrive.Core;
using InTheHand.Net.Sockets;

namespace BeverDrive
{
	/// <summary>
	/// Code for the splash screen part
	/// </summary>
	public partial class MainForm
	{
		private Label lblSplash;

		/// <summary>
		/// Shows the splash screen with startup messages
		/// </summary>
		/// <returns>0 on success, -1 on failure</returns>
		private int ShowSplashScreen()
		{
			this.lblSplash = new Label();
			this.lblSplash.AutoSize = true;
			this.lblSplash.Location = new System.Drawing.Point(38, 37);
			this.lblSplash.ForeColor = Color.FromArgb(211, 211, 211);
			this.lblSplash.Font = new Font("Arial", 14f, FontStyle.Bold);
			this.lblSplash.Name = "label1";
			this.lblSplash.Size = new System.Drawing.Size(0, 13);
			this.lblSplash.TabIndex = 0;
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

			// Check for Webdings
			if (!fail)
			{
				lblSplash.Text += "done\nChecking for Webdings font... ";
				Font f = new Font("Webdings", 28f, FontStyle.Bold);

				if (f.Name != "Webdings")
				{
					lblSplash.Text += string.Format("does not exist ({0})... exiting\n");
					fail = true;
					QuitWithError();
				}
			}

			// Check for Wingdings 3
			if (!fail)
			{
				lblSplash.Text += "done\nChecking for Wingdings 3 font... ";
				Font f = new Font("Wingdings 3", 28f, FontStyle.Bold);

				if (f.Name != "Wingdings 3")
				{
					lblSplash.Text += string.Format("does not exist ({0})... exiting\n");
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

			// Check path to vlc dlls
			if (!fail)
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

			// Check bluetooth support if it's enabled
			/*if (!fail)
			{
				if (bs.EnableBluetooth)
				{
					try
					{
						lblSplash.Text += "done\nChecking bluetooth support... ";
						BluetoothClient btClient = new BluetoothClient();
					}
					catch (Exception ex)
					{
						lblSplash.Text += string.Format("failed\n({0})... exiting\n", ex.Message);
						fail = true;
						QuitWithError();
					}
				}
				else
					lblSplash.Text += "done\nBluetooth disabled";
			}*/

			this.Invalidate();

			// Everything A-OK
			if (fail)
				return -1;
			else
				this.Controls.Remove(lblSplash);

			return 0;
		}

		private void QuitWithError()
		{
			var t = new Timer();
			t.Interval = 4000;
			t.Tick += new EventHandler(t_Tick);
			t.Start();
		}

		private void t_Tick(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
