using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Manager.Network;
using PongLibrary;
using System.Diagnostics;

namespace Pong
{
    public class PlayerStick : DrawableGameComponent
    {
        private SpriteBatch spritebatch;
        private NetworkManager networkManager;
        private BasicEffect basicEffect;

        private float width;
        private float height;

        private static Color color = Color.Black;
        private SpriteFont spriteFont;

        public PlayerStick(Game game) : base(game)
        {
            this.spritebatch = game.Services.GetService<SpriteBatch>();
            this.networkManager = game.Services.GetService<NetworkManager>();

            this.width = World.PlayerWidth;
            this.height = World.PlayerHeight;

            this.basicEffect = new BasicEffect(this.GraphicsDevice);
            this.basicEffect.TextureEnabled = true;
            this.basicEffect.VertexColorEnabled = true;
            this.basicEffect.LightingEnabled = false;
            this.basicEffect.FogEnabled = false;

            basicEffect.World = Matrix.Identity;
            basicEffect.View = Matrix.Identity;
            Viewport viewport = game.GraphicsDevice.Viewport;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewport.Width, viewport.Height, 0f, 0f, 1f);

            //Debug.WriteLine(this.Position);

        }

        protected override void LoadContent()
        {
            this.spriteFont = this.Game.Content.Load<SpriteFont>("default");
        }

        public override void Update(GameTime gameTime)
        {
           // this.Position = new Vector3(this.networkManager.Player.X, this.networkManager.Player.Y, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            // Vertex
            Vector3 v1 = new Vector3(this.Position.X - width / 2, this.Position.Y - height / 2, 0);
            Vector3 v2 = new Vector3(this.Position.X + width / 2, this.Position.Y - height / 2, 0);
            Vector3 v3 = new Vector3(this.Position.X + width / 2, this.Position.Y + height / 2, 0);
            Vector3 v4 = new Vector3(this.Position.X - width / 2, this.Position.Y + height / 2, 0);


            int[] indexData = { 0, 1, 2, 0, 2, 3};

            VertexPositionColor[] vertexData = { new VertexPositionColor(v1, color),
                                                 new VertexPositionColor(v2, color),
                                                 new VertexPositionColor(v3, color),
                                                 new VertexPositionColor(v4, color)};


            // These three lines are required if you use SpriteBatch, to reset the states that it sets
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            foreach (EffectPass effect in this.basicEffect.CurrentTechnique.Passes)
            {
                effect.Apply();
                this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertexData, 0, shapeVertexCount, indexData, 0, shapeIndexCount / 3);
            }
            */
        }
    }
}
