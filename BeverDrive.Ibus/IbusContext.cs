//
// Copyright 2011-2014 Sebastian Sjödin
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
using System.IO.Ports;
using System.Text;
using BeverDrive.Ibus.Extensions;
using BeverDrive.Ibus.Messages;
using BeverDrive.Ibus.Helpers;

namespace BeverDrive.Ibus
{
	//
	// IBUS Example message
	//                            |---------------- Source ID
	//                            |  |------------- Message length (DestID + data + checksum)
	//                            |  |  |---------- Destination ID
	//                            |  |  |  |------- Data
	//                            |  |  |  |     |---- XOR checksum
	// Poll cd-changer:          68 03 18 01    72
	// Cd-changer announce:      18 04 FF 02 01 E0
	// Cd-changer poll response: 18 04 FF 02 00 E1
	//
	public delegate void ButtonPressEventHandler(object sender, ButtonPressedEventArgs e);
	public delegate void InitEventHandler(object sender, EventArgs e);
	public delegate void ValidMessageEventHandler(object sender, ValidMessageRecievedEventArgs e);

	public class IbusContext : IIbusContext
	{
		public Mode CurrentMode { get; private set; }

		public bool CtsHolding
		{
			get { return this.comport.CtsHolding; }
		}

		/// <summary>
		/// Gets or sets the RTS pin, usually connected to the enable video pin
		/// </summary>
		public bool RtsEnable
		{
			get { return this.comport.RtsEnable; }
			set { this.comport.RtsEnable = value; }
		}

		/// <summary>
		/// Logs all messages with timestamp
		/// </summary>
		public List<KeyValuePair<long, string>> MessageLog { get; set; }

		public event ValidMessageEventHandler OnValidMessage;

		private SerialPort comport;
		private Receiver mr;
		private Sender ms;

		public IbusContext(String comPort)
		{
			this.MessageLog = new List<KeyValuePair<long, string>>();

			if (comPort != String.Empty)
			{
				this.comport = new SerialPort(comPort);
				this.comport.BaudRate = 9600;
				this.comport.DataBits = 8;
				this.comport.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
				this.comport.Handshake = Handshake.None;
				this.comport.Parity = Parity.Even;
				this.comport.StopBits = StopBits.One;
				this.comport.Open();

				this.mr = new Receiver();
				this.mr.OnValidMessage += new ValidMessageEventHandler(mr_OnValidMessage);

				this.ms = new Sender(this.comport);

				this.CurrentMode = Mode.Tape;
			}
		}

		~IbusContext()
		{
			if (this.comport.IsOpen)
				this.comport.Close();

			this.comport.Dispose();
		}

		public void Close()
		{
			if (this.comport.IsOpen)
				this.comport.Close();

			this.comport.Dispose();
		}

		public String Send(string message)
		{
			return this.Send(message.ToHexBytes());
		}

		public String Send(Message m)
		{
			this.Send(m.GetMessageAsByteArray());
			return m.ToString();
		}

		public String Send(byte[] msg)
		{
			return Send(msg, 0);
		}

		public String Send(byte[] msg, int repeat)
		{
			if (this.comport != null)
			{
				this.ms.QueueMessage(msg);
				return Strings.ByteArrayToString(msg, msg.Length);
			}
			else
				return "No COM port initialized";
		}

		public void SendInvalidData()
		{
			if (this.comport != null)
				this.comport.Write("asdf1234");
		}

		private void DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (this.comport.IsOpen)
			{
				Int32 bytes = this.comport.BytesToRead;
				byte[] buffer = new byte[bytes];
				this.comport.Read(buffer, 0, bytes);

				for (Int32 i = 0; i < bytes; i++)
					this.mr.ReceiveByte(buffer[i]);
			}
		}

		private void mr_OnValidMessage(object sender, ValidMessageRecievedEventArgs e)
		{
			// Remove the first message from log
			if (this.MessageLog.Count > 99)
				this.MessageLog.RemoveAt(0);

			this.MessageLog.Add(new KeyValuePair<long, string>(DateTime.Now.Ticks, e.Message));
			
			// Don't fire event if there isn't an event handler associated
			if (this.OnValidMessage != null)
			{
				this.OnValidMessage(this, new ValidMessageRecievedEventArgs(this.CurrentMode, e.Message));
			}

			this.mr.Reset();
		}
	}
}
