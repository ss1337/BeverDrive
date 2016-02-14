using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Gui.Controls;
using BeverDrive.Modules;

namespace BeverDrive.Gui
{
	public interface ICoreGui
	{
		MetroidButton BackButton { get; set; }
		Size BaseAreaSize { get; }
		Size ModuleAreaSize { get; }

		Panel BaseContainer { get; set; }							// Adjust this to fit everything to screen
		ClockPanel ClockContainer { get; set; }						// Contains lower portion with date/time and maybe text
		GraphicsPanel ModuleContainer { get; set; }					// All module controls goes into here


		void AddControl(AGraphicsControl ctrl);
		void ClearBaseContainer();
		void ClearModuleContainer();
		void Invalidate();
		void OnCommand(ModuleCommandEventArgs e);
		void Update1Hz();
		void Update50Hz();
	}
}
