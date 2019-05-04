using RemoteX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Windows.Forms;
using Windows.Storage.Streams;
using WindowsInput;

namespace RemoteX.Desktop
{
    class StringOperationRemoter : Remoter
    {
        private GattCharacteristic stringOperationCharacteristic;

        public StringOperationRemoter(GattDeviceService remoteService) : base(remoteService)
        {
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        public async void GetCharacteristics()
        {
            try
            {
                var characterristicsResult = await remoteService.GetCharacteristicsAsync();
                foreach (var cart in characterristicsResult.Characteristics)
                {
                    if (cart.Uuid == RemoteUuids.StringOperationCharacteristicGuid)
                    {
                        stringOperationCharacteristic = cart;
                    }
                }
                if (stringOperationCharacteristic == null)
                {
                    throw new Exception("没有键盘特征");
                }
                stringOperationCharacteristic.ValueChanged += stringOperationCharacteristic_ValueChanged;
                GetNotify();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        private async void GetNotify()
        {
            var status = await stringOperationCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if (status != GattCommunicationStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
        }

        private bool isVoiceTypingStop = false;
        private string oldString;
        /// <summary>
        /// 键盘控制信息改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void stringOperationCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            using (var reader = DataReader.FromBuffer(args.CharacteristicValue))
           {
                byte[] bytesData = new byte[reader.UnconsumedBufferLength];
                reader.ReadBytes(bytesData);
                string stringData = Encoding.UTF8.GetString(bytesData);
                if(stringData.Last() == (char)65532)
                {
                    isVoiceTypingStop = true;
                    return;
                }
                if(isVoiceTypingStop)
                {
                    isVoiceTypingStop = false;
                    return;
                }
                System.Diagnostics.Debug.WriteLine(stringData);
                SendKeys.SendWait(stringData);
                oldString = stringData;
            }
        }
    }
}
