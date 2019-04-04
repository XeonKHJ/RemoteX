using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RemoteX.Common
{
    [Serializable]
    public class RemoteCommand
    {
        public enum ControlType { Keyboard, Mouse };

        public ControlType CommandType { set; get; }

        public Point CursorPosition { set; get; }

        public VirtualKeyCode VirtualKeyCode { set; get; }
    }
}
