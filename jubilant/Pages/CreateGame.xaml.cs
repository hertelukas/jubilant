using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CreateGame.xaml
    /// </summary>
    public partial class CreateGame : Page
    {
        public CreateGame()
        {
            InitializeComponent();
        }

        private void MaxPlayers_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursor = MaxPlayers.SelectionStart;
            MaxPlayers.Text = new String(MaxPlayers.Text.Where(char.IsDigit).ToArray());
            MaxPlayers.SelectionStart = cursor;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            int players;
            if(!Int32.TryParse(MaxPlayers.Text, out players)) players = 20;
            Package createGame = new Packages.CreateGame(GameName.Text, players);
            createGame.Send();
        }

        private void GameName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("[^a-z0-9]");

            if (regex.IsMatch(GameName.Text))
            {
                Feedback.Content = "Name can't contain any special characters";
                CreateButton.IsEnabled = false;
            }
            else if(GameName.Text.Length == 0)
            {
                Feedback.Content = "Name can't be empty";
                CreateButton.IsEnabled = false;
            }
            else
            {
                Feedback.Content = "";
                CreateButton.IsEnabled = true;
            }
        }
    }
}
