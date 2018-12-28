using System;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool
{
    public partial class Form2 : Form
    {
        public Connection client;
        public string cmd = string.Empty;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            client.Send(cmd);
            this.Close();
        }
    }
}