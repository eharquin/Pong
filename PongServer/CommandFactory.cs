using PongServer.Commands;
using System;

namespace PongServer
{
    static class CommandFactory
    {
        public static ICommand GetCommand(PacketType packetType)
        {
            switch (packetType)
            {
                case PacketType.Login:
                    return new LoginCommand();

                case PacketType.Input:
                    return new InputCommand();

                default:
                    throw new ArgumentOutOfRangeException("packetType");
            }
        }
    }
}
