using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Pong.Manager.Graphics
{
    public sealed class SpriteManager : IDisposable, IServiceProvider
    {
        private SpriteBatch spriteBatch;
        private Game game;
        private bool isDisposed;
        private BasicEffect effect;

        public SpriteManager(Game game)
        {
            this.game = game ?? throw new ArgumentNullException("game");

            this.spriteBatch = new SpriteBatch(this.game.GraphicsDevice);

            this.isDisposed = false;

            this.effect = new BasicEffect(this.game.GraphicsDevice);
            this.effect.TextureEnabled = true;
            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = false;
            this.effect.FogEnabled = false;
            this.effect.World = Matrix.Identity;
            this.effect.Projection = Matrix.Identity;
            this.effect.View = Matrix.Identity;
        }
#nullable enable
        public void Begin(CameraManager? camera)
        {
            if (camera is null)
            {
                Viewport viewport = game.GraphicsDevice.Viewport;
                //this.effect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewport.Width, 0f, viewport.Height, 0, 1);
                this.effect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewport.Width, viewport.Height, 0f, 0f, 1f);
                this.effect.View = Matrix.Identity;
            }
            else
            {
                camera.UpdateMatrices();
                this.effect.Projection = camera.Projection;
                this.effect.View = camera.View;
            }

            this.spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, effect: this.effect);
        }
#nullable disable
        public void End()
        {
            this.spriteBatch.End();
        }

        public void Draw(Texture2D texture, Vector2 origin, Vector2 position, Color color)
        {
            this.spriteBatch.Draw(texture, position, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            this.spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Rectangle destinationRectangle, Color color)
        {
            this.spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Dispose()
        {
            if(this.isDisposed)
            {
                return;
            }

            this.effect?.Dispose();
            this.spriteBatch?.Dispose();
            this.isDisposed = true;
        }

        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
