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
namespace BeverDrive.Ibus.Messages.Predefined
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class Mid
	{
		public static Message Mid2Radio_ChangeStation(int number)
		{
			throw new Exception("Does not work correctly");
			//Message msg = new Message();
			//msg.DataAsByte = new byte[4] { 0x31, 0x00, 0x16, 0x00 };
			//msg.DataAsByte[3] = (byte)number;
			//msg.Destination = Devices.Radio;
			//msg.Source = Devices.Mid;
			//return msg;
		}


		public static Message Mid2Radio_ChangeToCdC()
		{
			throw new Exception("Does not work correctly");
			//Message msg = new Message();
			////msg.DataAsByte = new byte[4] { 0x31, 0xC3, 0x00, 0x0A };
			//msg.Destination = Devices.Radio;
			//msg.Source = Devices.Mid;
			//return msg;
		}
	}
}
