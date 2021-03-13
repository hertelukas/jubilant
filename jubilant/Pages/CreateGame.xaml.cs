using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Package createGame = new Packages.CreateGame(GameName.Text, Int32.Parse(MaxPlayers.Text));
            createGame.Send();
        }
    }
}
