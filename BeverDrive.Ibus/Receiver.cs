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
using System.Text;
using BeverDrive.Ibus.Helpers;

namespace BeverDrive.Ibus
{
	public class Receiver
	{
		public event ValidMessageEventHandler OnValidMessage;
		private System.Timers.Timer resetTimer;

		#region Properties
		private byte[] data;
		private Int32 length;
		private Boolean valid;

		public byte[] DataAsBytes
		{
			get { return this.data; }
		}

		public String DataAsString
		{
			get { return Strings.ByteArrayToString(this.data, this.length); }
		}

		public Int32 Length
		{
			// Overall length of message (i e including source and length bytes)
			get { return this.length; }
		}

		public Boolean Valid
		{
			get { return this.valid; }
		}
		#endregion

		public Receiver()
		{
			this.data = new byte[4096];
			this.resetTimer = new System.Timers.Timer();
			this.resetTimer.AutoReset = true;
			this.resetTimer.Interval = 20;
			this.resetTimer.Elapsed += new System.Timers.ElapsedEventHandler(resetTimer_Elapsed);
			this.resetTimer.Start();
		}

		void resetTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this.length = 0;
		}

		public void ReceiveByte(byte data)
		{
			// Reset communication idle timer
			this.resetTimer.Stop();
			this.resetTimer.Start();

			try
			{
				this.data[this.length] = data;
				this.length++;
			}
			catch { this.length = 0; }
			this.valid = this.CheckValidity();

			if (this.valid)
			{
				ValidMessageRecievedEventArgs e = new ValidMessageRecievedEventArgs(this.DataAsString);
				this.OnValidMessage(this, e);
			}
		}


		// Checks the validity of the current message
		private Boolean CheckValidity()
		{
			if (this.length < 2)
				return false;

			if (this.length < Convert.ToInt32(this.data[1]) + 2)
				return false;

			// Calculate XOR
			byte[] xordata = new byte[this.length - 1];

			for (int i = 0; i < this.length - 1; i++)
				xordata[i] = this.data[i];

			byte b = Xor.Calculate(xordata);

			// Compare calculated XOR to actual XOR value in message
			if (b.Equals(this.data[this.length - 1])) 
				return true;

			return false;
		}

		public void Reset()
		{
			this.length = 0;
		}
	}
}
