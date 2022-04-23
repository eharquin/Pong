using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongServer.Commands
{
    class LoginCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, List<PlayerConnection> playerConnections, int timeStep, Ball ball)
        {
            Player player = CreatePlayer(inc, playerConnections.ToList<Player>());
            playerConnections.Add(new PlayerConnection(player, inc.SenderConnection));

            Console.WriteLine("Send initial data to the new player");
            
            var initialSnapshot = new InitialSnapshotCommand();
            initialSnapshot.Run(server, inc, playerConnections, timeStep, ball);
        }

        private Player CreatePlayer(NetIncomingMessage inc, List<Player> players)
        {
            var random = new Random();
            var player = new Player();
            Console.WriteLine("Receive login information " + player.Name);
            player.Name = inc.ReadString();

            Console.WriteLine("Create new player");

            int X = random.Next(0, 75) * 10;
            int Y = random.Next(0, 43) * 10;

            Console.WriteLine(ManagerCollision.CheckCollision(player, X, Y, players, World.WorldWidth, World.WorldHeight));

            while(ManagerCollision.CheckCollision(player, X, Y, players, World.WorldWidth, World.WorldHeight))
            {
                X = random.Next(0, 75) * 10;
                Y = random.Next(0, 43) * 10;
            }

            player.X = X;
            player.Y = Y;
            player.UUID = inc.SenderConnection.RemoteUniqueIdentifier;

            Console.WriteLine("Initialize " + "[" + player.UUID + "] to position " + player.X + " " + player.Y);

            return player;
        }
    }
}
