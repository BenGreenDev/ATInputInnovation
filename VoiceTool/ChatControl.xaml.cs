using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;
using IRCChatApi;
using InputProcessing;
using System.Runtime.InteropServices;

namespace VoiceTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatControl : Window
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        IRCChat ircChat = new IRCChat();
        InputProcessing.InputManager inputManager = new InputProcessing.InputManager();
        Thread chatRenderThread;
        bool isChatRendering = true;
        IDictionary<IntPtr, string> openWindows;
        UserInputProfile userInputProfile;
        bool isPlaying = false;
        public bool IsClosed { get; private set; }

        public ChatControl(UserInputProfile _profile)
        {
            InitializeComponent();
            userInputProfile = _profile;
            init(userInputProfile);
        }

        private void init(UserInputProfile _profile)
        {
            isChatRendering = true;
            chatRenderThread = new Thread(RenderIRC);
            chatRenderThread.Start();
            inputManager.SetControlScheme(userInputProfile);

            RefreshOpenWindows();
            LoadGrid();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }

        public void UpdateUserInputProfile(UserInputProfile _userInputProfile)
        {
            isChatRendering = false;
            chatRenderThread.Abort();
            inputManager.EndProcessing();
            isPlaying = false;
            PlayButton.Content = "Play";

            userInputProfile = _userInputProfile;
            init(userInputProfile);
        }

        private void LoadGrid()
        {
            ActivationGrid.CancelEdit();
            ActivationGrid.ItemsSource = userInputProfile.actions;
        }

        public void RefreshOpenWindows()
        {
            TargetWindow.Items.Clear();
            openWindows = OpenWindowGetter.GetOpenWindows();

            foreach (var win in openWindows)
            {
                TargetWindow.Items.Add(win);
            }
        }

        public void RefreshDropdown(object sender, RoutedEventArgs e)
        {
            if (isPlaying == false)
            {
                RefreshOpenWindows();
            }
            else
            {
                var dialogue = new Alert("You cannot refresh this whilst playing");
                dialogue.Show();
            }
        }

        public void ConnectButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!ircChat.IsConnected())
            {
                bool successful = ircChat.InitaliseIRC(Username.Text, OAuth.Text, IRCChannelName.Text);

                if (successful)
                {
                    MessageBoxResult result = MessageBox.Show("IRC Connection", "Connection Successful", MessageBoxButton.OK);
                    ConnectButton.Content = "Disconnect";
                }
                else
                {

                    MessageBoxResult result = MessageBox.Show("IRC Connection", "Connection Failed", MessageBoxButton.OK);
                }
            }
            else
            {
                ircChat.Disconnect();
                ConnectButton.Content = "Connect";
            }
        }

        public void RenderIRC()
        {
            while(isChatRendering)
            {
                if(ircChat.CheckIfMessageIsInQueue())
                {
                    string stringToDispatch = ircChat.GetMessageFromQueue();
                    string text = "";
                    System.Windows.Application.Current.Dispatcher.Invoke((ThreadStart)delegate { text = ChatLog.Text; });

                    bool isValidInstruction = CheckIfIncommingStringIsCommand(stringToDispatch);
                    string renderString = "";

                    if (isValidInstruction)
                    {
                        inputManager.AddStringToIncomingInstructions(stringToDispatch);
                        renderString = "\r    Detected:" + stringToDispatch;
                    }

                    if (text.Length < 700)
                    {
                        Dispatcher.BeginInvoke((Action)(() => ChatLog.Text = ChatLog.Text + renderString));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => ChatLog.Text = renderString));
                    }
                    Thread.Sleep(20);
                }
            }
        }

        private bool CheckIfIncommingStringIsCommand(string stringToDispatch)
        {
            foreach(var instruction in userInputProfile.actions)
            {
                if(instruction.activationKeyword == stringToDispatch)
                {
                    return true;
                }
            }

            return false;
        }

        public void Play(object sender, RoutedEventArgs e)
        {
            if (TargetWindow.SelectedItem != null)
            {
                if (isPlaying == false)
                {
                    isPlaying = true;
                    PlayButton.Content = "Stop";
                    inputManager.SetTargetWindow(((KeyValuePair<IntPtr, string>)TargetWindow.SelectedValue).Key);
                    inputManager.StartProcessing();
                }
                else
                {
                    inputManager.EndProcessing();
                    isPlaying = false;
                    PlayButton.Content = "Play";
                }
            }
            else
            {
                var dialogue = new Alert("You cannot target a non-existant window");
                dialogue.Show();
            }
        }
    }
}
