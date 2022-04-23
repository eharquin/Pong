using Lidgren.Network;
using Microsoft.Xna.Framework;
using PongLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Pong.Manager.Network
{
    public class NetworkManager : GameComponent, IServiceProvider
    {
        private NetClient client;
        public Player Player { get; set; }
        public List<Player> Others { get; set; }

        public Pong Pong { get; set; }

        public bool Prediction { get; set; }
        public bool Reconciliation { get; set; }
        public bool Interpolation { get; set; }

        public int Lag { get; set; }

        private uint sequence;

        private readonly List<(uint sequence, Keys input)> pendinginputs;

        public NetworkManager(Game game) : base(game)
        {
            this.Prediction = true;
            this.Reconciliation = true;
            this.Interpolation = true;
            this.Lag = 0;

            this.Player = new Player();
            this.Others = new List<Player>();

            this.Pong = new Pong(game);
            this.Pong.Initialize();

            this.sequence = 0;
            this.pendinginputs = new List<(uint sequence, Keys input)>();

            var config = new NetPeerConfiguration("Pong");
            config.SimulatedMinimumLatency = 0.200f;
            config.SimulatedRandomLatency = (float) this.Lag/1000;
            this.client = new NetClient(config);
        }

        public bool Connect(String host, int port)
        {
            this.client.Start();

            var outmsg = this.client.CreateMessage();
            outmsg.Write((byte)PacketType.Connect);
            this.client.Connect(host, port, outmsg);

            return EstablishInfo();
        }

        public bool EstablishInfo()
        {
            var time = DateTime.Now;

            var delay = new TimeSpan(0, 0, 5);

            while (time + delay > DateTime.Now)
            {
                var inc = this.client.ReadMessage();

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
                                this.Player.Name = "Pong" + DateTime.Now.Millisecond;
                                this.Player.UUID = this.client.UniqueIdentifier;

                                var outmsg = this.client.CreateMessage();
                                outmsg.Write((byte)PacketType.Login);
                                outmsg.Write(this.Player.Name);
                                this.client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

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
            if (this.client.ConnectionStatus == NetConnectionStatus.Connected && Interpolation)
            {
                foreach (Player player in this.Others)
                    Interpolate(player);
                Interpolate(this.Pong);
            }
            NetIncomingMessage inc;

            while ((inc = this.client.ReadMessage()) != null)
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
                            case PacketType.InitialSnapshot:
                                this.InitialSnapshot(inc);
                                break;
                            case PacketType.Snapshot:
                                this.Snapshot(inc);
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

        // Snapshot Message 
        // TerrainWidth TerrainHeignt UUID P_Count posX posY  UUID_P1 posY_P1 posY_P1
        private void InitialSnapshot(NetIncomingMessage inc)
        {
            int playerCount = inc.ReadInt32();
            for(int i=0; i<playerCount; i++)
            {
                long UUID = inc.ReadInt64();
                if (this.Player.UUID == UUID)
                {
                    this.Player.X = inc.ReadFloat();
                    this.Player.Y = inc.ReadFloat();
                }
                else
                {
                    // TODO: receive timeStep for interpolate the entity
                    Player player = new Player();
                    player.UUID = UUID;
                    int timeStep = inc.ReadInt32();
                    float X = inc.ReadFloat();
                    float Y = inc.ReadFloat();
                    player.PositionBuffer.Add(new Tuple<Vector2, DateTime>(new Vector2(X, Y), DateTime.Now));
                    player.X = X;
                    player.Y = Y;
                    this.Others.Add(player);
                }
            }
            float ballX = inc.ReadFloat();
            float ballY = inc.ReadFloat();

            this.Pong.PositionBuffer.Add(new Tuple<Vector2, DateTime>(new Vector2(ballX, ballY), DateTime.Now));
            this.Pong.Position = new Vector2(ballX, ballY);

            float speedX = inc.ReadFloat();
            float speedY = inc.ReadFloat();

            this.Pong.Speed = new Vector2(speedX, speedY);

        }

        private void Snapshot(NetIncomingMessage inc)
        {
            int playerCount = inc.ReadInt32();
            for (int i = 0; i < playerCount; i++)
            {
                long UUID = inc.ReadInt64();
                if (this.Player.UUID == UUID)
                {
                    uint seq = inc.ReadUInt32();
                    this.Player.X = inc.ReadFloat();
                    this.Player.Y = inc.ReadFloat();
                    if (this.Reconciliation)
                        ReconcilInput(seq);
                    else
                        this.pendinginputs.Clear();
                }
                else if (this.Others.FirstOrDefault(p => p.UUID == UUID) != null)
                {
                    Player player = this.Others.FirstOrDefault(p => p.UUID == UUID);
                    //TODO : receive timestep and save into a data struct
                    int timeStep = inc.ReadInt32();
                    float X = inc.ReadFloat();
                    float Y = inc.ReadFloat();

                    if (Interpolation)
                    {
                        player.PositionBuffer.Add(new Tuple<Vector2, DateTime>(new Vector2(X, Y), DateTime.Now));
                        //Interpolate(player);
                    }
                    else
                    {
                        player.X = X;
                        player.Y = Y;
                    }
                }
                else
                {
                    Player player = new Player();
                    player.UUID = UUID;
                    int timeStep = inc.ReadInt32();
                    float X = inc.ReadFloat();
                    float Y = inc.ReadFloat();
                    player.PositionBuffer.Add(new Tuple<Vector2, DateTime>(new Vector2(X, Y), DateTime.Now));
                    player.X = X;
                    player.Y = Y;
                    this.Others.Add(player);
                }
            }

            float ballX = inc.ReadFloat();
            float ballY = inc.ReadFloat();

            if (Interpolation)
            {
                this.Pong.PositionBuffer.Add(new Tuple<Vector2, DateTime>(new Vector2(ballX, ballY), DateTime.Now));
            }
            else
            {
                this.Pong.Position = new Vector2(ballX, ballY);
            }


            //this.Pong.Position = new Vector2(ballX, ballY);

            float speedX = inc.ReadFloat();
            float speedY = inc.ReadFloat();

            this.Pong.Speed = new Vector2(speedX, speedY);
        }

        private void Interpolate(Player player)
        {

            DateTime now = DateTime.Now;
            DateTime render_timestamp = now - new TimeSpan(0, 0, 0, 0, 100);

            var buffer = player.PositionBuffer;

            while (buffer.Count >= 2 && buffer[1].Item2 <= render_timestamp)
            {
                buffer.RemoveAt(0);
            }

            // Interpolate between the two surrounding authoritative positions.
            if (buffer.Count >= 2 && buffer[0].Item2 <= render_timestamp && render_timestamp <= buffer[1].Item2)
            {
                var pos0 = buffer[0].Item1;
                var pos1 = buffer[1].Item1;
                var t0 = buffer[0].Item2;
                var t1 = buffer[1].Item2;

                player.X = (float)(pos0.X + (pos1.X - pos0.X) * (render_timestamp - t0).TotalMilliseconds / (t1 - t0).TotalMilliseconds);
                player.Y = (float)(pos0.Y + (pos1.Y - pos0.Y) * (render_timestamp - t0).TotalMilliseconds / (t1 - t0).TotalMilliseconds);

            }
        }

        private void Interpolate(Pong pong)
        {

            DateTime now = DateTime.Now;
            DateTime render_timestamp = now - new TimeSpan(0, 0, 0, 0, 100);

            var buffer = pong.PositionBuffer;

            while (buffer.Count >= 2 && buffer[1].Item2 <= render_timestamp)
            {
                buffer.RemoveAt(0);
            }

            // Interpolate between the two surrounding authoritative positions.
            if (buffer.Count >= 2 && buffer[0].Item2 <= render_timestamp && render_timestamp <= buffer[1].Item2)
            {
                var pos0 = buffer[0].Item1;
                var pos1 = buffer[1].Item1;
                var t0 = buffer[0].Item2;
                var t1 = buffer[1].Item2;

                pong.Position.X = (float)(pos0.X + (pos1.X - pos0.X) * (render_timestamp - t0).TotalMilliseconds / (t1 - t0).TotalMilliseconds);
                pong.Position.Y = (float)(pos0.Y + (pos1.Y - pos0.Y) * (render_timestamp - t0).TotalMilliseconds / (t1 - t0).TotalMilliseconds);

            }
        }

        public void SendInput(Keys input)
        {
            var pos = PredictInput(input);

            if (!ManagerCollision.CheckCollision(Player, pos.x, pos.y, Others, World.WorldWidth, World.WorldHeight))
            {
                if (Prediction)
                {
                    Player.X += pos.x;
                    Player.Y += pos.y;
                }

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


        private void ReconcilInput(uint seq)
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
        
        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
