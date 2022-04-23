using Lidgren.Network;
using PongLibrary;

namespace PongServer
{
    public class PlayerConnection : Player
    {
        public NetConnection Connection;
        public uint Sequence;

        public PlayerConnection()
        {

        }

        public PlayerConnection(Player player, NetConnection connection)
        {
            this.X = player.X;
            this.Y = player.Y;
            this.UUID = player.UUID;
            this.Name = player.Name;
            this.Color = player.Color;
            this.Connection = connection;
            this.Sequence = 0;
        }
    }
}
