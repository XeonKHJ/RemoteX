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
using RemoteX.Common;

namespace RemoteX.Desktop
{
    public class KeyboardRemoter : Remoter
    {


        private GattCharacteristic keyboardControlCharacteristic;
        static private InputSimulator inputSimulator;
        static private KeyboardSimulator keyboardSimulator;
        public KeyboardRemoter(GattDeviceService remoteService) : base(remoteService)
        {
            inputSimulator = new InputSimulator();
            keyboardSimulator = new KeyboardSimulator(inputSimulator);
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        public async void GetCharacteristics()
        {
            try
            {
                var characterristicsResult = await remoteService.GetCharacteristicsAsync();
                foreach(var cart in characterristicsResult.Characteristics)
                {
                    if(cart.Uuid == RemoteUuids.KeyboardOperationCharacteristicGuid)
                    {
                        keyboardControlCharacteristic = cart;
                    }
                }
                if(keyboardControlCharacteristic == null)
                {
                    throw new Exception("没有键盘特征");
                }
                keyboardControlCharacteristic.ValueChanged += KeyboardControlCharacteristic_ValueChanged;
                GetNotify();
            }
            catch(Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        private async void GetNotify()
        {
            var status = await keyboardControlCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if(status != GattCommunicationStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
        }

        /// <summary>
        /// 键盘控制信息改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void KeyboardControlCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            using (var reader = DataReader.FromBuffer(args.CharacteristicValue))
            {
                byte[] bytesData = new byte[4];
                byte[] firstkey = new byte[4];
                byte[] secondKey = new byte[4];
                reader.ReadBytes(bytesData);

                firstkey[0] = bytesData[0];
                secondKey[0] = bytesData[1];

                if(secondKey[0] != 0)
                {
                    keyboardSimulator.KeyDown((WindowsInput.Native.VirtualKeyCode)(BitConverter.ToInt32(secondKey, 0)));
                }
                keyboardSimulator.KeyPress((WindowsInput.Native.VirtualKeyCode)BitConverter.ToInt32(firstkey, 0));
                if (secondKey[0] != 0)
                {
                    keyboardSimulator.KeyUp((WindowsInput.Native.VirtualKeyCode)(BitConverter.ToInt32(secondKey, 0)));
                }
            }
        }
    }
}
