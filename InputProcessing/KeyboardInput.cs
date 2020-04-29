using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

[Flags]
public enum KeyModifiers
{
    Shift = 42,
    Ctrl = 29
}


namespace InputProcessing
{
    public enum KeyboardInputType
    {
        PRESS_AND_RELEASE,
        PRESS_KEY,
        RELEASE_KEY,
        TOGGLE_KEY,
        NONE
    }

    public class Hotkey
    {
        public Key Key;

        public ModifierKeys Modifiers;

        public ushort KeyCode;
        public ushort ScanCode;
        public uint ModifierScancode;

        public bool hasModifier;

        public Hotkey(Key _key, ModifierKeys _modifiers, ushort _keyCode, ushort _scanCode, bool _hasModifier)
        {
            Key = _key;
            Modifiers = _modifiers;
            KeyCode = _keyCode;
            ScanCode = _scanCode;
            hasModifier = _hasModifier;
        }

        public static string ToHex(int value)
        {
            return String.Format("0x{0:X}", value);
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }
    }

    public class KeyboardInput : Input
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public Hotkey hotkey;
        public KeyboardInputType keyboardInputType = KeyboardInputType.NONE;

        public List<int> keysToPress;

        public int keyToPress = 0x57;
        const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int KEYEVENTF_KEYUP = 0x0002;
        public int duration { get; set; }

        bool hasBeenPressed = false;
        bool usingScanCode = false;

        Stopwatch sw = new Stopwatch();

        public KeyboardInput()
        {
            duration = 100;
        }

        public KeyboardInput(int _keyToPress, bool _indefinite, int _duration, KeyboardInputType _keyboardInputType, Hotkey _hotKey)
        {
            hotkey = _hotKey;
            keyboardInputType = _keyboardInputType;
            keyToPress = _keyToPress;
            duration = _duration;
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
        }

        public override void ResetInput()
        {
            sw.Reset();
        }

        public override void PerformInput()
        {
            while (isActive)
            {
                if (GetForegroundWindow() == targetWindow)
                {
                    switch (keyboardInputType)
                    {
                        case KeyboardInputType.PRESS_AND_RELEASE:
                            PressAndRelease();
                            break;
                        case KeyboardInputType.PRESS_KEY:
                            HoldKey();
                            Thread.Sleep(20);
                            break;
                        case KeyboardInputType.RELEASE_KEY:
                            ReleaseKey();
                            break;
                        case KeyboardInputType.TOGGLE_KEY:
                            ToggleKey();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Hold key for a duration
        /// </summary>
        private void PressAndRelease()
        {
            if (hotkey.hasModifier)
            {
                switch (hotkey.Modifiers)
                {
                    case ModifierKeys.None:
                        break;
                    case ModifierKeys.Control:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode), (ushort)KeyModifiers.Ctrl, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
                        break;
                    case ModifierKeys.Shift:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode), (ushort)KeyModifiers.Shift, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SendKey(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
            }

            if (sw.ElapsedMilliseconds > duration)
            {
                if (hotkey.hasModifier)
                {
                    switch (hotkey.Modifiers)
                    {
                        case ModifierKeys.None:
                            break;
                        case ModifierKeys.Control:
                            SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode), (ushort)KeyModifiers.Ctrl, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode));
                            break;
                        case ModifierKeys.Shift:
                            SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode), (ushort)KeyModifiers.Shift, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    SendKey(hotkey.ScanCode, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode));
                }
                isActive = false;
            }
        }

        private void HoldKey()
        {
            if (hotkey.hasModifier)
            {
                switch (hotkey.Modifiers)
                {
                    case ModifierKeys.None:
                        break;
                    case ModifierKeys.Control:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode), (ushort)KeyModifiers.Ctrl, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
                        break;
                    case ModifierKeys.Shift:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode), (ushort)KeyModifiers.Shift, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
                        break;
                    default:
                        break;

                }
            }
            else
            {
                SendKey(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
            }

        }

        private void ReleaseKey()
        {
            if (hotkey.hasModifier)
            {
                switch (hotkey.Modifiers)
                {
                    case ModifierKeys.None:
                        break;
                    case ModifierKeys.Control:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode), (ushort)KeyModifiers.Ctrl, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode));
                        break;
                    case ModifierKeys.Shift:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode), (ushort)KeyModifiers.Shift, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SendKey(hotkey.ScanCode, (uint)(KeyEventF.KeyUp | KeyEventF.Scancode));
            }

            isActive = false;
        }

        /// <summary>
        /// Toggle a key once
        /// </summary>
        private void ToggleKey()
        {
            if (hotkey.hasModifier)
            {
                switch (hotkey.Modifiers)
                {
                    case ModifierKeys.None:
                        break;
                    case ModifierKeys.Control:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode), (ushort)KeyModifiers.Ctrl, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
                        break;
                    case ModifierKeys.Shift:
                        SendTwoKeys(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode), (ushort)KeyModifiers.Shift, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SendKey(hotkey.ScanCode, (uint)(KeyEventF.KeyDown | KeyEventF.Scancode));
            }

            isActive = false;
        }


        public override string GetUIRepresentation()
        {
            if (hotkey != null)
            {
                switch (keyboardInputType)
                {
                    case KeyboardInputType.PRESS_AND_RELEASE:
                        return "Hold " + hotkey.ToString() + " For " + duration.ToString() + " MS";
                    case KeyboardInputType.PRESS_KEY:
                        return "Hold " + hotkey.ToString() + " Indefinately";
                    case KeyboardInputType.RELEASE_KEY:
                        return "Release " + hotkey.ToString();
                    case KeyboardInputType.TOGGLE_KEY:
                        return "Toggle " + hotkey.ToString();
                    default:
                        return "Not Setup";
                }
            }
            else
            {
                return "Not Setup";
            }
        }

        public static void SendKey(ushort key, uint _dwFlag)
        {
            
            InputStruct[] inputs =
            {
            new InputStruct
            {
                type = (int) InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KeyboardInp
                    {
                        wVk = 0,
                        wScan = key,
                        dwFlags = _dwFlag,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(InputStruct)));
        }

        public static void SendTwoKeys(ushort key, uint _dwFlag, ushort modifierkey, uint _modifierdwFlag)
        {
            InputStruct[] inputs =
            {
            new InputStruct
            {
                type = (int) InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KeyboardInp
                    {
                        wVk = 0,
                        wScan = modifierkey,
                        dwFlags = _modifierdwFlag,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            },
            new InputStruct
            {
                type = (int) InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KeyboardInp
                    {
                        wVk = 0,
                        wScan = key,
                        dwFlags = _dwFlag,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(InputStruct)));
        }
    }
}

