using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PongLibrary;
using Pong.Manager.Graphics;
using Pong.Manager.Input;
using Pong.Manager.Network;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public enum GameState
    {
        MainMenu,
        Connecting,
        Connected
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;
        private ShapeManager shapeManager;
        private SpriteManager spriteManager;
        private TextManager textManager;
        private InputManager inputManager;
        private NetworkManager networkManager;

        public GameState State;

        private Texture2D caseButton;
        private SpriteFont spriteFont;

        // test ball
        private Pong pong;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.Services.AddService(graphics);

            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.Window.AllowUserResizing = true;
            this.Window.Title = "Pong";
        }

        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
            this.graphics.ApplyChanges();

            this.screenManager = new ScreenManager(this, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            this.Services.AddService(this.screenManager);

            this.networkManager = new NetworkManager(this);
            this.Services.AddService(this.networkManager);
            this.Components.Add(this.networkManager);

            this.inputManager = new InputManager(this);
            this.Services.AddService(this.inputManager);
            this.Components.Add(this.inputManager);

            this.shapeManager = new ShapeManager(this);
            this.Services.AddService(this.shapeManager);

            this.spriteManager = new SpriteManager(this);
            this.Services.AddService(this.spriteManager);

            this.textManager = new TextManager(this);
            this.Services.AddService(this.textManager);

            // test ball
            this.pong = new Pong(this);
            this.Components.Add(this.pong);

            this.State = GameState.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.caseButton = Content.Load<Texture2D>("case");
            this.spriteFont = Content.Load<SpriteFont>("default");
        }

        protected override void Update(GameTime gameTime)
        {
            this.ReadInput();

            base.Update(gameTime);
        }

        private void ReadInput()
        {
            if (this.inputManager.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (this.inputManager.isKeyPress(Keys.F11))
            {
                this.graphics.ToggleFullScreen();
                this.graphics.ApplyChanges();
            }

            if (this.State == GameState.Connected)
            {
                Keys input = 0;

                if (this.inputManager.IsKeyDown(Keys.Right))
                    input = Keys.Right;

                if (this.inputManager.IsKeyDown(Keys.Left))
                    input = Keys.Left;

                if (this.inputManager.IsKeyDown(Keys.Down))
                    input = Keys.Down;

                if (this.inputManager.IsKeyDown(Keys.Up))
                    input = Keys.Up;

                if (input != 0)
                    this.networkManager.SendInput(input);

                if (this.inputManager.IsLeftClic(new Rectangle(1100, 32, 32, 32)))
                {
                    this.networkManager.Prediction = !this.networkManager.Prediction;
                }
                if (this.inputManager.IsLeftClic(new Rectangle(1100, 96, 32, 32)))
                {
                    this.networkManager.Reconciliation = !this.networkManager.Reconciliation;
                }
                if (this.inputManager.IsLeftClic(new Rectangle(1100, 160, 32, 32)))
                {
                    this.networkManager.Interpolation = !this.networkManager.Interpolation;
                }
            }

            if(this.State == GameState.MainMenu)
            {
                if(this.inputManager.isKeyPress(Keys.Space))
                {
                    if(this.networkManager.Connect("192.168.1.173", 14241))
                    {
                        this.State = GameState.Connected;

                        this.pong.Enabled = false;
                        this.pong.Visible = false;
                    }
                }

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screenManager.Set();

            if (this.State == GameState.MainMenu)
            {
                this.GraphicsDevice.Clear(Color.PaleGoldenrod);

                this.pong.Draw(gameTime);
            }

            if (this.State == GameState.Connected)
            {
                this.GraphicsDevice.Clear(Color.MediumPurple);

                this.spriteManager.Begin(null);

                if(this.networkManager.Prediction)
                    this.spriteManager.Draw(this.caseButton, new Rectangle(0, 32, 32, 32), Vector2.Zero, new Vector2(1100, 32), 0f, new Vector2(1, 1), Color.White);
                else
                    this.spriteManager.Draw(this.caseButton, new Rectangle(0, 0, 32, 32), Vector2.Zero, new Vector2(1100, 32), 0f, new Vector2(1, 1), Color.White);

                if (this.networkManager.Reconciliation)
                    this.spriteManager.Draw(this.caseButton, new Rectangle(0, 32, 32, 32), Vector2.Zero, new Vector2(1100, 96), 0f, new Vector2(1, 1), Color.White);
                else
                    this.spriteManager.Draw(this.caseButton, new Rectangle(0, 0, 32, 32), Vector2.Zero, new Vector2(1100, 96), 0f, new Vector2(1, 1), Color.White); 
                
                if (this.networkManager.Interpolation)
                    this.spriteManager.Draw(this.caseButton, new Rectangle(0, 32, 32, 32), Vector2.Zero, new Vector2(1100, 160), 0f, new Vector2(1, 1), Color.White);
                else
                    this.spriteManager.Draw(this.caseButton, new Rectangle(0, 0, 32, 32), Vector2.Zero, new Vector2(1100, 160), 0f, new Vector2(1, 1), Color.White);

                this.spriteManager.End();

                this.networkManager.Pong.Draw(gameTime);

                this.textManager.Begin(null);

                this.textManager.Draw(this.spriteFont, "Prediction", new Vector2(1134, 40), Color.Black);
                this.textManager.Draw(this.spriteFont, "Reconciliation", new Vector2(1134, 104), Color.Black);
                this.textManager.Draw(this.spriteFont, "Interpolation", new Vector2(1134, 168), Color.Black);

                this.textManager.End();

                this.shapeManager.Begin(null);

                foreach (Player p in this.networkManager.Others)
                {
                    this.shapeManager.DrawPlayer(p.X, p.Y, 20, 120, Color.Tan);
                }

                this.shapeManager.DrawPlayer(this.networkManager.Player.X, this.networkManager.Player.Y, 20, 120, Color.Black);

                this.shapeManager.End();
            }
            this.screenManager.UnSet();
            this.screenManager.Present(this.spriteManager);

            base.Draw(gameTime);
        }
    }
}
