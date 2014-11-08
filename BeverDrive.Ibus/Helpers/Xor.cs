//
// Copyright 2011-2014 Sebastian Sj�din
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

namespace BeverDrive.Ibus.Helpers
{
	public class Xor
	{
		public static byte Calculate(byte[] data)
		{
			byte b = data[0];

			for (int i = 1; i < data.Length; i++)
				b = Convert.ToByte(b ^ data[i]);

			return b;
		}
	}
}
