using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Commands
{
    class InitialDataCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            var outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.InitialData);
            outmsg.Write(player.X);
            outmsg.Write(player.Y);

            outmsg.Write(players.Count);
            foreach (var p in players)
            {
                outmsg.WriteAllProperties(p);
            }

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
