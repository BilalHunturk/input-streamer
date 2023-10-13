using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MouseTracer.WindowService
{
    public class WindowManager
    {
        // Import the FindWindow function from the User32.dll library
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Import the SetWindowText function from the User32.dll library
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowTextA(IntPtr hWnd, string lpString);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public WindowManager()
        {
            StaticWindowConfig.Windows = new List<Window>();
        }


        public void SetIdToWındow()
        {
            EnumWindows((hWnd, lParam) =>
            {
                // Get the window title
                StringBuilder title = new StringBuilder(256);
                GetWindowText(hWnd, title, title.Capacity);

                string windowTitle = title.ToString();

                if (!string.IsNullOrEmpty(windowTitle))
                {
                    if (windowTitle.Contains(StaticWindowConfig.WindowName) && !windowTitle.Contains("Lau"))
                    {
                        int processId;
                        GetWindowThreadProcessId(hWnd, out processId);
                        string newWindowTitle = $"NosTale{processId}";
                        Window newWindow = new Window() { Id = processId, Title = newWindowTitle };
                        newWindow.SethWnd(hWnd);
                        StaticWindowConfig.Windows.Add(newWindow);
                        SetWindowText(hWnd, newWindowTitle);
                    }
                }
                return true; // Continue enumerating windows
            }, IntPtr.Zero);
            Console.WriteLine(StaticWindowConfig.Windows.Count());
        }
        
        public void getWindowsCurrentPosition()
        {
            
            foreach (var window in StaticWindowConfig.Windows)
            {
                Rect pos = new Rect();
                User32.GetWindowRect(window.GethWnd(), ref pos);
                window.SetPos(pos);
                Console.WriteLine($"Top : { window.GetPos().Top} Bottom : {window.GetPos().Bottom } Left : {window.GetPos().Left } Right : {window.GetPos().Right}");
                Console.WriteLine( "windowid : "+window.Id+" window is toggled ?:"+window.IsToggled );
            }
            
        }
    }
}
