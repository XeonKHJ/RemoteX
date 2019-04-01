using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace RemoteX.Mobile
{
    class RemoteGattService
    {
        GattServiceProvider serviceProvider;
        private static readonly Guid remoteGuid = new Guid("AD86E9A5-AB95-4D75-A4BC-2A969F26E028");
        private static readonly Guid keyboardControlGuid = new Guid("3E628CA1-6357-4452-BD7D-04DA25E3CE8E");

        public RemoteGattService()
        {

        }

        public async void PublishService()
        {
            await CreateService();
            GattServiceProviderAdvertisingParameters advParameters = new GattServiceProviderAdvertisingParameters
            {
                IsDiscoverable = true,
                IsConnectable = true
            };
            serviceProvider.StartAdvertising(advParameters);
        }


        /// <summary>
        /// 创建服务
        /// </summary>
        private async Task CreateService()
        {
            GattServiceProviderResult result = await GattServiceProvider.CreateAsync(remoteGuid);
            if (result.Error == BluetoothError.Success)
            {
                serviceProvider = result.ServiceProvider;
            }
            else
            {
                throw new Exception();
            }
            CreateCharacteristics();
        }

        private GattLocalCharacteristic characteristic;

        /// <summary>
        /// 创建特征
        /// </summary>
        private async void CreateCharacteristics()
        {
            var characteristicParameters = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.Notify
            };
            GattLocalCharacteristicResult characteristicResult = await serviceProvider.Service.CreateCharacteristicAsync(keyboardControlGuid, characteristicParameters);

            characteristic = characteristicResult.Characteristic;
            characteristic.ReadRequested += Characteristic_ReadRequested;
        }

        private async void Characteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs args)
        {
            var deferral = args.GetDeferral();

            // Our familiar friend - DataWriter.
            var writer = new DataWriter();
            // populate writer w/ some data. 
            // ... 

            var request = await args.GetRequestAsync();
            request.RespondWithValue(writer.DetachBuffer());

            deferral.Complete();
        }

        /// <summary>
        /// 通知
        /// </summary>
        public async void NotifyValue(int notifyData)
        {
            var writer = new DataWriter();

            writer.WriteInt32(notifyData);
            await writer.StoreAsync();

            await characteristic.NotifyValueAsync(writer.DetachBuffer());
        }


    }
}
