using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RemoteX.Desktop.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = System.IO.Directory.EnumerateFileSystemEntries(".");
            
            foreach(var file in files)
            {
                Console.WriteLine(file);
                var bytes = Encoding.UTF8.GetBytes(file);
            }
            System.Diagnostics.Process.Start(@"D:/Users/redal/Pictures\0.jpg");
        }
    }
}
