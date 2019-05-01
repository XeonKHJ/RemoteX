using System;
using System.Collections.Generic;
using System.Text;
using RemoteX.Common;
using System.Threading.Tasks;
using Plugin.BluetoothLE.Server;

namespace RemoteX.Controller
{
    class KeyboardRemoter
    {
        private IGattCharacteristic keyboardCharacteristic;
        internal KeyboardRemoter(IGattCharacteristic keyboardCharacteristic)
        {
            this.keyboardCharacteristic = keyboardCharacteristic; 
        }

        /// <summary>
        /// 发送点击事件
        /// </summary>
        /// <param name="virtualKeyCode"></param>
        internal void Click(VirtualKeyCode virtualKeyCode)
        {
            Parallel.ForEach(keyboardCharacteristic.SubscribedDevices, device => keyboardCharacteristic.Broadcast(BitConverter.GetBytes((int)virtualKeyCode), device));
        }
    }
}
