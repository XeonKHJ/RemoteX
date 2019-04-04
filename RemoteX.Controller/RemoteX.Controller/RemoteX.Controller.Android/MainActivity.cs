using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RemoteX.Controller.Droid
{
    [Activity(Label = "RemoteX.Controller", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            remoteGattService.OnAdvertiseCompleted += RemoteGattService_OnAdvertiseCompleted;
            remoteGattService.PublishService();
        }
        RemoteGattService remoteGattService;
        private void RemoteGattService_OnAdvertiseCompleted()
        {
            int i = 0;
            while(true)
            {
                byte[] intBytes = BitConverter.GetBytes(i++);
                remoteGattService.SendNotification(intBytes);
            }
        }
    }
}