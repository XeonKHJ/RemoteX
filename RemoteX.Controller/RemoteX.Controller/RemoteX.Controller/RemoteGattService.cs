using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;

namespace RemoteX.Controller
{
    public class RemoteGattService
    {
        
        private static readonly Guid remoteGuid = new Guid("AD86E9A5-AB95-4D75-A4BC-2A969F26E028");
        private static readonly Guid keyboardControlGuid = new Guid("3E628CA1-6357-4452-BD7D-04DA25E3CE8E");

        public RemoteGattService()
        {

        }

        public async void PublishService()
        {
            await CreateService();

            AdvertisementData advertisementData = new AdvertisementData { LocalName = "RemoteX Controller" };

            CrossBleAdapter.Current.Advertiser.Start(advertisementData);
        }

        Plugin.BluetoothLE.Server.IGattService controllerService;
        IGattServer gattServer;

        /// <summary>
        /// 创建服务
        /// </summary>
        private async Task CreateService()
        {
            var server = await CrossBleAdapter.Current.CreateGattServer();
            controllerService = server.CreateService(remoteGuid, true);
            CreateCharacteristics();

            server.AddService(controllerService);
        }

        private Plugin.BluetoothLE.Server.IGattCharacteristic characteristic;

        /// <summary>
        /// 创建特征
        /// </summary>
        private void CreateCharacteristics()
        {
            characteristic = controllerService.AddCharacteristic
                (
                keyboardControlGuid,
                CharacteristicProperties.Notify | CharacteristicProperties.Read,
                GattPermissions.Read
                );


            characteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                ;
            });

            characteristic.WhenReadReceived().Subscribe(x =>
            {
                var response = 3;
                x.Value = BitConverter.GetBytes(response);

                x.Status = GattStatus.Success;
            });
        }
    }
}
