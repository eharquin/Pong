using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongServer.Commands.Old
{
    class InputCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence)
        {
            var UUID = inc.ReadInt64();
            sequence = inc.ReadUInt32();
            var key = (Keys)inc.ReadByte();
            Console.WriteLine("Receive Input request information " + UUID + " " + sequence + " " + key);

            player = players.FirstOrDefault(p => p.UUID == UUID);
            if (player == null)
            {
                Console.WriteLine("Could not find player with name {0}", UUID);
                return;
            }

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

            if (!ManagerCollision.CheckCollision(player, x, y, players, World.WorldWidth, World.WorldHeight))
            {
                player.X += x;
                player.Y += y;
            }

            var command = new UpdatePositionCommand();
            command.Run(server, inc, player, players, sequence);
        }
    }
}
