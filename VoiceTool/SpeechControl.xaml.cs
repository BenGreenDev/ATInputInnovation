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
using SpeechRecognition;
using InputProcessing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;

namespace VoiceTool
{

  
    /// <summary>
    /// Interaction logic for SpeechControl.xaml
    /// </summary>
    public partial class SpeechControl : Window
    {
        UserInputProfile userInputProfile;
        SpeechManager speechManager = new SpeechManager();
        InputProcessing.InputManager inputManager = new InputProcessing.InputManager();
        IDictionary<IntPtr, string> openWindows;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        bool isPlaying = false;

        Thread speechRenderThread;
        bool isSpeechRendering = true;

        public bool IsClosed { get; private set; }

        public SpeechControl(UserInputProfile _profile)
        {
            InitializeComponent();
            userInputProfile = _profile;

            init(userInputProfile); 
        }

        void init(UserInputProfile _profile)
        {
            inputManager.SetControlScheme(userInputProfile);

            List<string> temp = new List<string>();

            foreach (var instruction in userInputProfile.actions)
            {
                temp.Add(instruction.activationKeyword);
            }

            speechManager.LoadCommands(temp);
            isSpeechRendering = true;
            speechRenderThread = new Thread(ProcessVoice);
            speechRenderThread.Start();

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
            isSpeechRendering = false;
            speechRenderThread.Abort();
            speechManager.StopRecognising();
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

            foreach(var win in openWindows)
            {
                TargetWindow.Items.Add(win);
            }
        }

        public void RefreshDropdown(object sender, RoutedEventArgs e)
        {
            if(isPlaying == false)
            {
                RefreshOpenWindows();
            }
            else
            {
                var dialogue = new Alert("You cannot refresh this whilst playing");
                dialogue.Show();
            }
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
                    speechManager.StartRecognising();
                }
                else
                {
                    speechManager.StopRecognising();
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

        public void ProcessVoice()
        {
            while (isSpeechRendering)
            {
                if (speechManager.CheckIfMessageIsInQueue())
                {
                    string stringToDispatch = speechManager.GetMessageFromQueue();
                    string text = "";
                    System.Windows.Application.Current.Dispatcher.Invoke((ThreadStart)delegate { text = ChatLog.Text; });

                    if (text.Length < 700)
                    {
                        Dispatcher.BeginInvoke((Action)(() => ChatLog.Text = ChatLog.Text + "\r    Detected:" + stringToDispatch));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => ChatLog.Text = "\r    Detected:" + stringToDispatch));
                    }
                    inputManager.AddStringToIncomingInstructions(stringToDispatch);
                } 
            }
        }
    }

}
