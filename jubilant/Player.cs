using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace jubilant
{
    public class Player
    {
        public Role role { get; set; }
        public int money { get; set; }
        public string username { get; set; }
        public Game game { get; set; }
        public int id { get; set; }

        public Player()
        {

        }

        public Player(int id, Role role, int money, string username, Game game)
        {
            this.id = id;
            this.role = role;
            this.money = money;
            this.username = username;
            this.game = game;
        }
    }
}
