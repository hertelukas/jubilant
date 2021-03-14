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

namespace jubilant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.SetWindow(this);
        }

        private void NavigationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Client.Connected())
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to disconnect?", "Closing...", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.No) e.Cancel = true;
                else
                {
                    Client.Disconnect();
                }
            }
        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }
    }
}
