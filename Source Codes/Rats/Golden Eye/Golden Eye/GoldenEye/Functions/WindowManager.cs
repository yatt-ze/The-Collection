using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GoldenEye.Functions
{
    using HWND = IntPtr;

    public class WindowManager
    {
        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;
            }, 0);

            return windows;
        }

        public string getWindowList()
        {
            string sortTrue = string.Empty;
            string sortFalse = string.Empty;

            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                if (IsWindowVisible(handle) == true)
                {
                    sortTrue += title.Replace("|", "/") + "²" + handle + "²" + "True" + "³";
                }
                else
                {
                    sortFalse += title.Replace("|", "/") + "²" + handle + "²" + "False" + "³";
                }
            }
            return sortTrue + sortFalse;
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowText(IntPtr hWnd, string text);

        public void renameWindow(String oldWindowName, String newName)
        {
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                if (title.Equals(oldWindowName))
                {
                    SetWindowText(handle, newName);
                }
            }
        }

        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_MINIMIZE = 6;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public void maximizeWindow(String windowName)
        {
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                if (title.Equals(windowName))
                {
                    ShowWindow(handle, SW_SHOWMAXIMIZED);
                }
            }
        }

        public void minimizeWindow(String windowName)
        {
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                if (title.Equals(windowName))
                {
                    ShowWindow(handle, SW_MINIMIZE);
                }
            }
        }

        public void showWindow(String windowName)
        {
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                if (title.Equals(windowName))
                {
                    ShowWindow(handle, SW_SHOW);
                }
            }
        }

        public void hideWindow(String windowName)
        {
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                if (title.Equals(windowName))
                {
                    ShowWindow(handle, SW_HIDE);
                }
            }
        }
    }
}