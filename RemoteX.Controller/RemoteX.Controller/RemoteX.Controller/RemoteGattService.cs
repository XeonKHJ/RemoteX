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
            CreateKeyboardOperationCharacteristic();
            CreateFileManageCharacteristic();
            CreateStringOperationCharacteristic();
            server.AddService(controllerService);
        }

        private Plugin.BluetoothLE.Server.IGattCharacteristic keyboardOpCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic mouseMotionCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic mouseEventCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic fileManageCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic programOpCharacteristic;
        private Plugin.BluetoothLE.Server.IGattCharacteristic stringOperationCharacteristic;

        private void CreateCharacteristics()
        {
            CreateKeyboardOperationCharacteristic();
        }


        /// <summary>
        /// 创建键盘操作特征
        /// </summary>
        private void CreateKeyboardOperationCharacteristic()
        {
            keyboardOpCharacteristic = controllerService.AddCharacteristic
                (
                RemoteUuids.KeyboardOperationCharacteristicGuid,
                CharacteristicProperties.Notify | CharacteristicProperties.Read,
                GattPermissions.Read
                );

            keyboardOpCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                ; //当订阅发生改变时
            });

            int number = 0;
            keyboardOpCharacteristic.WhenReadReceived().Subscribe(x =>
            {
                var response = ++number;
                x.Value = BitConverter.GetBytes(response);

                x.Status = GattStatus.Success;
            });
        }

        /// <summary>
        /// 创建鼠标移动特征
        /// </summary>
        private void CreateMouseMotionCharacteristic()
        {
            mouseMotionCharacteristic = controllerService.AddCharacteristic
            (
                RemoteUuids.StringOperationCharacteristicGuid,
                CharacteristicProperties.Notify,
                GattPermissions.Read
            );

            mouseMotionCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                ; //当订阅发生改变时
            });
        }

        private void CreatemMouseEventCharacteristic()
        {
            
        }

        /// <summary>
        /// 创建文件管理特征
        /// </summary>
        private void CreateFileManageCharacteristic()
        {
            fileManageCharacteristic = controllerService.AddCharacteristic
                (
                RemoteUuids.FileManageCharacteristicGuid,
                CharacteristicProperties.Notify | CharacteristicProperties.Read | CharacteristicProperties.Write,
                GattPermissions.Read | GattPermissions.Write
                );

            fileManageCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                ;
            });

            fileManageCharacteristic.WhenReadReceived().Subscribe(x =>
            {
                var response = 3;
                x.Value = BitConverter.GetBytes(response);

                x.Status = GattStatus.Success;
            });

            //收到目录数据
            fileManageCharacteristic.WhenWriteReceived().Subscribe(x =>
            {
                List<string> items = new List<string>();
                int stringBeginIndex = 0;
                int stringEndIndex = 0;
                for(int i = 0; i< x.Value.Length; ++i)
                {
                    if(x.Value[i] == 0)
                    {
                        stringEndIndex = i;
                        byte[] path = new byte[stringEndIndex - stringBeginIndex];
                        Array.Copy(x.Value, stringBeginIndex, path, 0, stringEndIndex - stringBeginIndex);
                        items.Add(Encoding.UTF8.GetString(path));
                        stringBeginIndex = stringEndIndex + 1;
                    }
                }
                System.Diagnostics.Debug.WriteLine("Received inside");
                OnFileManageWriteCompleted?.Invoke(items);
            });
        }
        internal delegate void OnWriteCompleteEventHandler(object arg);
        internal event OnWriteCompleteEventHandler OnFileManageWriteCompleted;
        private void CreateProgramOperatioCharacteristic()
        {
            
        }

        //创建字符串操作特征
        private void CreateStringOperationCharacteristic()
        {
            stringOperationCharacteristic = controllerService.AddCharacteristic
            (
                RemoteUuids.StringOperationCharacteristicGuid,
                CharacteristicProperties.Notify,
                GattPermissions.Read
            );

            stringOperationCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                ; //当订阅发生改变时
            });

            int number = 0;
        }

        /// <summary>
        /// 发送按键控制
        /// </summary>
        /// <param name="data"></param>
        public void SendKeyboardControl(byte[] data)
        {
            Parallel.ForEach(keyboardOpCharacteristic.SubscribedDevices, device => keyboardOpCharacteristic.Broadcast(data, device));
        }

        public void SendString(string stringToSend)
        {
            var data = Encoding.UTF8.GetBytes(stringToSend);
            Parallel.ForEach(stringOperationCharacteristic.SubscribedDevices, device => stringOperationCharacteristic.Broadcast(data, device));
        }

        public void SendDictionaryRequest(string path)
        {
            var pathBytes = Encoding.UTF8.GetBytes(path);
            Parallel.ForEach(keyboardOpCharacteristic.SubscribedDevices, device => fileManageCharacteristic.Broadcast(pathBytes, device));
        }
    }
}
