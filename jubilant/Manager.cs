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
        private static NavigationService nav;

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
                    Debug.WriteLine(package.content);
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
    }
}
