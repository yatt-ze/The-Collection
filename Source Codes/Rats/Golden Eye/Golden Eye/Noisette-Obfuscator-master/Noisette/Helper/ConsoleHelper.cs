using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisetteCore.Helper
{
    public class ConsoleHelper
    {
        public DateTime begin;

        public int ReturnValue { get; private set; }

        public void Debug(string msg)
        {
            WriteLineWithColor(ConsoleColor.Gray, "[DEBUG] " + msg);
        }

        public void DebugFormat(string format, params object[] args)
        {
            WriteLineWithColor(ConsoleColor.Gray, "[DEBUG] " + string.Format(format, args));
        }

        public void Info(string msg)
        {
            WriteLineWithColor(ConsoleColor.White, " [INFO] " + msg);
        }

        public void InfoFormat(string format, params object[] args)
        {
            WriteLineWithColor(ConsoleColor.White, " [INFO] " + string.Format(format, args));
        }

        public void Warn(string msg)
        {
            WriteLineWithColor(ConsoleColor.Yellow, " [WARN] " + msg);
        }

        public void WarnFormat(string format, params object[] args)
        {
            WriteLineWithColor(ConsoleColor.Yellow, " [WARN] " + string.Format(format, args));
        }

        public void WarnException(string msg, Exception ex)
        {
            WriteLineWithColor(ConsoleColor.Yellow, " [WARN] " + msg);
            WriteLineWithColor(ConsoleColor.Yellow, "Exception: " + ex);
        }

        public void Error(string msg)
        {
            WriteLineWithColor(ConsoleColor.Red, "[ERROR] " + msg);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            WriteLineWithColor(ConsoleColor.Red, "[ERROR] " + string.Format(format, args));
        }

        public void ErrorException(string msg, Exception ex)
        {
            WriteLineWithColor(ConsoleColor.Red, "[ERROR] " + msg);
            WriteLineWithColor(ConsoleColor.Red, "Exception: " + ex);
        }

        private static void WriteLineWithColor(ConsoleColor color, string txt)
        {
            ConsoleColor original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
            Console.ForegroundColor = original;
        }

        public void Progress(int progress, int overall)
        {
        }

        public void EndProgress()
        {
        }

        public void Finish()
        {
            DateTime now = DateTime.Now;
            string timeString = string.Format(
                "at {0}, {1}:{2:d2} elapsed.",
                now.ToShortTimeString(),
                (int)now.Subtract(begin).TotalMinutes,
                now.Subtract(begin).Seconds);
        }
    }
}