using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RemoteX.Common;
using System.Diagnostics;
using System.Threading;

namespace RemoteX.Controller.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MouseControlPage : ContentPage
    {
        public MouseControlPage()
        {
            InitializeComponent();
        }

        static public CursorPoint CursorPoint = new CursorPoint { X = 0, Y = 0 };
        Stopwatch sendTimeSpan = new Stopwatch();
        private void TouchpadPanGestureRecongnizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            sendTimeSpan.Start();
            CursorPoint = new CursorPoint { X = e.TotalX, Y = e.TotalY };
            if ((CursorPoint.X == 0 && CursorPoint.Y == 0) || sendTimeSpan.ElapsedMilliseconds > 50)
            {
                App.ControllerService.SendMousePostion(CursorPoint);
                sendTimeSpan.Reset();
            }
        }

        private Stopwatch stopwatch = new Stopwatch();

        private void TouchpadTapGestureRecongnizer_Tapped(object sender, EventArgs e)
        {
            App.ControllerService.SendMouseEvent(RemoteCommand.MouseEvent.LeftClick, 0);
            stopwatch.Reset();
        }

        private void TouchpadPinchGestureReconizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            App.ControllerService.SendMouseEvent(RemoteCommand.MouseEvent.RightClick, 0);
        }

        Stopwatch verticalScrollerSendWatch = new Stopwatch();
        private void VerticalScrollerPanGestureRecongnizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            verticalScrollerSendWatch.Start();
            if(e.TotalY == 0 || verticalScrollerSendWatch.ElapsedMilliseconds > 50)
            {
                App.ControllerService.SendMouseEvent(RemoteCommand.MouseEvent.VerticalScroll, (int)e.TotalY);
                verticalScrollerSendWatch.Reset();
            }
        }

        private void VerticalScrollerTapGestureRecongnizer_Tapped(object sender, EventArgs e)
        {
            ;
        }

        private void VerticalScrollerPinchGestureReconizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {

        }

        Stopwatch horizontalScrollerSendWatch = new Stopwatch();
        private void HorizontalScrollerPanGestureRecongnizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            horizontalScrollerSendWatch.Start();
            if(e.TotalX == 0 || horizontalScrollerSendWatch.ElapsedMilliseconds > 50)
            {
                App.ControllerService.SendMouseEvent(RemoteCommand.MouseEvent.HorizontalScroll, (int)e.TotalX);
                horizontalScrollerSendWatch.Reset();
            }
        }

        private void HorizontalScrollerTapGestureRecongnizer_Tapped(object sender, EventArgs e)
        {

        }

        private void HorizontalScrollerPinchGestureReconizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {

        }

        private void VerticalScrollerGrid_Focused(object sender, FocusEventArgs e)
        {
            Debug.WriteLine("VerticalScrollerGrid_Focused");
        }

        private void VerticalScrollerGrid_Unfocused(object sender, FocusEventArgs e)
        {
            Debug.WriteLine("VerticalScrollerGrid_Focused");
        }
    }
}