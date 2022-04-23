using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Commands.Old
{
    class UpdatePositionCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            // TODO: On modifie l'état du world 

            var outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.PlayerPositionUpdate);
            outmsg.Write(sequence);
            outmsg.Write(player.X);
            outmsg.Write(player.Y);


            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);

            var command = new PlayerToAllCommand();
            command.Run(server, inc, player, players, sequence);

            Console.WriteLine("Send pos: " + player.X + " " + player.Y);
        }
    }
}
