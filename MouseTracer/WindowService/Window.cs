using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseTracer.WindowService
{
    public class Window
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsToggled { get; set; }
        public bool IsMain { get; set; }
        private IntPtr hWnd { get; set; }
        private Rect Position { get; set; }

        public Window()
        {
            hWnd = IntPtr.Zero;
            Position = new Rect();
        }

        public Rect GetPos() 
        {
            return Position;
        }
        public void SetPos(Rect pos) 
        {
            Position = pos;
        }

        public IntPtr GethWnd()
        {
            return hWnd;
        }

        public void SethWnd(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }
    }
}
