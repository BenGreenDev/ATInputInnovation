using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InputProcessing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Reflection;

namespace VoiceTool
{
    /// <summary>
    /// Interaction logic for InputSetup.xaml
    /// </summary>
    public partial class KeyboardInputSetup : Window
    {
        public Instruction instruction { get; set; }

        UserSetup userSetup;

        public Hotkey hotKey;

        public KeyboardInputSetup(Instruction _instruction, UserSetup _userSetup)
        {
            InitializeComponent();

            instruction = _instruction;
            ScreenTitle.Content = instruction.activationKeyword;
            userSetup = _userSetup;

            if(instruction.input != null)
            {
                SetupUI();
            }
        }

        private void SetupUI()
        {
            KeyboardInput temp = (KeyboardInput)instruction.input;
            
            switch(temp.keyboardInputType)
            {
                case KeyboardInputType.PRESS_AND_RELEASE:
                    PressAndRelease.IsChecked = true;
                    break;
                case KeyboardInputType.PRESS_KEY:
                    HoldKey.IsChecked = true;
                    break;
                case KeyboardInputType.RELEASE_KEY:
                    ReleaseKey.IsChecked = true;
                    break;
                case KeyboardInputType.TOGGLE_KEY:
                    ToggleKey.IsChecked = true;
                    break;
            }

            NumberTextBox.Text = temp.duration.ToString();

            hotKey = temp.hotkey;

            if (hotKey != null)
            {
                KeyTextBox.Text = hotKey.ToString();
            }
        }
        public void SubmitInputs(object sender, RoutedEventArgs e)
        {
            Input tempInput = new Input();

            if((bool)PressAndRelease.IsChecked)
            {
                tempInput = new KeyboardInput(1, false, int.Parse(NumberTextBox.Text), KeyboardInputType.PRESS_AND_RELEASE, hotKey);
            }
            else if((bool)HoldKey.IsChecked)
            {
                tempInput = new KeyboardInput(1, false, int.Parse(NumberTextBox.Text), KeyboardInputType.PRESS_KEY, hotKey);
            }
            else if((bool)ReleaseKey.IsChecked)
            {
                tempInput = new KeyboardInput(1, false, int.Parse(NumberTextBox.Text), KeyboardInputType.RELEASE_KEY, hotKey);
            }
            else if((bool)ToggleKey.IsChecked)
            {
                tempInput = new KeyboardInput(1, false, int.Parse(NumberTextBox.Text), KeyboardInputType.TOGGLE_KEY, hotKey);
            }

            userSetup.UpdateInput(instruction.id, tempInput);
            this.Close();
        }

        public void DiscardInputs(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");

            if (regex.IsMatch(e.Text))
            {
                e.Handled = regex.IsMatch(e.Text);
            }
        }

        private void KeyValidationTextBox(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            ushort keyCode = (ushort)System.Windows.Input.KeyInterop.VirtualKeyFromKey(key);

            MSG l_Msg;
            ushort l_Scancode;
            PresentationSource source = e.InputSource;

            var l_MSGField = source.GetType().GetField("_lastKeyboardMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            l_Msg = (MSG)l_MSGField.GetValue(source);
            l_Scancode = (ushort)(l_Msg.lParam.ToInt32() >> 16);

            if (modifiers == ModifierKeys.None &&
                (key == Key.Delete || key == Key.Back || key == Key.Escape))
            {
                hotKey = null;
                KeyTextBox.Text = "";
                return;
            }

            if (key == Key.System)
            {
                key = e.SystemKey;
            }

            if (key == Key.LeftCtrl ||
                key == Key.RightCtrl ||
                key == Key.RWin ||
                key == Key.LeftAlt ||
                key == Key.LeftShift ||
                key == Key.RightAlt ||
                key == Key.RightShift ||
                key == Key.LWin ||
                key == Key.OemClear ||
                key == Key.Apps ||
                key == Key.Clear)
            {
                return;
            }

            if(modifiers != ModifierKeys.None)
            {
                if(modifiers == ModifierKeys.Shift || modifiers == ModifierKeys.Control)
                {
                    hotKey = new Hotkey(key, modifiers, keyCode, l_Scancode, true);
                }
                else
                {
                    return;
                }
            }
            else
            {
                hotKey = new Hotkey(key, modifiers, keyCode, l_Scancode, false);
            }

            KeyTextBox.Text = hotKey.ToString();
        }
    }
}
