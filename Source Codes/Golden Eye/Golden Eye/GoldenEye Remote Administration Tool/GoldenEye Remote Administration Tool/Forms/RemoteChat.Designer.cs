namespace GoldenEye_Remote_Administration_Tool.Forms
{
    partial class RemoteChat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteChat));
            this.velyseForm1 = new VelyseTheme.VelyseForm();
            this.velyseControlBox1 = new VelyseTheme.VelyseControlBox();
            this.t_TextBox2 = new MonoFlat.t_TextBox();
            this.button2 = new MonoFlat.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.t_Label1 = new MonoFlat.t_Label();
            this.t_TextBox1 = new MonoFlat.t_TextBox();
            this.button1 = new MonoFlat.Button();
            this.velyseForm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // velyseForm1
            // 
            this.velyseForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.Controls.Add(this.velyseControlBox1);
            this.velyseForm1.Controls.Add(this.t_TextBox2);
            this.velyseForm1.Controls.Add(this.button2);
            this.velyseForm1.Controls.Add(this.richTextBox1);
            this.velyseForm1.Controls.Add(this.t_Label1);
            this.velyseForm1.Controls.Add(this.t_TextBox1);
            this.velyseForm1.Controls.Add(this.button1);
            this.velyseForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.velyseForm1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.Location = new System.Drawing.Point(0, 0);
            this.velyseForm1.Name = "velyseForm1";
            this.velyseForm1.Padding = new System.Windows.Forms.Padding(10, 70, 10, 9);
            this.velyseForm1.RoundCorners = VelyseTheme.VelyseForm.VLRoundCorner.Round;
            this.velyseForm1.Sizable = true;
            this.velyseForm1.Size = new System.Drawing.Size(540, 383);
            this.velyseForm1.SmartBounds = true;
            this.velyseForm1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.velyseForm1.TabIndex = 0;
            this.velyseForm1.Text = "Remote Chat";
            this.velyseForm1.Text_Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.TextColor = System.Drawing.Color.White;
            this.velyseForm1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseForm1.Click += new System.EventHandler(this.velyseForm1_Click);
            // 
            // velyseControlBox1
            // 
            this.velyseControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.velyseControlBox1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseControlBox1.EnableHoverHighlight = false;
            this.velyseControlBox1.EnableMaximizeButton = true;
            this.velyseControlBox1.EnableMinimizeButton = true;
            this.velyseControlBox1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseControlBox1.Location = new System.Drawing.Point(438, 0);
            this.velyseControlBox1.Name = "velyseControlBox1";
            this.velyseControlBox1.Size = new System.Drawing.Size(100, 25);
            this.velyseControlBox1.TabIndex = 7;
            this.velyseControlBox1.Text = "velyseControlBox1";
            // 
            // t_TextBox2
            // 
            this.t_TextBox2.BackColor = System.Drawing.Color.Transparent;
            this.t_TextBox2.Enabled = false;
            this.t_TextBox2.Font = new System.Drawing.Font("Tahoma", 11F);
            this.t_TextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(183)))), ((int)(((byte)(191)))));
            this.t_TextBox2.Image = null;
            this.t_TextBox2.Location = new System.Drawing.Point(13, 332);
            this.t_TextBox2.MaxLength = 32767;
            this.t_TextBox2.Multiline = false;
            this.t_TextBox2.Name = "t_TextBox2";
            this.t_TextBox2.ReadOnly = false;
            this.t_TextBox2.Size = new System.Drawing.Size(362, 43);
            this.t_TextBox2.TabIndex = 5;
            this.t_TextBox2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.t_TextBox2.UseSystemPasswordChar = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button2.Image = null;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(381, 332);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 41);
            this.button2.TabIndex = 6;
            this.button2.Text = "Send";
            this.button2.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Wheat;
            this.richTextBox1.Location = new System.Drawing.Point(13, 120);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(514, 206);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // t_Label1
            // 
            this.t_Label1.AutoSize = true;
            this.t_Label1.BackColor = System.Drawing.Color.Transparent;
            this.t_Label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.t_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(125)))), ((int)(((byte)(132)))));
            this.t_Label1.Location = new System.Drawing.Point(165, 88);
            this.t_Label1.Name = "t_Label1";
            this.t_Label1.Size = new System.Drawing.Size(69, 15);
            this.t_Label1.TabIndex = 3;
            this.t_Label1.Text = "Your Name:";
            // 
            // t_TextBox1
            // 
            this.t_TextBox1.BackColor = System.Drawing.Color.Transparent;
            this.t_TextBox1.Font = new System.Drawing.Font("Tahoma", 11F);
            this.t_TextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(183)))), ((int)(((byte)(191)))));
            this.t_TextBox1.Image = null;
            this.t_TextBox1.Location = new System.Drawing.Point(240, 73);
            this.t_TextBox1.MaxLength = 32767;
            this.t_TextBox1.Multiline = false;
            this.t_TextBox1.Name = "t_TextBox1";
            this.t_TextBox1.ReadOnly = false;
            this.t_TextBox1.Size = new System.Drawing.Size(135, 41);
            this.t_TextBox1.TabIndex = 1;
            this.t_TextBox1.Text = "Administrator";
            this.t_TextBox1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.t_TextBox1.UseSystemPasswordChar = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button1.Image = null;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(381, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start Chatting";
            this.button1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RemoteChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 383);
            this.Controls.Add(this.velyseForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteChat";
            this.Text = "Remote Chat";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.RemoteChat_Load);
            this.velyseForm1.ResumeLayout(false);
            this.velyseForm1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public VelyseTheme.VelyseForm velyseForm1;
        private VelyseTheme.VelyseControlBox velyseControlBox1;
        private MonoFlat.t_TextBox t_TextBox2;
        private MonoFlat.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private MonoFlat.t_Label t_Label1;
        private MonoFlat.t_TextBox t_TextBox1;
        private MonoFlat.Button button1;
    }
}