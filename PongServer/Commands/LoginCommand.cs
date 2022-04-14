using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;

namespace PongServer.Commands
{
    class LoginCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            player = CreatePlayer(inc, players);

            Console.WriteLine("Send initial data to the new player");
            var command1 = new InitialDataCommand();
            command1.Run(server, inc, player, players, sequence);

            var command2 = new PlayerToAllCommand();
            command2.Run(server, inc, player, players, sequence);
        }

        private Player CreatePlayer(NetIncomingMessage inc, List<Player> players)
        {
            var random = new Random();
            var player = new Player();
            Console.WriteLine("Receive login information " + player.Name);
            player.Name = inc.ReadString();

            Console.WriteLine("Create new player");

            //player.X = random.Next(0, 75) * 10;
            //player.Y = random.Next(0, 43) * 10;

            player.X = 600;
            player.Y = 200;

            player.UUID = inc.SenderConnection.RemoteUniqueIdentifier;

            Console.WriteLine("Initialize " + "[" + player.UUID + "] to position " + player.X + " " + player.Y);
            players.Add(player);

            return player;
        }
    }
}
