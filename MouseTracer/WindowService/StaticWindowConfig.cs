using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseTracer.WindowService
{
    public static class StaticWindowConfig
    {
        public static string WindowName = "NosTale";
        public static List<Window> Windows { get; set; }

        public static int DoubleClickInterval = 70;

        public static bool KeyboardWork = false;
        public static bool MouseWork = false;
        

        public static List<Window> GetWindows()
        {
            return Windows;
        }
        public static List<Window> GetWindowsIfToggled()
        {
            return Windows.FindAll(window => window.IsToggled);
        }
        public static Window GetMainWindow()
        {
            return Windows.Find(window => window.IsMain);
        }
    }
}
