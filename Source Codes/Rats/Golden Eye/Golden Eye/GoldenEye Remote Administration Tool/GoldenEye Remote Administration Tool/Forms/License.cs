using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool.Forms
{
    public partial class License : Form
    {
        public Form1 frm1;
        private static TcpClient client = new TcpClient();
        private static IPEndPoint point = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 9008); // 192.168.178.43

        public string hwid = string.Empty;
        public string User = string.Empty;
        public string Pass = string.Empty;
        public string ratver = "1.1.6";

        public License()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        public string hideMePls(string r, string d)
        {
            if (this.InvokeRequired)
            {
                return (string)this.Invoke((Func<string, string, string>)hideMePls, r, d);
            }
            this.ShowInTaskbar = false;
            this.Opacity = 0;
            return ""; // lesender Zugriff
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        }

        private int trieds = 0;

        private void Connect(string username, string password, string hwid)
        {
            wtff:
            if (trieds > 5)
            {
                MessageBox.Show("Could not connect to License Server!");
                Environment.Exit(0);
            }
            try
            {
                client.Connect(point);
                Send("REQLOGIN|" + username + "|" + password + "|" + hwid + "|" + "0 Clients" + "|" + ratver);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch
            {
                Thread.Sleep(200);
                trieds = trieds + 1;
                goto wtff;
            }
        }

        private void ConnectButRegister(string username, string password, string hwid, string licensekey)
        {
            wtff:
            if (trieds > 5)
            {
                MessageBox.Show("Could not connect to License Server!");
                Environment.Exit(0);
            }
            try
            {
                client.Connect(point);
                Send("REQREGISTER|" + username + "|" + password + "|" + hwid + "|" + licensekey + "|" + ratver);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch
            {
                Thread.Sleep(200);
                trieds = trieds + 1;
                goto wtff;
            }
        }

        private void Read(IAsyncResult ar)
        {
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                Parse(reader.ReadLine());
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception eax)
            {
                Thread.Sleep(500);
                //  MessageBox.Show(eax.ToString());
                MessageBox.Show("Lost connection to License Server");
                try
                {
                    frm1.Close();
                }
                catch { }
                Environment.Exit(0);
            }
        }

        private void Parse(string msg)
        {
            string[] cut = msg.Split('|');
            switch (cut[0])
            {
                case "GRAND":
                    Invoke(new _doMagic(doMagic));
                    break;

                case "NOACC":
                    MessageBox.Show("Error while Authentification:\n\n" + cut[1]);
                    Invoke(new _doMagic2(doMagic2));
                    Environment.Exit(0);
                    break;

                case "REGOK":
                    MessageBox.Show("The Registration of your License Key was successful!\n The Application will restart now. After that, you can log in to your Account.");
                    var filePath = Assembly.GetExecutingAssembly().Location;
                    Process.Start(filePath);
                    Environment.Exit(0);
                    break;

                case "RECVBUILD":
                    string path = cut[1];
                    File.WriteAllBytes(path, Convert.FromBase64String(cut[2]));
                    MessageBox.Show("The File has been successfully created!\nPath:" + path, "Building Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "RECVUPD":
                    string newVer = cut[1];
                    string dlLink = cut[2];
                    MessageBox.Show("There is a new Version out!\n Starting to update on Version " + newVer + "...", "Auto Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    WebClient webClienta = new WebClient();
                    webClienta.DownloadFile(dlLink, Application.StartupPath + @"\" + Path.GetFileNameWithoutExtension(Application.StartupPath) + " - " + newVer);
                    MessageBox.Show("Successfully downloaded the newest Version.\nThe Program will now close and start the newest Version!", "Auto Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(Application.StartupPath + @"\" + Path.GetFileNameWithoutExtension(Application.StartupPath) + " - " + newVer + ".exe");
                    Environment.Exit(0);
                    break;
            }
        }

        private delegate void _doMagic();

        private void doMagic()
        {
            frm1 = new Form1(this);
            frm1.authed = true;
            frm1.t_Label13.Text = "ONLINE";
            frm1.t_Label13.ForeColor = Color.Green;
            frm1.Show();
            Send("ASKUPDATE|" + frm1.RatVersion);
            hideMePls("x", "d");
        }

        private delegate void _doMagic2();

        private void doMagic2()
        {
            frm1.authed = false;
        }

        public void Send(string msg)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(msg);
                writer.Flush();
            }
            catch
            {
            }
        }

        private void velyseButton1_Click(object sender, EventArgs e)
        {
            if (t_TextBox1.Text.Contains("|"))
            {
                MessageBox.Show("You cant use the Character '|' !");
                return;
            }
            if (t_TextBox2.Text.Contains("|"))
            {
                MessageBox.Show("You cant use the Character '|' !");
                return;
            }

            if (velyseCheckbox1.Checked == true)
            {
                string loginInfo = t_TextBox1.Text + "|" + t_TextBox2.Text;
                string encrypted = AES_Encrypt(loginInfo, hwid);
                File.WriteAllText(Application.StartupPath + @"\Configuration\LoginRememberMe.ini", encrypted);
            }
            Connect(t_TextBox1.Text, t_TextBox2.Text, hwid);
            User = t_TextBox1.Text;
            Pass = t_TextBox2.Text;
        }

        private void velyseButton2_Click(object sender, EventArgs e)
        {
            if (t_TextBox3.Text.Contains("|"))
            {
                MessageBox.Show("You cant use the Character '|' !");
                return;
            }
            if (t_TextBox4.Text.Contains("|"))
            {
                MessageBox.Show("You cant use the Character '|' !");
                return;
            }
            if (t_TextBox5.Text.Contains("|"))
            {
                MessageBox.Show("You cant use the Character '|' !");
                return;
            }

            ConnectButRegister(t_TextBox3.Text, t_TextBox4.Text, hwid, t_TextBox5.Text);
        }

        private void velyseForm1_Click(object sender, EventArgs e)
        {
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
        public string AES_Decrypt(string input, string pass)
        {
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string decrypted = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = System.Security.Cryptography.CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESDecrypter = AES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(input);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return decrypted;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
        public string getRAMSerial()
        {
            string ramSerial = "";
            string query = "SELECT SerialNumber FROM Win32_PhysicalMemory";
            foreach (ManagementObject MO in getInfo(query))
            {
                if (MO["SerialNumber"] != null)
                {
                    ramSerial = ramSerial + MO["SerialNumber"].ToString();
                }
            }

            return ramSerial;
        }

        public ManagementObjectCollection getInfo(string query)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection oC = searcher.Get();
            searcher.Dispose();
            return oC;
        }

        public string getHardwareHash()
        {
            string hwHash = getRAMSerial();
            hwHash = hwHash.Replace(":", "");
            hwHash = hwHash.Replace(" ", "");
            byte[] bytes = Encoding.Unicode.GetBytes(hwHash);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = "";
            foreach (byte b in hash)
            {
                hashString += string.Format("{0:x2}", b);
            }
            return Encode(hashString);
        }

        public string Decode(string input)
        {
            return Run(input);
        }

        public string Encode(string input)
        {
            return Run(input);
        }

        private string Run(string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = (int)array[i];

                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }

        private void License_Load(object sender, EventArgs e)
        {
            hwid = getHardwareHash();
            if(File.Exists(Application.StartupPath + @"\Configuration\LoginRememberMe.ini"))
            {
                string decrypted = AES_Decrypt(File.ReadAllText(Application.StartupPath + @"\Configuration\LoginRememberMe.ini"), hwid);
                Connect(decrypted.Split('|')[0], decrypted.Split('|')[1], hwid);
                User = decrypted.Split('|')[0];
                Pass = decrypted.Split('|')[1];
            }

        }
    }
}