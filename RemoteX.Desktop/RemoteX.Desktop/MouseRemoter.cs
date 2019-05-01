using RemoteX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Desktop
{
    internal class MouseRemoter : Remoter
    {

        private GattCharacteristic mouseMotionCharacteristic;

        public MouseRemoter(GattDeviceService remoteService) : base(remoteService)
        {
        }

        public async void GetCharacteristics()
        {
            try
            {
                var characterristicsResult = await remoteService.GetCharacteristicsAsync();
                foreach (var cart in characterristicsResult.Characteristics)
                {
                    if (cart.Uuid == RemoteUuids.KeyboardOperationCharacteristicGuid)
                    {
                        mouseMotionCharacteristic = cart;
                    }
                }
                if (mouseMotionCharacteristic == null)
                {
                    throw new Exception("没有鼠标特征");
                }
                mouseMotionCharacteristic.ValueChanged += MouseMotionCharacteristic_ValueChanged;
            }
            catch(Exception excepiton)
            {
                System.Diagnostics.Debug.WriteLine(excepiton.Message);
            }
        }

        private void MouseMotionCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            //to-do
        }

        /// <summary>
        /// 订阅通知
        /// </summary>
        private async void GetNotify()
        {
            var status = await mouseMotionCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if (status != GattCommunicationStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
        }
    }
}
