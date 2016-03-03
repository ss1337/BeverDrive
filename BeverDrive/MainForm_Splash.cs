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
using BeverDrive.Gui.Controls;
using nVlc.LibVlcWrapper.Implementation.Exceptions;

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
			if (!fail)
			{
				fail = SplashTest(
					"Checking that Config.xml exists...\n",
					"can't find Config.xml...\n\nExiting...",
					!System.IO.File.Exists("Config.xml"));
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
					lblSplash.Text += string.Format("failed ({0})...\n\nExiting...", ex.Message);
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
					lblSplash.Text += string.Format("failed ({0})... \n\nExiting...", ex.Message);
					fail = true;
					QuitWithError();
				}
			}

			// Check path to vlc dlls, only if we are running in Windows
			if (!fail && !this.IsRunningMono)
			{
				fail = SplashTest(
					"done\nChecking VLC path... ",
					string.Format("can't find libvlc.dll and libvlccore.dll in \n   {0}\n\nExiting...", bs.VlcPath),
					!System.IO.File.Exists(bs.VlcPath + "\\libvlc.dll") || !System.IO.File.Exists(bs.VlcPath + "\\libvlccore.dll"));
			}

			// Try to initialize vlc
			if (!fail)
			{
				lblSplash.Text += "done\nTrying to initialize libvlc... ";

				BeverDriveContext.Initialize();
				try
				{
					VlcContext.Initialize(BeverDriveContext.Settings.VlcPath);
				}
				catch (LibVlcNotFoundException)
				{
					if (IsRunningMono)
						lblSplash.Text += "couldn't find libvlc, missing symlink?\n\nExiting...";
					else
						lblSplash.Text += string.Format("couldn't find libvlc at {0}\n\nExiting...", BeverDriveContext.Settings.VlcPath);

					fail = true;
					QuitWithError();
				}
				catch (LibVlcInitException)
				{
					lblSplash.Text += "something went wrong when initializing libvlc\n\nExiting...";
					fail = true;
					QuitWithError();
				}
			}

			// Check if music path exists
			if (!fail)
			{
				fail = SplashTest(
					"done\nChecking music path... ",
					string.Format("can't find music root {0}\n\nExiting...", bs.MusicRoot),
					!System.IO.Directory.Exists(bs.MusicRoot));
			}

			// Check if video path exists
			if (!fail)
			{
				fail = SplashTest(
					"done\nChecking video path... ",
					string.Format("can't find video root {0}\n\nExiting...", bs.VideoRoot),
					!System.IO.Directory.Exists(bs.VideoRoot));
			}

			// TODO: Check modules loaded here...

			// Everything A-OK
			if (fail)
				return -1;
			else
				this.Controls.Remove(lblSplash);

			return 0;
		}

		private bool SplashTest(string msg1, string msg2, bool condition)
		{
			lblSplash.Text += msg1;

			if (condition)
			{
				lblSplash.Text += msg2;
				QuitWithError();
				return true;
			}

			return false;
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
