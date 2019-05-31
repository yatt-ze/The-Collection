using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using Noisette.Core;
using Type = Noisette.Core.Type;

namespace Noisette
{
    public partial class MainForm : Form
    {
        #region Declarations

        public string DirectoryName = "";
        public int ConstantKey;
        public int ConstantNum;
        public MethodDef Methoddecryption;
        public TypeDef Typedecryption;
        public MethodDef MethodeResource;
        public TypeDef TypeResource;
        public ModuleDefMD module;
        public int x;
        public int DeobedStringNumber;

        #endregion

        #region Form_and_Design

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
            );

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 10, 10));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {

                //



                textBox0.Visible = false;
                label5.Visible = false;
                label4.Visible = false;
                panel2.Visible = false;
                shapedPanel1.Visible = false;
                pictureBox5.Visible = false;


                pictureBox4.Visible = true;

                label1.Visible = true;
                label1.BringToFront();
                label2.Visible = true;
                label2.BringToFront();
                pictureBox6.Visible = true;
                label6.Visible = true;
                label6.BringToFront();

                logbox.Visible = true;
                logbox.BringToFront();
                //
                Array array = (Array) e.Data.GetData(DataFormats.FileDrop);
                if (array == null) return;
                string text = array.GetValue(0).ToString();
                int num = text.LastIndexOf(".", StringComparison.Ordinal);
                if (num == -1) return;
                string text2 = text.Substring(num);
                text2 = text2.ToLower();
                if (text2 != ".exe" && text2 != ".dll") return;
                //Activate();

                textBox1.Text = text;
                ModuleDefMD module = ModuleDefMD.Load(textBox1.Text);
                logbox.AppendText("---------------------------" + Environment.NewLine);
                logbox.AppendText("Obfuscation process started on " + module.Name + Environment.NewLine);
                logbox.AppendText("---------------------------" + Environment.NewLine);


                DoObfusction(module);

                logbox.AppendText("Done ! :)" + Environment.NewLine);

                int num2 = text.LastIndexOf("\\", StringComparison.Ordinal);
                if (num2 != -1)
                {
                    DirectoryName = text.Remove(num2, text.Length - num2);
                }
                if (DirectoryName.Length == 2)
                {
                    DirectoryName += "\\";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                //
                textBox0.Visible = true;
                label5.Visible = true;
                label4.Visible = true;
                panel2.Visible = true;
                shapedPanel1.Visible = true;
                pictureBox5.Visible = true;

                label1.Visible = false;
                label2.Visible = false;
                pictureBox4.Visible = false;

                //
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;

        }

        private void textBox0_TextChanged(object sender, EventArgs e)
        {

        }



        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

            Point[] polygonPoints2 = new Point[5];

            polygonPoints2[0] = new Point(0, 0);
            polygonPoints2[1] = new Point(0, 0);
            polygonPoints2[2] = new Point(0, 0);
            polygonPoints2[3] = new Point(200, 35);
            polygonPoints2[4] = new Point(0, 200);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            e.Graphics.DrawPolygon(new Pen(new SolidBrush(Color.FromArgb(214, 32, 54))), polygonPoints2);
            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(214, 32, 54)), polygonPoints2);

            Point[] polygonPoints = new Point[5];

            polygonPoints[0] = new Point(0, 290);
            polygonPoints[1] = new Point(0, 50);
            polygonPoints[2] = new Point(290, 0);
            polygonPoints[3] = new Point(290, 156);
            polygonPoints[4] = new Point(250, 290);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            e.Graphics.DrawPolygon(new Pen(new SolidBrush(Color.FromArgb(234, 228, 228))), polygonPoints);
            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(234, 228, 228)), polygonPoints);

        }


        #endregion


        public static void DoObfusction(ModuleDefMD module)
        {
            //outline constant
            //OutlineConstant(module);


            //rename all
            RenameModule(module);
            var opts = new ModuleWriterOptions(module);
            opts.Logger = DummyLogger.NoThrowInstance;
            module.Write(module.Location + "_protected.exe", opts);
        }

        public static void OutlineConstant(ModuleDefMD module)
        {
            /*Our arrays*/
            //Constant
            List<MethodDef> ProxyMethodConst = new List<MethodDef>();
            //String
            List<MethodDef> ProxyMethodStr = new List<MethodDef>();

            foreach (TypeDef type in module.Types)
            {
                if (type.IsGlobalModuleType)
                {
                    continue;
                }
                for (int i = 0; i < type.Methods.Count; i++)
                {
                    MethodDef method = type.Methods[i];
                    if (Helper.IsValidMethod(method) && (!ProxyMethodConst.Contains(method)))
                    {
                        for (int index = 0; index < method.Body.Instructions.Count; index++)
                        {
                            Instruction instr = method.Body.Instructions[index];
                            if (instr.IsLdcI4())
                            {
                                MethodDef proxy_method = Core.Helper.CreateReturnMethodDef(instr.GetLdcI4Value(), method);
                                type.Methods.Add(proxy_method);
                                ProxyMethodConst.Add(proxy_method);
                                instr.OpCode = OpCodes.Call;
                                instr.Operand = proxy_method;
                            }
                            else if (instr.OpCode == OpCodes.Ldc_R4)
                            {
                                MethodDef proxy_method = Core.Helper.CreateReturnMethodDef(instr, method);
                                type.Methods.Add(proxy_method);
                                ProxyMethodConst.Add(proxy_method);
                                instr.OpCode = OpCodes.Call;
                                instr.Operand = proxy_method;
                            }
                            if (instr.Operand is string && instr.OpCode == OpCodes.Ldstr)
                            {
                                MethodDef proxy_method = Core.Helper.CreateReturnMethodDef(instr, method);
                                type.Methods.Add(proxy_method);
                                ProxyMethodConst.Add(proxy_method);
                                instr.OpCode = OpCodes.Call;
                                instr.Operand = proxy_method;
                            }

                        }
                    }
                }
            }
        }


        public static void RenameModule(ModuleDefMD module)
        {
            List<String> Methname = new List<string>(Renaming.Method1);
            List<String> Typename = new List<string>(Renaming.Type1);

            Random random = new Random();





            //

            foreach (TypeDef type in module.Types)
            {
                if (type.IsGlobalModuleType) continue;
                string new_name_type = Typename[random.Next(0, Typename.Count)];
                Typename.Remove(new_name_type);
                type.Name = new_name_type;
                foreach (MethodDef method in type.Methods)
                {
                    if (method.IsConstructor) continue;
                    if (!method.HasBody) continue;
                    if (method.FullName.Contains("My.")) continue; //VB gives cancer anyway
                    if (method.FullName.Contains("ch.")) continue;
                    if (method.FullName.Contains(".resources")) continue;
                    string new_name_meth = Methname[random.Next(0, Methname.Count)];
                    Methname.Remove(new_name_meth);
                    method.Name = new_name_meth;
                    foreach (Parameter arg in method.Parameters)
                    {
                        string new_name_param = Methname[random.Next(0, Methname.Count)];
                        Methname.Remove(new_name_param);
                        arg.Name = new_name_param;
                    }
                    if (!method.Body.HasVariables) continue;
                    foreach (var variable in method.Body.Variables)
                    {
                        string new_name_var = Methname[random.Next(0, Methname.Count)];
                        Methname.Remove(new_name_var);
                        variable.Name = new_name_var;
                    }
                }

                foreach (PropertyDef prop in type.Properties)
                {
                    string new_name_property = Methname[random.Next(0, Methname.Count)];
                    Methname.Remove(new_name_property);
                    prop.Name = new_name_property;
                }
                foreach (FieldDef field in type.Fields)
                {
                    string new_name_fields = Methname[random.Next(0, Methname.Count)];
                    Methname.Remove(new_name_fields);
                    field.Name = new_name_fields;
                }
            }


        }

        public static string tst()
        {
            return "test";
        }



        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Noisette - The nuts-breaker obfuscator"
                + Environment.NewLine + 
                "Made by XenocodeRCE - 2016"
                + Environment.NewLine + 
                "dnlib by 0xd4d");
        }
    }
}