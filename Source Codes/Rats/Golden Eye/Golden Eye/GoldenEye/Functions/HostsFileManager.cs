using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;

namespace GoldenEye.Functions
{
    public class HostsFileManager
    {
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

        public string getHostsText()
        {
            if(IsAdmin() == true)
            {

                return File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts")).Replace(Environment.NewLine, "[newline]");

            } else
            {

                return "Client has no Administrator Privileges!";

            }
            
        }
        public void setHostsText(string text) // Funktioniert nicht (Schreibt nur die 1. Zeile oder so ka)
        {
            if (IsAdmin() == true)
            {
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts"), text.Replace("[newline]", Environment.NewLine));
            }
            else
            { }
        }
    }
}
