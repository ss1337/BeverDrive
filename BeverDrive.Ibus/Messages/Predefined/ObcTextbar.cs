//
// Copyright 2015 Sebastian Sjödin
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

namespace BeverDrive.Ibus.Messages.Predefined
{
	public class ObcTextbar
	{
		public static Message SetUrgentText(string text)
		{
			// 1A 35 00 <Text>
			// Example message "  CHECK CONTROL OK  "
			// 30 19 80 1A 35 00 20 20 43 48 45 43 4B 20 43 4F 4E 54 52 4F 4C 20 4F 4B 20 20 83

			byte[] data = new byte[text.Length + 3];
			data[0] = 0x1A;
			data[1] = 0x35;
			data[2] = 0x00;

			for (int i = 0; i < text.Length; i++)
				data[3 + i] = (byte)text[i];

			return new Message(Devices.CCM, Devices.IKE, data);
		}
	}
}
