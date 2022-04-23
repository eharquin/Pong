using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PongLibrary
{
    public class Player
    {
        public long UUID { get; set; }
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Color Color { get; set; }

        public List<Tuple<Vector2, DateTime>> PositionBuffer = new List<Tuple<Vector2, DateTime>>();

        public Player()
        {

        }
    }
}
