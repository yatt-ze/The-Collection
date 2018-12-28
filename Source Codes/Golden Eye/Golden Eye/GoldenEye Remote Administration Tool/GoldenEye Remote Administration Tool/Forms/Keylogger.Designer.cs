namespace GoldenEye_Remote_Administration_Tool.Forms
{
    partial class Keylogger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Keylogger));
            this.velyseForm1 = new VelyseTheme.VelyseForm();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.velyseButton1 = new VelyseTheme.VelyseButton();
            this.velyseControlBox1 = new VelyseTheme.VelyseControlBox();
            this.velyseForm1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // velyseForm1
            // 
            this.velyseForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.Controls.Add(this.panel2);
            this.velyseForm1.Controls.Add(this.panel1);
            this.velyseForm1.Controls.Add(this.velyseControlBox1);
            this.velyseForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.velyseForm1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.Location = new System.Drawing.Point(0, 0);
            this.velyseForm1.Name = "velyseForm1";
            this.velyseForm1.Padding = new System.Windows.Forms.Padding(10, 70, 10, 9);
            this.velyseForm1.RoundCorners = VelyseTheme.VelyseForm.VLRoundCorner.Round;
            this.velyseForm1.Sizable = true;
            this.velyseForm1.Size = new System.Drawing.Size(800, 450);
            this.velyseForm1.SmartBounds = true;
            this.velyseForm1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.velyseForm1.TabIndex = 0;
            this.velyseForm1.Text = "Keylogger";
            this.velyseForm1.Text_Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.TextColor = System.Drawing.Color.White;
            this.velyseForm1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseForm1.Click += new System.EventHandler(this.velyseForm1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(10, 120);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(780, 321);
            this.panel2.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(780, 321);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.velyseButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(780, 50);
            this.panel1.TabIndex = 1;
            // 
            // velyseButton1
            // 
            this.velyseButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(28)))));
            this.velyseButton1.Location = new System.Drawing.Point(13, 8);
            this.velyseButton1.MouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(132)))), ((int)(((byte)(165)))));
            this.velyseButton1.Name = "velyseButton1";
            this.velyseButton1.Size = new System.Drawing.Size(191, 33);
            this.velyseButton1.TabIndex = 0;
            this.velyseButton1.Text = "Get Keylogs";
            this.velyseButton1.TextColor = System.Drawing.Color.White;
            this.velyseButton1.VMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(122)))), ((int)(((byte)(155)))));
            this.velyseButton1.Click += new System.EventHandler(this.velyseButton1_Click);
            // 
            // velyseControlBox1
            // 
            this.velyseControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.velyseControlBox1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseControlBox1.EnableHoverHighlight = false;
            this.velyseControlBox1.EnableMaximizeButton = true;
            this.velyseControlBox1.EnableMinimizeButton = true;
            this.velyseControlBox1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseControlBox1.Location = new System.Drawing.Point(698, 0);
            this.velyseControlBox1.Name = "velyseControlBox1";
            this.velyseControlBox1.Size = new System.Drawing.Size(100, 25);
            this.velyseControlBox1.TabIndex = 0;
            this.velyseControlBox1.Text = "velyseControlBox1";
            // 
            // Keylogger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.velyseForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Keylogger";
            this.Text = "Keylogger";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.velyseForm1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VelyseTheme.VelyseForm velyseForm1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private VelyseTheme.VelyseButton velyseButton1;
        private VelyseTheme.VelyseControlBox velyseControlBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}