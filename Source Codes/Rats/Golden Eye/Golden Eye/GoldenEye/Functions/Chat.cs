using System;
using System.IO;
using System.Windows.Forms;

namespace GoldenEye
{
    public partial class Chat : Form
    {
        public Chat()
        {
            InitializeComponent();
        }

        public MainWorker ec2;

        public string addchat(string name, string txt)
        {
            if (this.InvokeRequired)
            {
                return (string)this.Invoke((Func<string, string, string>)addchat, name, txt);
            }
            richTextBox1.Text += name + ": " + txt + Environment.NewLine;

            return ""; // lesender Zugriff
        }

        public string closeme(string r, string d)
        {
            if (this.InvokeRequired)
            {
                return (string)this.Invoke((Func<string, string, string>)closeme, r, d);
            }
            this.Close();
            return ""; // lesender Zugriff
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            try
            {
                this.ShowInTaskbar = false;
                this.TopMost = true;
                this.MinimizeBox = false;
                this.MaximizeBox = false;

            } catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StreamWriter writer = new StreamWriter(ec2.client.GetStream());
            writer.WriteLine("chatleft|" + Environment.UserName);
            writer.Flush();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += Environment.UserName + "[You]: " + textBox1.Text + Environment.NewLine;
            StreamWriter writer = new StreamWriter(ec2.client.GetStream());
            writer.WriteLine("chatMSG|" + Environment.UserName + "|" + textBox1.Text);
            writer.Flush();
            textBox1.Text = "";
        }
    }
}