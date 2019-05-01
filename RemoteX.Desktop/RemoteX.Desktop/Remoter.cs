using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using RemoteX.Common;

namespace RemoteX.Desktop
{
    public class Remoter
    {
        protected GattDeviceService remoteService;

        public Remoter(GattDeviceService remoteService)
        {
            this.remoteService = remoteService;
        }
    }
}
