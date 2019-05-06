using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using System.Diagnostics;
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
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.PRIOR));
        }

        private void PageDownButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.NEXT));
        }

        private void EndSliderButton_Clicked(object sender, EventArgs e)
        {
            App.ControllerService.SendKeyboardControl(BitConverter.GetBytes((int)VirtualKeyCode.ESCAPE));
        }

        Stopwatch sendWatch = new Stopwatch();
        /// <summary>
        /// 加速器变换检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            sendWatch.Start();
            var timeSpan = accelerometerStopwatch.ElapsedMilliseconds;
            var relativeDistance = CaculateRelativeDistance(e.Reading.Acceleration, timeSpan);
            accelerometerStopwatch.Restart();
            if(sendWatch.ElapsedMilliseconds > 50)
            {
                App.ControllerService.SendMousePostion(new CursorPoint((int)-relativeDistance.X * 10, (int)-relativeDistance.Y * 20));
                sendWatch.Restart();
            }
            Debug.WriteLine(relativeDistance.X.ToString() + ", " + relativeDistance.Y.ToString());
        }

        private Stopwatch accelerometerStopwatch = new Stopwatch();
        private Vector3 currentSpeed = new Vector3(0, 0, 0);
        private Vector3 oldDistance = new Vector3(0, 0, 0);
        private Vector3 newDistance = new Vector3(0, 0, 0);
        private Vector3 CaculateRelativeDistance(Vector3 acceleration, long timeSpan)
        {
            var degravityAcceleration = acceleration - new Vector3(0, 0, 1);
            //Debug.WriteLine("加速度仪：" + accelration.ToString());
            

            //调整精度
            int temp = (int)(degravityAcceleration.X * 10);
            degravityAcceleration.X = (float)((float)temp) / 10;

            temp = (int)(degravityAcceleration.Y * 10);
            degravityAcceleration.Y = (float)((float)temp) / 10;

            temp = (int)(degravityAcceleration.Z * 10);
            degravityAcceleration.Z = (float)((float)temp) / 10;

            degravityAcceleration *= (float)9.81;

            //Debug.WriteLine("加速度仪器乘法：" + degravityAcceleration.ToString());
            currentSpeed += degravityAcceleration * ((float)timeSpan / (float)1000.0);

            //调整精度
            temp = (int)(currentSpeed.X * 10);
            currentSpeed.X = (float)((float)temp) / 10;

            temp = (int)(currentSpeed.Y * 10);
            currentSpeed.Y = (float)((float)temp) / 10;

            temp = (int)(currentSpeed.Z * 10);
            currentSpeed.Z = (float)((float)temp) / 10;

            Debug.WriteLine("当前速度" + currentSpeed.ToString());

            newDistance += currentSpeed * ((float)timeSpan / (float)1000);
            Debug.WriteLine("当前距离：" + newDistance.ToString());

            Vector3 vector3 = new Vector3(newDistance.X, degravityAcceleration.Y, degravityAcceleration.Z);

            return vector3;
        }

        private void CursorButton_Clicked(object sender, EventArgs e)
        {
            if(Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                accelerometerStopwatch.Stop();
                //sendWatch.Stop();
            }
            else
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Start(SensorSpeed.Fastest);

                accelerometerStopwatch.Restart();
                //sendWatch.Start();
            }
        }
    }
}