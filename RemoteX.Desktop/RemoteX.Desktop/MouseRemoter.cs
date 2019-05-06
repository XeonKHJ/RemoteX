using RemoteX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using WindowsInput;
using System.Diagnostics;
using static RemoteX.Common.RemoteCommand;

namespace RemoteX.Desktop
{
    internal class MouseRemoter : Remoter
    {

        private GattCharacteristic mouseMotionCharacteristic;
        private GattCharacteristic mouseEventCharacteristic;

        static private InputSimulator inputSimulator;
        private static MouseSimulator mouseSimulator;

        public MouseRemoter(GattDeviceService remoteService) : base(remoteService)
        {
            inputSimulator = new InputSimulator();
            mouseSimulator = new MouseSimulator(inputSimulator);
        }

        public async void GetCharacteristics()
        {
            try
            {
                var characterristicsResult = await remoteService.GetCharacteristicsAsync();
                foreach (var cart in characterristicsResult.Characteristics)
                {
                    if (cart.Uuid == RemoteUuids.MouseMotionCharacteristicGuid)
                    {
                        mouseMotionCharacteristic = cart;
                    }
                    if(cart.Uuid == RemoteUuids.MouseEventCharacteristicGuid)
                    {
                        mouseEventCharacteristic = cart;
                    }
                }
                if (mouseMotionCharacteristic == null)
                {
                    throw new Exception("没有鼠标特征");
                }
                mouseMotionCharacteristic.ValueChanged += MouseMotionCharacteristic_ValueChanged;
                mouseEventCharacteristic.ValueChanged += MouseEventCharacteristic_ValueChanged;
                GetNotify();
                //ReadCursorPoints();
            }
            catch(Exception excepiton)
            {
                System.Diagnostics.Debug.WriteLine(excepiton.Message);
            }
        }

        Stopwatch stopwatch = new Stopwatch();
        private int oldVertical = 0;
        private int oldHorizontal = 0;
        private int leftClickNo = 0;
        /// <summary>
        /// 鼠标事件收到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void MouseEventCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            using (var reader = DataReader.FromBuffer(args.CharacteristicValue))
            {
                byte[] bytesData = new byte[2 * sizeof(int)];
                reader.ReadBytes(bytesData);
                int mouseEventInt = BitConverter.ToInt32(bytesData, 0);
                switch((MouseEvent)mouseEventInt)
                {
                    case MouseEvent.LeftClick:
                        if(isLeftButtonDown)
                        {
                            mouseSimulator.LeftButtonUp();
                            isLeftButtonDown = false;
                            leftClickNo = 0;
                            stopwatch.Reset();
                        }
                        else if(++leftClickNo == 2)
                        {
                            leftClickNo = 0;
                            if(stopwatch.IsRunning)
                            {
                                mouseSimulator.LeftButtonDoubleClick();
                            }
                            
                            stopwatch.Reset();
                        }
                        else
                        {
                            stopwatch.Start();
                            await Task.Delay(300);
                            if (stopwatch.IsRunning)
                            {
                                mouseSimulator.LeftButtonClick();
                                leftClickNo = 0;
                                stopwatch.Reset();
                            }
                            Debug.WriteLine("LeftClick");
                        }
                        break;
                    case MouseEvent.RightClick:
                        mouseSimulator.RightButtonClick();
                        break;
                    case MouseEvent.LeftDown:
                        mouseSimulator.LeftButtonDown();
                        break;
                    case MouseEvent.RightDown:
                        mouseSimulator.RightButtonDown();
                        Debug.WriteLine("LeftDown");
                        break;
                    case MouseEvent.LeftUp:
                        mouseSimulator.LeftButtonUp();
                        Debug.WriteLine("Up");
                        break;
                    case MouseEvent.RightUp:
                        mouseSimulator.RightButtonUp();
                        break;
                    case MouseEvent.VerticalScroll:
                        int verticalScrollAmount = BitConverter.ToInt32(bytesData, sizeof(int));
                        if(verticalScrollAmount != 0)
                        {
                            int amount = (verticalScrollAmount - oldVertical) / 5;
                            if(amount > 0)
                            {
                                for (int i = 0; i < amount; ++i)
                                {
                                    mouseSimulator.VerticalScroll(1);
                                }
                            }
                            else if(amount < 0)
                            {
                                for(int i = 0; i < -amount; ++i)
                                {
                                    mouseSimulator.VerticalScroll(-1);
                                }
                            }
                        }
                        oldVertical = verticalScrollAmount;
                        break;
                    case MouseEvent.HorizontalScroll:
                        int horizontalScrollAmount = BitConverter.ToInt32(bytesData, sizeof(int));
                        if(horizontalScrollAmount != 0)
                        {
                            int amount = (horizontalScrollAmount - oldHorizontal) / 5;
                            if(amount > 0)
                            {
                                for (int i = 0; i < amount; ++i)
                                {
                                    mouseSimulator.HorizontalScroll(1);
                                }
                            }
                            else if(amount < 0)
                            {
                                for (int i = 0; i < -amount; ++i)
                                {
                                    mouseSimulator.HorizontalScroll(-1);
                                }
                            }
                        }
                        oldHorizontal = horizontalScrollAmount;
                        break;
                }
            }
        }

