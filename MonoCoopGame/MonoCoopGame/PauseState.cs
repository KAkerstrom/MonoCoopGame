using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.UI;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class PauseState : State
    {
        private Menu menu;
        private Controller controller;
        private State gameState;
        private Texture2D background;

        public PauseState(GraphicsDevice graphics, Controller controller, State gameState, Texture2D background) : base(graphics)
        {
            this.gameState = gameState;
            this.controller = controller;
            this.background = background;
            TitleMenuItem resumeItem = new TitleMenuItem("Resume");
            TitleMenuItem exitItem = new TitleMenuItem("Exit");

            List<MenuItem> menuItems = new List<MenuItem>
            {
                resumeItem,
                exitItem
            };
            int xPos = graphics.PresentationParameters.Bounds.X + 50;
            int yPos = graphics.PresentationParameters.Bounds.Height / 3;
            int xSize = graphics.PresentationParameters.Bounds.Width - 100;
            int ySize = graphics.PresentationParameters.Bounds.Height / 2;
            Rectangle menuBounds = new Rectangle(xPos, yPos, xSize, ySize);
            menu = new Menu(menuBounds, menuItems);

            resumeItem.MenuItemActivated += ResumeItem_MenuItemActivated;
            exitItem.MenuItemActivated += ExitItem_MenuItemActivated;
        }

        private void ResumeItem_MenuItemActivated(MenuItem item)
        {
            CurrentState = gameState;
        }

        private void ExitItem_MenuItemActivated(MenuItem item)
        {
            QuitGame();
        }

        public override void Draw()
        {
            graphics.Clear(new Color(99, 197, 207));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(background, graphics.PresentationParameters.Bounds, Color.White);

            SpriteFont font = Utility.Fonts["blocks"];
            string pauseString = "GAME PAUSED";
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
            if (controller.ButtonPressed(Buttons.Start))
                ResumeItem_MenuItemActivated(null);
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
