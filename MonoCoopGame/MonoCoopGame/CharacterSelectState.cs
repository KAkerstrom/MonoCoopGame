using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.UI;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class CharacterSelectState : State
    {
        private CharacterSelectMenu[] menus = new CharacterSelectMenu[4];
        private List<Controller> controllers = new List<Controller>();
        private PlayerManager playerManager;
        private GameState gameState;
        private int gameStartTimer = 0;

        public CharacterSelectState(GraphicsDevice graphics) : base(graphics)
        {
            Point menuSize = new Point(graphics.PresentationParameters.Bounds.Width / 2, graphics.PresentationParameters.Bounds.Height / 2);
            for (int i = 0; i < 4; i++)
            {
                Rectangle bounds = new Rectangle
                    (
                        (i % 2) * menuSize.X,
                        (i / 2) * menuSize.Y,
                        menuSize.X,
                        menuSize.Y
                    );
                menus[i] = new CharacterSelectMenu(i, bounds);
            }

            playerManager = new PlayerManager();
            playerManager.PlayerConnected += PlayerConnected;

            TileMap map = new TileMap(40, 24);
            gameState = new GameState(graphics, map, new List<Player>());
        }

        private void PlayerConnected(int playerIndex)
        {
            controllers.Add(new Controller(playerIndex));
        }

        private bool AllPlayersReady()
        {
            bool atLeastOneActive = false;
            foreach (CharacterSelectMenu menu in menus)
                if (menu.Active)
                {
                    if (menu.Ready)
                        atLeastOneActive = true;
                    else
                        return false;
                }
            return atLeastOneActive;
        }

        public override void Draw()
        {
            graphics.Clear(new Color(99, 197, 207));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            foreach (CharacterSelectMenu menu in menus)
                menu.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Step()
        {
            if (AllPlayersReady())
            {
                List<Player> players = new List<Player>();
                foreach (CharacterSelectMenu menu in menus)
                    if (menu.Active)
                        players.Add(menu.CreatePlayer());
                gameState.Players = players;
                CurrentState = gameState;
            }

            foreach (Controller c in controllers)
            {
                bool unassigned = true;
                foreach (CharacterSelectMenu menu in menus)
                    if (menu.Active && menu.controllerIndex == c.PlayerIndex)
                        unassigned = false;

                if (unassigned)
                {
                    c.Update();
                    if (c.ButtonPressed(Buttons.Start))
                        for (int i = 0; i < 4; i++)
                            if (!menus[i].Active)
                            {
                                menus[i].Activate(c.PlayerIndex);
                                break;
                            }
                }
            }

            foreach (CharacterSelectMenu menu in menus)
                menu.Step();
        }
    }
}
