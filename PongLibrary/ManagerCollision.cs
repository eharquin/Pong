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

            var rec = new Rectangle((int)(nextPos.X - World.PlayerWidth/2), (int)(nextPos.Y- World.PlayerHeight / 2), (int)World.PlayerWidth, (int)World.PlayerHeight);

            foreach (var p in players)
            {
                if (p.UUID != player.UUID)
                {
                    if (rec.Intersects(new Rectangle((int)(p.X - World.PlayerWidth / 2), (int)(p.Y - World.PlayerHeight / 2), (int)World.PlayerWidth, (int)World.PlayerHeight)))
                    {
                        return true;
                    }
                }
            }

            var map = new Rectangle(0, 0, width, height);


            if (!map.Contains(rec))
            {
                return true;
            }


            return false;
        }
    }
}
