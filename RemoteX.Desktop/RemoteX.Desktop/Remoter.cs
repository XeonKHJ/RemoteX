using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace RemoteX.Desktop
{
    public class Remoter
    {
        protected BluetoothLEDevice remoteController;

        public Remoter(BluetoothLEDevice lEDevice)
        {
            remoteController = lEDevice;
        }
    }
}
