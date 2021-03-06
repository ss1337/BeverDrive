﻿namespace BeverDrive.IbusEmulator
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnWheelLeft = new System.Windows.Forms.Button();
			this.btnWheelRight = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.chkRts = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Right knob";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 30);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(32, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "-";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(60, 30);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(32, 32);
			this.button2.TabIndex = 2;
			this.button2.Text = "+";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(12, 69);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(80, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Push";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(12, 99);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(80, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "Connect";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(12, 162);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(80, 20);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "COM2";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 143);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "COM port";
			// 
			// btnWheelLeft
			// 
			this.btnWheelLeft.Location = new System.Drawing.Point(116, 30);
			this.btnWheelLeft.Name = "btnWheelLeft";
			this.btnWheelLeft.Size = new System.Drawing.Size(32, 32);
			this.btnWheelLeft.TabIndex = 8;
			this.btnWheelLeft.Text = "<-";
			this.btnWheelLeft.UseVisualStyleBackColor = true;
			this.btnWheelLeft.Click += new System.EventHandler(this.btnWheelLeft_Click);
			// 
			// btnWheelRight
			// 
			this.btnWheelRight.Location = new System.Drawing.Point(154, 30);
			this.btnWheelRight.Name = "btnWheelRight";
			this.btnWheelRight.Size = new System.Drawing.Size(32, 32);
			this.btnWheelRight.TabIndex = 9;
			this.btnWheelRight.Text = "->";
			this.btnWheelRight.UseVisualStyleBackColor = true;
			this.btnWheelRight.Click += new System.EventHandler(this.btnWheelRight_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(113, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Wheel buttons";
			// 
			// chkRts
			// 
			this.chkRts.AutoSize = true;
			this.chkRts.Location = new System.Drawing.Point(116, 74);
			this.chkRts.Name = "chkRts";
			this.chkRts.Size = new System.Drawing.Size(54, 17);
			this.chkRts.TabIndex = 10;
			this.chkRts.Text = "RTS?";
			this.chkRts.UseVisualStyleBackColor = true;
			this.chkRts.Click += new System.EventHandler(this.chkRts_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(207, 191);
			this.Controls.Add(this.chkRts);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnWheelRight);
			this.Controls.Add(this.btnWheelLeft);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Form1";
			this.Text = "IbusEmulator";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnWheelLeft;
		private System.Windows.Forms.Button btnWheelRight;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkRts;
	}
}

