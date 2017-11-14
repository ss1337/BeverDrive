//
// Copyright 2017 Sebastian Sjödin
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
using BeverDrive.Core;
using BeverDrive.Core.Extensions;
using BeverDrive.Modules;

namespace BeverDrive.Gui.Controls
{
	public enum OverlayBoxButtons
	{
		None = 0,
		OK = 1,
		OKCancel = 2
	}

	public enum OverlayBoxResult
	{
		None = 0,
		OK = 1,
		Cancel = 2
	}

	public class OverlayBox : AGraphicsControl
	{
		private bool okSelected;
		private StringFormat stringFormat;

		public OverlayBoxButtons Buttons { get; set; }

		public string Caption { get; set; }

		public OverlayBoxResult Result { get; set; }

		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
				if (this.Buttons == OverlayBoxButtons.OKCancel)
				{
					okSelected = false;
				}
			}
		}

		public OverlayBox()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			var height = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height;

			this.stringFormat = new StringFormat();
			this.stringFormat.Alignment = StringAlignment.Center;
			this.stringFormat.LineAlignment = StringAlignment.Center;

			this.Location = new System.Drawing.Point((width - 400) / 2, (height - 250) / 2);
			this.Size = new System.Drawing.Size(400, 250);
			this.Result = OverlayBoxResult.None;
		}

		public void OnCommand(ModuleCommandEventArgs e)
		{
			switch (e.Command)
			{
				case ModuleCommands.SelectLeft:
					if (this.Buttons == OverlayBoxButtons.OKCancel)
						okSelected = !okSelected;
					
					break;

				case ModuleCommands.SelectRight:
					if (this.Buttons == OverlayBoxButtons.OKCancel)
						okSelected = !okSelected;
	
					break;

				case ModuleCommands.SelectClick:
					if (!okSelected)
						this.Result = OverlayBoxResult.Cancel;

					this.RaiseClick(this, new EventArgs());
					break;

				default:
					break;
			}

			BeverDriveContext.CurrentCoreGui.Invalidate();
		}

		public override void PaintToBuffer(Graphics graphic)
		{
			Rectangle outerRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
			Rectangle captRect;
			Rectangle textRect;

			graphic.FillRectangle(BeverDrive.Gui.Styles.Brushes.BackBrush, outerRect);
			textRect = new Rectangle(this.Location.X + 7, this.Location.Y + 7, this.Width - 14, this.Height - 14);

			if (string.IsNullOrEmpty(this.Caption))
			{
				captRect = new Rectangle();
			}
			else
			{
				captRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, 30);
				textRect.Height -= 30;
				graphic.FillRectangle(BeverDrive.Gui.Styles.Brushes.ForeBrush, captRect);
				graphic.DrawString(this.Caption, BeverDrive.Gui.Styles.Fonts.GuiFont18, new SolidBrush(Color.Black), captRect, this.stringFormat);
			}

			// Button
			if (this.Buttons == OverlayBoxButtons.OK)
			{
				var btnRect = new Rectangle(this.Location.X + 135, this.Location.Y + 200, 130, 36);
				textRect.Height = this.Height - 64;
				graphic.FillHollowRectangle(BeverDrive.Gui.Styles.Brushes.SelectedBrush, btnRect, 3);
				graphic.DrawString("OK", BeverDrive.Gui.Styles.Fonts.GuiFont24, BeverDrive.Gui.Styles.Brushes.SelectedBrush, btnRect, this.stringFormat);
			}

			// Button
			if (this.Buttons == OverlayBoxButtons.OKCancel)
			{
				var btnRect1 = new Rectangle(this.Location.X + this.Width / 3 * 1 - 100, this.Location.Y + 200, 130, 36);
				var btnRect2 = new Rectangle(this.Location.X + this.Width / 3 * 2 - 30, this.Location.Y + 200, 130, 36);
				textRect.Height = this.Height - 64;

				var okBrush = this.okSelected ? BeverDrive.Gui.Styles.Brushes.SelectedBrush : BeverDrive.Gui.Styles.Brushes.ForeBrush;
				var cBrush = this.okSelected ? BeverDrive.Gui.Styles.Brushes.ForeBrush : BeverDrive.Gui.Styles.Brushes.SelectedBrush;

				graphic.FillHollowRectangle(okBrush, btnRect1, 3);
				graphic.DrawString("OK", BeverDrive.Gui.Styles.Fonts.GuiFont24, okBrush, btnRect1, this.stringFormat);

				graphic.FillHollowRectangle(cBrush, btnRect2, 3);
				graphic.DrawString("Cancel", BeverDrive.Gui.Styles.Fonts.GuiFont24, cBrush, btnRect2, this.stringFormat);
			}

			graphic.FillHollowRectangle(BeverDrive.Gui.Styles.Brushes.SelectedBrush, outerRect, 3);
			graphic.DrawString(this.Text, BeverDrive.Gui.Styles.Fonts.GuiFont18, BeverDrive.Gui.Styles.Brushes.ForeBrush, textRect, this.stringFormat);
		}
	}
}
