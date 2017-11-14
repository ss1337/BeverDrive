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
using System.Linq;
using System.Text;
using BeverDrive.Core;
using BeverDrive.Gui.Controls;
using BeverDrive.Gui.Styles;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	[MenuText("File manager")]
	public class FileManager_Step1 : Module
	{
		private FileManagerListControl browser;
		private TextButton button1;
		private TextButton button2;
		private TextButton button3;
		private TextButton button4;
		private TextButton button5;
		private OverlayBox msgBox;
		private Label title;

		public FileManager_Step1()
		{
		}

		public override void Back()
		{
			BeverDriveContext.SetActiveModule("");
		}

		public override void Init()
		{
			this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			if (this.msgBox.Visible)
			{
				this.msgBox.OnCommand(e);
				return;
			}

			BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "File manager";
			base.OnCommand(e);

			if (this.SelectedIndex == this.browser.Items.Count)
				this.SelectedIndex--;

			this.browser.SelectedIndex = this.SelectedIndex;

			if (e.Command == ModuleCommands.SelectClick)
			{
				this.browser.Select();
				this.SelectedIndex = this.browser.SelectedIndex;
			}

			if (e.Command == ModuleCommands.Show)
			{
				this.browser.Refresh();
			}
		}

		private void CreateControls()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			var height = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height;

			int browserHeight = 9;
			if (BeverDriveContext.Settings.VideoMode == VideoMode.Mode_169)
				browserHeight = 7;


			this.browser = new FileManagerListControl();
			this.browser.HeightInItems = browserHeight;
			this.browser.Name = "list1";
			this.browser.Width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			this.browser.Location = new System.Drawing.Point(0, BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Height - this.browser.Height);
			this.browser.Index = 0;

			this.button1 = new TextButton();
			this.button1.Font = Fonts.GuiFont18;
			this.button1.ForeColor = Colors.ForeColor;
			this.button1.Index = -5;
			this.button1.Location = new System.Drawing.Point(width / 6 * 1 - 75, 70);
			this.button1.Size = new System.Drawing.Size(120, 36);
			this.button1.Text = "Copy dir";
			this.button1.Click += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Not implemented yet"; };
			this.button1.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Copy to..."; };

			this.button2 = new TextButton();
			this.button2.Font = Fonts.GuiFont18;
			this.button2.ForeColor = Colors.ForeColor;
			this.button2.Index = -4;
			this.button2.Location = new System.Drawing.Point(width / 6 * 2 - 75, 70);
			this.button2.Size = new System.Drawing.Size(120, 36);
			this.button2.Text = "Copy file";
			this.button2.Click += (sender, e) => { this.GotoStep2(FileManager_Mode.CopyFiles); };
			this.button2.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Copy to..."; };

			this.button3 = new TextButton();
			this.button3.Font = Fonts.GuiFont18;
			this.button3.ForeColor = Colors.ForeColor;
			this.button3.Index = -3;
			this.button3.Location = new System.Drawing.Point(width / 6 * 3 - 75, 70);
			this.button3.Size = new System.Drawing.Size(120, 36);
			this.button3.Text = "Move dir";
			this.button3.Click += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Not implemented yet"; };
			this.button3.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Move to..."; };

			this.button4 = new TextButton();
			this.button4.Font = Fonts.GuiFont18;
			this.button4.ForeColor = Colors.ForeColor;
			this.button4.Index = -2;
			this.button4.Location = new System.Drawing.Point(width / 6 * 4 - 75, 70);
			this.button4.Size = new System.Drawing.Size(120, 36);
			this.button4.Text = "Move file";
			this.button4.Click += (sender, e) => { this.GotoStep2(FileManager_Mode.MoveFiles); };
			this.button4.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Move to..."; };

			this.button5 = new TextButton();
			this.button5.Font = Fonts.GuiFont18;
			this.button5.ForeColor = Colors.ForeColor;
			this.button5.Index = -1;
			this.button5.Location = new System.Drawing.Point(width / 6 * 5 - 75, 70);
			this.button5.Size = new System.Drawing.Size(120, 36);
			this.button5.Text = "Delete";
			this.button5.Click += (sender, e) => { this.DeleteStep1(); };
			this.button5.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Delete..."; };

			this.msgBox = new OverlayBox();
			this.msgBox.Buttons = OverlayBoxButtons.OKCancel;
			this.msgBox.Caption = "";
			this.msgBox.Text = "";
			this.msgBox.Visible = false;
			this.msgBox.Click += (sender, e) => { this.DeleteStep2(); };

			this.title = new Label();
			this.title.Font = Fonts.GuiFont18;
			this.title.ForeColor = Colors.ForeColor;
			this.title.Location = new System.Drawing.Point(0, 16);
			this.title.Size = new System.Drawing.Size(width, 50);
			this.title.Text = "File manager";
			this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			base.Controls.Add(this.button1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.browser);
			base.Controls.Add(this.msgBox);
			base.Controls.Add(this.title);
		}

		private void DeleteStep1()
		{
			var count = this.browser.Items.Count(i => { return i.Selected; });
			if (count == 0)
			{
				BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "No files selected";
				return;
			}

			this.msgBox.Text = string.Format("Delete {0} files, are you sure?", count);
			this.msgBox.Visible = true;
		}

		private void DeleteStep2()
		{
			this.browser.Items.ForEach(x =>
			{
				if (x.Selected)
				{
					try
					{
						System.IO.File.Delete(x.FullPath);
					}
					catch
					{
					}
				}
			});
			this.msgBox.Visible = false;
			this.browser.Refresh();
		}

		private void GotoStep2(FileManager_Mode mode)
		{
			// Find module by name
			var step2 = (FileManager_Step2)BeverDriveContext.LoadedModules.Find(x => { return x.GetType().Name.Equals("FileManager_Step2"); });
			step2.ItemsToCopyMove.Clear();
			step2.Mode = mode;

			if (mode == FileManager_Mode.CopyDirectory || mode == FileManager_Mode.MoveDirectory)
			{
				if (this.browser.CurrentItem.Name != "> My computer")
					step2.ItemsToCopyMove.Add(this.browser.CurrentItem);
			}
			else
			{
				this.browser.Items.ForEach(i =>
				{
					if (i.Selected)
						step2.ItemsToCopyMove.Add(i);
				});
			}

			if (step2.ItemsToCopyMove.Count > 0)
				BeverDriveContext.SetActiveModule("FileManager_Step2");
			else
				BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "No files selected";
		}
	}
}
