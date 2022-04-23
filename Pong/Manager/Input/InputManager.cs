using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pong.Manager.Graphics;
using System;

namespace Pong.Manager.Input
{
    public class InputManager : GameComponent, IServiceProvider
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState lastKeyboradState;

        private MouseState currentMouseState;
        private MouseState lastMouseState;

        private ScreenManager screenManager;


        public InputManager(Game game) : base(game)
        {
            this.currentKeyboardState = Keyboard.GetState();
            this.lastKeyboradState = this.currentKeyboardState;

            this.currentMouseState = Mouse.GetState();
            this.lastMouseState = this.currentMouseState;

            this.screenManager = game.Services.GetService<ScreenManager>();
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

        private bool EnsureInScreen()
        {
            Rectangle screenDestinationRectangle = this.screenManager.GetDestinationRectangle();

            if (screenDestinationRectangle.Contains(this.currentMouseState.X, this.currentMouseState.Y))
                return true;

            return false;
        }

        public bool IsRightClic(Rectangle zone)
        {
            if (this.lastMouseState.RightButton == ButtonState.Pressed && this.currentMouseState.RightButton == ButtonState.Released)
            {
                if (zone.Contains(this.currentMouseState.X, this.currentMouseState.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsLeftClic(Rectangle zone)
        {
            if (this.lastMouseState.LeftButton == ButtonState.Pressed && this.currentMouseState.LeftButton == ButtonState.Released && EnsureInScreen())
            {
                Rectangle screenRectangle = this.screenManager.GetDestinationRectangle();

                float posx = this.currentMouseState.X - screenRectangle.X;
                float posy = this.currentMouseState.Y - screenRectangle.Y;

                posx = posx / ((float)screenRectangle.Width / screenManager.Width);
                posy = posy / ((float)screenRectangle.Height / screenManager.Height);

                if(zone.Contains(posx, posy))
                    return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            this.lastKeyboradState = this.currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();

            this.lastMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();
        }


        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
