using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RemoteX.Common
{
    public struct CursorPoint
    {
        public int X { set; get; }
        public int Y { set; get; }
        CursorPoint(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
    }
}
