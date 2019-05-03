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
            uint length = args.CharacteristicValue.Length;
            using (var reader = DataReader.FromBuffer(args.CharacteristicValue))
            {
                byte[] bytesData = new byte[length];
                reader.ReadBytes(bytesData);
                var path = Encoding.UTF8.GetString(bytesData);
                if (IsPathAFile(path))
                {

                    System.Diagnostics.Process.Start(path);
                }
                else
                {


                    EnterDictionary(path);
                }
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

        private IEnumerable<string> itemsInTheCurrentFolder;

        /// <summary>
        /// 获取磁盘路径
        /// </summary>
        /// <param name="currentDictionary"></param>
        private List<string> GetDrives()
        {
            List<string> driveNames = new List<string>();
            var drives = DriveInfo.GetDrives();
            foreach(var drive in drives)
            {
                char[] drivePath = new char[drive.Name.Length + 1];

                //drive.Name.CopyTo(0, drivePath, 0, drive.Name.Length);
                //drivePath[drivePath.Length - 1] = '/';
                //Encoding.UTF8.GetBytes(drivePath);
                driveNames.Add(drive.Name);
            }
            return driveNames;
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
            itemsInTheCurrentFolder = items;
            SendCurrentDictionaryInfomation();
        }

        private bool IsPathAFile(string path)
        {
            if (path == "*")
            {
                return false;
            }
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
            
            foreach(var path in itemsInTheCurrentFolder)
            {
                //准备一条路径
                char[] pathToSend = path.ToCharArray();
                if(!IsPathAFile(path))
                {
                    if (path.Last() != '\\' && path.Last() != '/')
                    {
                        pathToSend = new char[path.Length + 1];
                        path.CopyTo(0, pathToSend, 0, path.Length);
                        pathToSend[pathToSend.Length - 1] = '\\';
                    }
                }
                var bytes = Encoding.UTF8.GetBytes(pathToSend);
                byte[] bytesToSend = new byte[bytes.Length + 1];
                bytes.CopyTo(bytesToSend, 0);
                bytesToSend[bytesToSend.Length - 1] = 0;
                //发送
                writer.WriteBytes(bytesToSend);
                try
                {
                    var result = await fileManageRemoterCharacteristic.WriteValueAsync(writer.DetachBuffer());
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                }
            }

            string endString = "RemoteFileEnumEnds";
            var endBytes = Encoding.UTF8.GetBytes(endString.ToCharArray());
            byte[] endBytesToSend = new byte[endBytes.Length + 1];
            endBytes.CopyTo(endBytesToSend, 0);
            endBytesToSend[endBytesToSend.Length - 1] = 0;

            writer.WriteBytes(Encoding.UTF8.GetBytes(endString));
            try
            {
                var result = await fileManageRemoterCharacteristic.WriteValueAsync(writer.DetachBuffer());
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }
    }
}
