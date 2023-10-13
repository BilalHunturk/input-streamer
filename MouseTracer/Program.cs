using Keystroke.API;
using MouseTracer.WindowService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace MouseTracer
{
    internal static class Program
    {
        public static MouseHook MouseHook;
        public static KeyStrokeApiController keystrokeAPIc;
        public static WindowManager windowManager;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                MouseHook = new PollingHook(16);
                windowManager = new WindowManager();
                keystrokeAPIc = new KeyStrokeApiController();
                MouseHook.Start();
                keystrokeAPIc.StartProcess();

                Application.Run(new MainWindow());
                MouseHook.Stop();
                //keystrokeAPIc.Stop();
            }
            catch (Exception e )
            {

                MessageBox.Show(e.Message);
            }
            
        }
    }
}
