using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.UI;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class ExitState : State
    {
        private Menu menu;
        private Controller controller;
        private State previousState;

        public ExitState(GraphicsDevice graphics, Controller controller, State previousState) : base(graphics)
        {
            this.previousState = previousState;
            this.controller = controller;
            TitleMenuItem ExitItem = new TitleMenuItem("Yes");
            TitleMenuItem CancelItem = new TitleMenuItem("No");

            List<MenuItem> menuItems = new List<MenuItem>
            {
                ExitItem,
                CancelItem
            };
            int xPos = graphics.PresentationParameters.Bounds.X + 50;
            int yPos = graphics.PresentationParameters.Bounds.Height / 3;
            int xSize = graphics.PresentationParameters.Bounds.Width - 100;
            int ySize = graphics.PresentationParameters.Bounds.Height / 2;
            Rectangle menuBounds = new Rectangle(xPos, yPos, xSize, ySize);
            menu = new Menu(menuBounds, menuItems);

            ExitItem.MenuItemActivated += ExitItem_MenuItemActivated;
            CancelItem.MenuItemActivated += CancelItem_MenuItemActivated;
        }

        private void ExitItem_MenuItemActivated(MenuItem item)
        {
            QuitGame();
        }

        private void CancelItem_MenuItemActivated(MenuItem item)
        {
            CurrentState = previousState;
        }

        public override void Draw()
        {
            graphics.Clear(new Color(99, 197, 207));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            SpriteFont font = Utility.Fonts["blocks"];
            string pauseString = "Are you sure you\nwant to exit?";
            int xPosition = (int)((graphics.PresentationParameters.Bounds.Width - font.MeasureString(pauseString).X) / 2);
            int yPosition = (int)((graphics.PresentationParameters.Bounds.Height / 4 - font.MeasureString(pauseString).Y) / 2);
            Vector2 pauseStringPoint = new Vector2(xPosition, yPosition);
            spriteBatch.DrawString(Utility.Fonts["blocks"], pauseString, pauseStringPoint, Color.Black);

            menu.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Step()
        {
            controller.Update();
            if (controller.ButtonPressed(Buttons.A))
                menu.ActivateItem();
            if (controller.ButtonPressed(Buttons.B))
                CancelItem_MenuItemActivated(null);
            if (controller.State.ThumbSticks.Left.Y > 0.5f
                && controller.PreviousState.ThumbSticks.Left.Y <= 0.5f)
                menu.DecrementIndex(false);
            if (controller.State.ThumbSticks.Left.Y < -0.5f
                && controller.PreviousState.ThumbSticks.Left.Y >= -0.5f)
                menu.IncrementIndex(false);
        }
    }
}
