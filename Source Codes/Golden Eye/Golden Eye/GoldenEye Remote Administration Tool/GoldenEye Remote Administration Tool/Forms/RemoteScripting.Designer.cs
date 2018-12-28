namespace GoldenEye_Remote_Administration_Tool.Forms
{
    partial class RemoteScripting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteScripting));
            this.velyseForm1 = new VelyseTheme.VelyseForm();
            this.button1 = new MonoFlat.Button();
            this.t_TextBox1 = new MonoFlat.t_TextBox();
            this.velyseGroupBox1 = new VelyseTheme.VelyseGroupBox();
            this.velyseRadioButton3 = new VelyseTheme.VelyseRadioButton();
            this.velyseRadioButton2 = new VelyseTheme.VelyseRadioButton();
            this.velyseRadioButton1 = new VelyseTheme.VelyseRadioButton();
            this.velyseControlBox1 = new VelyseTheme.VelyseControlBox();
            this.velyseForm1.SuspendLayout();
            this.velyseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // velyseForm1
            // 
            this.velyseForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.Controls.Add(this.button1);
            this.velyseForm1.Controls.Add(this.t_TextBox1);
            this.velyseForm1.Controls.Add(this.velyseGroupBox1);
            this.velyseForm1.Controls.Add(this.velyseControlBox1);
            this.velyseForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.velyseForm1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.Location = new System.Drawing.Point(0, 0);
            this.velyseForm1.Name = "velyseForm1";
            this.velyseForm1.Padding = new System.Windows.Forms.Padding(10, 70, 10, 9);
            this.velyseForm1.RoundCorners = VelyseTheme.VelyseForm.VLRoundCorner.Round;
            this.velyseForm1.Sizable = true;
            this.velyseForm1.Size = new System.Drawing.Size(491, 455);
            this.velyseForm1.SmartBounds = true;
            this.velyseForm1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.velyseForm1.TabIndex = 0;
            this.velyseForm1.Text = "Remote Scripting";
            this.velyseForm1.Text_Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.TextColor = System.Drawing.Color.White;
            this.velyseForm1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseForm1.Click += new System.EventHandler(this.velyseForm1_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button1.Image = null;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(12, 410);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(463, 33);
            this.button1.TabIndex = 6;
            this.button1.Text = "Execute Script";
            this.button1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // t_TextBox1
            // 
            this.t_TextBox1.BackColor = System.Drawing.Color.Transparent;
            this.t_TextBox1.Font = new System.Drawing.Font("Tahoma", 11F);
            this.t_TextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(183)))), ((int)(((byte)(191)))));
            this.t_TextBox1.Image = null;
            this.t_TextBox1.Location = new System.Drawing.Point(12, 162);
            this.t_TextBox1.MaxLength = 32767;
            this.t_TextBox1.Multiline = true;
            this.t_TextBox1.Name = "t_TextBox1";
            this.t_TextBox1.ReadOnly = false;
            this.t_TextBox1.Size = new System.Drawing.Size(463, 242);
            this.t_TextBox1.TabIndex = 5;
            this.t_TextBox1.Text = "@echo off";
            this.t_TextBox1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.t_TextBox1.UseSystemPasswordChar = false;
            // 
            // velyseGroupBox1
            // 
            this.velyseGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.velyseGroupBox1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.velyseGroupBox1.Controls.Add(this.velyseRadioButton3);
            this.velyseGroupBox1.Controls.Add(this.velyseRadioButton2);
            this.velyseGroupBox1.Controls.Add(this.velyseRadioButton1);
            this.velyseGroupBox1.GroupBoxFont = new System.Drawing.Font("Arial", 9F);
            this.velyseGroupBox1.Location = new System.Drawing.Point(12, 73);
            this.velyseGroupBox1.Name = "velyseGroupBox1";
            this.velyseGroupBox1.PanelSide = VelyseTheme.VelyseGroupBox.PanelDr.Top;
            this.velyseGroupBox1.Size = new System.Drawing.Size(463, 83);
            this.velyseGroupBox1.TabIndex = 1;
            this.velyseGroupBox1.Text = "Script Type";
            this.velyseGroupBox1.TextColor = System.Drawing.Color.White;
            this.velyseGroupBox1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseGroupBox1.Click += new System.EventHandler(this.velyseGroupBox1_Click);
            // 
            // velyseRadioButton3
            // 
            this.velyseRadioButton3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseRadioButton3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.velyseRadioButton3.Checked = false;
            this.velyseRadioButton3.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(104)))), ((int)(((byte)(160)))));
            this.velyseRadioButton3.ElipseColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.velyseRadioButton3.Location = new System.Drawing.Point(292, 45);
            this.velyseRadioButton3.Name = "velyseRadioButton3";
            this.velyseRadioButton3.Size = new System.Drawing.Size(75, 22);
            this.velyseRadioButton3.TabIndex = 3;
            this.velyseRadioButton3.Text = "VBS";
            this.velyseRadioButton3.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.velyseRadioButton3.Click += new System.EventHandler(this.velyseRadioButton3_Click);
            // 
            // velyseRadioButton2
            // 
            this.velyseRadioButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseRadioButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.velyseRadioButton2.Checked = false;
            this.velyseRadioButton2.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(104)))), ((int)(((byte)(160)))));
            this.velyseRadioButton2.ElipseColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.velyseRadioButton2.Location = new System.Drawing.Point(211, 45);
            this.velyseRadioButton2.Name = "velyseRadioButton2";
            this.velyseRadioButton2.Size = new System.Drawing.Size(75, 22);
            this.velyseRadioButton2.TabIndex = 2;
            this.velyseRadioButton2.Text = "HTML";
            this.velyseRadioButton2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.velyseRadioButton2.Click += new System.EventHandler(this.velyseRadioButton2_Click);
            // 
            // velyseRadioButton1
            // 
            this.velyseRadioButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseRadioButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.velyseRadioButton1.Checked = true;
            this.velyseRadioButton1.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(104)))), ((int)(((byte)(160)))));
            this.velyseRadioButton1.ElipseColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.velyseRadioButton1.Location = new System.Drawing.Point(130, 45);
            this.velyseRadioButton1.Name = "velyseRadioButton1";
            this.velyseRadioButton1.Size = new System.Drawing.Size(75, 22);
            this.velyseRadioButton1.TabIndex = 1;
            this.velyseRadioButton1.Text = "BATCH";
            this.velyseRadioButton1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.velyseRadioButton1.Click += new System.EventHandler(this.velyseRadioButton1_Click);
            // 
            // velyseControlBox1
            // 
            this.velyseControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.velyseControlBox1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseControlBox1.EnableHoverHighlight = false;
            this.velyseControlBox1.EnableMaximizeButton = true;
            this.velyseControlBox1.EnableMinimizeButton = true;
            this.velyseControlBox1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseControlBox1.Location = new System.Drawing.Point(389, 0);
            this.velyseControlBox1.Name = "velyseControlBox1";
            this.velyseControlBox1.Size = new System.Drawing.Size(100, 25);
            this.velyseControlBox1.TabIndex = 0;
            this.velyseControlBox1.Text = "velyseControlBox1";
            // 
            // RemoteScripting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 455);
            this.Controls.Add(this.velyseForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteScripting";
            this.Text = "Remote Scripting";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.RemoteScripting_Load);
            this.velyseForm1.ResumeLayout(false);
            this.velyseGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VelyseTheme.VelyseForm velyseForm1;
        private VelyseTheme.VelyseControlBox velyseControlBox1;
        private MonoFlat.Button button1;
        private MonoFlat.t_TextBox t_TextBox1;
        private VelyseTheme.VelyseGroupBox velyseGroupBox1;
        private VelyseTheme.VelyseRadioButton velyseRadioButton3;
        private VelyseTheme.VelyseRadioButton velyseRadioButton2;
        private VelyseTheme.VelyseRadioButton velyseRadioButton1;
    }
}