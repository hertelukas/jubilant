using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private NavigationService nav;
        public Menu()
        {
            InitializeComponent();
            HelloText.Content = $"Hello {Manager.username}!";
            Manager.SetMenu(this);
        }

        private void GamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GamesList.SelectedIndex == -1) JoinGameButton.IsEnabled = false;
            else JoinGameButton.IsEnabled = true;
        }

        private void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService((Page)this);
            if (nav != null) nav.Navigate(new CreateGame());
        }

        public void UpdateGames(Dictionary<int, Game> games)
        {
            GamesList.Items.Clear();
            foreach (var game in games)
            {

                Grid grid = new Grid
                {
                    Width = 284
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition {
                    Width = new GridLength(1, GridUnitType.Star)
                });

                TextBlock title = new TextBlock
                {
                    Text = game.Value.name
                };

                TextBlock players = new TextBlock
                {
                    Text = $"{game.Value.players.Count} / {game.Value.maxPlayers}",
                    HorizontalAlignment = HorizontalAlignment.Right
                };


                players.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(title);
                grid.Children.Add(players);


                GamesList.Items.Add(grid);
            }
        }
    }
}
