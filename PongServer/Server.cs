using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using PongLibrary;
using PongServer.Commands;
using PongServer.Manager;
using PongServer.Util;

namespace PongServer
{
    public class Server
    {
        private NetServer server;
        private List<Player> players;

        private ManagerLogger managerLogger;

        public Server(ManagerLogger managerLogger)
        {
            players = new List<Player>();
            this.managerLogger = new ManagerLogger(LogCategory.Debug);
            var config = new NetPeerConfiguration("Pong");
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set applIdentifier to \"Pong\"");

            config.Port = 14241;
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set server port to 14241");

            config.SimulatedMinimumLatency = 0.200f;
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set server SimulatedMinimumLatency to 200ms");

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Enable message type NetIncomingMessageType.ConnectionApproval");


            server = new NetServer(config);
        }


        public void Run()
        {
            server.Start();
            managerLogger.AddLogMessage(LogCategory.Info, "Server", "Server started...");
            

            while (true)
            {

                NetIncomingMessage inc = server.ReadMessage();

                if (inc == null) continue;

                managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Message receive: " + inc.MessageType.ToString());

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Error: " + inc.ReadString());
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        managerLogger.AddLogMessage(LogCategory.Debug, "Server", "DebugMessage: " + inc.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        StatusChangedMessage(inc);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        var connectionApproval = new ConnectionApprovalCommand();
                        connectionApproval.Run(server, inc, null, null, 0);
                        break;
                    case NetIncomingMessageType.Data:
                        DataMessage(inc);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Warning: " + inc.ReadString());
                        break;
                }
            }
        }

        private void StatusChangedMessage(NetIncomingMessage inc)
        {
            var status = (NetConnectionStatus)inc.ReadByte();
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "NetConnectionStatus: " + status.ToString());

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
                    managerLogger.AddLogMessage(LogCategory.Debug, "Server", inc.SenderConnection + " is connected. Waiting for login info...");
                    break;
                case NetConnectionStatus.Disconnecting:
                    var command1 = new DisconnectCommand();
                    command1.Run(server, inc, null, players, 0);
                    break;
                case NetConnectionStatus.Disconnected:
                    var command = new DisconnectCommand();
                    command.Run(server, inc, null, players, 0);
                    break;
            }
        }


        private void DataMessage(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            var command = CommandFactory.GetCommand(packetType);
            command.Run(server, inc, null, players, 0);
        }
    }
}