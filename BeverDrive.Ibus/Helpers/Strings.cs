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

namespace BeverDrive.Ibus.Helpers
{
	public class Strings
	{
		public static bool MessageCompare(string original, string compareWith)
		{

			return true;
		}

		public static string ByteArrayToString(byte[] array)
		{
			StringBuilder bob = new StringBuilder(array.Length);

			foreach (byte singleByte in array)
				bob.Append(singleByte.ToString("X2") + " ");

			return bob.ToString().Trim();
		}

		public static string ByteArrayToString(byte[] array, int length)
		{
			StringBuilder bob = new StringBuilder(array.Length);

			for(int i = 0; i < length; i++)
				bob.Append(array[i].ToString("X2") + " ");

			return bob.ToString().Trim();
		}

		public static byte[] StringToByteArray(string text)
		{
			byte []bytes = new byte[text.Length / 2];

			for(int i = 0 ; i < text.Length ; i += 2)
			{
				bytes[i / 2] = byte.Parse(text[i].ToString() + text[i + 1].ToString(), 
					System.Globalization.NumberStyles.HexNumber);
			}

			return bytes;
		}
	}
}
