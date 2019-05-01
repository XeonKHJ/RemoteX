using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using RemoteX.Common;

namespace RemoteX.Controller
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            remoteGattService = new RemoteGattService();
            remoteGattService.PublishService();
            
        }
        RemoteGattService remoteGattService;

        int i = 1;
        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            remoteGattService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.MEDIA_PLAY_PAUSE));
        }

        private void PreButton_Clicked(object sender, EventArgs e)
        {
            remoteGattService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.MEDIA_PREV_TRACK));
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            remoteGattService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.MEDIA_NEXT_TRACK));
        }
    }
}
