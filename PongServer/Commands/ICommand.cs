using Lidgren.Network;
using PongLibrary;
using System.Collections.Generic;

namespace PongServer.Commands
{
    interface ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, List<PlayerConnection> playerConnections, int timeStep, Ball ball);

        public static PlayerConnection FindPlayerConnection(NetConnection connection, List<PlayerConnection> playerConnections)
        {
            PlayerConnection playerConnection = playerConnections.Find(x => x.Connection == connection);
            if (playerConnection != null)
                return playerConnection;

            throw new KeyNotFoundException();
        }
    }
}
