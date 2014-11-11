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
using System.Linq;
using System.Text;
using InTheHand.Net.Sockets;
using BeverDrive.Gui.Core;

namespace BeverDrive.Gui.Modules
{
	public partial class Bluetooth : AModule
	{
		private BluetoothClient btClient;
		private bool discovering;

		public void DiscoverDevices()
		{
			bool foundDevice = false;
			string deviceName = "";
			var form = BeverDriveContext.CurrentMainForm;

			BluetoothDeviceInfo[] peers = btClient.DiscoverDevices();
			if (peers.Count() > 0)
			{
				BluetoothDeviceInfo p = peers.FirstOrDefault(x => x.Connected == true);
				if (p != null)
				{
					deviceName = p.DeviceName;
					foundDevice = true;
				}
			}

			if (form.InvokeRequired)
				form.Invoke(new Action<string, bool>(UpdateDeviceName), new object[] { deviceName, foundDevice });
			else
				this.UpdateDeviceName(deviceName, foundDevice);

			discovering = false;
		}

		public override void Update1Hz()
		{
			if (this.btClient == null)
				return;

			if (isActive && !discovering)
			{
				discovering = true;
				System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(DiscoverDevices));
				t.Start();
			}
		}

		private void UpdateDeviceName(string deviceName, bool found)
		{
			if (found)
			{
				this.lbl_bt1.Text = string.Format("Connected device: {0}", deviceName);
				this.lbl_bt2.Text = "";
			}
			else
			{
				this.lbl_bt1.Text = "Connected device: ";
				this.lbl_bt2.Text = "Connect device now";
			}
			
			this.lbl_bt1.Invalidate();
			this.lbl_bt2.Invalidate();
		}
	}
}
