//
// Copyright 2012-2014 Sebastian Sjödin
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BeverDrive.Gui.Core
{
	public class Logger
	{
		public static void AddWarning(string message)
		{
			LogToFile("WARNING: " + message);
		}

		public static void AddError(string message)
		{
			LogToFile("ERROR:   " + message);
		}

		public static void Clear()
		{
			string file = GetPathToLogfile();
			if (File.Exists(file))
				File.Delete(file);
		}

		private static void LogToFile(string line)
		{
			var ts = new StreamWriter(GetPathToLogfile(), true);
			ts.WriteLine(line);
			ts.Close();
		}

		private static string GetPathToLogfile()
		{
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			return Path.Combine(path, "bever.log");
		}
	}
}
