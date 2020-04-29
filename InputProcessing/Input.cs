using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace InputProcessing
{
    public class Input
    {
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetForegroundWindow();

        protected Thread inputThread;
        public IntPtr targetWindow;

        public bool isActive = false;
        public bool isPaused = true;
        public virtual void StartInput(){}

        public virtual void StopInput() {}

        public virtual void ResetInput() { }
        public virtual string GetUIRepresentation() { return "Not Done Yet"; }

        public virtual void PerformInput() {}

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint SendInput(uint nInputs, InputStruct[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetMessageExtraInfo();

        [Flags]
        protected enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008,
        }

        [Flags]
        protected enum MouseEventF
        {
            MouseMove = 0x0001,
            LeftPressed = 0x0002,
            LeftReleased = 0x0004,
            RightPressed = 0x0008,
            RightReleased = 0x0010
        }

        [Flags]
        protected enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        protected struct InputStruct
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        protected struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInp ki;
            [FieldOffset(0)] public readonly HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct KeyboardInp
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public readonly uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct HardwareInput
        {
            public readonly uint uMsg;
            public readonly ushort wParamL;
            public readonly ushort wParamH;
        }
    }
}
