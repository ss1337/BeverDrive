using System;
using System.Collections.Generic;
using System.Text;

namespace BeverDrive.Ibus.Messages
{
	public class ValidMessageRecievedEventArgs : EventArgs
	{
		public String Message { get; set; }
	}
}
