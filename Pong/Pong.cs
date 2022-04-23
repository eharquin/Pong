using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Pong.Manager.Graphics;
using PongLibrary;
using System.Collections.Generic;

namespace Pong
{
    public class Pong : DrawableGameComponent
    {

        private ScreenManager screenManager;

        private Texture2D circle;
        private Texture2D border;

        public Vector2 Speed;
        public Vector2 Position;

        private Color pongColor;

        public List<Tuple<Vector2, DateTime>> PositionBuffer = new List<Tuple<Vector2, DateTime>>();

        private static readonly Rectangle wallUp = new Rectangle(0, 0, 1280, 1);
        private static readonly Rectangle wallDown = new Rectangle(0, 720, 1280, 720);
        private static readonly Rectangle wallLeft = new Rectangle(1280, 0, 1280, 720);
        private static readonly Rectangle wallRight = new Rectangle(0, 0, 1, 720);

        private float radius;


        public Pong(Game game) : base(game)
        {
            this.screenManager = game.Services.GetService<ScreenManager>();
            this.pongColor = Color.Black;

            this.Speed = new Vector2(1, 1);
            this.Position = new Vector2(640, 360);
            this.radius = 50;

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.circle = DrawHelpers.CreateCircleTexture((int)this.radius, this.Game.GraphicsDevice);
            this.border = DrawHelpers.CreateCircleTexture((int)this.radius, this.Game.GraphicsDevice);
        }

        
        public override void Update(GameTime gameTime)
        {
            double time = gameTime.TotalGameTime.TotalSeconds;
            float r = (float)Math.Sin(time) / 2 + .5f;
            float g = (float)Math.Sin(time + 2) / 2 + .5f;
            float b = (float)Math.Sin(time + 4) / 2 + .5f;

            this.pongColor = new Color(r, g, b);


            if (wallUp.Intersects(new Rectangle(this.Position.ToPoint(), new Point((int)this.radius))))
            {
                this.Speed.Y = -this.Speed.Y;
            }

            if (wallDown.Intersects(new Rectangle(this.Position.ToPoint(), new Point((int)this.radius))))
            {
                this.Speed.Y = -this.Speed.Y;
            }

            if (wallLeft.Intersects(new Rectangle(this.Position.ToPoint(), new Point((int)this.radius))))
            {
                this.Speed.X = -this.Speed.X;
            }

            if (wallRight.Intersects(new Rectangle(this.Position.ToPoint(), new Point((int)this.radius))))
            {
                this.Speed.X = -this.Speed.X;
            }


            this.Position = this.Position + this.Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10;
        }
        

        public override void Draw(GameTime gameTime)
        {

            SpriteManager spriteManager = this.Game.Services.GetService<SpriteManager>();
            spriteManager.Begin(null);

            spriteManager.Draw(this.border, Vector2.Zero, new Vector2(this.Position.X, this.Position.Y), Color.Black);
            spriteManager.Draw(this.circle, Vector2.Zero, new Vector2(this.Position.X, this.Position.Y), this.pongColor);
            
            spriteManager.End();
        }
    }

}
