using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using WindowsInput;
using Windows.Storage.Streams;

namespace RemoteX.Desktop
{
    public class KeyboardRemoter : Remoter
    {
        // {6A323314-750D-449E-8BC6-7AC2E4EB84BC}
        private static readonly Guid remoteGuid = new Guid("AD86E9A5-AB95-4D75-A4BC-2A969F26E028");
        private static readonly Guid keyboardControlGuid = new Guid("3E628CA1-6357-4452-BD7D-04DA25E3CE8E");

        public KeyboardRemoter(BluetoothLEDevice lEDevice) : base(lEDevice)
        { }

        private GattCharacteristic keyboardControlCharacteristic;
        public void GetCharacteristics()
        {
            var service = remoteController.GetGattService(remoteGuid);
            var characterristics = service.GetCharacteristics(keyboardControlGuid);
            keyboardControlCharacteristic = characterristics[0];
            keyboardControlCharacteristic.ValueChanged += KeyboardControlCharacteristic_ValueChanged;
        }

        private void KeyboardControlCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            var value = reader.ReadInt32();
            System.Diagnostics.Debug.WriteLine(value);
        }
    }
}
