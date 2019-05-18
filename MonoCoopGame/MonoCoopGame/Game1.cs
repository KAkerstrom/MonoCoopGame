using System;
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

        Viewport gameView;
        Viewport playerGuiView;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 900;

            playerGuiView = new Viewport(0, 0, 900, 100);
            gameView = new Viewport(0, 100, 900, 500);
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

            TileMap map = new TileMap(60, 40);
            gameState = new GameState(map, new System.Collections.Generic.List<Character>());
            camera = new Camera(gameView, 30 * 16, 20 * 16);
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
            {
                int xCenter = 0, yCenter = 0;
                foreach (Character p in gameState.Characters)
                    if (p is Player)
                    {
                        xCenter += p.Pos.X;
                        yCenter += p.Pos.Y;
                    }
                xCenter /= gameState.Characters.Count;
                yCenter /= gameState.Characters.Count;
                camera.SetCenter(xCenter, yCenter);

                int xDistance = 0, yDistance = 0;
                foreach (Character p in gameState.Characters)
                    if (p is Player)
                    {
                        if (Math.Abs(p.Pos.X - xCenter) > xDistance)
                            xDistance = Math.Abs(p.Pos.X - xCenter);
                        if (Math.Abs(p.Pos.Y - yCenter) > yDistance)
                            yDistance = Math.Abs(p.Pos.Y - yCenter);
                    }
                float xRatio = xDistance / (gameView.Width / 2f);
                float yRatio = yDistance / (gameView.Height / 2f);
                float zoom = 0.6f / MathHelper.Max(xRatio, yRatio);
                camera.SetZoom(Math.Max(0.8f, Math.Min(3f, zoom)));
            }

            if (GamePad.GetState(0).Buttons.Start == ButtonState.Pressed)
            {
                if (graphics.IsFullScreen)
                {
                    playerGuiView = new Viewport(0, 0, 900, 100);
                    gameView = new Viewport(0, 100, 900, 500);
                    graphics.PreferredBackBufferHeight = 600;
                    graphics.PreferredBackBufferWidth = 900;
                    graphics.IsFullScreen = false;
                    camera.SetZoom(2f);
                }
                else
                {
                    gameView = new Viewport(0, 100, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height - 100);
                    playerGuiView = new Viewport(0, 0, GraphicsDevice.DisplayMode.Width, 100);
                    graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                    graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    graphics.IsFullScreen = true;
                    camera.SetZoom(3f);
                }
                camera.SetView(gameView);
                graphics.ApplyChanges();
            }
            camera.MoveTowardDestination();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(99, 197, 207));
            graphics.GraphicsDevice.Viewport = gameView;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            gameState.Draw(spriteBatch);
            spriteBatch.End();

            graphics.GraphicsDevice.Viewport = playerGuiView;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            gameState.DrawGUI(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
