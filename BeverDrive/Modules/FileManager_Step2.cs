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
using System.IO;

namespace BeverDrive.Modules
{
	public enum FileManager_Mode
	{
		CopyDirectory = 0,
		CopyFiles = 1,
		MoveDirectory = 2,
		MoveFiles = 3
	}

	[BackButtonVisible(false)]
	public class FileManager_Step2 : Module
	{
		private FileManagerListControl browser;
		private TextButton button1;
		private TextButton button2;
		private Label title;

		public List<FileSystemItem> ItemsToCopyMove { get; set; }
		public FileManager_Mode Mode { get; set; }

		public FileManager_Step2()
		{
			this.ItemsToCopyMove = new List<FileSystemItem>();
		}

		public override void Back()
		{
			throw new NotImplementedException();
		}

		public override void Init()
		{
			this.CreateControls();
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			base.OnCommand(e);

			if (this.SelectedIndex == this.browser.Items.Count)
				this.SelectedIndex--;

			this.browser.SelectedIndex = this.SelectedIndex;

			if (e.Command == ModuleCommands.SelectClick)
			{
				this.browser.Select();
				this.SelectedIndex = this.browser.SelectedIndex;
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
			this.button1.Index = -2;
			this.button1.Location = new System.Drawing.Point(width / 3 * 1 - 75, 70);
			this.button1.Size = new System.Drawing.Size(130, 36);
			this.button1.Text = "OK";
			this.button1.Click += (sender, e) => { this.Execute(); };
			this.button1.Hover += (sender, e) => { this.UpdateText(); };

			this.button2 = new TextButton();
			this.button2.Font = Fonts.GuiFont18;
			this.button2.ForeColor = Colors.ForeColor;
			this.button2.Index = -1;
			this.button2.Location = new System.Drawing.Point(width / 3 * 2 - 75, 70);
			this.button2.Size = new System.Drawing.Size(130, 36);
			this.button2.Text = "Cancel";
			this.button2.Click += (sender, e) => { BeverDriveContext.SetActiveModule("FileManager_Step1"); };
			this.button2.Hover += (sender, e) => { BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Cancel"; };

			this.title = new Label();
			this.title.Font = Fonts.GuiFont18;
			this.title.ForeColor = Colors.ForeColor;
			this.title.Location = new System.Drawing.Point(0, 16);
			this.title.Size = new System.Drawing.Size(width, 50);
			this.title.Text = "File manager";
			this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			base.Controls.Add(this.button1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.browser);
			base.Controls.Add(this.title);
		}

		private void UpdateText()
		{
			switch(this.Mode)
			{
				case FileManager_Mode.CopyDirectory:
					BeverDriveContext.CurrentCoreGui.ClockContainer.Text = string.Format("Copy {0} here", this.ItemsToCopyMove[0].Name);
					break;

				case FileManager_Mode.CopyFiles:
					BeverDriveContext.CurrentCoreGui.ClockContainer.Text = string.Format("Copy {0} files here", this.ItemsToCopyMove.Count);
					break;

				case FileManager_Mode.MoveDirectory:
					BeverDriveContext.CurrentCoreGui.ClockContainer.Text = string.Format("Move {0} here", this.ItemsToCopyMove[0].Name);
					break;

				case FileManager_Mode.MoveFiles:
					BeverDriveContext.CurrentCoreGui.ClockContainer.Text = string.Format("Move {0} files here", this.ItemsToCopyMove.Count);
					break;

				default:
					break;
			}
		}

		private void Execute()
		{
			if (this.browser.CurrentItem.Name == "> My computer")
				return;

			string dest = this.browser.CurrentDirectory.FullName;

			switch (this.Mode)
			{
				case FileManager_Mode.CopyDirectory:
					// TODO: Implement...
					break;

				case FileManager_Mode.CopyFiles:
					try
					{
						this.ItemsToCopyMove.ForEach(i =>
						{
							System.IO.File.Copy(i.FullPath, Path.Combine(dest, i.Name));
						});
					}
					catch
					{
					}
					break;

				case FileManager_Mode.MoveDirectory:
					// TODO: Implement...
					BeverDriveContext.CurrentCoreGui.ClockContainer.Text = "Not implemented yet";
					break;

				case FileManager_Mode.MoveFiles:
					try
					{
						this.ItemsToCopyMove.ForEach(i =>
						{
							System.IO.File.Move(i.FullPath, Path.Combine(dest, i.Name));
						});
					}
					catch
					{
					}
					break;

				default:
					break;
			}

			// Return to filemanager step1
			BeverDriveContext.SetActiveModule("FileManager_Step1");
		}
	}
}
