using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Commands
{
    public class InitialSnapshotCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, List<PlayerConnection> playerConnections, int timeStep, Ball ball)
        {
            PlayerConnection newPlayerConnection = ICommand.FindPlayerConnection(inc.SenderConnection, playerConnections);

            var outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.InitialSnapshot);
            outmsg.Write(playerConnections.Count);
            foreach (PlayerConnection playerConnection in playerConnections)
            {
                outmsg.Write(playerConnection.UUID);
                if (newPlayerConnection.UUID != playerConnection.UUID)
                    outmsg.Write(timeStep);
                outmsg.Write(playerConnection.X);
                outmsg.Write(playerConnection.Y);
            }

            outmsg.Write(ball.X);
            outmsg.Write(ball.Y);

            outmsg.Write(ball.SpeedX);
            outmsg.Write(ball.SpeedY);

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
