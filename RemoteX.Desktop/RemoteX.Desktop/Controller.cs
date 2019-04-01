using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using WindowsInput;

namespace RemoteX.Desktop
{
    class MouseController
    {

        public MouseController()
        {
            ;
        }

        public void MoveTo(Point pos)
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
        }
    }
}
