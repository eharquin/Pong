using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Manager.Graphics
{
    public sealed class ScreenManager : IDisposable, IServiceProvider
    {
        private static readonly int minWidth = 64;
        private static readonly int maxWidth = 4096;
        private static readonly int minHeight = 64;
        private static readonly int maxHeight = 4096;

        private Game game;
        private RenderTarget2D renderTarget;

        private bool isDisposed;
        private bool isSet;

        public int Width
        {
            get { return this.renderTarget.Width; }
        }

        public int Height
        {
            get { return this.renderTarget.Height; }
        }


        public ScreenManager(Game game, int width, int height)
        {
            this.game = game ?? throw new ArgumentNullException("game");

            width = MathHelper.Clamp(width, ScreenManager.minWidth, ScreenManager.maxWidth);
            height = MathHelper.Clamp(height, ScreenManager.minHeight, ScreenManager.maxHeight);

            this.renderTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);

        }

        public void Set()
        {
            if(this.isSet)
            {
                throw new InvalidOperationException("Render target is already set.");
            }
            this.game.GraphicsDevice.SetRenderTarget(renderTarget);
            this.isSet = true;
        }

        public void UnSet()
        {
            if (!this.isSet)
            {
                throw new InvalidOperationException("Render target is not set.");
            }

            this.game.GraphicsDevice.SetRenderTarget(null);
            this.isSet = false;
        }

        public void Present(SpriteManager spriteManager)
        {
            if (spriteManager is null)
            {
                throw new ArgumentNullException("spriteManager");
            }

            this.game.GraphicsDevice.Clear(Color.Black);

            Rectangle destinationRectangle = GetDestinationRectangle();

            spriteManager.Begin(null);
            spriteManager.Draw(this.renderTarget, null, destinationRectangle, Color.White);
            spriteManager.End();
        }

        public Rectangle GetDestinationRectangle()
        {
            Rectangle backBufferBounds = this.game.GraphicsDevice.PresentationParameters.Bounds;
            float backBufferAspectRatio = (float)backBufferBounds.Width / backBufferBounds.Height;
            float renderTargetAspectRatio = (float)this.Width / this.Height;

            float x = 0f;
            float y = 0f;
            float width = backBufferBounds.Width;
            float height = backBufferBounds.Height;

            if (renderTargetAspectRatio < backBufferAspectRatio)
            {
                width = height * renderTargetAspectRatio;
                x = (backBufferBounds.Width - width) / 2f;
            }
            else if (renderTargetAspectRatio > backBufferAspectRatio)
            {
                height = width / renderTargetAspectRatio;
                y = (backBufferBounds.Height - height) / 2f;
            }

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public void Dispose()
        {
            if(this.isDisposed)
            {
                return;
            }

            this.renderTarget?.Dispose();
            this.isDisposed = true;
        }

        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
