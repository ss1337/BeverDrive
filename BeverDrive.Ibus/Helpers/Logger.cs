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
using System.Xml;

namespace BeverDrive.Ibus.Helpers
{
	public class LogEvent
	{
		public String Description;
		public String Direction;
		public String Message;
		public DateTime Timestamp;

		public LogEvent(String description, String direction, String message)
		{
			this.Description = description;
			this.Direction = direction;
			this.Message = message;
			this.Timestamp = DateTime.Now;
		}
	}


	public class Logger
	{
		private bool Enabled;
		private String Filename;
		private List<LogEvent> Events;

		public Logger()
		{
			this.Events = new List<LogEvent>();
			this.FindDrive();
		}

		public void LogEvent(String description, String direction, String message)
		{
			this.Events.Add(new LogEvent(description, direction, message));
		}

		public void WriteFile()
		{
			if (this.Enabled)
			{
				XmlDocument xdoc = new XmlDocument();
				XmlDeclaration decl = xdoc.CreateXmlDeclaration("1.0", null, null);
				XmlElement root = xdoc.CreateElement("IbusEvents");
				XmlWriter writer = XmlWriter.Create(this.Filename);
				xdoc.AppendChild(decl);

				foreach (LogEvent ev in this.Events)
				{
					XmlElement xev = xdoc.CreateElement("IbusEvent");
					xev.SetAttribute("Timestamp", ev.Timestamp.Ticks.ToString());
					xev.SetAttribute("Message", ev.Message);
					xev.SetAttribute("Direction", ev.Direction);
					xev.SetAttribute("Description", ev.Description);
					root.AppendChild(xev);
				}

				xdoc.AppendChild(root);
				xdoc.WriteTo(writer);
				writer.Flush();
				writer.Close();
			}
		}

		private void FindDrive()
		{
			System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

			for (int i = 0; i < drives.Length; i++)
			{
				System.IO.DriveInfo drive = drives[i];

				if (drive.DriveType.Equals(System.IO.DriveType.Removable) && drive.IsReady)
				{
					this.Filename = String.Format("{0}Ibus_{1}{2}.xml", drive.RootDirectory.Name, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString().Replace(":",""));
					this.Enabled = true;
				}
			}
		}
	}
}
