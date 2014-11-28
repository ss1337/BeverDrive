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
	public class GraphicBrowserTest : AModule
	{
		private Label ctrl_title;
		private GraphicBrowser br;

		public GraphicBrowserTest()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;
			var height = BeverDriveContext.CurrentCoreGui.ModuleContainer.Height;

			this.ctrl_title = new Label();
			this.ctrl_title.Font = Fonts.GuiFont36;
			this.ctrl_title.ForeColor = Colors.SelectedColor;
			this.ctrl_title.Location = new System.Drawing.Point(0, 16);
			this.ctrl_title.Size = new System.Drawing.Size(width, 50);
			this.ctrl_title.Text = "GraphicBrowser Test";
			this.ctrl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

			this.br = new GraphicBrowser("D:\\BeverDrive");
			this.br.Location = new System.Drawing.Point(0, 180);
			this.br.Size = new System.Drawing.Size(width, 380);
		}

		public override void Init()
		{
		}

		public override void OnCommand(ModuleCommandEventArgs e)
		{
			switch (e.Command)
			{
				case ModuleCommands.Show:
					this.Show();
					break;
				case ModuleCommands.Hide:
					this.Hide();
					break;
				case ModuleCommands.SelectClick:
					this.br.Select();
					break;
				case ModuleCommands.SelectNext:
					this.br.SelectedIndex++;
					break;
				case ModuleCommands.SelectPrevious:
					this.br.SelectedIndex--;
					break;
				default:
					break;
			}

			BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
		}

		public override void Update1Hz()
		{
			BeverDriveContext.CurrentCoreGui.ModuleContainer.Invalidate();
		}

		private void Show()
		{
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			this.ShowControls();
		}

		private void Hide()
		{
		}
	}
}
