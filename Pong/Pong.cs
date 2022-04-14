using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;

namespace Pong
{
    public class Pong : GameObject
    {

        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private NetworkManager networkManager;
        private Texture2D circle;
        private Texture2D border;
        private SpriteFont spriteFont;

        private Color pongColor;

        public Pong(Game game) : base(game)
        {
            this.graphicsDevice = game.Services.GetService<GraphicsDeviceManager>().GraphicsDevice;
            this.content = game.Content;
            this.spriteBatch = game.Services.GetService<SpriteBatch>();
            this.networkManager = game.Services.GetService<NetworkManager>();
            this.pongColor = Color.Black;

        }

        protected override void LoadContent()
        {
            this.circle = DrawHelpers.CreateCircleTexture(100, this.graphicsDevice);
            this.border = DrawHelpers.CreateCircleTexture(108, this.graphicsDevice);

            this.spriteFont = this.content.Load<SpriteFont>("default");
        }

        
        public override void Update(GameTime gameTime)
        {
            double time = gameTime.TotalGameTime.TotalSeconds;
            float r = (float)Math.Sin(time) / 2 + .5f;
            float g = (float)Math.Sin(time + 2) / 2 + .5f;
            float b = (float)Math.Sin(time + 4) / 2 + .5f;

            this.pongColor = new Color(r, g, b);
        }
        

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(this.border, new Vector2(this.networkManager.Player.X - 4, this.networkManager.Player.Y - 4), Color.Black);
            spriteBatch.Draw(this.circle, new Vector2(this.networkManager.Player.X, this.networkManager.Player.Y), this.pongColor);

            foreach (var player in networkManager.Others)
            {
                spriteBatch.Draw(circle, new Vector2(player.X, player.Y), Color.LightBlue);
                spriteBatch.DrawString(spriteFont, player.Name, new Vector2(player.X, player.Y + 50), Color.Black);
            }

            spriteBatch.End();
        }
    }

}
