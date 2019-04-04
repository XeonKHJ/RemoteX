using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace RemoteX.Controller.UWP
{
    public sealed partial class MainPage
    {
        RemoteGattServiceUWP RemoteController = new RemoteGattServiceUWP();
        public MainPage()
        {
            this.InitializeComponent();

            //LoadApplication(new RemoteX.Controller.App());

            RemoteController.PublishService();
        }

        int i = 0;
        private void NotifyButton_Click(object sender, RoutedEventArgs e)
        {
            RemoteController.NotifyValue(i++);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if((ControllerNavigator.SelectedItem as NavigationViewItem).Content.ToString() == "媒体控制")
            {
                contentFrame.Navigate(typeof(MediaControlPage), RemoteController);
            }
            else if ((ControllerNavigator.SelectedItem as NavigationViewItem).Content.ToString() == "键盘控制")
            {
                contentFrame.Navigate(typeof(KeyboardControlPage), RemoteController);
            }
        }
    }
}
