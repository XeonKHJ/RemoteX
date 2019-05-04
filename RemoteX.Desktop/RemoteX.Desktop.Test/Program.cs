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
            
            while(true)
            {
                InputSimulator inputSimulator = new InputSimulator();
                MouseSimulator keyboardSimulator = new MouseSimulator(inputSimulator);
                keyboardSimulator.MoveMouseBy(1, 1);
            }
            //System.Diagnostics.Process.Start(@"D:/Users/redal/Pictures\0.jpg");
        }
    }
}
