using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Gui.Core;
using InTheHand.Net.Sockets;

namespace BeverDrive.Gui
{
	public partial class SplashForm : Form
	{
		public SplashForm()
		{
			InitializeComponent();
			this.BackColor = Color.FromArgb(62, 67, 130);
			this.label1.ForeColor = Color.FromArgb(211, 211, 211);
			this.label1.Font = new Font("Arial", 16f, FontStyle.Bold);

			bool fail = false;
			BeverDriveSettings bs = null;

			// Check that config exists
			if (!fail && !System.IO.File.Exists("Config.xml"))
			{
				label1.Text += "Config.xml doesn't exist... exiting\n";
				fail = true;
				QuitWithError();
			}
			else
			{
				label1.Text += "Config.xml exists...\n";
			}

			// Parse config...
			if (!fail)
			{
				try
				{
					label1.Text += "Parsing Config.xml... ";
					bs = new BeverDrive.Gui.Core.BeverDriveSettings();
				}
				catch (Exception ex)
				{
					label1.Text += string.Format("failed ({0})... exiting\n", ex.Message);
					fail = true;
					QuitWithError();
				}
			}

			// Check com port
			if (!fail)
			{
				label1.Text += "done\n";

				// Try to initialize com port
				try
				{
					label1.Text += string.Format("Initializing com port {0}... ", bs.ComPort);
				}
				catch (Exception ex)
				{
					label1.Text += string.Format("failed ({0})... exiting\n", ex.Message);
					fail = true;
					QuitWithError();
				}
			}

			// Check vlc paths
			if (!fail)
			{
				label1.Text += "done\nChecking VLC path... ";

				if (!System.IO.File.Exists(bs.VlcPath + "\\libvlc.dll") || !System.IO.File.Exists(bs.VlcPath + "\\libvlccore.dll"))
				{
					label1.Text += string.Format("can't find libvlc.dll and libvlccore.dll in {0}... exiting\n", bs.VlcPath);
					fail = true;
					QuitWithError();
				}
			}

			// Check music root
			if (!fail)
			{
				label1.Text += "done\nChecking music path... ";

				if (!System.IO.Directory.Exists(bs.MusicRoot))
				{
					label1.Text += string.Format("can't find music root {0}... exiting\n", bs.MusicRoot);
					fail = true;
					QuitWithError();
				}
			}

			// Check video root
			if (!fail)
			{
				label1.Text += "done\nChecking video path... ";

				if (!System.IO.Directory.Exists(bs.MusicRoot))
				{
					label1.Text += string.Format("can't find music root {0}... exiting\n", bs.MusicRoot);
					fail = true;
					QuitWithError();
				}
			}

			// Check bluetooth support
			if (!fail)
			{
				if (bs.EnableBluetooth)
				{
					try
					{
						label1.Text += "done\nChecking bluetooth support... ";
						BluetoothClient btClient = new BluetoothClient();
					}
					catch (Exception ex)
					{
						label1.Text += string.Format("failed ({0})... exiting\n", ex.Message);
						fail = true;
						QuitWithError();
					}
				}
				else
					label1.Text += "done\nBluetooth disabled";
			}

			// Everything A-OK
			if (!fail)
			{
				// Wait for window to be closeable
				CloseSplash();
			}
		}

		private void CloseSplash()
		{
			var t = new Timer();
			t.Interval = 400;
			t.Tick += new EventHandler(t_Tick1);
			t.Start();
		}

		private void QuitWithError()
		{
			var t = new Timer();
			t.Interval = 1500;
			t.Tick += new EventHandler(t_Tick2);
			t.Start();
		}

		private void t_Tick1(object sender, EventArgs e)
		{
			this.Close();
		}

		private void t_Tick2(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
