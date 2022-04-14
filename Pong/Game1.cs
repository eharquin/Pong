using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Input;
using PongLibrary;
using System;
using System.Diagnostics;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private NetworkManager networkManager;

        private bool connectionResult;

        private Color backgroundColor;

        private KeyboardInput keyboardInput;

        private PlayerStick player;


        public Game1(string[] args)
        {
            Debug.WriteLine("test");
            Debug.Flush();
            this.graphics = new GraphicsDeviceManager(this);
            this.Services.AddService(graphics);

            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            int lag = 0;
            bool prediction = true;
            bool reconciliation = true;
            bool interpolation = false;

            this.backgroundColor = Color.MediumPurple;

            if (args != null && args.Length == 4)
            {
                lag = int.Parse(args[0]);
                prediction = bool.Parse(args[1]);
                reconciliation = bool.Parse(args[1]);
                interpolation = bool.Parse(args[1]);

                this.backgroundColor = Color.Purple;
            }

            this.networkManager = new NetworkManager(this, lag, prediction, reconciliation, interpolation);
            this.Services.AddService(this.networkManager);
            this.Components.Add(this.networkManager);
        }

        protected override void Initialize()
        {

            this.connectionResult = this.networkManager.Start();

            this.Window.Title = this.networkManager.Player.Name;

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(spriteBatch);

            this.keyboardInput = new KeyboardInput(this);
            this.Services.AddService(keyboardInput);
            this.Components.Add(keyboardInput);

            this.player = new PlayerStick(this);
            this.Components.Add(player);

            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

            if (this.keyboardInput.IsKeyDown(Keys.Escape))
            {
                networkManager.Disconnect();
                Exit();
            }

            Keys input = 0;

            if (this.keyboardInput.IsKeyDown(Keys.Right))
                input = Keys.Right;

            if (this.keyboardInput.IsKeyDown(Keys.Left))
                input = Keys.Left;

            if (this.keyboardInput.IsKeyDown(Keys.Down))
                input = Keys.Down;

            if (this.keyboardInput.IsKeyDown(Keys.Up))
                input = Keys.Up;

            if (input != 0)
            {
                networkManager.SendInput(input);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!this.connectionResult)
            {
                this.GraphicsDevice.Clear(Color.PaleGoldenrod);
                return;
            }

            this.GraphicsDevice.Clear(backgroundColor);

            foreach(Player p in this.networkManager.Others)
            {
                PlayerStick player = new PlayerStick(this);
                player.Position = new Vector3(p.X, p.Y, 0);
                player.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
