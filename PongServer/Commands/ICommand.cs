using Lidgren.Network;
using PongLibrary;
using System.Collections.Generic;

namespace PongServer.Commands
{
    interface ICommand
    {
        void Run(NetServer server, NetIncomingMessage inc, Player player, List<Player> players, uint sequence);
    }
}
