using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace jubilant.Pages
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            Manager.SetStartPage(this);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Feedback.Content = "Logging in...";

            Manager.ip = ip.Text;
            Manager.username = Username.Text.Trim();

            Thread thread = new Thread(Client.StartClient);
            thread.Start();
        }

        private void CheckLegalInput()
        {
            if (Username.Text.IndexOf(":") != -1)
            {
                Feedback.Content = "Username can't contain ':'";
                LoginButton.IsEnabled = false;
            }
            else if(Username.Text.Trim().IndexOf(" ") != -1)
            {
                Feedback.Content = "Username can't contain any white spaces";
                LoginButton.IsEnabled = false;
            }
            else if(Username.Text.Length == 0)
            {
                Feedback.Content = "Username can't be empty";
                LoginButton.IsEnabled = false;
            }
            else
            {
                Feedback.Content = "";
                LoginButton.IsEnabled = true;
            }

        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLegalInput();
        }

        public void UsernameTaken()
        {
            Feedback.Content = "Username taken";
        }
    }
}
