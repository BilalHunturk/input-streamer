using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseTracer
{
    public class KeyboardStateEventArgs : EventArgs
    {
        public KeyEventArgs Key { get; set; }

        public KeyboardStateEventArgs(KeyEventArgs key)
        {
            Key = key;
        }
    }
}
