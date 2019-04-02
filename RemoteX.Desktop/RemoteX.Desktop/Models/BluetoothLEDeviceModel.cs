using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace RemoteX.Desktop.Models
{
    class BluetoothLEDeviceModel : IEquatable<BluetoothLEDeviceModel>
    {
        public BluetoothLEDeviceModel(DeviceInformation lEDevice)
        {
            Name = lEDevice.Name;
            Id = lEDevice.Id;
            Kind = lEDevice.Kind.ToString();
            //Address = lEDevice.Properties.
        }

        public BluetoothLEDeviceModel(string id)
        {
            Id = id;
        }

        //public DeviceInformation LEDevice { set; get; }

        public string Name { set; get; }
        public string Id { set; get; }
        public string Kind { set; get; }

        public bool Equals(BluetoothLEDeviceModel other)
        {
            return Id == other.Id;
        }
    }
}
