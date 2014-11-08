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

namespace BeverDrive.Ibus.Messages
{
	public class Radio
	{
		public const string Menu = "68 04 3B 46 0C 1D";
		public const string PlayCd = "68 05 18 38 03 00 4E";
		public const string ReqTrack = "68 05 18 38 00 00 4D";
		public const string SetRandomOn = "68 05 18 38 08 01 44";
		public const string SetRandomOff = "68 05 18 38 08 00 45";
		public const string SetCd1 = "68 05 18 38 06 01 4A";
		public const string SetCd2 = "68 05 18 38 06 02 49";
		public const string SetCd3 = "68 05 18 38 06 03 48";
		public const string SetCd4 = "68 05 18 38 06 04 4F";
		public const string SetCd5 = "68 05 18 38 06 05 4E";
		public const string SetCd6 = "68 05 18 38 06 06 4D";
		public const string StopCd = "68 05 18 38 01 00 4C";
	}
}
