using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RemoteX.Common
{
    public struct CursorPoint
    {
        public double X { set; get; }
        public double Y { set; get; }
        public CursorPoint(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
    }
}
