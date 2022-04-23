using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Commands
{
    public class SnapshotCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, List<PlayerConnection> playerConnections, int timeStep, Ball ball)
        {
            // TODO: Only send modified data
            
            foreach(PlayerConnection playerConnection in playerConnections)
            {
                var outmsg = server.CreateMessage();
                outmsg.Write((byte)PacketType.Snapshot);
                outmsg.Write(playerConnections.Count);
                foreach (PlayerConnection playerConnectionEmit in playerConnections)
                {
                    outmsg.Write(playerConnectionEmit.UUID);
                    if (playerConnection.UUID == playerConnectionEmit.UUID)
                        outmsg.Write(playerConnection.Sequence);
                    else
                        outmsg.Write(timeStep);
                    outmsg.Write(playerConnectionEmit.X);
                    outmsg.Write(playerConnectionEmit.Y);
                }
                server.SendMessage(outmsg, playerConnection.Connection, NetDeliveryMethod.ReliableOrdered);
            }
        }
    }
}
