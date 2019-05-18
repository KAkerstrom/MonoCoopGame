using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monoCoopGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gameState;
        PlayerManager playerManager;
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 900;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //graphics.ToggleFullScreen();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Sprite.LoadSprites(this.Content, "img");
            Utility.Fonts.Add("playerGUI", Content.Load<SpriteFont>("playerGUI"));

            TileMap map = new TileMap(50, 50);
            gameState = new GameState(map, new System.Collections.Generic.List<Character>());
            camera = new Camera(GraphicsDevice.Viewport, 0, 0);
            playerManager = new PlayerManager();
            playerManager.PlayerConnected += PlayerManager_PlayerConnected;
        }

        private void PlayerManager_PlayerConnected(int playerIndex)
        {
            Player testPlayer = new Player(playerIndex, 250, 250, 1);
            gameState.Characters.Add(testPlayer);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameState.Step();

            if (gameState.Characters.Count > 0)
                camera.SetCenter(gameState.Characters[0].Pos.X, gameState.Characters[0].Pos.Y);

            if (GamePad.GetState(0).Buttons.Start == ButtonState.Pressed)
            {
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferHeight = 600;
                    graphics.PreferredBackBufferWidth = 900;
                    graphics.IsFullScreen = false;
                    camera.SetZoom(2f);
                }
                else
                {
                    graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                    graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    graphics.IsFullScreen = true;
                    camera.SetZoom(3f);
                }
                camera.SetView(GraphicsDevice.Viewport);
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(99, 197, 207));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            gameState.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            gameState.DrawGUI(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
