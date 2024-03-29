﻿using Lidgren.Network;
using PongLibrary;
using System;
using System.Collections.Generic;

namespace PongServer.Commands
{
    class ConnectionApproveCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, List<PlayerConnection> playerConnections, int timeStep, Ball ball)
        {
            var data = inc.ReadByte();
            if (data == (byte)PacketType.Connect)
            {
                Console.WriteLine("Connection accepted...");
                Console.WriteLine("Connected to " + inc.SenderConnection.ToString());
                inc.SenderConnection.Approve();
            }
            else
            {
                inc.SenderConnection.Deny("Didn't send correct information.");
                Console.WriteLine("Connection denied.");
            }
        }
    }
}
