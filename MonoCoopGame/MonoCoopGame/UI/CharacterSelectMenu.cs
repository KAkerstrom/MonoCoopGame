using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame.UI
{
    class CharacterSelectMenu
    {
        public enum MenuStates { PressStart, Selection, Ready }

        public int controllerIndex { get; private set; }
        public MenuStates State = MenuStates.PressStart;
        public Rectangle Bounds;
        public bool Active { get { return State != MenuStates.PressStart; } }

        private string name = "";
        private Inventory buyInventory = new Inventory();
        private Inventory sellInventory = new Inventory();

        private Controller controller;

        public CharacterSelectMenu(Rectangle drawBounds)
        {
            Bounds = drawBounds;
        }

        public void Activate(int controllerIndex)
        {
            State = MenuStates.Selection;
            this.controllerIndex = controllerIndex;
            controller = new Controller(controllerIndex);
        }

        public void Deactivate()
        {
            State = MenuStates.PressStart;
            controllerIndex = 0;
            controller = null;
        }

        public void Step()
        {
            if (Active)
            {
                controller.Update();
                switch (State)
                {
                    case MenuStates.Selection:
                        if (controller.ButtonPressed(Buttons.B))
                            Deactivate();
                        else if (controller.ButtonPressed(Buttons.A))
                            State = MenuStates.Ready;
                        break;

                    case MenuStates.Ready:
                        if (controller.ButtonPressed(Buttons.B))
                            State = MenuStates.Selection;
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = Utility.Fonts["blocks"];
            string phrase;
            Vector2 drawPoint;
            switch (State)
            {
                case MenuStates.PressStart:
                    phrase = "Press\nStart";
                    drawPoint = new Vector2
                        (
                            Bounds.X + (Bounds.Width - (int)font.MeasureString(phrase).X) / 2,
                            Bounds.Y + (Bounds.Height - (int)font.MeasureString(phrase).Y) / 2
                        );
                    spriteBatch.DrawString(font, phrase, drawPoint, Color.Black);
                    break;

                case MenuStates.Selection:
                    DrawSelection(spriteBatch);
                    break;

                case MenuStates.Ready:
                    phrase = "Ready";
                    drawPoint = new Vector2
                        (
                            Bounds.X + (Bounds.Width - (int)font.MeasureString(phrase).X) / 2,
                            Bounds.Y + (Bounds.Height - (int)font.MeasureString(phrase).Y) / 2
                        );
                    spriteBatch.DrawString(font, phrase, drawPoint, Color.Black);
                    break;
            }
        }

        private void DrawSelection(SpriteBatch spriteBatch)
        {
            // Draw Character
            Rectangle drawBounds = new Rectangle
                (
                    Bounds.X + Tile.TILE_SIZE,
                    Bounds.Y + Tile.TILE_SIZE,
                    Tile.TILE_SIZE,
                    Tile.TILE_SIZE
                );
            spriteBatch.Draw(Sprite.GetTexture("powerup"), drawBounds, Color.White);

            // Draw Name
            drawBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE * 2,
                Bounds.Y + Tile.TILE_SIZE,
                Bounds.Width - Tile.TILE_SIZE * 3,
                Tile.TILE_SIZE
            );
            spriteBatch.Draw(Sprite.GetTexture("powerup"), drawBounds, Color.White);

            // Draw Buy Inventory
            drawBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE,
                Bounds.Y + Tile.TILE_SIZE * 2,
                Bounds.Width - Tile.TILE_SIZE * 2,
                Tile.TILE_SIZE
            );
            buyInventory.Draw(spriteBatch, drawBounds);

            // Draw Sell Inventory
            drawBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE,
                Bounds.Y + Tile.TILE_SIZE * 4,
                Bounds.Width - Tile.TILE_SIZE * 2,
                Tile.TILE_SIZE
            );
            buyInventory.Draw(spriteBatch, drawBounds);

            // Draw Ready button
            drawBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE * 4,
                Bounds.Y + Tile.TILE_SIZE * 5,
                Bounds.Width - Tile.TILE_SIZE * 8,
                Tile.TILE_SIZE
            );
            spriteBatch.Draw(Sprite.GetTexture("powerup"), drawBounds, Color.White);
            string ready = "Ready";
            SpriteFont readyFont = Utility.Fonts["blocks"];
            Vector2 readySize = readyFont.MeasureString(ready);
            Vector2 readyStringPos = new Vector2
                (
                    Bounds.X + (Bounds.Width - readySize.X) / 2,
                    Bounds.Y + (Bounds.Height - readySize.Y) / 2
                );
            spriteBatch.DrawString(readyFont, ready, readyStringPos, Color.Black);
        }
    }
}
