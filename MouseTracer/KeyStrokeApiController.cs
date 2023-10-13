using Keystroke.API;
using Keystroke.API.CallbackObjects;
using MouseTracer.WindowService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseTracer
{
    public  class KeyStrokeApiController
    {
        KeystrokeAPI keystrokeAPI;
        // private readonly Timer keyStroketimer;
        private KeyCode lastKey = KeyCode.None;

        private const int WM_KEYUP = 0x101;
        private const int WM_KEYDOWN = 0x100;

        public bool work = false;

        public KeyStrokeApiController()
        {
            keystrokeAPI = new KeystrokeAPI();
            //keyStroketimer = new Timer {  Interval = interval };
            //keyStroketimer.Tick += KeyStroke_Tick;
        }

        private void KeyStroke_Tick(object sender, EventArgs e)
        {
            StartProcess();
        }

        public void Start()
        {
            //keyStroketimer.Start();
        }

        public void Stop()
        {
            //keyStroketimer.Stop();
        }

        public void StartProcess()
        {
            keystrokeAPI.CreateKeyboardHook((key) => 
          {
              //if (key.KeyCode == lastKey || key == null)
              //   return;

              //if (key.KeyCode == KeyCode.None)
              //{
              //    lastKey = KeyCode.None;
              //    return;
              //}
              //lastKey = key.KeyCode;

              if (!StaticWindowConfig.KeyboardWork)
              {
                  if (key.KeyCode == KeyCode.F6)
                  {
                      StaticWindowConfig.KeyboardWork = true;
                  }
                  return;
              }
              if (key.KeyCode == KeyCode.F6 && StaticWindowConfig.KeyboardWork)
              {
                  StaticWindowConfig.KeyboardWork = false;
                  return;
              }

              CallPressed(key);
            });
        }

        public void CallPressed(KeyPressed keyPressed)
        {
            var keypressed = (IntPtr)(int)keyPressed.KeyCode;
            foreach (var window in StaticWindowConfig.GetWindowsIfToggled())
            {
                User32.SendMessage(window.GethWnd(), WM_KEYDOWN, keypressed, IntPtr.Zero);
                User32.SendMessage(window.GethWnd(), WM_KEYUP, keypressed, IntPtr.Zero);
            }
            Thread.Sleep(10);
            Console.WriteLine();
            //lastKey = KeyCode.None;
        }
    }

}
