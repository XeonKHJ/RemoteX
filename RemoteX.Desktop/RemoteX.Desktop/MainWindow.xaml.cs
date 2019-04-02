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
            leDeviceFinder.StartFinding();
            leDeviceFinder.Added += LeDeviceFinder_Added;
            leDeviceFinder.Removed += LeDeviceFinder_Removed;
        }

        private void LeDeviceFinder_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                SynchronizationContext.SetSynchronizationContext(new
                    DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));
                SynchronizationContext.Current.Post(pl =>
                {
                    LEDevices.Remove(new BluetoothLEDeviceModel(args.Id));
                }, null);
            });
        }

        private void LeDeviceFinder_Added(DeviceWatcher sender, DeviceInformation args)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
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

        KeyboardRemoter keyboardRemoter;
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            BluetoothLEDeviceModel LEDevice = DevicesView.SelectedItem as BluetoothLEDeviceModel;
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(LEDevice.Id);
            keyboardRemoter = new KeyboardRemoter(bluetoothLeDevice);
            keyboardRemoter.GetCharacteristics();
        }
    }
}
