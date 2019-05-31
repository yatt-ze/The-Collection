using GoldenEye_Remote_Administration_Tool;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HunaRAT.Forms
{
    public partial class RemoteDesktop : Form
    {
        public Connection clientLcl;
        public Size Sz;

        public RemoteDesktop()
        {
            InitializeComponent();
        }

        private void RemoteDesktop_Load(object sender, EventArgs e)
        {
            clientLcl.Send("REMSIZE");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (button1.Text == "Stop")
            {
                MessageBox.Show("Please Stop the Remote Desktop Connection first!");
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            panel1.Left = (this.Width / 2) - (panel1.Width / 2);
            button6.Left = (this.Width / 2) - (button6.Width / 2);

        }
        Point pt = new Point(1, 1);
        private void mousemoveREAL(object sender, MouseEventArgs e)
        {

        }
        private void mousemoveDOWN(object sender, MouseEventArgs e)
        {
            if (button2.BackColor.Equals(Color.Green))
            {
                Point PP = new Point(e.X * (Sz.Width / pictureBox1.Width), e.Y * (Sz.Height / pictureBox1.Height));

                int but = 0;
                if (e.Button == MouseButtons.Left)
                {
                    but = 1;
                }
                if (e.Button == MouseButtons.Right)
                {
                    but = 2;
                }
                //      clientLcl.Send("REMOUS|" + PP.X + "|" + PP.Y + "|" + but);
                clientLcl.Send("REMOUSdown|" + PP.X + "|" + (PP.Y + 30) + "|" + but);
            }
        }
        private void mousemoveUP(object sender, MouseEventArgs e)
        {
            if (button2.BackColor.Equals(Color.Green))
            {
                Point PP = new Point(e.X * (Sz.Width / pictureBox1.Width), e.Y * (Sz.Height / pictureBox1.Height));

                int but = 0;
                if (e.Button == MouseButtons.Left)
                {
                    but = 1;
                }
                if (e.Button == MouseButtons.Right)
                {
                    but = 2;
                }

                clientLcl.Send("REMOUSup|" + PP.X + "|" + (PP.Y + 30) + "|" + but);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                // Remote Desktop starten
                clientLcl.Send("RDESKTOP|start");
                button1.Text = "Stop";
            }
            else
            {
                // Remote Desktop beenden
                clientLcl.Send("RDESKTOP|stop");
                button1.Text = "Start";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            button6.Visible = true;
            button5.Visible = false;
            this.ActiveControl = pictureBox1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            button6.Visible = false;
            button5.Visible = true;
            this.ActiveControl = pictureBox1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.BackColor.Equals(Color.Red))
            {
                button2.BackColor = Color.Green;
            } else
            {
                button2.BackColor = Color.Red;
            }
        }
    }
}