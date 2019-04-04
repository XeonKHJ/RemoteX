using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RemoteX.Common;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RemoteX.Controller.UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MediaControlPage : Page
    {
        public MediaControlPage()
        {
            this.InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            RemoteGattService.NotifyValue((int)VirtualKeyCode.MEDIA_PLAY_PAUSE);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            RemoteGattService.NotifyValue((int)VirtualKeyCode.MEDIA_NEXT_TRACK);
        }

        RemoteGattServiceUWP RemoteGattService;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RemoteGattService = e.Parameter as RemoteGattServiceUWP;
        }
    }
}
