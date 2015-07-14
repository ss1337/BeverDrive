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
using System.Linq;
using System.Text;

namespace BeverDrive.Ibus
{
	public class Sender
	{
		private SerialPort comport;
		private Queue<byte[]> messageQueue;
		private System.Timers.Timer pollQueue;

		public Sender(SerialPort comport)
		{
			this.comport = comport;
			this.messageQueue = new Queue<byte[]>();
			this.pollQueue = new System.Timers.Timer();
			this.pollQueue.AutoReset = true;
			this.pollQueue.Interval = 50;
			this.pollQueue.Elapsed += new System.Timers.ElapsedEventHandler(pollQueue_Elapsed);
			this.pollQueue.Start();
		}

		void pollQueue_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
            bool msgSent = false;

			if (messageQueue.Count == 0)
				return;

			this.pollQueue.Stop();
			byte[] msg = messageQueue.Dequeue();

			if (this.comport != null)
			{
                while (!msgSent)
                {
                    if (this.comport.CtsHolding)
                    {
                        this.comport.Write(msg, 0, msg.Length);
                        msgSent = true;
                    }
                }
			}

			this.pollQueue.Start();
		}

		public void QueueMessage(byte[] message)
		{
			this.messageQueue.Enqueue(message);
		}
	}
}
