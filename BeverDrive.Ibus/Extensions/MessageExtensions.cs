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

namespace BeverDrive.Ibus.Extensions
{
	public static class MessageExtensions
	{
		public static byte[] ToHexBytes(this string message)
		{
			string[] hexvalues = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			int length = hexvalues.Length;
			byte[] result = new byte[length];

			for(int i = 0; i < length; i++)
			{
				int val = Convert.ToInt32(hexvalues[i], 16);
				result[i] = (byte)val;
			}

			return result;
		}

		public static bool IsMessage(this string message, string expectedMessage)
		{
			return message.IsMessage(expectedMessage, false);
		}

		public static bool IsMessage(this string message, string expectedMessage, bool startsWith)
		{
			var org = message.ToCharArray();
			var cmp = expectedMessage.ToCharArray();

			if (!startsWith && org.Length != cmp.Length)
				return false;

			for (int i = 0; i < cmp.Length; i++)
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
