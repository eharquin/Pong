using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;

namespace PongServer.Commands
{
    class PlayerToAllCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            var outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.Player);
            outmsg.WriteAllProperties(player);

            server.SendToAll(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            Console.WriteLine("Send to all exept "+ player.Name +  "the player update ");
        }
    }
}
