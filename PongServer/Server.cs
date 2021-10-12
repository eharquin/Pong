using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using PongLibrary;
using PongServer.Commands;

namespace PongServer
{
    public class Server
    {
        private NetServer _server;
        private List<Player> _players;

        private int _updatePerSecond;

        public Server()
        {
            _players = new List<Player>();

            var config = new NetPeerConfiguration("Pong");
            config.Port = 14241;
            config.SimulatedMinimumLatency = 0.200f;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(config);
        }


        public void Run()
        {
            _server.Start();

            Console.WriteLine("Server started..." + Environment.NewLine);


            while (true)
            {

                NetIncomingMessage inc = _server.ReadMessage();

                if (inc == null) continue;

                Console.WriteLine("Message receive: " + inc.MessageType.ToString());

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        Console.WriteLine(inc.ReadString());
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(inc.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        StatusChangedMessage(inc);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        var connectionApproval = new ConnectionApprovalCommand();
                        connectionApproval.Run(_server, inc, null, null, 0);
                        break;
                    case NetIncomingMessageType.Data:
                        DataMessage(inc);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(inc.ReadString());
                        break;
                }

                Console.WriteLine(Environment.NewLine);
            }
        }

        private void StatusChangedMessage(NetIncomingMessage inc)
        {
            var status = (NetConnectionStatus)inc.ReadByte();
            Console.WriteLine(status.ToString());
            switch (status)
            {
                case NetConnectionStatus.None:
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    Console.WriteLine(inc.SenderConnection + " is connected. Waiting for login info...");
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    break;
            }
        }


        private void DataMessage(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            var command = CommandFactory.GetCommand(packetType);
            command.Run(_server, inc, null, _players, 0);
        }
    }
}