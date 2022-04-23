using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Pong.Manager.Graphics
{
    public sealed class TextManager : IDisposable
    {
        private SpriteBatch spriteBatch;
        private Game game;
        private bool isDisposed;
        private BasicEffect effect;

        public TextManager(Game game)
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

            this.spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp, rasterizerState: RasterizerState.CullNone, effect: this.effect);
        }
#nullable disable
        public void End()
        {
            this.spriteBatch.End();
        }
        
        public void Draw(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            this.spriteBatch.DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
    }
}
