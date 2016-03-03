//
// Copyright 2016 Sebastian Sjödin
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

namespace BeverDrive.Gui.Controls
{
	public class TeletypeLabel : System.Windows.Forms.Label
	{
		private string ttext;
		private int charsPrinted;
		private long ticks;

		public TeletypeLabel()
		{
			charsPrinted = 0;
			ticks = 0;
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			long elapsed = 0;

			if (ticks > 0)
			{
				elapsed = DateTime.Now.Ticks - ticks;
				charsPrinted += (int)(elapsed / 25000);
			}

			this.ttext = this.Text;
			this.Text = this.Text.Length > charsPrinted ? this.Text.Substring(0, charsPrinted) : this.Text;
			base.OnPaint(e);
			this.Text = this.ttext;

			ticks = DateTime.Now.Ticks;
		}
	}
}
