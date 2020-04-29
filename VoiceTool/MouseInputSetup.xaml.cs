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
using System.Text.RegularExpressions;
using InputProcessing;

namespace VoiceTool
{
    /// <summary>
    /// Interaction logic for MouseInputSetup.xaml
    /// </summary>
    public partial class MouseInputSetup : Window
    {
        private Instruction instruction;
        private UserSetup userSetup;

        public MouseInputSetup(Instruction _instruction, UserSetup _userSetup)
        {
            InitializeComponent();
            instruction = _instruction;
            userSetup = _userSetup;
            SetupScreen();
        }

        private void SetupScreen()
        {
            MouseInput temp = (MouseInput)instruction.input;

            if (temp != null)
            {
                switch (temp.mouseInputType)
                {
                    case MouseInputType.PRESS_AND_RELEASE:
                        PressAndRelease.IsChecked = true;
                        break;
                    case MouseInputType.DOUBLE_CLICK:
                        DoubleClick.IsChecked = true;
                        break;
                    case MouseInputType.PRESS_MOUSE_BUTTON:
                        PressKey.IsChecked = true;
                        break;
                    case MouseInputType.RELEASE_MOUSE_BUTTON:
                        ReleaseKey.IsChecked = true;
                        break;
                    case MouseInputType.MOUSE_MOVE:
                        MouseMove.IsChecked = true;
                        break;
                    default:
                        break;
                }

                if(temp.clickType)
                {
                    RightClick.IsChecked = true;
                }
                else
                {
                    LeftClick.IsChecked = true;
                }

                XMovement.Text = temp.XMove.ToString();
                YMovement.Text = temp.YMove.ToString();
                SensitivityBox.Text = temp.mouseSensitivity.ToString();
            }
        }

        private void NegNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^-[^0-9]+");

            if (regex.IsMatch(e.Text))
            {
                e.Handled = regex.IsMatch(e.Text);
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");

            if (regex.IsMatch(e.Text))
            {
                e.Handled = regex.IsMatch(e.Text);
            }
        }

        public void DiscardInputs(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public void SubmitInputs(object sender, RoutedEventArgs e)
        {
            Input tempInput = new Input();

            if ((bool)PressAndRelease.IsChecked)
            {
                tempInput = new MouseInput(MouseInputType.PRESS_AND_RELEASE, (bool)RightClick.IsChecked, Convert.ToInt16(NumberTextBox.Text));
            }
            else if ((bool)DoubleClick.IsChecked)
            {
                tempInput = new MouseInput(MouseInputType.DOUBLE_CLICK, (bool)RightClick.IsChecked);
            }
            else if ((bool)PressKey.IsChecked)
            {
                tempInput = new MouseInput(MouseInputType.PRESS_MOUSE_BUTTON, (bool)RightClick.IsChecked);
            }
            else if ((bool)ReleaseKey.IsChecked)
            {
                tempInput = new MouseInput(MouseInputType.RELEASE_MOUSE_BUTTON, (bool)RightClick.IsChecked);
            }
            else if ((bool)MouseMove.IsChecked)
            {
                tempInput = new MouseInput(MouseInputType.MOUSE_MOVE, int.Parse(XMovement.Text), int.Parse(YMovement.Text), 2);
            }

            userSetup.UpdateInput(instruction.id, tempInput);
            this.Close();
        }
    }
}
