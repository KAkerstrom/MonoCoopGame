using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.Powerups;
using monoCoopGame.Tiles;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class GameState : State
    {
        public List<IEntity> Entities = new List<IEntity>();
        public List<Player> Players;
        public TileMap Map;

        private Camera camera;
        private Viewport gameView;
        private Viewport playerGuiView;
        private PlayerManager playerManager;

        public GameState(GraphicsDevice graphics, TileMap map, List<Player> players) : base(graphics)
        {
            Map = map;
            Players = players;
            playerManager = new PlayerManager();
            playerManager.PlayerConnected += PlayerManager_PlayerConnected;

            playerGuiView = new Viewport(0, 0, 900, 100);
            gameView = new Viewport(0, 100, 900, 500);
            camera = new Camera(gameView, 20 * Tile.TILE_SIZE, 12 * Tile.TILE_SIZE, 0.65f);
        }

        private void PlayerManager_PlayerConnected(int playerIndex)
        {
            Player testPlayer = new Player(playerIndex, playerIndex, playerIndex, 20 * Tile.TILE_SIZE, 12 * Tile.TILE_SIZE);
            Players.Add(testPlayer);
        }

        public override void Step()
        {
            if (Utility.R.Next(50) == 0)
            {
                Point randomPoint = new Point(Utility.R.Next(0, Map.GridWidth), Utility.R.Next(0, Map.GridHeight));
                IEntity radiusPower = new BombRadiusPowerup(randomPoint);
                AddEntity(radiusPower);
            }

            foreach (Player p in Players)
            {
                p.Step(this);
                if (p.Controller.ButtonPressed(Buttons.Start))
                    Pause(p.Controller);
            }
            for (int i = Entities.Count - 1; i >= 0; i--)
                Entities[i].Step(this);
            Map.Step(this);

            if (false) // Remove fullscreen until I can get it working
            {
                if (IsFullScreen)
                {
                    IsFullScreen = false;
                    playerGuiView = new Viewport(0, 0, 900, 100);
                    gameView = new Viewport(0, 100, 900, 500);
                }
                else
                {
                    IsFullScreen = true;
                    gameView = new Viewport(0, 100, graphics.DisplayMode.Width, graphics.DisplayMode.Height - 100);
                    playerGuiView = new Viewport(0, 0, graphics.DisplayMode.Width, 100);
                }
                camera.SetView(gameView);
            }
            UpdateCamera();
            camera.MoveTowardDestination();
        }

        private void Pause(Controller controller)
        {
            int w, h;
            w = graphics.PresentationParameters.BackBufferWidth;
            h = graphics.PresentationParameters.BackBufferHeight;
            RenderTarget2D screenshot;
            screenshot = new RenderTarget2D(graphics, w, h, false, SurfaceFormat.Bgra32, DepthFormat.None);
            graphics.SetRenderTarget(screenshot);
            graphics.Clear(new Color(99, 197, 207));
            graphics.Viewport = gameView;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            DrawGame(spriteBatch);
            spriteBatch.End();
            graphics.SetRenderTarget(null);
            Texture2D pauseBackground = new Texture2D(graphics, screenshot.Width, screenshot.Height);
            Color[] texdata = new Color[screenshot.Width * screenshot.Height];
            screenshot.GetData(texdata);
            pauseBackground.SetData(texdata);

            CurrentState = new PauseState(graphics, controller, this, pauseBackground);
        }

        public override void Draw()
        {
            graphics.Clear(new Color(99, 197, 207));

            graphics.Viewport = gameView;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            DrawGame(spriteBatch);
            spriteBatch.End();

            graphics.Viewport = playerGuiView;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            DrawGUI(spriteBatch);
            spriteBatch.End();
        }

        private void DrawGame(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
            foreach (Player p in Players)
                p.Draw(spriteBatch);
            foreach (IEntity e in Entities)
                e.Draw(spriteBatch);
        }

        private void DrawGUI(SpriteBatch spriteBatch)
        {
            foreach (Character character in Players)
                if (character is Player)
                    ((Player)character).DrawGUI(spriteBatch);
        }

        private void UpdateCamera()
        {
            if (Players.Count > 0)
            {
                List<Point> playerPositions = new List<Point>();
                foreach (Player p in Players)
                    playerPositions.Add(p.Hitbox.Center);
                camera.UpdateGameCamera(playerPositions);
            }
        }

        public void AddEntity(IEntity entity)
        {
            Entities.Add(entity);
            entity.EntityDestroyed += EntityDestroyed;
        }

        private void EntityDestroyed(IEntity entity)
        {
            Entities.Remove(entity);
        }
    }
}
