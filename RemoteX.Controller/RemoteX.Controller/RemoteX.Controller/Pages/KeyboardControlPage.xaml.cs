using RemoteX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteX.Controller.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KeyboardControlPage : ContentPage
	{
		public KeyboardControlPage ()
		{
			InitializeComponent ();
		}

        private void UpButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.UP));
        }

        private void TabButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.TAB));
        }

        private void LeftButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.LEFT));
        }

        private void Enter_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.RETURN));
        }

        private void RightButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.RIGHT));
        }

        private void DownButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.DOWN));
        }

        private void EnterTextButton_Clicked(object sender, EventArgs e)
        {
            enterEntry.Focus();
        }

        private void EnterEntry_Focused(object sender, FocusEventArgs e)
        {
            enterEntry.Text = "s";
            System.Diagnostics.Debug.WriteLine("EnterEntry_Focused");
        }

        private bool isChangedByClean = false;
        private bool isVoiceEnterCompleted = false;
        private void EnterEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("EnterEntry_TextChanged");
            if(e.NewTextValue == "" || e.NewTextValue == null)
            {
                enterEntry.Text = "s";
            }

            if (e.NewTextValue != null && e.OldTextValue!=null)
            {
                if (enterEntry.IsFocused)
                {
                    if (e.NewTextValue.Length > e.OldTextValue.Length)
                    {
                        if (e.NewTextValue.Last() == (char)65532) //如果不是语音识别结束符号
                        {
                            return;
                        }

                        var newTextCharArray = e.NewTextValue.ToCharArray();
                        string toSend = new string(newTextCharArray, e.OldTextValue.Length, e.NewTextValue.Length - e.OldTextValue.Length);
                        System.Diagnostics.Debug.WriteLine(toSend);
                        
                        App.ControllerService.SendString(toSend);
                    }
                    else if (e.NewTextValue.Length == e.OldTextValue.Length - 1)
                    {
                        if(e.OldTextValue.Last() != (char)65532) //如果不是语音识别结束符号
                        {
                            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.BACK));
                            isChangedByClean = false;
                        }

                    }
                }
            }
            

            //isChangedByClean = false;
            //if (e.NewTextValue != "")
            //{
            //    char lastChar = e.NewTextValue[0];
            //    System.Diagnostics.Debug.WriteLine(lastChar);
            //    App.ControllerService.SendCharacter(lastChar);
            //    enterEntry.Text = "";
            //    isChangedByClean = true;
            //}
            //else
            //{
            //    if(!isChangedByClean)
            //    {
            //        App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.BACK));
            //    }
            //}
        }

        private void EnterEntry_Unfocused(object sender, FocusEventArgs e)
        {
            enterEntry.Text = "";
            System.Diagnostics.Debug.WriteLine("EnterEntry_Unfocused");
        }

        private void StartMenuButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.LWIN));
        }

        private void SwitchButton_Clicked(object sender, EventArgs e)
        {
            byte winKey = (byte)VirtualKeyCode.LWIN;
            byte tabKey = (byte)VirtualKeyCode.TAB;
            byte[] keyToSend = { tabKey, winKey, 0, 0 };
            App.ControllerService.SendKeyboardControl(keyToSend);
        }
    }
}