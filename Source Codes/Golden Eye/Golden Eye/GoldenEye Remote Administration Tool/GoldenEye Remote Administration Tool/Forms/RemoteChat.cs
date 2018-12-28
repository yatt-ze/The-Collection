using System;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class RemoteChat : Form
    {
        public Connection clientlcl;

        public RemoteChat()
        {
            InitializeComponent();
        }

        private void RemoteChat_Load(object sender, EventArgs e)
        {
        }

        public void addmsg(string name, string msg)
        {
            richTextBox1.Text += name + ": " + msg + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Start Chatting"))
            {
                t_TextBox2.Enabled = true;
                button2.Enabled = true;
                t_TextBox1.Enabled = false;
                clientlcl.Send("launchChat|" + t_TextBox1.Text);
                button1.Text = "Stop Chatting";
            }
            else
            {
                clientlcl.Send("EndChat|" + t_TextBox1.Text);
                button1.Text = "Start Chatting";
                t_TextBox2.Enabled = false;
                button2.Enabled = false;
                t_TextBox1.Enabled = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (button1.Text.Equals("Stop Chatting"))
            {
                MessageBox.Show("Please Stop the Chatting first!");
                e.Cancel = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientlcl.Send("chatMsg|" + t_TextBox1.Text + "|" + t_TextBox2.Text);
            richTextBox1.Text += t_TextBox1.Text + "[You]: " + t_TextBox2.Text + Environment.NewLine;
            t_TextBox2.Text = "";
        }

        public void closedByMachine()
        {
            richTextBox1.Text += "------------- ! ATTENTION ! -------------" + Environment.NewLine;
            richTextBox1.Text += "The User has left(closed) the Chat!" + Environment.NewLine;
            richTextBox1.Text += "-----------------------------------------" + Environment.NewLine;

            clientlcl.Send("EndChat|" + t_TextBox1.Text);
            button1.Text = "Start Chatting";
            t_TextBox2.Enabled = false;
            button2.Enabled = false;
            t_TextBox1.Enabled = true;
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }
    }
}