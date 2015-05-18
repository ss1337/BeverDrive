//
// Copyright 2012-2015 Sebastian Sjödin
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
using System.Drawing;
using System.Text;
using System.Xml;
using BeverDrive.Ibus.Messages.Predefined;

namespace BeverDrive.Core
{
	public enum VideoMode
	{
		Mode_169,
		Mode_43
	}

	public class BeverDriveSettings
	{
		public string ComPort { get; private set; }
		public bool EnableIbus { get; private set; }
		public string Greeting { get; private set; }
		public string MusicFileTypes { get; private set; }
		public string VideoFileTypes { get; private set; }
		public string MusicRoot { get; private set; }
		public string VideoRoot { get; private set; }
		public bool HideCursor { get; private set; }
		public bool ReadOnlyFileSystem { get; private set; }
		public int OffsetBottom { get; set; }
		public int OffsetLeft { get; set; }
		public int OffsetRight { get; set; }
		public int OffsetTop { get; set; }
		public LightWipers.TvMode TvMode { get; set; }
		public VideoMode VideoMode { get; set; }
		public string VlcPath { get; set; }

		public Color BackColor { get; set; }
		public Color ForeColor { get; set; }
		public Color SelectedColor { get; set; }
		public Color ClockBackgroundColor { get; set; }
		public Color ClockForegroundColor { get; set; }

		public List<string> Modules { get; set; }

		public int DebugTrack { get; set; }

		public BeverDriveSettings()
		{
			// Parse config.xml
			String xmlFile = "Config.xml";
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(xmlFile);
			XmlNodeList nodes = xdoc.SelectNodes("//config/settings/setting");

			this.Modules = new List<string>();
			this.ComPort = this.ReadStringSetting("ComPort", nodes);
			this.EnableIbus = this.ReadBoolSetting("EnableIbus", nodes);
			this.Greeting = this.ReadStringSetting("Greeting", nodes);
			this.MusicRoot = this.ReadStringSetting("MusicRoot", nodes);
			this.VideoRoot = this.ReadStringSetting("VideoRoot", nodes);
			this.MusicFileTypes = this.ReadStringSetting("MusicFileTypes", nodes);
			this.VideoFileTypes = this.ReadStringSetting("VideoFileTypes", nodes);
			
			this.HideCursor = this.ReadBoolSetting("HideCursor", nodes);
			this.OffsetBottom = this.ReadIntSetting("OffsetBottom", nodes);
			this.OffsetLeft = this.ReadIntSetting("OffsetLeft", nodes);
			this.OffsetRight = this.ReadIntSetting("OffsetRight", nodes);
			this.OffsetTop = this.ReadIntSetting("OffsetTop", nodes);
			this.TvMode = this.ReadTvModeSetting("TvMode", nodes);
			this.VideoMode = this.ReadVideoModeSetting("VideoMode", nodes);
			this.VlcPath = this.ReadStringSetting("VlcPath", nodes);

			this.DebugTrack = this.ReadIntSetting("DebugTrack", nodes);

			this.BackColor = this.ReadColorSetting("BackgroundColor", nodes);
			this.ForeColor = this.ReadColorSetting("ForegroundColor", nodes);
			this.SelectedColor = this.ReadColorSetting("SelectedColor", nodes);
			this.ClockBackgroundColor = this.ReadColorSetting("ClockBackgroundColor", nodes);
			this.ClockForegroundColor = this.ReadColorSetting("ClockForegroundColor", nodes);

			// Tweak OffsetTop and OffsetBottom depending on video mode
			if (this.VideoMode == VideoMode.Mode_169)
			{
				OffsetBottom += 60;
				OffsetTop += 60;
			}

			// Parse module list
			XmlNodeList moduleNodes = xdoc.SelectNodes("//config/modules/module");
			foreach (XmlNode xn in moduleNodes)
			{
				var mn = (XmlAttribute)xn.Attributes.GetNamedItem("name");
				this.Modules.Add(mn.Value);
			}
		}

		/// <summary>
		/// Reads settings for a module, if they exist
		/// </summary>
		/// <param name="moduleName">Complete type name of module, e g BeverDrive.Modules.MainModule</param>
		/// <returns></returns>
		public IEnumerable<KeyValuePair<string, string>> ReadModuleSettings(string moduleName)
		{
			var result = new List<KeyValuePair<string, string>>();

			String xmlFile = "Config.xml";
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(xmlFile);

			XmlNodeList moduleNodes = xdoc.SelectNodes("//config/modules/module");
			foreach (XmlNode xn in moduleNodes)
			{
				var mn = (XmlAttribute)xn.Attributes.GetNamedItem("name");
				if (mn.Value == moduleName)
				{
					foreach (XmlNode xs in xn.ChildNodes)
					{
						if (xs.Name.ToLower() == "setting")
						{
							XmlAttribute attrName = (XmlAttribute)xs.Attributes.GetNamedItem("name");
							XmlAttribute attrValue = (XmlAttribute)xs.Attributes.GetNamedItem("value");
							result.Add(new KeyValuePair<string, string>(attrName.Value, attrValue.Value));
						}
					}

					break;
				}
			}

			return result;
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

		private LightWipers.TvMode ReadTvModeSetting(String name, XmlNodeList nodes)
		{
			/* 
			 * Possible values are:
			 * Mode_169Zoom_60Hz,
			 * Mode_169Zoom_50Hz,
			 * Mode_169_60Hz,
			 * Mode_169_50Hz,
			 * Mode_43_60Hz,
			 * Mode_43_50Hz
			 */

			var result = LightWipers.TvMode.Mode_43_60Hz;

			foreach (XmlNode xn in nodes)
			{
				XmlAttribute attrName = (XmlAttribute)xn.Attributes.GetNamedItem("name");
				XmlAttribute attrValue = (XmlAttribute)xn.Attributes.GetNamedItem("value");

				if (attrName.Value.Equals(name))
				{
					// Parse tv mode here
					if (attrValue.Value.ToLower().Equals("mode_169zoom_60hz"))
						result = LightWipers.TvMode.Mode_169Zoom_60Hz;

					if (attrValue.Value.ToLower().Equals("mode_169zoom_50hz"))
						result = LightWipers.TvMode.Mode_169Zoom_50Hz;

					if (attrValue.Value.ToLower().Equals("mode_169_60hz"))
						result = LightWipers.TvMode.Mode_169_60Hz;

					if (attrValue.Value.ToLower().Equals("mode_169_50hz"))
						result = LightWipers.TvMode.Mode_169_50Hz;

					if (attrValue.Value.ToLower().Equals("mode_43_60hz"))
						result = LightWipers.TvMode.Mode_43_60Hz;

					if (attrValue.Value.ToLower().Equals("mode_43_50hz"))
						result = LightWipers.TvMode.Mode_43_50Hz;

				}
			}
			
			return result;
		}

		private VideoMode ReadVideoModeSetting(String name, XmlNodeList nodes)
		{
			/* 
			 * Possible values are:
			 * Mode_169,
			 * Mode_43
			 */

			var result = VideoMode.Mode_43;

			foreach (XmlNode xn in nodes)
			{
				XmlAttribute attrName = (XmlAttribute)xn.Attributes.GetNamedItem("name");
				XmlAttribute attrValue = (XmlAttribute)xn.Attributes.GetNamedItem("value");

				if (attrName.Value.Equals(name))
				{
					// Parse tv mode here
					if (attrValue.Value.ToLower().Equals("mode_169"))
						result = VideoMode.Mode_169;

					if (attrValue.Value.ToLower().Equals("mode_43"))
						result = VideoMode.Mode_43;
				}
			}

			return result;
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
