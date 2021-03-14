using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace jubilant
{
    public static class Manager
    {
        public static int myId { get; set; } = -1;
        public static int gameId { get; set; } = -1;
        public static string username { get; set; }
        public static string ip { get; set; } = "192.168.1.187";
        private static NavigationWindow window;
        private static Pages.StartPage startPage;
        private static Pages.Menu menuPage;
        private static NavigationService nav;
        private static Dictionary<int, Game> games { get; set; } = new Dictionary<int, Game>();

        public static void HandlePackage(Package package)
        {
            switch (package.packageId)
            {
                case PackageType.Welcome:
                    break;
                case PackageType.WelcomeReceived:
                    myId = package.playerId;
                    Debug.WriteLine($"My id is {myId}");
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        nav = NavigationService.GetNavigationService((Page)startPage);
                        Debug.WriteLine(nav == null);
                        if (nav != null) nav.Navigate(new Pages.Menu());
                    });
                    break;
                case PackageType.UsernameTaken:
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        startPage.UsernameTaken();
                    });
                    Debug.WriteLine("Username is taken");
                    break;
                case PackageType.GameCreated:
                    HandleNewGame(package);
                    break;
                default:
                    break;
            }
        }

        public static void SetWindow(NavigationWindow window)
        {
            Manager.window = window;
        }

        public static void SetStartPage(Pages.StartPage page)
        {
            Manager.startPage = page;
        }

        public static void SetMenu(Pages.Menu menu)
        {
            Manager.menuPage = menu;
        }

        private static void HandleNewGame(Package package)
        {
            string[] args = package.content.Split(",");
            string gameName = args[0];

            bool parseSuccess = false;

            int players = 0;
            parseSuccess = Int32.TryParse(args[1], out players) || parseSuccess;
            int gameId = 0;
            parseSuccess = Int32.TryParse(args[2], out gameId) || parseSuccess;
            int adminId = 0;
            parseSuccess = Int32.TryParse(args[3], out adminId) || parseSuccess;

            if (!parseSuccess) Debug.WriteLine("Parsing of new game package failed.");
            else
            {
                Game game = new Game(gameName, players, gameId, adminId);
                if (games.TryAdd(gameId, game))
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        menuPage.UpdateGames(games);
                    });
                }

            }

        }
    }
}
