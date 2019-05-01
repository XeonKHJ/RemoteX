using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.BluetoothLE.Server;

namespace RemoteX.Controller
{
    class FileOperationRemoter
    {
        private IGattCharacteristic fileManageCharacteristic;

        internal FileOperationRemoter(IGattCharacteristic fileManageCharacteristic)
        {
            this.fileManageCharacteristic = fileManageCharacteristic;
        }

        public void OpenFileOrFolder(string path)
        {
            Parallel.ForEach(fileManageCharacteristic.SubscribedDevices, device => fileManageCharacteristic.Broadcast(Encoding.UTF8.GetBytes(path), device));
        }
    }
}
