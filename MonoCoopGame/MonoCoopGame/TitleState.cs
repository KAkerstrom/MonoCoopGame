using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.UI;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class TitleState : State
    {
        private Menu menu;
        private List<IController> controllers;
        private CharacterSelectState selectState;

        public TitleState(GraphicsDevice graphics) : base(graphics)
        {
            controllers = PlayerManager.GetConnectedControllers();
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

            selectState = new CharacterSelectState(graphics);
        }

        private void PlayItem_MenuItemActivated(MenuItem item)
        {
            CurrentState = selectState;
            foreach (IController controller in controllers)
                controller.Update();
        }

        private void SettingsItem_MenuItemActivated(MenuItem item)
        {

        }

        private void AboutItem_MenuItemActivated(MenuItem item)
        {

        }

        private void ExitItem_MenuItemActivated(MenuItem item)
        {
            CurrentState = new ExitState(graphics, null, this);
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
            foreach (IController controller in controllers)
            {
                controller.Update();
                if (controller.ButtonPressed(Buttons.A)
                    || controller.ButtonPressed(Buttons.Start))
                    menu.ActivateItem();
                if (controller.LeftStickMoved(Directions.North)
                    || controller.ButtonPressed(Buttons.DPadUp))
                    menu.DecrementIndex(false);
                if (controller.LeftStickMoved(Directions.South)
                    || controller.ButtonPressed(Buttons.DPadDown))
                    menu.IncrementIndex(false);
            }
        }
    }
}
