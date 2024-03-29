﻿using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongServer.Commands
{
    class InputCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, List<PlayerConnection> playerConnections, int timeStep, Ball ball)
        {
            var UUID = inc.ReadInt64();
            uint sequence = inc.ReadUInt32();
            var key = (Keys)inc.ReadByte();
            Console.WriteLine("Receive Input request information " + UUID + " " + sequence + " " + key);

            PlayerConnection playerConnection = playerConnections.FirstOrDefault(p => p.UUID == UUID);
            if (playerConnection == null)
            {
                Console.WriteLine("Could not find player with name {0}", UUID);
                return;
            }

            playerConnection.Sequence = sequence;

            float x = 0;
            float y = 0;

            switch (key)
            {
                case Keys.Left:
                    x-= 10;
                    break;

                case Keys.Right:
                    x += 10;
                    break;

                case Keys.Down:
                    y += 10;
                    break;

                case Keys.Up:
                    y -= 10;
                    break;
            }

            if (!ManagerCollision.CheckCollision(playerConnection, x, y, playerConnections.ToList<Player>(), World.WorldWidth, World.WorldHeight))
            {
                playerConnection.X += x;
                playerConnection.Y += y;
            }
        }
    }
}
