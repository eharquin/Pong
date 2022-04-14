using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongServer.Commands
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

            Console.WriteLine(ManagerCollision.CheckCollision(player, x, y, players, 800, 480));


            if (!ManagerCollision.CheckCollision(player, x, y, players, 800, 480))
            {
                player.X += x;
                player.Y += y;
            }

            Console.WriteLine("Send the update player to all");

            var command = new UpdatePositionCommand();
            command.Run(server, inc, player, players, sequence);
        }
    }
}
