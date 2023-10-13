using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using MouseTracer.WindowService;

namespace MouseTracer
{
    public class Tracer : IDisposable
    {
        public Bitmap Image { get; private set; }

        //private readonly //graphics //graph;

        private readonly Rectangle screenBounds;

        private MouseState currentState;

        //private MouseState currentKey;

        private MouseState previousState;

        private MouseState previousPosState;

        private double fadeTravelCounter = 0;

        private Stopwatch noMovementTimer = new Stopwatch();

        private bool running = false;

        public bool FadeOverTime { get; set; } = true;

        public bool DrawClicks { get; set; } = true;

        public bool DrawMouseMove { get; set; } = true;

        public bool DrawMouseStops { get; set; } = true;

        const uint WM_LBUTTONDBLCLK = 0x0203;
        const uint WM_CAPTURECHANGED = 0x0215;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint WM_KEYDOWN = 0x0100; // Key down message
        const uint WM_KEYUP = 0x0101;   // Key up message

        public Tracer()
        {
        }

        public void Dispose()
        {
            SetRunning(false);
            Image.Dispose();
        }

        public void SetRunning(bool run)
        {
            if (run == running) return;

            if (run)
            {
                previousPosState = previousState = currentState = null;
                noMovementTimer.Restart();
				Program.MouseHook.MouseAction += DoMouseEvent;
            }
            else
            {
				Program.MouseHook.MouseAction -= DoMouseEvent;
            }

            running = run;
        }

        private void DoMouseEvent(object sender, MouseStateEventArgs e)
        {
            UpdateMouseHistory(e);


            if (DrawClicks)
            {
                DoDrawMouseClick();
            }
        }

		private void UpdateMouseHistory(MouseStateEventArgs e)
		{
			var pos = e.Position;
			pos.X -= screenBounds.X;
			pos.Y -= screenBounds.Y;

            var newState = new MouseState(pos, e.Buttons);
            //var newKey = new MouseState(e.key);

            if (currentState != null && currentState.Position != newState.Position)
            {
                previousPosState = currentState;
            }

            previousState = currentState;
            currentState = newState;
            //currentKey = newKey;

        }

	
       
        private void DoDrawMouseClick()
        {
            //const float CCD = 15; // click circle diameter

            if (previousPosState == null)
            {
                return;
            }

            var buttons = currentState.Buttons & ~previousState.Buttons;
            
            if (buttons == MouseButtons.None)
            {
                    return;
            }

            var cur = currentState.Position;
            Console.WriteLine("Current position of cursor is, x : "+cur.X+" y :"+cur.Y);

            if (buttons.HasFlag(MouseButtons.Left))
            {
                // This method should be asynchronous
                SendSameClickToOtherClients(cur);
            }

            if (buttons.HasFlag(MouseButtons.Middle))
            {
                // This method should be asynchronous
                SendDoubleClickToOtherClients(cur);
            }

        }

        private void SendDoubleClickToOtherClients(Point cur)
        {
            int lParam = GetCursorPos(cur);
            foreach (var window in StaticWindowConfig.GetWindowsIfToggled())
            {

                User32.SendMessage(window.GethWnd(), WM_LBUTTONDBLCLK, (IntPtr)0x0001, (IntPtr)lParam);
                User32.SendMessage(window.GethWnd(), WM_LBUTTONUP, IntPtr.Zero, (IntPtr)lParam);

            }
        }
        
        private static void SendSameClickToOtherClients(Point cur)
        {

            int lParam = GetCursorPos(cur);
            var windows = StaticWindowConfig.GetWindowsIfToggled();
            foreach (var window in windows)
            {
                User32.PostMessage(window.GethWnd(), WM_LBUTTONDOWN, 1, lParam);
                User32.PostMessage(window.GethWnd(), WM_LBUTTONUP, 0, lParam);
                
            }
            Console.WriteLine(windows.FindAll(window => window.IsToggled).Count);
        }

        private static int GetCursorPos(Point cur)
        {
            var posToSend = CalcPosToSend(cur, StaticWindowConfig.GetMainWindow());

            Console.WriteLine("pos to send other client X : " + posToSend.X + " Y : " + posToSend.Y);
            int lParam = (posToSend.Y << 16) | posToSend.X;
            return lParam;
        }


        /// <summary>
        /// this method takes the current position of cursor and then subtract it from origin.  
        /// So that we can find which pos it should click for any resolution. But the thing is 
        /// the resolutions should be same ? 
        /// </summary>
        /// <param name="cur"> current position of cursor (mouse)</param>
        /// <param name="window"> handle to window (hWnd) </param>
        /// <returns></returns>
        private static Point CalcPosToSend(Point cur, Window window)
        {
            Console.WriteLine("The main windows id is : "+window.Id);
            Point posToSend = new Point();
            var pos = cur;
            posToSend.X = pos.X - window.GetPos().Left - 3 ;
            posToSend.Y = pos.Y - window.GetPos().Top - 26;
            Console.WriteLine("posToSend to the other clients is x : "+posToSend.X+" y : "+posToSend.Y);
            return posToSend;
        }

        private class MouseState
        {
            public readonly Point Position;

            public readonly MouseButtons Buttons;

            public readonly Keys key;

            public MouseState(Point position, MouseButtons buttons)
            {
                Position = position;
                Buttons = buttons;
            }
            public MouseState(Keys key)
            {
                this.key = key;
            }
        }
    }
}
