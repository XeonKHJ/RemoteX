using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Desktop.Win32APIStructures
{
    struct INPUT
    {
        public UInt32 type;

        [FieldOffset(sizeof(UInt32)]
        public MOUSEINPUT mi;

        [FieldOffset(sizeof(UInt32)]
        public KEYBDINPUT ki;

        //[FieldOffset(sizeof(UInt32)]
        //public HARDWAREINPUT hi;
    }
}
