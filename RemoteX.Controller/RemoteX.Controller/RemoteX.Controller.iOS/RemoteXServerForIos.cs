using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreBluetooth;

namespace RemoteX.Controller.iOS
{
    public class RemoteXServerForIos
    {
        private CBPeripheralManager cBPeripheralManager;
        private CBMutableService remoteService;
        object abc;
        public RemoteXServerForIos()
        {
            CBUUID[] cBUUIDs = new CBUUID[1];
            CBUUID cBUUID = CBUUID.FromString("AD86E9A5-AB95-4D75-A4BC-2A969F26E028");
            cBPeripheralManager = new CBPeripheralManager();
            remoteService = new CBMutableService(cBUUID, true);
            cBPeripheralManager.AddService(remoteService);
            cBUUIDs.Append(cBUUID);
            var advertisingOption = new StartAdvertisingOptions
            {
                LocalName = "RemoteX Controller"
            };
            advertisingOption.ServicesUUID = cBUUIDs;
            cBPeripheralManager.StartAdvertising(advertisingOption);
        }
    }
}