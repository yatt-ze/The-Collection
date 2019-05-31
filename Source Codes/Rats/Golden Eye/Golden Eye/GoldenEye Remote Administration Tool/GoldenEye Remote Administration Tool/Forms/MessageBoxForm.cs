using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class MessageBoxForm : Form
    {
        public List<Connection> Clientlist = new List<Connection>();

        public MessageBoxForm()
        {
            InitializeComponent();
        }

        private void velyseGroupBox4_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Connection client in Clientlist)
            {
                client.Send("MESSAGEBOX|" + checkIcon() + "|" + checkButtons() + "|" + t_TextBox1.Text + "|" + t_TextBox2.Text.Replace(Environment.NewLine, "[newline]") + "|" + velyseNumericButton1.Value.ToString());
            }
        }

        private string checkButtons()
        {
            if (velyseRadioButton5.Checked == true)
            {
                return "YesNo";
            }
            else if (velyseRadioButton6.Checked == true)
            {
                return "YesNoCancel";
            }
            else if (velyseRadioButton7.Checked == true)
            {
                return "Ok";
            }
            else if (velyseRadioButton8.Checked == true)
            {
                return "OkCancel";
            }
            else if (velyseRadioButton9.Checked == true)
            {
                return "RetryCancel";
            }
            else if (velyseRadioButton10.Checked == true)
            {
                return "AbortRetryIgnore";
            }
            else
            {
                return "Ok";
            }
        }

        private string checkIcon()
        {
            if (velyseRadioButton1.Checked == true)
            {
                return "Info";
            }
            else if (velyseRadioButton2.Checked == true)
            {
                return "Question";
            }
            else if (velyseRadioButton3.Checked == true)
            {
                return "Warning";
            }
            else if (velyseRadioButton4.Checked == true)
            {
                return "Error";
            }
            else
            {
                return "Info";
            }
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }
    }
}