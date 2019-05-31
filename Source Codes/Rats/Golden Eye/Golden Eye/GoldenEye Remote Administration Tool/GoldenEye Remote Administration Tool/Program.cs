using GoldenEye_Remote_Administration_Tool.Forms;
using System;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new License());
        }
    }
}