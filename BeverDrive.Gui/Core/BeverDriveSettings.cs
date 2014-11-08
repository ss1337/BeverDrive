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
using System.Text;
using System.Xml;
using System.Drawing;

namespace BeverDrive.Gui.Core
{
	public class BeverDriveSettings
	{
		public string ComPort { get; private set; }
		public bool EnableBluetooth { get; private set; }
		public string MusicRoot { get; private set; }
		public string VideoRoot { get; private set; }
		public bool HideCursor { get; private set; }
		public bool ReadOnlyFileSystem { get; private set; }
		public int OffsetBottom { get; set; }
		public int OffsetLeft { get; set; }
		public int OffsetRight { get; set; }
		public int OffsetTop { get; set; }
		public string VlcPath { get; set; }

		public Color BackColor { get; set; }
		public Color ForeColor { get; set; }
		public Color SelectedColor { get; set; }
		public Color ClockBackgroundColor { get; set; }
		public Color ClockForegroundColor { get; set; }

		public int DebugTrack { get; set; }

		public BeverDriveSettings()
		{
			// Parse config.xml
			String xmlFile = "Config.xml";
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(xmlFile);
			XmlNodeList nodes = xdoc.SelectNodes("//config/settings/setting");

			this.ComPort = this.ReadStringSetting("ComPort", nodes);
			this.EnableBluetooth = this.ReadBoolSetting("EnableBluetooth", nodes);
			this.MusicRoot = this.ReadStringSetting("MusicRoot", nodes);
			this.VideoRoot = this.ReadStringSetting("VideoRoot", nodes);
			
			this.HideCursor = this.ReadBoolSetting("HideCursor", nodes);
			this.OffsetBottom = this.ReadIntSetting("OffsetBottom", nodes);
			this.OffsetLeft = this.ReadIntSetting("OffsetLeft", nodes);
			this.OffsetRight = this.ReadIntSetting("OffsetRight", nodes);
			this.OffsetTop = this.ReadIntSetting("OffsetTop", nodes);
			this.VlcPath = this.ReadStringSetting("VlcPath", nodes);

			this.DebugTrack = this.ReadIntSetting("DebugTrack", nodes);

			this.BackColor = this.ReadColorSetting("BackgroundColor", nodes);
			this.ForeColor = this.ReadColorSetting("ForegroundColor", nodes);
			this.SelectedColor = this.ReadColorSetting("SelectedColor", nodes);
			this.ClockBackgroundColor = this.ReadColorSetting("ClockBackgroundColor", nodes);
			this.ClockForegroundColor = this.ReadColorSetting("ClockForegroundColor", nodes);
		}

		private bool ReadBoolSetting(String name, XmlNodeList nodes)
		{
			try { return Convert.ToBoolean(this.ReadStringSetting(name, nodes)); }
			catch { }
			return false;
		}

		private Color ReadColorSetting(String name, XmlNodeList nodes)
		{
			string val = this.ReadStringSetting(name, nodes);
			if (!val.Contains("#") || val.Length != 7)
				return Color.White;

			int r = Convert.ToInt32(val.Substring(1, 2), 16);
			int g = Convert.ToInt32(val.Substring(3, 2), 16);
			int b = Convert.ToInt32(val.Substring(5, 2), 16);
			return Color.FromArgb(r, g, b);
		}

		private int ReadIntSetting(String name, XmlNodeList nodes)
		{
			try { return Convert.ToInt32(this.ReadStringSetting(name, nodes)); }
			catch { }
			return 0;
		}

		private string ReadStringSetting(String name, XmlNodeList nodes)
		{
			foreach (XmlNode xn in nodes)
			{
				XmlAttribute attrName = (XmlAttribute)xn.Attributes.GetNamedItem("name");
				XmlAttribute attrValue = (XmlAttribute)xn.Attributes.GetNamedItem("value");

				if (attrName.Value.Equals(name))
					return attrValue.Value;
			}

			return "";
		}

		public void Save()
		{
			if (ReadOnlyFileSystem) { return; }

			String xmlFile = "Config.xml";
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(xmlFile);

			foreach (XmlNode xn in xdoc.SelectNodes("//config/settings/setting"))
			{
				//if (xn.Attributes[0].Value.Equals("CurrentDisc"))
				//    xn.Attributes[1].Value = this.CurrentDisc.ToString();

				//if (xn.Attributes[0].Value.Equals("CurrentTrack"))
				//    xn.Attributes[1].Value = this.CurrentTrack.ToString();
			}

			xdoc.Save(xmlFile);
		}
	}
}
