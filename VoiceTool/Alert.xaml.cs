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
using System.Windows.Shapes;

namespace VoiceTool
{
    /// <summary>
    /// Interaction logic for Alert.xaml
    /// </summary>
    public partial class Alert : Window
    {
        public Alert(string alertMessage)
        {
            InitializeComponent();
            AlertMessage.Text = alertMessage;
        }
        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
