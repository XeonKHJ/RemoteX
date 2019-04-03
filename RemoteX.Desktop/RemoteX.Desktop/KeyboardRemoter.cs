using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using WindowsInput;
using Windows.Storage.Streams;
using Windows.Devices;
using Windows;
using Windows.Devices.Enumeration;

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

        /// <summary>
        /// 获取特性
        /// </summary>
        public async void GetCharacteristics()
        {
            BluetoothLEDevice bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(remoteController.DeviceId);
            var hidDevices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(remoteGuid));
            //System.Diagnostics.Debug.WriteLine(remoteController.ConnectionStatus.ToString());
            try
            {
                var servicesResult = await remoteController.GetGattServicesForUuidAsync(remoteGuid);
                var service = servicesResult.Services[0];
                var characterristicsResult = await service.GetCharacteristicsAsync();
                foreach(var cart in characterristicsResult.Characteristics)
                {
                    if(cart.Uuid == keyboardControlGuid)
                    {
                        keyboardControlCharacteristic = cart;
                    }
                }
                if(keyboardControlCharacteristic == null)
                {
                    throw new Exception("没有键盘特征");
                }
                keyboardControlCharacteristic.ValueChanged += KeyboardControlCharacteristic_ValueChanged;
            }
            catch(Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// 键盘控制信息改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void KeyboardControlCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            var value = reader.ReadInt32();
            System.Diagnostics.Debug.WriteLine(value);
        }
    }
}
