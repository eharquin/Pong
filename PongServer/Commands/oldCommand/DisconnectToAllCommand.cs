using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Commands.Old
{
    class DisconnectToAllCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            var outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.DisconnectPlayer);
            outmsg.Write(player.UUID);

            server.SendToAll(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
