using Lidgren.Network;
using Microsoft.Xna.Framework;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class NetworkManager : GameComponent, IServiceProvider
    {
        private NetClient client;

        public Player Player { get; set; }
        public List<Player> Others { get; set; }

        public int Lag {get; set;}
        public bool Prediction { get; set; }
        public bool Reconciliation { get; set; }
        public bool Interpolation { get; set; }

        private uint sequence;

        private readonly List<(uint sequence, Keys input)> pendinginputs;

        public NetworkManager(Game game, int lag, bool prediction, bool reconciliation, bool interpolation) : base(game)
        {
            var config = new NetPeerConfiguration("Pong");
            config.SimulatedMinimumLatency = 0.200f;
            config.SimulatedRandomLatency = (float) lag/1000;
            client = new NetClient(config);

            Prediction = prediction;
            Reconciliation = reconciliation;
            Interpolation = interpolation;

            Player = new Player();
            Others = new List<Player>();

            sequence = 0;
            pendinginputs = new List<(uint sequence, Keys input)>();

        }

        public bool Start()
        {
            client.Start();

            var outmsg = client.CreateMessage();

            outmsg.Write((byte)PacketType.Connect);

            client.Connect("localhost", 14241, outmsg);

            return EstablishInfo();
        }

        public bool EstablishInfo()
        {
            var time = DateTime.Now;

            var delay = new TimeSpan(0, 0, 5);

            while (time + delay > DateTime.Now)
            {
                var inc = client.ReadMessage();

                if (inc == null) continue;

                Debug.WriteLine(inc.MessageType);

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)inc.ReadByte();
                        Debug.WriteLine(status.ToString());
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
                                var outmsg = client.CreateMessage();

                                Player.Name = "Pong" + DateTime.Now.Millisecond;
                                Player.UUID = client.UniqueIdentifier;

                                outmsg.Write((byte)PacketType.Login);
                                outmsg.Write(Player.Name);


                                client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
                                return true;

                            case NetConnectionStatus.Disconnecting:
                                break;
                            case NetConnectionStatus.Disconnected:
                                break;
                        }
                        break;
                }
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            NetIncomingMessage inc;

            while ((inc = client.ReadMessage()) != null)
            {
                Debug.WriteLine(inc.MessageType);

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)inc.ReadByte();
                        Debug.WriteLine(status.ToString());
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        break;
                    case NetIncomingMessageType.Data:
                        
                        switch ((PacketType)inc.ReadByte())
                        {
                            case PacketType.InitialData:
                                InitialData(inc);
                                break;

                            case PacketType.Player:
                                ReadPLayer(inc);
                                break;

                            case PacketType.AllPlayers:
                                ReadAllPlayer(inc);
                                break;

                            case PacketType.PlayerPositionUpdate:
                                PlayerPositionUpdate(inc);
                                break;

                            case PacketType.DisconnectPlayer:
                                DisconnectPlayer(inc);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                }

                Debug.WriteLine(Environment.NewLine);
            }
        }

        private void DisconnectPlayer(NetIncomingMessage inc)
        {

            var UUID = inc.ReadInt64();

            var player = Others.FirstOrDefault(p => p.UUID == UUID);
            if (player == null)
            {
                Console.WriteLine("Could not find player with name {0}", inc.SenderConnection);
                return;
            }

            var b = Others.Remove(player);

        }

        public void Disconnect()
        {
            client.Disconnect("ciao");
        }

        private void PlayerPositionUpdate(NetIncomingMessage inc)
        {
            var seq = inc.ReadUInt32();

            float x = inc.ReadFloat();
            float y = inc.ReadFloat();

            Debug.WriteLine("Receive position update : sequence=" + seq + " x=" + x + " y=" + y);

            // Received the authoritative position of the client
            Player.X = x;
            Player.Y = y;


            if(Reconciliation)
            {
                // server reconciliation Reapply all the inputs not yet processed by the server
                var j = 0;
                while (j < pendinginputs.Count)
                {
                    var input = pendinginputs[j];

                    if (input.sequence <= seq)
                    {
                        // Already processed. Its effect is already taken in account into the world update we just go, we can drop it
                        pendinginputs.RemoveAt(j);
                    }
                    else
                    {
                        // Not processed by the server yet. Reapply it
                        switch (input.input)
                        {
                            case Keys.Left:
                                Player.X -= 10;
                                break;

                            case Keys.Right:
                                Player.X += 10;
                                break;

                            case Keys.Down:
                                Player.Y += 10;
                                break;

                            case Keys.Up:
                                Player.Y -= 10;
                                break;
                        }

                        j++;

                    }
                }
            }
            else
            {
                pendinginputs.Clear();
            }
        }

        private void InitialData(NetIncomingMessage inc)
        {
            Player.X = inc.ReadFloat();
            Player.Y = inc.ReadFloat();

            ReadAllPlayer(inc);

            Debug.WriteLine(client.UniqueIdentifier);
        }

        private void ReadPLayer(NetIncomingMessage inc)
        {
            var player = new Player();
            inc.ReadAllProperties(player);

            Debug.WriteLine(player.UUID +"     " + Player.UUID);


            if (player.UUID == Player.UUID)
            {
                Debug.WriteLine("receive self");
            }
            else if (Others.Any(p => p.UUID == player.UUID))
            {
                var oldPlayer = Others.FirstOrDefault(p => p.UUID == player.UUID);
                oldPlayer.X = player.X;
                oldPlayer.Y = player.Y;
            }
            else
            {
                Others.Add(player);
            }
        }
        private void ReadAllPlayer(NetIncomingMessage inc)
        {
            int nb = inc.ReadInt32();
            Debug.WriteLine("Receive player list " + nb);
            for (int i = 0; i < nb; i++)
            {
                ReadPLayer(inc);
            }
        }

        public void SendInput(Keys input)
        {

            if (Prediction)
            {
                var pos = PredictInput(input);

                Console.WriteLine("Collision : " + ManagerCollision.CheckCollision(Player, pos.x, pos.y, Others, 800, 480));


                if (!ManagerCollision.CheckCollision(Player, pos.x, pos.y, Others, 800, 480))
                {
                    Player.X += pos.x;
                    Player.Y += pos.y;

                    pendinginputs.Add((sequence, input));

                    var outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketType.Input);
                    outmsg.Write(Player.UUID);
                    outmsg.Write(sequence);
                    outmsg.Write((byte)input);

                    client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

                    ++sequence;
                }
            }

            else
            {
                var pos = PredictInput(input);

                if (!ManagerCollision.CheckCollision(Player, pos.x, pos.y, Others, 800, 480))
                {
                    pendinginputs.Add((sequence, input));

                    var outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketType.Input);
                    outmsg.Write(Player.UUID);
                    outmsg.Write(sequence);
                    outmsg.Write((byte)input);

                    client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

                    ++sequence;
                }
            }
        }

        private (float x, float y) PredictInput(Keys input)
        {
            float x = 0f, y = 0f;

            switch (input)
            {
                case Keys.Left:
                    x -= 10;
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
            return (x, y);
        }

        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
