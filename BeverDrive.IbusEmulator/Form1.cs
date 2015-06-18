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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Ibus;

namespace BeverDrive.IbusEmulator
{
	public partial class Form1 : Form
	{
		IbusContext ibus;

		public Form1()
		{
			InitializeComponent();
			ibus = new BeverDrive.Ibus.IbusContext(textBox1.Text);
			this.ibus.OnValidMessage += new ValidMessageEventHandler(ibus_OnValidMessage);
		}

		private void ibus_OnValidMessage(object sender, ValidMessageRecievedEventArgs e)
		{
			if (this.InvokeRequired)
				this.Invoke(new Action<string>(ProcessMessage), new object[] { e.Message });
			else
				this.ProcessMessage(e.Message);
		}

		private void ProcessMessage(string message)
		{
			// Process ibus message here...
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.ibus.Send(BeverDrive.Ibus.Messages.Predefined.BordMonitor.RightKnobLeft(1));
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.ibus.Send(BeverDrive.Ibus.Messages.Predefined.BordMonitor.RightKnobRight(1));
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.ibus.Send(BeverDrive.Ibus.Messages.Predefined.BordMonitor.RightKnobPush());
		}

		private void button4_Click(object sender, EventArgs e)
		{
			this.ibus.Send("68 17 3B 23 62 30 20 20 07 20 20 20 20 20 08 43 44 20 36 2D 39 39 20 20 26");
		}

		private void btnWheelLeft_Click(object sender, EventArgs e)
		{
			this.ibus.Send(BeverDrive.Ibus.Messages.Other.Wheel_PrevTrack);
		}

		private void btnWheelRight_Click(object sender, EventArgs e)
		{
			this.ibus.Send(BeverDrive.Ibus.Messages.Other.Wheel_NextTrack);
		}

		private void chkRts_Click(object sender, EventArgs e)
		{
			if (this.chkRts.Checked)
			{
				this.ibus.Send(BeverDrive.Ibus.Messages.BordMonitor.Menu);
			}
			else
			{
				this.ibus.Send("68 17 3B 23 62 30 20 20 07 20 20 20 20 20 08 43 44 20 36 2D 39 39 20 20 26");
			}
		}
	}
}
