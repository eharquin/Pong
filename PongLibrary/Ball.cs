using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongLibrary
{
    public class Ball
    {
        public float SpeedX;
        public float SpeedY;

        public float X;
        public float Y;

        public List<Tuple<Vector2, DateTime>> PositionBuffer = new List<Tuple<Vector2, DateTime>>();

        public static readonly float radius = 50f;

        private static readonly Rectangle wallUp = new Rectangle(0, 0, 1280, 1);
        private static readonly Rectangle wallDown = new Rectangle(0, 720, 1280, 720);
        private static readonly Rectangle wallLeft = new Rectangle(1280, 0, 1280, 720);
        private static readonly Rectangle wallRight = new Rectangle(0, 0, 1, 720);

        public static int CheckWallCollision(int X, int Y, int radius)
        {
            if (wallUp.Intersects(new Rectangle(new Point(X, Y), new Point(radius))))
            {
                return 1;
            }

            if (wallDown.Intersects(new Rectangle(new Point(X, Y), new Point(radius))))
            {
                return 2;
            }

            if (wallLeft.Intersects(new Rectangle(new Point(X, Y), new Point(radius))))
            {
                return 3;
            }

            if (wallRight.Intersects(new Rectangle(new Point(X, Y), new Point(radius))))
            {
                return 4;
            }

            return 0;
        }

        public static bool CheckPlayerCollision(int x, int y, int radius, List<Player> players)
        {
            Rectangle ballRectangle = new Rectangle(new Point(x, y), new Point(radius));
            foreach (Player player in players)
            {
                var rec = new Rectangle((int)(player.X - World.PlayerWidth / 2), (int)(player.Y - World.PlayerHeight / 2), (int)World.PlayerWidth, (int)World.PlayerHeight);
                if (rec.Intersects(ballRectangle))
                    return true;
            }
            return false;
        }
    }
}
