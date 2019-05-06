using RemoteX.Common;
using RemoteX.Controller.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RemoteX.Controller
{
    public partial class App : Application
    {
        static internal RemoteGattService ControllerService;
        public App()
        {
            InitializeComponent();
            ControllerService = new RemoteGattService();
            ControllerService.PublishService();
            App.ControllerService.OnFileManageWriteCompleted += FileControlListPage.ControllerService_OnFileManageWriteCompleted;
            App.ControllerService.OnReadReceived += ControllerService_OnReadReceived;
            MainPage = new MainMasterDetailPage();
        }

        private Common.CursorPoint ControllerService_OnReadReceived()
        {
            System.Diagnostics.Debug.WriteLine(MouseControlPage.CursorPoint.X.ToString() + ", " + MouseControlPage.CursorPoint.Y.ToString());
            return MouseControlPage.CursorPoint;
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
