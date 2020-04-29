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

namespace VoiceTool
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        public UserInputProfile userInputProfile = new UserInputProfile();
        private UserSetup userSetupWindow;
        private SpeechControl speechControlWindow;
        private ChatControl chatControlWindow;

        public HomeScreen()
        {
            InitializeComponent();
        }

        public void OpenControlScheme(object sender, RoutedEventArgs e)
        {
            if (userSetupWindow == null)
            {
                userSetupWindow = new UserSetup(this);
                App.Current.MainWindow = userSetupWindow;
                userSetupWindow.Show();
            }
            else
            {
                if(userSetupWindow.IsClosed)
                {
                    userSetupWindow = new UserSetup(this);
                    App.Current.MainWindow = userSetupWindow;
                    userSetupWindow.Show();
                }
                else
                {
                    userSetupWindow.Topmost = true;
                }
            }
        }

        public void OpenTwitch(object sender, RoutedEventArgs e)
        {
            if (userInputProfile.actions.Count > 0)
            {
                if (chatControlWindow == null)
                {
                    chatControlWindow = new ChatControl(userInputProfile);
                    App.Current.MainWindow = speechControlWindow;
                    chatControlWindow.Show();
                }
                else
                {
                    if (chatControlWindow.IsClosed)
                    {
                        chatControlWindow = new ChatControl(userInputProfile);
                        App.Current.MainWindow = chatControlWindow;
                        chatControlWindow.Show();
                    }
                    else
                    {
                        chatControlWindow.Topmost = true;
                    }
                }
            }
        }

        public void OpenVoice(object sender, RoutedEventArgs e)
        {
            if (userInputProfile.actions.Count > 0)
            {
                if (speechControlWindow == null)
                {
                    speechControlWindow = new SpeechControl(userInputProfile);
                    App.Current.MainWindow = speechControlWindow;
                    speechControlWindow.Show();
                }
                else
                {
                    if (speechControlWindow.IsClosed)
                    {
                        speechControlWindow = new SpeechControl(userInputProfile);
                        App.Current.MainWindow = speechControlWindow;
                        speechControlWindow.Show();
                    }
                    else
                    {
                        speechControlWindow.Topmost = true;
                    }
                }
            }
        }

        public void UpdateUserInputProfile(UserInputProfile _newProfile)
        {
            userInputProfile = _newProfile;
            if(speechControlWindow != null)
            {
                speechControlWindow.UpdateUserInputProfile(userInputProfile);
            }

            if(chatControlWindow != null)
            {
                chatControlWindow.UpdateUserInputProfile(userInputProfile);
            }
        }
    }
}
