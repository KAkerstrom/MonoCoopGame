using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.UI;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class TitleState : State
    {
        private Menu menu;
        private Controller controller;

        public TitleState(GraphicsDevice graphics) : base(graphics)
        {
            TitleMenuItem playItem = new TitleMenuItem("Play");
            TitleMenuItem settingsItem = new TitleMenuItem("Settings");
            TitleMenuItem aboutItem = new TitleMenuItem("About");
            TitleMenuItem exitItem = new TitleMenuItem("Exit");

            List<MenuItem> menuItems = new List<MenuItem>
            {
                playItem,
                settingsItem,
                aboutItem,
                exitItem
            };
            Rectangle menuBounds = graphics.PresentationParameters.Bounds;
            menuBounds.Inflate(-50, -50);
            menu = new Menu(menuBounds, menuItems);

            playItem.MenuItemActivated += PlayItem_MenuItemActivated;
            settingsItem.MenuItemActivated += SettingsItem_MenuItemActivated;
            aboutItem.MenuItemActivated += AboutItem_MenuItemActivated;
            exitItem.MenuItemActivated += ExitItem_MenuItemActivated;
        }

        private void PlayItem_MenuItemActivated(MenuItem item)
        {
            TileMap map = new TileMap(40, 24);
            CurrentState = new GameState(graphics, map, new List<Player>());
        }

        private void SettingsItem_MenuItemActivated(MenuItem item)
        {

        }

        private void AboutItem_MenuItemActivated(MenuItem item)
        {

        }

        private void ExitItem_MenuItemActivated(MenuItem item)
        {
            QuitGame();
        }

        public override void Draw()
        {
            graphics.Clear(new Color(99, 197, 207));

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            menu.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Step()
        {
            if (controller == null)
            {
                for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
                    if (GamePad.GetState(i).IsConnected)
                        controller = new Controller(i);
                return;
            }
            controller.Update();
            if (controller.ButtonPressed(Buttons.A))
                menu.ActivateItem();
            if (controller.State.ThumbSticks.Left.Y > 0.5f
                && controller.PreviousState.ThumbSticks.Left.Y <= 0.5f)
                menu.DecrementIndex(false);
            if (controller.State.ThumbSticks.Left.Y < -0.5f
                && controller.PreviousState.ThumbSticks.Left.Y >= -0.5f)
                menu.IncrementIndex(false);
        }
    }
}
