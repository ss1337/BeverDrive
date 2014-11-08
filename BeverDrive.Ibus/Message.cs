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
	public class Message
	{
		// Example message
		//                            |---------------- Source ID
		//                            |  |------------- Message length (DestID + data + checksum)
		//                            |  |  |---------- Destination ID
		//                            |  |  |  |------- Data
		//                            |  |  |  |     |---- XOR checksum
		// Poll cd-changer:          68 03 18 01    72
		// Cd-changer announce:      18 04 FF 02 01 E0
		// Cd-changer poll response: 18 04 FF 02 00 E1
		//
		//
		//180A68390209003F00010076
		//
        // Bas Mitten:       68 04 6A 36 60 50
        // Treble Mitten:    68 04 6A 36 C0 F0
        // Fader+, Knapp:    68 04 6A 36 9C AC
        // Fader mitten:     68 04 6A 36 80 B0
        // Fader-, Knapp:    68 04 6A 36 8A BA
        // Balance mitten:   68 04 6A 36 40 70
        // CDC, Knapp:       C0 06 68 31 C3 00 0A 56
		// Radio station 5:  C0 06 68 31 00 16 05 8C
		public Devices Source { get; private set; }
		public Devices Destination  { get; private set; }
		public byte[] Data { get; private set; }

		public byte Length
		{
			get
			{
				if (this.Data == null)
					return 0x02;
				else
					return Convert.ToByte(this.Data.Length + 2);
			}
		}

		public byte Checksum
		{
			get
			{
				byte b = (byte)this.Source;
				b = Convert.ToByte(b ^ this.Length);
				b = Convert.ToByte(b ^ (byte)this.Destination);

				if (this.Data != null)
					for (int i = 0; i < this.Data.Length; i++)
						b = Convert.ToByte(b ^ this.Data[i]);

				return b;
			}
		}

		public Message(byte[] message)
		{
			// Check validity of message
			// 18 04 FF 02 01 E0

			this.Source = (Devices)message[0];
			this.Destination = (Devices)message[2];
			this.Data = new byte[message.Length - 4];
			
			for (int i = 0; i < message.Length - 4; i++)
				this.Data[i] = message[i + 3];

			if (this.Checksum != message[message.Length - 1])
				throw new Exception("Checksum is incorrect");
		}

		public Message(Devices source, Devices destination, byte[] data)
		{
			this.Data = data;
			this.Destination = destination;
			this.Source = source;
		}

		public byte[] GetMessageAsByteArray()
		{
			int length = 4;

			if (this.Data != null)
				length += this.Data.Length;

			byte[] msg = new byte[length];
			msg[0] = (byte)this.Source;						// Source ID
			msg[1] = this.Length;							// Length of message
			msg[2] = (byte)this.Destination;				// Destination ID

			if (this.Data != null)
				for(int i = 0; i < this.Data.Length; i++)
					msg[i + 3] = this.Data[i];

			msg[length - 1] = this.Checksum;

			// For cd-changer poll, this should be: 104 3 24 1 114
			return msg;
		}

		public string GetMessageAsString()
		{
			var array = this.GetMessageAsByteArray();
			StringBuilder bob = new StringBuilder(array.Length);

			foreach (byte singleByte in array)
				bob.Append(singleByte.ToString("X2") + " ");

			return bob.ToString().Trim();
		}


		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj.GetType().Name == "String")
			{
				var org = this.ToString().ToCharArray();
				var cmp = ((string)obj).ToCharArray();

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

			if (obj.GetType().Name != "Message")
				return false;

			return ((Message)obj).ToString() == this.ToString();
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			return this.GetMessageAsString();
		}

		public bool Contains(string data)
		{
			var org = this.ToString().ToCharArray();
			var cmp = data.ToCharArray();
			int cmp_index = 0;
			bool match = false;

			if (org.Length < cmp.Length)
				return false;

			for (int i = 0; i < org.Length; i++)
			{
				while (cmp[cmp_index] == 'X')
					cmp_index++;

				if (org[i] == cmp[cmp_index] || org[i] == 'X')
				{
					// Possible match
					match = true;

					for (int j = cmp_index; j < cmp.Length; j++)
					{
						if (org[i + j] != cmp[j] && org[i + j] != 'X' && cmp[j] != 'X')
						{
							match = false;
							break;
						}
					}

					if (match)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks whether the message starts with certain data. Data can be wildcarded (01 02 0X).
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool StartsWith(string data)
		{
			var org = this.ToString().ToCharArray();
			var cmp = data.ToCharArray();

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
