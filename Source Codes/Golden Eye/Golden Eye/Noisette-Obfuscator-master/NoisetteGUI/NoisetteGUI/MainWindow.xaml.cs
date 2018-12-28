using dnlib.DotNet;
using Noisette;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NoisetteGUI
{
    public partial class MainWindow : Window
    {
        BackgroundWorker bgWorker = new BackgroundWorker();
        public static string _file;

        Storyboard sb, sb2;
        int error;
        string errorMG;

        public MainWindow()
        {
            InitializeComponent();
            bgWorker.DoWork +=
         new DoWorkEventHandler(bgWorker_DoWork);

            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.WorkerReportsProgress = true;

            sb = this.FindResource("Rct_white_storyB") as Storyboard;

        }

        public void startanimation()
        {
           
        }
        private void ExitButton_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void MaximizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void DragTheForm(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void textBox_Drop(object sender, DragEventArgs e)
        {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0) return;
            int error = 0;
            string errormsg = null;
            _file = files[0];
            bgWorker.RunWorkerAsync();

            this.Box_dropArea.Visibility = Visibility.Hidden;
            this.textBlock.Visibility = Visibility.Hidden;
            this.textBox.Visibility = Visibility.Hidden;

            sb.Begin();
            
        }

        public void OnDragOver(object sender, DragEventArgs e)

        {
            e.Effects = DragDropEffects.All;

            e.Handled = true;
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (error != 0)
            {
                EndWindow end = new NoisetteGUI.EndWindow
                {
                    NoError_btn = { Visibility = Visibility.Hidden },
                    NoError_txt = { Visibility = Visibility.Hidden },
                    Error_btn = { Visibility = Visibility.Visible },
                    Error_txt = { Visibility = Visibility.Visible }
                };
                end.LogTXT.Document.Blocks.Clear();
                end.LogTXT.AppendText(errorMG);
                end.Show();
                this.Hide();
            }
            else
            {
                EndWindow end = new NoisetteGUI.EndWindow();
                end.LogTXT.Document.Blocks.Clear();
                end.LogTXT.AppendText("All is ok apparently... :)");
                end.Show();
                this.Hide();
            }
        }


        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                NoisetteCore.Obfuscation.ObfuscationProcess Obf =
                    new NoisetteCore.Obfuscation.ObfuscationProcess(ModuleDefMD.Load(_file));
                Obf.DoObfusction();
            }
            catch (Exception ex)
            {
                //something went wrong
                error = 1;
                errorMG = ex.ToString();
            }
           
        }
    }
}