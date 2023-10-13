using Keystroke.API;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MouseTracer
{
	public class PollingHook : MouseHook
	{
		private readonly Timer pollTimer;

		private Point prevPos;

		private MouseButtons prevButtons;

		//private KeyStrokeApiController keyStrokeApi;

		public PollingHook(int intervalMs)
		{
			if (intervalMs <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(intervalMs), "Interval must be positive");
			}
            //keyStrokeApi = new KeyStrokeApiController(intervalMs);
            pollTimer = new Timer
			{
				Interval = intervalMs
			};
			pollTimer.Tick += PollTimer_Tick;
		}

		public override void Start()
		{
			base.Start();
			pollTimer.Start();
		}

		public override void Stop()
		{
			base.Stop();
			pollTimer.Stop();
		}

		private void PollTimer_Tick(object sender, EventArgs e)
		{
            //keyStrokeApi.StartProcess();
            var pos = Cursor.Position;

			var buttons = MouseButtons.None;

			if (IsKeyPressed(Keys.LButton))
				buttons |= MouseButtons.Left;
			if (IsKeyPressed(Keys.RButton))
				buttons |= MouseButtons.Right;
			if (IsKeyPressed(Keys.MButton))
				buttons |= MouseButtons.Middle;
			

            //Console.WriteLine(button.ToString());

            if (pos != prevPos || buttons != prevButtons)
			{
                EnqueueNewEvent(new MouseStateEventArgs(pos, buttons));
            }

			prevPos = pos;
			prevButtons = buttons;

        }

		#region P/Invoke

		private bool IsKeyPressed(Keys key) => (GetKeyState((int)key) & 0x8000) != 0;

		[DllImport("USER32.dll")]
		private static extern short GetKeyState(int nVirtKey);

		#endregion
	}
}
