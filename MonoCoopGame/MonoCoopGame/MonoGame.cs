using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.Tiles;

namespace monoCoopGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonoGame : Game
    {
        GraphicsDeviceManager graphics;
        State state;

        public MonoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Mono Coop Game";

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
            // TODO: use this.Content to load your game content here
            Sprite.LoadSprites(Content, "img");
            Utility.Fonts.Add("playerGUI", Content.Load<SpriteFont>("playerGUI"));
            Utility.Fonts.Add("blocks", Content.Load<SpriteFont>("Fonts/blocks"));
            Utility.Fonts.Add("blocks_small", Content.Load<SpriteFont>("Fonts/blocks_small"));
            Utility.Fonts.Add("quantityFont", Content.Load<SpriteFont>("Fonts/quantityFont"));

            state = new TitleState(GraphicsDevice);
            State.CurrentState = state;
            State.FullScreenChanged += State_FullScreenChanged;
            State.GameQuit += State_GameQuit;
            State.StateChanged += State_StateChanged;
        }

        private void State_StateChanged(State newState)
        {
            state = newState;
        }

        private void State_GameQuit()
        {
            Exit();
        }

        private void State_FullScreenChanged(bool fullscreen)
        {
            if (State.IsFullScreen)
            {
                graphics.PreferredBackBufferHeight = 600;
                graphics.PreferredBackBufferWidth = 900;
                graphics.IsFullScreen = true;
            }
            else
            {
                graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphics.IsFullScreen = true;
            }
            graphics.ApplyChanges();
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

            state.Step();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            state.Draw();
            base.Draw(gameTime);
        }
    }
}
