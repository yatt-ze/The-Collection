using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GoldenEye
{
    public class ClientInfo
    {
        public string getInstalledAv()
        {
            try
            {
                string returnThis = "";
                using (var searcher = new ManagementObjectSearcher(@"\\" +
                                        Environment.MachineName +
                                        @"\root\SecurityCenter",
                                        "SELECT * FROM AntivirusProduct"))
                {
                    var searcherInstance = searcher.Get();
                    foreach (var instance in searcherInstance)
                    {
                        returnThis = returnThis + instance["displayName"].ToString() + " ";
                    }
                }

                using (var searcher = new ManagementObjectSearcher(@"\\" +
                                                    Environment.MachineName +
                                                    @"\root\SecurityCenter2",
                                                    "SELECT * FROM AntivirusProduct"))
                {
                    var searcherInstance = searcher.Get();

                    foreach (var instance in searcherInstance)
                    {
                        returnThis = returnThis + instance["displayName"].ToString() + " ";
                    }
                }
                return returnThis;
            }
            catch { return "Unknown"; }
        }

        public String GetSystemUpTimeInfo()
        {
            try
            {
                var time = GetSystemUpTime();
                var upTime = string.Format("{0:D2} Days {1:D2} Hours {2:D2} Minutes {3:D2} Seconds", time.Days, time.Hours, time.Minutes, time.Seconds);
                return String.Format("{0}", upTime);
            }
            catch (Exception ex)
            {
                //handle the exception your way
                return String.Empty;
            }
        }

        private static TimeSpan GetSystemUpTime()
        {
            try
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }
            catch (Exception ex)
            {
                //handle the exception your way
                return new TimeSpan(0, 0, 0, 0);
            }
        }

        public string getCpuName()
        {
            ManagementObjectSearcher objvide =
            new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            string returnThis = "";
            foreach (ManagementObject obj in objvide.Get())
            {
                returnThis = returnThis + obj["Name"] + " ";
            }
            return returnThis;
        }

        public string getGpuName()
        {
            ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_VideoController");
            string returnThis = "";
            foreach (ManagementObject obj in objvide.Get())
            {
                returnThis = returnThis + obj["Name"] + " ";
            }
            return returnThis;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        public string getInstalledRAM()
        {
            long memKb;
            GetPhysicallyInstalledSystemMemory(out memKb);
            return (memKb / 1024 / 1024) + " GB";
        }

        public string getMonitorCount()
        {
            int count = 0;
            foreach (var screen in Screen.AllScreens)
            {
                count++;
            }
            return count.ToString();
        }
    }
}