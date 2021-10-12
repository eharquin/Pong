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
            var name = inc.ReadString();
            sequence = inc.ReadUInt32();
            var key = (Keys)inc.ReadByte();
            Console.WriteLine("Receive Input request information " + name + " " + sequence + " " + key);

            player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
            {
                Console.WriteLine("Could not find player with name {0}", name);
                return;
            }

            switch (key)
            {
                case Keys.Left:
                    player.X-=10;
                    break;

                case Keys.Right:
                    player.X+=10;
                    break;

                case Keys.Down:
                    player.Y+=10;
                    break;

                case Keys.Up:
                    player.Y-=10;
                    break;
            }

            Console.WriteLine("Send the update player to all");

            var command = new UpdatePositionCommand();
            command.Run(server, inc, player, players, sequence);
        }
    }
}
