using System;
using System.Collections.Generic;
using System.Text;

namespace PongLibrary
{
    public class World
    {
        public static int WorldWidth = 1280;
        public static int WorldHeight = 720;

        public static float PlayerWidth = 20;
        public static float PlayerHeight = 120;

        public List<Player> Players { get; set; }

        public World()
        {
            this.Players = new List<Player>();
        }
    }
}
