using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth;
using WindowsInput;
using Windows.Devices.Enumeration;
using System.Diagnostics;
using RemoteX.Desktop.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using RemoteX.Common;

namespace RemoteX.Desktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DevicesView.ItemsSource = LEDevices;
            FindDevices();
        }

        public void FindDevices()
        {
            LEDeviceFinder leDeviceFinder = new LEDeviceFinder();
            leDeviceFinder.Added += LeDeviceFinder_Added;
            leDeviceFinder.Removed += LeDeviceFinder_Removed;
            leDeviceFinder.StartFinding();
        }

        /// <summary>
        /// 删除不在范围内的蓝牙设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void LeDeviceFinder_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                SynchronizationContext.SetSynchronizationContext(new
                    DispatcherSynchronizationContext(Application.Current.Dispatcher));
                SynchronizationContext.Current.Post(pl =>
                {
                    LEDevices.Remove(new BluetoothLEDeviceModel(args.Id));
                }, null);
            });
        }

        /// <summary>
        /// 添加在范围内的蓝牙设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void LeDeviceFinder_Added(DeviceWatcher sender, DeviceInformation args)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                SynchronizationContext.SetSynchronizationContext(new
                    DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));
                SynchronizationContext.Current.Post(pl =>
                {
                    if (args.Name != "")
                    {
                        LEDevices.Add(new BluetoothLEDeviceModel(args));
                    }
                }, null);
            });
        }

        ObservableCollection<BluetoothLEDeviceModel> LEDevices = new ObservableCollection<BluetoothLEDeviceModel>();

        private KeyboardRemoter keyboardRemoter;
        private FileOperationRemoter fileOperationRemoter;
        private GattDeviceService remoteService;
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            BluetoothLEDeviceModel LEDevice = DevicesView.SelectedItem as BluetoothLEDeviceModel;
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(LEDevice.Id);
            try
            {
                var servicesResult = await bluetoothLeDevice.GetGattServicesForUuidAsync(RemoteUuids.RemoteXServiceGuid);
                remoteService = servicesResult.Services[0];
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            keyboardRemoter = new KeyboardRemoter(remoteService);
            fileOperationRemoter = new FileOperationRemoter(remoteService);
            keyboardRemoter.GetCharacteristics();
            fileOperationRemoter.GetCharacteristics();
        }
    }
}