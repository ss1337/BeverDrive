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
using System.Linq;
using System.Text;
using BeverDrive.Ibus.Constants;

namespace BeverDrive.Ibus
{
	public class ButtonPressedEventArgs : EventArgs
	{
		public Buttons ButtonPressed { get; set; }
	}

	public class ValidMessageRecievedEventArgs : EventArgs
	{
		public Mode CurrentMode { get; private set; }
		public string Message { get; private set; }

		public ValidMessageRecievedEventArgs(string message)
		{
			this.Message = message;
		}

		public ValidMessageRecievedEventArgs(Mode currentMode, string message)
		{
			this.CurrentMode = currentMode;
			this.Message = message;
		}

		public bool IsMessage(string expectedMessage)
		{
			var org = this.Message.ToCharArray();
			var cmp = expectedMessage.ToCharArray();

			if (org.Length != cmp.Length)
				return false;

			for (int i = 0; i < org.Length; i++)
			{
				if (org[i] != 'X' && cmp[i] != 'X')
				{
					if (org[i] != cmp[i])
						return false;
				}
			}

			return true;
		}
	}
}
