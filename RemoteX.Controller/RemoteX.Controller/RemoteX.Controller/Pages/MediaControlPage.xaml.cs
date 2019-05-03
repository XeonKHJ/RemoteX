using RemoteX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteX.Controller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaControlPage : ContentPage
    {
        public MediaControlPage()
        {
            InitializeComponent();
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.MEDIA_PLAY_PAUSE));
        }

        private void PreButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.MEDIA_PREV_TRACK));
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.MEDIA_NEXT_TRACK));
        }

        private void VolumnUpButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.VOLUME_UP));
        }

        private void VolumnDownButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.VOLUME_DOWN));
        }
    }
}