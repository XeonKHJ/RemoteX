using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemoteX.Common;

namespace RemoteX.Controller
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SlideControlPage : ContentPage
	{
		public SlideControlPage ()
		{
			InitializeComponent ();
		}

        private void ShowSlidesButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.F5));
        }

        private void PageUpButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.UP));
        }

        private void PageDownButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.DOWN));
        }
    }
}