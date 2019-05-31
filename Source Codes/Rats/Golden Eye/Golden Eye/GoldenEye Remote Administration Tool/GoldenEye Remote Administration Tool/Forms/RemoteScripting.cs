using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class RemoteScripting : Form
    {
        public List<Connection> Clientlist = new List<Connection>();

        public RemoteScripting()
        {
            InitializeComponent();
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }

        private void checkbuttons()
        {
            if (velyseRadioButton1.Checked == true)
            {
                // BATCH
                t_TextBox1.Text = @"@echo off
title Hello there
echo This is a test batchfile!
pause
";
            }
            else if (velyseRadioButton2.Checked == true)
            {
                // HTML
                t_TextBox1.Text = @"<!DOCTYPE html>
<html>
<body>
<p>This is a example test.</p>
</body>
</html>
";
            }
            else if (velyseRadioButton3.Checked == true)
            {
                // VBS

          
                t_TextBox1.Text = "x = msgbox(\"VB Scripting!\", 0, \"Yay!\")";
            }
        }

        private void velyseRadioButton1_Click(object sender, EventArgs e)
        {
            checkbuttons();
        }

        private void velyseRadioButton2_Click(object sender, EventArgs e)
        {
            checkbuttons();
        }

        private void velyseRadioButton3_Click(object sender, EventArgs e)
        {
            checkbuttons();
        }

        private void velyseGroupBox1_Click(object sender, EventArgs e)
        {
        }

        private void velyseRadioButton4_Click(object sender, EventArgs e)
        {
            checkbuttons();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Connection client in Clientlist)
            {
                if (velyseRadioButton1.Checked == true)
                {
                    client.Send("script|" + "bat" + "|" + t_TextBox1.Text.Replace(Environment.NewLine, "[newline]"));
                }
                else if (velyseRadioButton2.Checked == true)
                {
                    client.Send("script|" + "html" + "|" + t_TextBox1.Text.Replace(Environment.NewLine, "[newline]"));
                }
                else if (velyseRadioButton3.Checked == true)
                {
                    client.Send("script|" + "vbs" + "|" + t_TextBox1.Text.Replace(Environment.NewLine, "[newline]"));
                }
            }
        }

        private void RemoteScripting_Load(object sender, EventArgs e)
        {
            t_TextBox1.Text = @"@echo off
title Hello there
echo This is a test batchfile!
pause
";
        }
    }
}