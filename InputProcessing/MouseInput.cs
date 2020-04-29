using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputProcessing
{
    public enum MouseInputType
    {
        PRESS_AND_RELEASE,
        DOUBLE_CLICK,
        PRESS_MOUSE_BUTTON,
        RELEASE_MOUSE_BUTTON,
        MOUSE_MOVE,
        NONE
    }

    public class MouseInput : Input
    {
        public MouseInputType mouseInputType = MouseInputType.NONE;

        public int XMove = 0;
        public int YMove = 0;
        private float xAmountMoved = 0.0f;
        private float yAmountMoved = 0.0f;
        private float lookDuration = 1000.0f;
        public float mouseSensitivity = 2;

        public bool clickType;

        Stopwatch sw = new Stopwatch();

        private int clickCounter = 1;

        public int duration { get; set; }
        public MouseInput() { }


        /// <summary>
        /// ClickType false = left click, true = right click
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="clickType"></param>
        public MouseInput(MouseInputType _type, bool _clickType, int _duration)
        {
            mouseInputType = _type;
            clickType = _clickType;
            duration = _duration;
        }

        public MouseInput(MouseInputType _type, bool _clickType)
        {
            mouseInputType = _type;
            clickType = _clickType;
        }

        /// <summary>
        /// Setup for a mouse move event
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MouseInput(MouseInputType _type, int x, int y, int sentivity)
        {
            mouseInputType = _type;
            XMove = x;
            YMove = y;
            mouseSensitivity = sentivity;
        }
        public override string GetUIRepresentation()
        {
            switch (mouseInputType)
            {
                case MouseInputType.PRESS_AND_RELEASE:
                    return "Press and Release " + (clickType? "Right Click" : "Left Click");
                case MouseInputType.DOUBLE_CLICK:
                    return "Double Click " + (clickType? "Right" : "Left");
                case MouseInputType.PRESS_MOUSE_BUTTON:
                    return "Hold " + (clickType ? "Right Click" : "Left Click");
                case MouseInputType.RELEASE_MOUSE_BUTTON:
                    return "Release " + (clickType ? "Right Click" : "Left Click"); ;
                case MouseInputType.MOUSE_MOVE:
                    return "Move Mouse X:" + XMove + " Y: " + YMove;
                default:
                    return "Not Setup";
            }
        }

        public override void StartInput()
        {
            if (inputThread != null || isActive == true)
            {
                StopInput();
            }

            inputThread = new Thread(PerformInput);
            inputThread.Start();
            sw.Start();
            isActive = true;
        }

        public override void StopInput()
        {
            isActive = false;
            sw.Stop();
            sw.Reset();
            clickCounter = 1;
        }
        public override void ResetInput()
        {
            sw.Reset();
            clickCounter = 1;
        }

        public override void PerformInput()
        {
            while (isActive)
            {
                if (GetForegroundWindow() == targetWindow)
                {
                    switch (mouseInputType)
                    {
                        case MouseInputType.PRESS_AND_RELEASE:
                            PressAndRelease();
                            break;
                        case MouseInputType.DOUBLE_CLICK:
                            DoubleClick();
                            break;
                        case MouseInputType.PRESS_MOUSE_BUTTON:
                            PressMouseButton();
                            break;
                        case MouseInputType.RELEASE_MOUSE_BUTTON:
                            ReleaseMouseButton();
                            break;
                        case MouseInputType.MOUSE_MOVE:
                            ProcessMouseMove();
                            break;
                    }

                    Thread.Sleep(200);
                }
            }
        }

        private void PressMouseButton()
        {
            SendMouseEvent(clickType ? (uint)MouseEventF.RightPressed : (uint)MouseEventF.LeftPressed);
        }

        private void ReleaseMouseButton()
        {
            SendMouseEvent(clickType ? (uint)MouseEventF.RightReleased : (uint)MouseEventF.LeftReleased);
            isActive = false;
        }
        private void PressAndRelease()
        {
            //False == left click
            SendMouseEvent(clickType? (uint)MouseEventF.RightPressed : (uint)MouseEventF.LeftPressed);
 
            if (sw.ElapsedMilliseconds > duration)
            {
                SendMouseEvent(clickType? (uint)MouseEventF.RightReleased : (uint)MouseEventF.LeftReleased);
                isActive = false;
            }
        }

        private void DoubleClick()
        {
            if (clickCounter < 5)
            {
                if (clickCounter % 2 != 0)
                {
                    SendMouseEvent(clickType ? (uint)MouseEventF.RightPressed : (uint)MouseEventF.LeftPressed);
                }
                else
                {
                    SendMouseEvent(clickType ? (uint)MouseEventF.RightReleased : (uint)MouseEventF.LeftReleased);
                }
                clickCounter++;
            }
            else
            {
                isActive = false;
            }
        }
        private void ProcessMouseMove()
        {
            if (sw.ElapsedMilliseconds < lookDuration)
            {
                MoveMouse((int)((XMove / (lookDuration / 1000)) * mouseSensitivity), (int)((YMove / (lookDuration / 1000)) * mouseSensitivity), (uint)(MouseEventF.MouseMove));
            }
            else
            {
                isActive = false;
            }
        }

        private void MoveMouse(int xMoveAmount, int yMoveAmount, uint _dwFlags)
        {
            InputStruct[] inputs =
            {
            new InputStruct
            {
                type = (int) InputType.Mouse,
                u = new InputUnion
                {
                    mi = new MouseInput
                    {
                        dx = xMoveAmount,
                        dy = yMoveAmount,
                        mouseData = 0,
                        dwFlags = _dwFlags,
                        time = 0,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(InputStruct)));
        }

        public void SendMouseEvent(uint _dwFlags)
        {
            InputStruct[] inputs =
            {
            new InputStruct
            {
                type = (int) InputType.Mouse,
                u = new InputUnion
                {
                    mi = new MouseInput
                    {
                        dx = 0,
                        dy = 0,
                        mouseData = 0,
                        dwFlags = _dwFlags,
                        time = 0,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(InputStruct)));
        }
    }
}
