using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PongLibrary
{
    public static class ManagerCollision
    {
        public static bool CheckCollision(Player player, float X, float Y, List<Player> players, int width, int height)
        {

            Vector2 nextPos = new Vector2(player.X + X, player.Y + Y);

            var rec = new Rectangle((int)(nextPos.X - Player.Width/2), (int)(nextPos.Y-Player.Height/2), (int)Player.Width, (int)Player.Height);

            foreach (var p in players)
            {
                if (p.UUID != player.UUID)
                {
                    if (rec.Intersects(new Rectangle((int)(p.X - Player.Width / 2), (int)(p.Y - Player.Height / 2), (int)Player.Width, (int)Player.Height)))
                    {
                        return true;
                    }
                }
            }

            var map = new Rectangle(0, 0, width, height);

            Console.WriteLine("map: " + map.Contains(rec));

            if (!map.Contains(rec))
            {

                Debug.WriteLine(map + " " + rec);
                return true;
            }


            return false;
        }
    }
}
