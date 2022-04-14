using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongServer.Commands
{
    public class DisconnectCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            var connection = server.Connections.FirstOrDefault(c => c == inc.SenderConnection) ;
            if (connection == null)
            {
                Console.WriteLine("Could not find player with name {0}", inc.SenderConnection);
                return;
            }

            player = players.FirstOrDefault(p => p.UUID == connection.RemoteUniqueIdentifier);
            if (player == null)
            {
                Console.WriteLine("Could not find player with name {0}", inc.SenderConnection);
                return;
            }


            players.Remove(player);
            Console.WriteLine("Remove player from the list");

            var command = new DisconnectToAllCommand();
            command.Run(server, inc, player, players, 0);
        }
    }
}
