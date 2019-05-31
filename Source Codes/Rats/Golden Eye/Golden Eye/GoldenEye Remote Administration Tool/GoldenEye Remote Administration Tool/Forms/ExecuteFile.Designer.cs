namespace HunaRAT.Forms
{
    partial class ExecuteFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecuteFile));
            this.themeContainer1 = new MonoFlat.ThemeContainer();
            this.button2 = new MonoFlat.Button();
            this.controlBox1 = new MonoFlat.ControlBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new MonoFlat.RadioButton();
            this.radioButton1 = new MonoFlat.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new MonoFlat.CheckBox();
            this.checkBox1 = new MonoFlat.CheckBox();
            this.t_Label3 = new MonoFlat.t_Label();
            this.textBox3 = new MonoFlat.t_TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new MonoFlat.RadioButton();
            this.radioButton4 = new MonoFlat.RadioButton();
            this.radioButton6 = new MonoFlat.RadioButton();
            this.radioButton5 = new MonoFlat.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new MonoFlat.Button();
            this.t_Label2 = new MonoFlat.t_Label();
            this.textBox2 = new MonoFlat.t_TextBox();
            this.t_Label1 = new MonoFlat.t_Label();
            this.textBox1 = new MonoFlat.t_TextBox();
            this.themeContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // themeContainer1
            // 
            this.themeContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.themeContainer1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.themeContainer1.Controls.Add(this.button2);
            this.themeContainer1.Controls.Add(this.controlBox1);
            this.themeContainer1.Controls.Add(this.groupBox1);
            this.themeContainer1.Controls.Add(this.groupBox3);
            this.themeContainer1.Controls.Add(this.groupBox2);
            this.themeContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themeContainer1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.themeContainer1.Location = new System.Drawing.Point(0, 0);
            this.themeContainer1.Name = "themeContainer1";
            this.themeContainer1.Padding = new System.Windows.Forms.Padding(10, 70, 10, 9);
            this.themeContainer1.RoundCorners = true;
            this.themeContainer1.Sizable = true;
            this.themeContainer1.Size = new System.Drawing.Size(568, 478);
            this.themeContainer1.SmartBounds = true;
            this.themeContainer1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.themeContainer1.TabIndex = 4;
            this.themeContainer1.Text = "Execute a File";
            this.themeContainer1.Click += new System.EventHandler(this.themeContainer1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.button2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button2.Image = null;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(12, 427);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(545, 39);
            this.button2.TabIndex = 10;
            this.button2.Text = "Execute File";
            this.button2.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // controlBox1
            // 
            this.controlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBox1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.controlBox1.EnableHoverHighlight = false;
            this.controlBox1.EnableMaximizeButton = false;
            this.controlBox1.EnableMinimizeButton = false;
            this.controlBox1.Location = new System.Drawing.Point(456, 15);
            this.controlBox1.Name = "controlBox1";
            this.controlBox1.Size = new System.Drawing.Size(100, 25);
            this.controlBox1.TabIndex = 4;
            this.controlBox1.Text = "controlBox1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Execution Mode";
            // 
            // radioButton2
            // 
            this.radioButton2.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.radioButton2.Checked = false;
            this.radioButton2.Location = new System.Drawing.Point(350, 28);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(101, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.Text = "File from URL";
            this.radioButton2.CheckedChanged += new MonoFlat.RadioButton.CheckedChangedEventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(83, 28);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(80, 17);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.Text = "Local File";
            this.radioButton1.CheckedChanged += new MonoFlat.RadioButton.CheckedChangedEventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.t_Label3);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 278);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(545, 140);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File Options";
            // 
            // checkBox2
            // 
            this.checkBox2.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.checkBox2.Checked = false;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.checkBox2.Location = new System.Drawing.Point(95, 97);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(89, 16);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "Protect File";
            // 
            // checkBox1
            // 
            this.checkBox1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.checkBox1.Checked = false;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.checkBox1.Location = new System.Drawing.Point(19, 97);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(71, 16);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Hide File";
            // 
            // t_Label3
            // 
            this.t_Label3.AutoSize = true;
            this.t_Label3.BackColor = System.Drawing.Color.Transparent;
            this.t_Label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(125)))), ((int)(((byte)(132)))));
            this.t_Label3.Location = new System.Drawing.Point(55, 23);
            this.t_Label3.Name = "t_Label3";
            this.t_Label3.Size = new System.Drawing.Size(95, 21);
            this.t_Label3.TabIndex = 11;
            this.t_Label3.Text = "Drop File as:";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Transparent;
            this.textBox3.Font = new System.Drawing.Font("Tahoma", 11F);
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(183)))), ((int)(((byte)(191)))));
            this.textBox3.Image = null;
            this.textBox3.Location = new System.Drawing.Point(19, 47);
            this.textBox3.MaxLength = 32767;
            this.textBox3.Multiline = false;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = false;
            this.textBox3.Size = new System.Drawing.Size(165, 43);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = "TestFile.exe";
            this.textBox3.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox3.UseSystemPasswordChar = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton7);
            this.groupBox4.Controls.Add(this.radioButton4);
            this.groupBox4.Controls.Add(this.radioButton6);
            this.groupBox4.Controls.Add(this.radioButton5);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(203, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(322, 103);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Drop File to:";
            // 
            // radioButton7
            // 
            this.radioButton7.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.radioButton7.Checked = false;
            this.radioButton7.Location = new System.Drawing.Point(176, 62);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(122, 17);
            this.radioButton7.TabIndex = 9;
            this.radioButton7.Text = "Appdata Roaming";
            // 
            // radioButton4
            // 
            this.radioButton4.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(31, 25);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(95, 17);
            this.radioButton4.TabIndex = 6;
            this.radioButton4.Text = "Temp Folder";
            // 
            // radioButton6
            // 
            this.radioButton6.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.radioButton6.Checked = false;
            this.radioButton6.Location = new System.Drawing.Point(31, 62);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(104, 17);
            this.radioButton6.TabIndex = 8;
            this.radioButton6.Text = "Appdata Local";
            // 
            // radioButton5
            // 
            this.radioButton5.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.radioButton5.Checked = false;
            this.radioButton5.Location = new System.Drawing.Point(176, 25);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(115, 17);
            this.radioButton5.TabIndex = 7;
            this.radioButton5.Text = "Programs Folder";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.t_Label2);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.t_Label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(545, 128);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Path / URL";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button1.Image = null;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(443, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 41);
            this.button1.TabIndex = 8;
            this.button1.Text = "Choose";
            this.button1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button1.Click += new System.EventHandler(this.button4_Click);
            // 
            // t_Label2
            // 
            this.t_Label2.AutoSize = true;
            this.t_Label2.BackColor = System.Drawing.Color.Transparent;
            this.t_Label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(125)))), ((int)(((byte)(132)))));
            this.t_Label2.Location = new System.Drawing.Point(7, 85);
            this.t_Label2.Name = "t_Label2";
            this.t_Label2.Size = new System.Drawing.Size(83, 21);
            this.t_Label2.TabIndex = 9;
            this.t_Label2.Text = "Direct D/L:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Transparent;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Tahoma", 11F);
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(183)))), ((int)(((byte)(191)))));
            this.textBox2.Image = null;
            this.textBox2.Location = new System.Drawing.Point(95, 74);
            this.textBox2.MaxLength = 32767;
            this.textBox2.Multiline = false;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = false;
            this.textBox2.Size = new System.Drawing.Size(342, 43);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "https://example.com/files/PleaseExecute.exe";
            this.textBox2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.UseSystemPasswordChar = false;
            // 
            // t_Label1
            // 
            this.t_Label1.AutoSize = true;
            this.t_Label1.BackColor = System.Drawing.Color.Transparent;
            this.t_Label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(125)))), ((int)(((byte)(132)))));
            this.t_Label1.Location = new System.Drawing.Point(7, 37);
            this.t_Label1.Name = "t_Label1";
            this.t_Label1.Size = new System.Drawing.Size(71, 21);
            this.t_Label1.TabIndex = 7;
            this.t_Label1.Text = "File Path:";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Transparent;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 11F);
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(183)))), ((int)(((byte)(191)))));
            this.textBox1.Image = null;
            this.textBox1.Location = new System.Drawing.Point(95, 25);
            this.textBox1.MaxLength = 32767;
            this.textBox1.Multiline = false;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = false;
            this.textBox1.Size = new System.Drawing.Size(342, 43);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.UseSystemPasswordChar = false;
            // 
            // ExecuteFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 478);
            this.Controls.Add(this.themeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExecuteFile";
            this.Text = "Execute a File";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.ExecuteFile_Load);
            this.themeContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private MonoFlat.ThemeContainer themeContainer1;
        private MonoFlat.RadioButton radioButton2;
        private MonoFlat.RadioButton radioButton1;
        private MonoFlat.ControlBox controlBox1;
        private MonoFlat.t_TextBox textBox1;
        private MonoFlat.t_TextBox textBox2;
        private MonoFlat.t_Label t_Label1;
        private MonoFlat.Button button1;
        private MonoFlat.t_Label t_Label2;
        private MonoFlat.CheckBox checkBox2;
        private MonoFlat.CheckBox checkBox1;
        private MonoFlat.t_Label t_Label3;
        private MonoFlat.t_TextBox textBox3;
        private MonoFlat.RadioButton radioButton7;
        private MonoFlat.RadioButton radioButton4;
        private MonoFlat.RadioButton radioButton6;
        private MonoFlat.RadioButton radioButton5;
        private MonoFlat.Button button2;
    }
}