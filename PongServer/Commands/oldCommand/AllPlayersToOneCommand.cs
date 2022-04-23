using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;

namespace PongServer.Commands.Old
{
    class AllPlayersToOneCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            var outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.AllPlayers);
            outmsg.Write(players.Count);

            foreach (var p in players)
            {
                outmsg.WriteAllProperties(p);
            }

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
