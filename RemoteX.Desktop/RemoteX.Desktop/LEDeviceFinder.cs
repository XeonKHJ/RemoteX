using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace RemoteX.Desktop
{
    public class LEDeviceFinder
    {
        DeviceWatcher deviceWatcher;

        private readonly string aqsstring = BluetoothLEDevice.GetDeviceSelector();
        private readonly string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };

        // BT_Code: Example showing paired and non-paired in a single query.
        private readonly string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

        public LEDeviceFinder()
        {
            deviceWatcher =
                    DeviceInformation.CreateWatcher(
                        aqsAllBluetoothLEDevices,
                        requestedProperties,
                        DeviceInformationKind.AssociationEndpoint);

            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
        }

        public void StartFinding()
        {
            deviceWatcher.Start();
        }

        public void StopFinding()
        {
            deviceWatcher.Stop();
        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Updated?.Invoke(sender, args);
        }

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            Added?.Invoke(sender, args);
        }

        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            EnumerationCompleted?.Invoke(sender, args);
        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Removed?.Invoke(sender, args);
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            Stopped?.Invoke(sender, args);
        }

        public event TypedEventHandler<DeviceWatcher, DeviceInformation> Added;
        public event TypedEventHandler<DeviceWatcher, object> EnumerationCompleted;
        public event TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> Removed;
        public event TypedEventHandler<DeviceWatcher, object> Stopped;
        public event TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> Updated;

    }
}
