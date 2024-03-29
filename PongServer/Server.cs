﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Lidgren.Network;
using PongLibrary;
using PongServer.Commands;
using PongServer.Manager;
using PongServer.Util;


namespace PongServer
{
    public class Server
    {
        private NetServer server;

        private List<PlayerConnection> playerConnections;
        private Ball ball;

        private static readonly Rectangle wallUp = new Rectangle(0, 0, 1280, 1);
        private static readonly Rectangle wallDown = new Rectangle(0, 720, 1280, 720);
        private static readonly Rectangle wallLeft = new Rectangle(1280, 0, 1280, 720);
        private static readonly Rectangle wallRight = new Rectangle(0, 0, 1, 720);

        private ManagerLogger managerLogger;
        private int timeStep;

        public Server(ManagerLogger managerLogger)
        {
            this.playerConnections = new List<PlayerConnection>();

            this.ball = new Ball();
            this.ball.X = 640;
            this.ball.Y = 360;

            this.ball.SpeedX = 5;
            this.ball.SpeedY = 5;

            this.managerLogger = new ManagerLogger(LogCategory.Debug);

            this.timeStep = 0;
            
            var config = new NetPeerConfiguration("Pong");
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set appIdentifier to \"Pong\"");

            config.Port = 14241;
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set server port to 14241");

            config.SimulatedMinimumLatency = 0.200f;
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set server SimulatedMinimumLatency to 200ms");

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Enable message type NetIncomingMessageType.ConnectionApproval");

            config.MaximumConnections = 2;
            managerLogger.AddLogMessage(LogCategory.Debug, "Server", "Set maximun connections accepted to 2");

            server = new NetServer(config);
        }

        
        public void Run()
        {
            server.Start();
            managerLogger.AddLogMessage(LogCategory.Info, "Server", "Server started...");

            DateTime timeSnapshot = DateTime.Now;
            DateTime timeBallUpdate = DateTime.Now;
            TimeSpan delaySnapshot = new TimeSpan(0, 0, 0, 0, 100);
            TimeSpan delayBallUpdate = new TimeSpan(0, 0, 0, 0, 10);

            while (true)
            {

                if (timeSnapshot + delaySnapshot <= DateTime.Now)
                {
                    var snapshotCommand = new SnapshotCommand();
                    snapshotCommand.Run(server, null, playerConnections, timeStep, ball);

                    timeSnapshot = DateTime.Now;
                    this.timeStep++;
                }

                if(timeBallUpdate + delayBallUpdate <= DateTime.Now)
                {
                    int res = Ball.CheckWallCollision((int)this.ball.X, (int)this.ball.Y, (int)Ball.radius);

                    switch (res)
                    {
                        case 0:
                            break;
                        case 1:
                            this.ball.SpeedY = -this.ball.SpeedY;
                            break;
                        case 2:
                            this.ball.SpeedY = -this.ball.SpeedY;
                            break;
                        case 3:
                            this.ball.SpeedX = -this.ball.SpeedX;
                            break;
                        case 4:
                            this.ball.SpeedX = -this.ball.SpeedX;
                            break;
                    }

                    if(Ball.CheckPlayerCollision((int)this.ball.X, (int)this.ball.Y, (int)Ball.radius, this.playerConnections.ToList<Player>()))
                    {
                        this.ball.SpeedX = -this.ball.SpeedX;
                    }


                    this.ball.X += this.ball.SpeedX;
                    this.ball.Y += this.ball.SpeedY;

                    timeBallUpdate = DateTime.Now;
                }
                
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
                        var connectionApprove = new ConnectionApproveCommand();
                        connectionApprove.Run(server, inc, playerConnections, timeStep, ball);
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
                    //var command1 = new DisconnectCommand();
                    //command1.Run(server, inc, null, players, 0);
                    break;
                case NetConnectionStatus.Disconnected:
                    //var command = new DisconnectCommand();
                    //command.Run(server, inc, null, players, 0);
                    break;
            }
        }

        private void DataMessage(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            var command = CommandFactory.GetCommand(packetType);
            command.Run(server, inc, playerConnections, timeStep, this.ball);
        }
    }
}