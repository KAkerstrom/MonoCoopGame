using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoCoopGame.InventoryItems;
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
        private enum SubMenuItems { Character, Name, Buy, Ready }

        public int controllerIndex { get; private set; }
        public MenuStates State = MenuStates.PressStart;
        public Rectangle Bounds;
        public bool Active { get { return State != MenuStates.PressStart; } }
        public bool Ready { get { return State == MenuStates.Ready; } }

        private int playerIndex;
        private int character;
        private Inventory buyInventory = new Inventory();
        private Inventory sellInventory = new Inventory();
        private TextEntry nameEntry;
        private SubMenuItems currentItem = SubMenuItems.Character;

        private IController controller;

        public CharacterSelectMenu(int index, Rectangle drawBounds)
        {
            playerIndex = character = index;
            Bounds = drawBounds;
            Rectangle nameBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE * 4,
                Bounds.Y + Tile.TILE_SIZE,
                Bounds.Width - Tile.TILE_SIZE * 5,
                Tile.TILE_SIZE * 2
            );
            nameEntry = new TextEntry(6, nameBounds);
            buyInventory.ShowQuantity = false;
            buyInventory = new Inventory
            (
                new List<InventoryItem>
                {
                    new BombItem(1),
                    new BushItem(1),
                    new WallItem(1),
                    new SlimeItem(1),
                    new PushBlockItem(1),
                    new BulletItem(1),
                    new DoorItem(1),
                    new ShovelItem(1),
                    new MineItem(1),
                }
            );
        }

        public Player CreatePlayer()
        {
            return new Player
                (
                    nameEntry.Text,
                    playerIndex,
                    controller.ControllerIndex,
                    character,
                    sellInventory,
                    20 * Tile.TILE_SIZE,
                    12 * Tile.TILE_SIZE
                );
        }

        public void Activate(int controllerIndex)
        {
            State = MenuStates.Selection;
            this.controllerIndex = controllerIndex;
            controller = ControllerFactory.GetController(controllerIndex);
        }

        public void Deactivate()
        {
            State = MenuStates.PressStart;
            currentItem = SubMenuItems.Character;
            controllerIndex = 0;
            controller = null;
            buyInventory = new Inventory();
            sellInventory = new Inventory();
        }

        public void Step()
        {
            if (Active)
            {
                controller.Update();
                switch (State)
                {
                    case MenuStates.Selection:
                        StepSelection();
                        break;

                    case MenuStates.Ready:
                        if (controller.ButtonPressed(Buttons.B))
                            State = MenuStates.Selection;
                        break;
                }
            }
        }

        private void StepSelection()
        {
            if (!nameEntry.Active)
            {
                if (currentItem > SubMenuItems.Name)
                    if (controller.ButtonPressed(Buttons.DPadDown)
                        || (controller.LeftStick.Y < -0.5
                            && controller.PreviousLeftStick.Y >= -0.5))
                    {
                        int itemIndex = (int)currentItem;
                        if (++itemIndex <= (int)SubMenuItems.Ready)
                            currentItem = (SubMenuItems)itemIndex;
                    }
                    else if (controller.ButtonPressed(Buttons.DPadUp)
                        || (controller.LeftStick.Y > 0.5
                            && controller.PreviousLeftStick.Y <= 0.5))
                    {
                        int itemIndex = (int)currentItem;
                        if (--itemIndex >= (int)SubMenuItems.Character)
                            currentItem = (SubMenuItems)itemIndex;
                    }

                switch (currentItem)
                {
                    case SubMenuItems.Character:
                        if (controller.ButtonPressed(Buttons.A))
                            if (++character > 5)
                                character = 0;
                        if (controller.ButtonPressed(Buttons.DPadRight)
                            || (controller.LeftStick.X > 0.5
                                && controller.PreviousLeftStick.X <= 0.5))
                            currentItem = SubMenuItems.Name;
                        else if (controller.ButtonPressed(Buttons.DPadDown)
                            || (controller.LeftStick.Y < -0.5
                                && controller.PreviousLeftStick.Y >= -0.5))
                            currentItem = SubMenuItems.Buy;
                        break;

                    case SubMenuItems.Name:
                        if (controller.ButtonPressed(Buttons.A))
                            nameEntry.Activate(controller);
                        if (controller.ButtonPressed(Buttons.DPadLeft)
                            || (controller.LeftStick.X < -0.5
                                && controller.PreviousLeftStick.X >= -0.5))
                            currentItem = SubMenuItems.Character;
                        else if (controller.ButtonPressed(Buttons.DPadDown)
                            || (controller.LeftStick.Y < -0.5
                                && controller.PreviousLeftStick.Y >= -0.5))
                            currentItem = SubMenuItems.Buy;
                        break;

                    case SubMenuItems.Buy:
                        if (controller.ButtonPressed(Buttons.DPadRight)
                        || (controller.LeftStick.X > 0.2
                            && controller.PreviousLeftStick.X <= 0.2))
                            buyInventory.IncrementIndex();
                        else if (controller.ButtonPressed(Buttons.DPadLeft)
                        || (controller.LeftStick.X < -0.2
                            && controller.PreviousLeftStick.X >= -0.2))
                            buyInventory.DecrementIndex();
                        else if (controller.ButtonPressed(Buttons.A))
                        {
                            InventoryItem item = buyInventory.GetCurrentItem().Copy();
                            item.Quantity = 1;
                            sellInventory.AddItem(item);
                        }
                        else if (controller.ButtonPressed(Buttons.B))
                        {
                            if (sellInventory.GetItem(buyInventory.GetCurrentItem().Name) != null)
                                sellInventory.DepleteItem(buyInventory.GetCurrentItem().Name, 1);
                        }
                        break;

                    case SubMenuItems.Ready:
                        if (controller.ButtonPressed(Buttons.A))
                            State = MenuStates.Ready;
                        break;
                }

                if (controller.ButtonPressed(Buttons.B))
                {
                    if (currentItem == SubMenuItems.Character)
                        Deactivate();
                    else
                        currentItem = SubMenuItems.Character;
                }
            }

            nameEntry.Step();
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
                Tile.TILE_SIZE * 2,
                Tile.TILE_SIZE * 2
            );
            spriteBatch.Draw(Sprite.GetTexture(currentItem == SubMenuItems.Character ? "reticle" : "powerup2"), drawBounds, Color.White);
            spriteBatch.Draw(Sprite.GetTexture($"char{character}_walk_s_1"), drawBounds, Color.White);

            // Draw Name
            nameEntry.Draw(spriteBatch);

            // Draw Buy Inventory
            drawBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE * 4,
                Bounds.Y + Tile.TILE_SIZE * 4,
                Bounds.Width - Tile.TILE_SIZE * 5,
                Tile.TILE_SIZE
            );
            spriteBatch.Draw(Sprite.GetTexture(currentItem == SubMenuItems.Buy ? "reticle" : "powerup2"), drawBounds, Color.White);
            buyInventory.Draw(spriteBatch, drawBounds);

            int currentQuantityOwned = 0;
            InventoryItem item = sellInventory.GetItem(buyInventory.GetCurrentItem().Name);
            if (item != null)
                currentQuantityOwned = item.Quantity;
            Vector2 drawPoint = new Vector2(Bounds.X + Tile.TILE_SIZE * 4, Bounds.Y + Tile.TILE_SIZE * 6);
            spriteBatch.DrawString(Utility.Fonts["blocks"], currentQuantityOwned.ToString(), drawPoint, Color.Black);

            // Draw Ready button
            drawBounds = new Rectangle
            (
                Bounds.X + Tile.TILE_SIZE * 4,
                Bounds.Y + Tile.TILE_SIZE * 8,
                Bounds.Width - Tile.TILE_SIZE * 8,
                Tile.TILE_SIZE
            );
            spriteBatch.Draw(Sprite.GetTexture(currentItem == SubMenuItems.Ready ? "reticle" : "powerup2"), drawBounds, Color.White);
            string ready = currentItem.ToString(); //"Ready";
            SpriteFont readyFont = Utility.Fonts["blocks"];
            Vector2 readySize = readyFont.MeasureString(ready);
            Vector2 readyStringPos = new Vector2
                (
                    drawBounds.X + (drawBounds.Width - readySize.X) / 2,
                    drawBounds.Y + (drawBounds.Height - readySize.Y) / 2
                );
            spriteBatch.DrawString(readyFont, ready, readyStringPos, Color.Black);
        }
    }
}
