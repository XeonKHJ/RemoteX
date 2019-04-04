using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Common;
using WindowsInput.Native;
using System.Drawing;
using VirtualKeyCode = RemoteX.Common.VirtualKeyCode;

namespace RemoteX.Desktop
{
    [Serializable]
    public class RemoteCommand
    {
        public enum ControlType { Keyboard, Mouse};

        public ControlType CommandType { set; get; }

        public CursorPoint CursorPosition { set; get; }

        public VirtualKeyCode VirtualKeyCode { set; get; }
    }
}
