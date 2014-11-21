using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeverDrive.Core;
using BeverDrive.Core.Styles;

namespace BeverDrive.Modules
{
	[BackButtonVisible(true)]
	public class GraphicBrowserTest : AModule
	{
		private Label ctrl_title;
		private BeverDrive.Controls.GraphicBrowser br;

		public GraphicBrowserTest()
		{
			var width = BeverDriveContext.CurrentCoreGui.ModuleAreaSize.Width;

			this.ctrl_title = new Label();
			this.ctrl_title.AutoSize = false;
			this.ctrl_title.Font = Fonts.GuiFont32;
			this.ctrl_title.BackColor = System.Drawing.Color.Transparent;
			this.ctrl_title.ForeColor = Colors.ForeColor;
			this.ctrl_title.Location = new System.Drawing.Point(42, 16);
			this.ctrl_title.Size = new System.Drawing.Size(width - 84, 50);
			this.ctrl_title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.ctrl_title.Text = "Graphic browser";

			this.br = new BeverDrive.Controls.GraphicBrowser("D:\\BeverDrive");
			this.br.AutoSize = false;
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
		}

		private void Show()
		{
			BeverDriveContext.CurrentCoreGui.BackButton.Selected = false;
			BeverDriveContext.CurrentCoreGui.AddControl(ctrl_title);
			BeverDriveContext.CurrentCoreGui.AddControl(br);
			ctrl_title.Invalidate();
		}

		private void Hide()
		{
		}
	}
}
