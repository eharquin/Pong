using Microsoft.Xna.Framework;

namespace Pong
{
    public class GameObject : DrawableGameComponent
    {
        public Vector3 Position;

        public GameObject(Game game) : base(game)
        {

        }
    }
}
