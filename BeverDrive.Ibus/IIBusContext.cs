using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeverDrive.Ibus
{
	public interface IIBusContext
	{
		Mode CurrentMode { get; }
		bool CtsHolding { get; }
		bool RtsEnable { get; set; }
		event ValidMessageEventHandler OnValidMessage;

		String Send(string message);
		String Send(Message m);
		String Send(byte[] msg);
		String Send(byte[] msg, int repeat);
	}
}
