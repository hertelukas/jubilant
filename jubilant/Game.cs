using System;
using System.Collections.Generic;
using System.Text;

namespace jubilant
{
    public class Game
    {
        public List<Player> players { get; set; } = new List<Player>();
        public int maxPlayers { get; set; }
        public string name { get; set; }
        public int adminId { get; set; }

        public int id { get; set; }
        public Game(string name, int maxPlayers, int id, int adminId)
        {
            this.name = name;
            this.maxPlayers = maxPlayers;
            this.id = id;
            this.adminId = adminId;
        }
    }
}
