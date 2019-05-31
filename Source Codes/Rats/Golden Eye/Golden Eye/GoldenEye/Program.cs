using System;

namespace GoldenEye
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            MainWorker pr = new MainWorker();
            pr.Main();
        }
    }
}