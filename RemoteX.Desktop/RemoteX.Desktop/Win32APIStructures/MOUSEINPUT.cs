using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Desktop.Win32APIStructures
{
    public struct MOUSEINPUT
    {
        public Int32 dx;
        public Int32 dy;
        public UInt32 mouseData;
        public UInt32 dwFlags;
        public UInt32 time;
        public ulong dwExtraInfo;
    }
}
