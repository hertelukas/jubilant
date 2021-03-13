using System;
using System.Collections.Generic;
using System.Text;

namespace jubilant.Packages
{
    class CreateGame : Package
    {
        private string gameName;
        private int maxPlayers;

        public CreateGame(string name, int players) : base(PackageType.CreateGame)
        {
            gameName = name;
            maxPlayers = players;
        }

        public override string GetContent()
        {
            return $"{gameName},{maxPlayers}";
        }
    }
}
