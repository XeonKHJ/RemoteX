using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WindowsInput;

namespace RemoteX.Desktop.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = System.IO.Directory.EnumerateFileSystemEntries("D:/");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            InputSimulator inputSimulator = new InputSimulator();
            KeyboardSimulator keyboardSimulator = new KeyboardSimulator(inputSimulator);
            double x = 0;
            double y = 0;
                keyboardSimulator.KeyPress(WindowsInput.Native.VirtualKeyCode.MODECHANGE);
            //System.Diagnostics.Process.Start(@"D:/Users/redal/Pictures\0.jpg");
        }
    }
}