        private bool isLeftButtonDown = false;
        private CursorPoint oldCursorPoint = new CursorPoint { X = 0, Y = 0 };
        private void MouseMotionCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            using (var reader = DataReader.FromBuffer(args.CharacteristicValue))
            {

                
                //如果是长按的话
                if(stopwatch.IsRunning)
                {
                    mouseSimulator.LeftButtonDown();
                    isLeftButtonDown = true;
                    stopwatch.Reset();
                }

                byte[] bytesData = new byte[2*sizeof(double)];
                reader.ReadBytes(bytesData);
                CursorPoint cursorPoint = new CursorPoint
                {
                    X = BitConverter.ToDouble(bytesData, 0),
                    Y = BitConverter.ToDouble(bytesData, sizeof(double))
                };
                System.Diagnostics.Debug.WriteLine("new: " + cursorPoint.X.ToString() + ", " + cursorPoint.Y.ToString());
                System.Diagnostics.Debug.WriteLine("new: " + oldCursorPoint.X.ToString() + ", " + oldCursorPoint.Y.ToString());
                if (cursorPoint.X == 0 && cursorPoint.Y == 0)
                {
                    oldCursorPoint = cursorPoint;
                    System.Diagnostics.Debug.WriteLine("归0");
                    return;
                }
                var moveX = (cursorPoint.X - oldCursorPoint.X);
                var moveY = (cursorPoint.Y - oldCursorPoint.Y);
                Debug.WriteLine("PC: " + moveX.ToString() + ", " + moveY.ToString());

                int movXTotal = 0;
                int movYTotal = 0;
                while (true)
                {
                    bool stopMovingX = false;
                    bool stopMovingY = false;
                    if(moveX == 0)
                    {
                        stopMovingX = true;
                    }
                    if(moveY == 0)
                    {
                        stopMovingY = true;
                    }

                    if(moveX > 0)
                    {
                        if(++movXTotal <  moveX)
                        {
                            mouseSimulator.MoveMouseBy(1, 0);
                        }
                        else
                        {
                            stopMovingX = true;
                        }
                    }
                    else if(moveX < 0)
                    {
                        if(--movXTotal > moveX)
                        {
                            mouseSimulator.MoveMouseBy(-1, 0);
                        }
                        else
                        {
                            stopMovingX = true;
                        }
                    }

                    if(moveY > 0)
                    {
                        if (++movYTotal < moveY)
                        {
                            mouseSimulator.MoveMouseBy(0, 1);
                        }
                        else
                        {
                            stopMovingY = true;
                        }
                    }
                    else if(moveY < 0)
                    {
                        if(--movYTotal > moveY)
                        {
                            mouseSimulator.MoveMouseBy(0, -1);
                        }
                        else
                        {
                            stopMovingY = true;
                        }
                    }
                    if(stopMovingX && stopMovingY)
                    {
                        break;
                    }
                    
                }
                //mouseSimulator.MoveMouseBy((int)moveX, (int)moveY);
                oldCursorPoint = cursorPoint;
            }
        }

        private async void ReadCursorPoints()
        {
                while (true)
                {
                    try
                    {
                        var result = await mouseMotionCharacteristic.ReadValueAsync();
                        if(result.Status != GattCommunicationStatus.Success)
                        {
                            continue;
                        }
                        System.Diagnostics.Debug.WriteLine(result.Status.ToString());
                        using (var reader = DataReader.FromBuffer(result.Value))
                        {
                            byte[] bytesData = new byte[2 * sizeof(double)];
                            reader.ReadBytes(bytesData);
                            CursorPoint cursorPoint = new CursorPoint
                            {
                                X = BitConverter.ToDouble(bytesData, 0),
                                Y = BitConverter.ToDouble(bytesData, sizeof(double))
                            };
                            System.Diagnostics.Debug.WriteLine(cursorPoint.X.ToString() + ", " + cursorPoint.Y.ToString());
                            if (cursorPoint.X == 0 && cursorPoint.Y == 0)
                            {
                                oldCursorPoint = cursorPoint;
                                continue;
                            }
                            var moveX = (cursorPoint.X - oldCursorPoint.X);
                            var moveY = (cursorPoint.Y - oldCursorPoint.Y);
                            
                            mouseSimulator.MoveMouseBy((int)moveX, (int)moveY);
                            oldCursorPoint = cursorPoint;
                        }
                    }
                    catch
                    {
                        ;
                    }
                }
        }

        /// <summary>
        /// 订阅通知
        /// </summary>
        private async void GetNotify()
        {
            var status = await mouseMotionCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if (status != GattCommunicationStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }

            status = await mouseEventCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if (status != GattCommunicationStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(status.ToString());
            }
        }
    }
}
