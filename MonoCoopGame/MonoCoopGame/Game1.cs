using System;
using System.Collections.Generic;
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
            Sprite.LoadSprites(Content, "img");
            Utility.Fonts.Add("playerGUI", Content.Load<SpriteFont>("playerGUI"));

            TileMap map = new TileMap(40, 24);
            gameState = new GameState(map, new List<Player>());
            camera = new Camera(gameView, 20 * 16, 12 * 16);
            playerManager = new PlayerManager();
            playerManager.PlayerConnected += PlayerManager_PlayerConnected;
        }

        private void PlayerManager_PlayerConnected(int playerIndex)
        {
            Player testPlayer = new Player(playerIndex, 15 * 16, 10 * 16, 1);
            gameState.Players.Add(testPlayer);
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

            if (gameState.Players.Count > 0)
            {
                int xCenter = 0, yCenter = 0;
                foreach (Character p in gameState.Players)
                {
                    xCenter += p.Pos.X;
                    yCenter += p.Pos.Y;
                }
                xCenter /= gameState.Players.Count;
                yCenter /= gameState.Players.Count;
                camera.SetCenter(xCenter, yCenter);

                int xDistance = 0, yDistance = 0;
                foreach (Character p in gameState.Players)
                {
                    if (Math.Abs(p.Hitbox.Center.X - xCenter) > xDistance)
                        xDistance = Math.Abs(p.Pos.X - xCenter);
                    if (Math.Abs(p.Hitbox.Center.Y - yCenter) > yDistance)
                        yDistance = Math.Abs(p.Pos.Y - yCenter);
                }
                float xRatio = xDistance / (gameView.Width / 2f);
                float yRatio = yDistance / (gameView.Height / 2f);
                float zoom = 0.6f / MathHelper.Max(xRatio, yRatio);
                camera.SetZoom(Math.Max(graphics.IsFullScreen ? 1.7f : 1.3f, Math.Min(2.5f, zoom)));
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
                    //camera.SetZoom(1.3f);
                }
                else
                {
                    gameView = new Viewport(0, 100, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height - 100);
                    playerGuiView = new Viewport(0, 0, GraphicsDevice.DisplayMode.Width, 100);
                    graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                    graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    graphics.IsFullScreen = true;
                    //camera.SetZoom(1.7f);
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
