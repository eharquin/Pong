using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong.Input
{
    public class KeyboardInput : GameComponent
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState lastKeyboradState;

        public KeyboardInput(Game game) : base(game)
        {
            this.currentKeyboardState = Keyboard.GetState();
            this.lastKeyboradState = this.currentKeyboardState;
        }

        public bool IsKeyDown(Keys key)
        {
            return this.currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return this.currentKeyboardState.IsKeyUp(key);
        }

        public bool isKeyPress(Keys key)
        {
            return this.currentKeyboardState.IsKeyDown(key) && this.lastKeyboradState.IsKeyUp(key);
        }

        public override void Update(GameTime gameTime)
        {
            this.lastKeyboradState = this.currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();
        }
    }
}
