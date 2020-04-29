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
using System.IO;
using Newtonsoft.Json;

namespace VoiceTool
{
    /// <summary>
    /// Interaction logic for UserSetup.xaml
    /// </summary>
    public partial class UserSetup : Window
    {
        UserInputProfile userInputProfile;
        HomeScreen homeScreen;

        private const string FILEDIRECTORY = "../../../SaveFiles/";
        public bool IsClosed { get; private set; }

        KeyboardInputSetup keyboardInputSetupScreen;
        MouseInputSetup mouseInputSetupScreen;
        public UserSetup(HomeScreen _homeScreen)
        {
            InitializeComponent();
            homeScreen = _homeScreen;
            userInputProfile = homeScreen.userInputProfile;
            ConfigureDropdown();
            LoadGrid();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }

        private void LoadGrid()
        {
            InputDataGrid.ItemsSource = userInputProfile.actions;
        }

        private void ConfigureDropdown()
        {
            LoadingDropdown.Items.Clear();

            List<string> filesInFolder = Directory.GetFiles(FILEDIRECTORY, "*.*", SearchOption.AllDirectories).Where(file => new string[] { ".json" }.Contains(System.IO.Path.GetExtension(file))).ToList();

            int i = 0;

            foreach(var fileName in filesInFolder)
            {
                string[] words = fileName.Split('/');
                LoadingDropdown.Items.Add(words.Last());

                UserInputProfile temp = JsonConvert.DeserializeObject<UserInputProfile>(File.ReadAllText(FILEDIRECTORY + words.Last()));

                if (temp.profileId == userInputProfile.profileId)
                {
                    LoadingDropdown.SelectedIndex = i;
                }

                i++;
            }
        }

       
        public void ReloadGrid()
        {
            InputDataGrid.ItemsSource = null;
            InputDataGrid.ItemsSource = userInputProfile.actions;
        }

        public void UpdateInput(string _id, Input updatedInput)
        {
            InputDataGrid.CancelEdit();

            foreach (var instruction in userInputProfile.actions)
            {
                if(instruction.id == _id)
                {
                    instruction.input = updatedInput;
                }
            }

            ReloadGrid();
        }

        public void RemoveItem(object sender, RoutedEventArgs e)
        {
            var row = GetParent<DataGridRow>((Button)sender);
            var index = InputDataGrid.Items.IndexOf(row.Item);

            userInputProfile.actions.RemoveAt(index);
            InputDataGrid.CancelEdit();
            InputDataGrid.Items.Refresh();
        }

        private TargetType GetParent<TargetType>(DependencyObject o)
            where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }


        public void NewProfile(object sender, RoutedEventArgs e)
        {
            userInputProfile = new UserInputProfile();
            ReloadGrid();
        }
        public void AddKeyboard(object sender, RoutedEventArgs e)
        {
            userInputProfile.actions.Add(new Instruction("", new KeyboardInput(), 1, InstructionType.KEYBOARD));
            InputDataGrid.Items.Refresh();
        }

        public void AddMouse(object sender, RoutedEventArgs e)
        {
            userInputProfile.actions.Add(new Instruction("", new MouseInput(), 1, InstructionType.MOUSE));
            InputDataGrid.Items.Refresh();
        }

        public void ShowInputCreationScreen(object sender, RoutedEventArgs e)
        {
            Instruction obj = ((FrameworkElement)sender).DataContext as Instruction;

            CloseUnusedWindows();
            if (obj.instructionType == InstructionType.KEYBOARD)
            {
                keyboardInputSetupScreen = new KeyboardInputSetup(obj, this);
                keyboardInputSetupScreen.Show();
            }
            else
            {
                mouseInputSetupScreen = new MouseInputSetup(obj, this);
                mouseInputSetupScreen.Show();
            }
        }

        private void CloseUnusedWindows()
        {
            if (keyboardInputSetupScreen != null)
            {
                keyboardInputSetupScreen.Close();
                keyboardInputSetupScreen = null;
            }

            if (mouseInputSetupScreen != null)
            {
                mouseInputSetupScreen.Close();
                mouseInputSetupScreen = null;
            }
        }

        public void SaveCurrentProfile(object sender, RoutedEventArgs e)
        {
            if (userInputProfile.actions.Count > 0)
            {
                var dialog = new AlertDialogue();
                if (dialog.ShowDialog() == true)
                {
                    if (dialog.ResponseText != "")
                    {
                        
                            string jsonTypeNameAll = JsonConvert.SerializeObject(userInputProfile, Formatting.Indented, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });


                            File.WriteAllText(FILEDIRECTORY + dialog.ResponseText + ".json", jsonTypeNameAll);
                            

                        ConfigureDropdown();
                    }
                }
            }
        }

        public void LoadSelectedProfile(object sender, RoutedEventArgs e)
        {
            if(LoadingDropdown.SelectedItem.ToString() != "")
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

                userInputProfile = JsonConvert.DeserializeObject<UserInputProfile>(File.ReadAllText(FILEDIRECTORY + LoadingDropdown.SelectedItem.ToString()), settings);

                ReloadGrid();
            }
        }

        public void ConfirmControls(object sender, RoutedEventArgs e)
        {
            if(userInputProfile != null)
            {
                if(userInputProfile.actions.Count > 0)
                {
                    homeScreen.UpdateUserInputProfile(userInputProfile);
                    this.Close();
                }
                else
                {
                    var dialog = new Alert("Cannot set empty control scheme");
                    dialog.Show();
                }
            }
        }
    }
}
