using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using RemoteX.Common;

namespace RemoteX.Controller
{
    public class RemoteGattService
    {
        public RemoteGattService()
        {

        }

        public async void PublishService()
        {
            await CreateService();

            AdvertisementData advertisementData = new AdvertisementData { LocalName = "RemoteX Controller" };

            CrossBleAdapter.Current.Advertiser.Start(advertisementData);

            OnAdvertiseCompleted?.Invoke();

        }

        Plugin.BluetoothLE.Server.IGattService controllerService;
        IGattServer gattServer;

        /// <summary>
        /// 创建服务
        /// </summary>
        private async Task CreateService()
        {
            var server = await CrossBleAdapter.Current.CreateGattServer();
            controllerService = server.CreateService(RemoteUuids.RemoteXServiceGuid, true);
            CreateKeyboardOpeartionCharacteristic();

            server.AddService(controllerService);
        }

        private Plugin.BluetoothLE.Server.IGattCharacteristic keyboardOpCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic mouseMotionCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic mouseEventCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic fileManageCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic programOpCharacteristic;

        private void CreateCharacteristics()
        {
            CreateKeyboardOpeartionCharacteristic();
            
        }


        /// <summary>
        /// 创建键盘操作特征
        /// </summary>
        private void CreateKeyboardOpeartionCharacteristic()
        {
            keyboardOpCharacteristic = controllerService.AddCharacteristic
                (
                RemoteUuids.KeyboardOpControlGuid,
                CharacteristicProperties.Notify | CharacteristicProperties.Read,
                GattPermissions.Read
                );

            keyboardOpCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                ;
            });

            keyboardOpCharacteristic.WhenReadReceived().Subscribe(x =>
            {
                var response = 3;
                x.Value = BitConverter.GetBytes(response);

                x.Status = GattStatus.Success;
            });
        }

        IDisposable notifyBroadcast = null;
        public void SendKeyboardControl(byte[] data)
        {
            Parallel.ForEach(keyboardOpCharacteristic.SubscribedDevices, device => keyboardOpCharacteristic.Broadcast(data, device));
        }




        public delegate void OnAdvertiseCompletedHandler();
        public event OnAdvertiseCompletedHandler OnAdvertiseCompleted;
    }
}
