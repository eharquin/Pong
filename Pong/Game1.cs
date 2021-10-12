using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using PongLibrary;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private NetworkManager _networkManager;

        private Texture2D _circle;
        private SpriteFont _spriteFont;


        private bool ConnectionResult;

        private Color _color;

        private KeyboardState _lastKeyboradState;

        public Game1(string[] args)
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            int lag = 0;
            bool prediction = true;
            bool reconciliation = true;
            bool interpolation = false;

            _color = Color.MediumPurple;

            if (args.Length == 4)
            {
                lag = int.Parse(args[0]);
                prediction = bool.Parse(args[1]);
                reconciliation = bool.Parse(args[1]);
                interpolation = bool.Parse(args[1]);

                _color = Color.Purple;
            }

            _networkManager = new NetworkManager(lag, prediction, reconciliation, interpolation);

            _lastKeyboradState = Keyboard.GetState();

            
        }

        protected override void Initialize()
        {
            ConnectionResult = _networkManager.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            _circle = createCircleText(100);

            _spriteFont = Content.Load<SpriteFont>("default");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Keys input = 0;

            if (_lastKeyboradState.IsKeyDown(Keys.Right))
                input = Keys.Right;

            if (_lastKeyboradState.IsKeyDown(Keys.Left))
                input = Keys.Left;

            if (_lastKeyboradState.IsKeyDown(Keys.Down))
                input = Keys.Down;

            if (_lastKeyboradState.IsKeyDown(Keys.Up)/* && Keyboard.GetState().IsKeyUp(Keys.Up)*/)
                input = Keys.Up;

            if (input != 0)
            {
                _networkManager.SendInput(input);
                _networkManager.SendInput(input);
            }

            _networkManager.Update(gameTime);

            _lastKeyboradState = Keyboard.GetState();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            if (!ConnectionResult)
            {
                GraphicsDevice.Clear(Color.PaleGoldenrod);
                return;
            }

            GraphicsDevice.Clear(_color);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_circle, new Vector2(_networkManager.Player.X, _networkManager.Player.Y), Color.White);
            _spriteBatch.DrawString(_spriteFont, _networkManager.Player.Name, new Vector2(_networkManager.Player.X, _networkManager.Player.Y+50), Color.Black);

            foreach (var player in _networkManager.Others)
            {
                _spriteBatch.Draw(_circle, new Vector2(player.X, player.Y), Color.LightBlue);
                _spriteBatch.DrawString(_spriteFont, player.Name, new Vector2(player.X, player.Y+50), Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        Texture2D createCircleText(int radius)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
