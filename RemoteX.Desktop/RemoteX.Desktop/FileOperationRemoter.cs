using RemoteX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.IO;
using Windows.Storage.Streams;

namespace RemoteX.Desktop
{
    internal class FileOperationRemoter : Remoter
    {

        private GattCharacteristic fileOperationRemoterCharacteristic;
        private GattCharacteristic fileManageRemoterCharacteristic;

        public FileOperationRemoter(GattDeviceService remoteService) : base(remoteService)
        {
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        public async void GetCharacteristics()
        {
            //System.Diagnostics.Debug.WriteLine(remoteController.ConnectionStatus.ToString());
            try
            {
                var characterristicsResult = await remoteService.GetCharacteristicsAsync();
                foreach (var cart in characterristicsResult.Characteristics)
                {
                    if (cart.Uuid == RemoteUuids.FileManageCharacteristicGuid)
                    {
                        fileManageRemoterCharacteristic = cart;
                    }
                }
                if (fileManageRemoterCharacteristic == null)
                {
                    throw new Exception("没有键盘特征");
                }
                fileManageRemoterCharacteristic.ValueChanged += FileManageRemoter_ValueChanged;

                GetNotify();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// 收到消息后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void FileManageRemoter_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var path = args.CharacteristicValue.ToString();
            if(IsPathAFile(path))
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                EnterDictionary(path);
            }
        }

        /// <summary>
        /// 订阅通知
        /// </summary>
        private async void GetNotify()
        {
            var status = await fileManageRemoterCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if (status != GattCommunicationStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
        }

        private List<string> itemsInTheCurrentFolder;

        /// <summary>
        /// 获取磁盘路径
        /// </summary>
        /// <param name="currentDictionary"></param>
        private List<string> GetDrives()
        {
            List<string> itemsInTheCurrentFolderToAdd = new List<string>();
            var drives = DriveInfo.GetDrives();
            foreach(var drive in drives)
            {
                itemsInTheCurrentFolder.Add(drive.Name);
            }
            return itemsInTheCurrentFolderToAdd;
        }

        /// <summary>
        /// 进入目录
        /// </summary>
        /// <param name="dictionary">文件夹路径</param>
        private void EnterDictionary(string dictionary)
        {
            IEnumerable<string> items;
            if(dictionary == "*")
            {
                items = GetDrives().AsEnumerable();
            }
            else
            {
                items = System.IO.Directory.EnumerateFileSystemEntries(dictionary);
            }
            OnEnteredDicionary?.Invoke();
        }
        private delegate void OnEnteredDictionaryEventHandler();
        private event OnEnteredDictionaryEventHandler OnEnteredDicionary;

        private bool IsPathAFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 传输当前目录信息
        /// </summary>
        private async void SendCurrentDictionaryInfomation()
        {
            var writer = new DataWriter();
            int byteLength = 0;
            
            foreach(var path in itemsInTheCurrentFolder)
            {
                byteLength += (path.Length + 1);
            }

            byte[] bytes = new byte[byteLength];

            int copyBeginIndex = 0;
            foreach (var path in itemsInTheCurrentFolder)
            {
                byte[] bArray = Encoding.UTF8.GetBytes(path);
                bArray.CopyTo(bytes, copyBeginIndex);
                copyBeginIndex += bArray.Length;
                bytes[copyBeginIndex++] = 0;
            }
            writer.WriteBytes(bytes);
            var result = await fileManageRemoterCharacteristic.WriteValueAsync(writer.DetachBuffer());
        }
    }
}
