using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace PongLibrary
{
    public class Player
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public NetConnection Connection { get; set; }


        public Player()
        {

        }
    }
}
