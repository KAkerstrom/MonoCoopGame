using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using monoCoopGame.InventoryItems;
using System.Reflection;
using System.Linq;

namespace monoCoopGame
{
    public partial class Inventory
    {
        private int index = 0;
        private List<InventoryItem> inventory;

        public Inventory()
        {
            inventory = new List<InventoryItem>
            {
                new BombItem(9999),
                new BushItem(9999),
                new WallItem(9999),
                new SlimeItem(9999),
                new PushBlockItem(9999),
                new BulletItem(9999),
                new DoorItem(9999),
                new ShovelItem(9999),
            };
        }

        public void AddItem(InventoryItem item)
        {
            if (item.Quantity <= 0)
                throw new Exception("Cannot add less than 1 item to inventory.");

            InventoryItem foundItem = inventory.Find(x => x.Name == item.Name);
            if (foundItem != null)
                foundItem.Quantity += item.Quantity;
            else
            {
                inventory.Add(item);
            }
        }

        public void DepleteItem(string itemName, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Cannot deplete less than 1 item from inventory.");

            InventoryItem foundItem = inventory.Find(x => x.Name == itemName);
            if (foundItem != null)
            {
                foundItem.Quantity -= quantity;
                if (foundItem.Quantity <= 0)
                    RemoveAll(itemName);
            }

        }

        public void RemoveAll(string itemName)
        {
            InventoryItem foundItem = inventory.Find(x => x.Name == itemName);
            int itemIndex = inventory.IndexOf(foundItem);
            if (index > itemIndex)
                index--;
            //if (index >= inventory.Count)
            //    index = 0;

            inventory.Remove(foundItem);
        }

        public void IncrementIndex()
        {
            if (++index >= inventory.Count)
                index = 0;
        }

        public void DecrementIndex()
        {
            if (--index < 0)
                index = inventory.Count - 1;
        }

        public InventoryItem GetCurrentItem()
        {
            if (inventory.Count > 0)
                return inventory[index];
            return null;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawArea)
        {
            int width = drawArea.Width / 5;
            int height = drawArea.Height;
            for (int i = 2; i >= 0; i--)
                for (int j = -1; j <= 1; j += 2)
                {
                    int itemIndex = (index + (i * j)) % (inventory.Count);
                    if (itemIndex < 0)
                        itemIndex = inventory.Count + itemIndex;

                    Rectangle itemRect = new Rectangle(drawArea.X + (i * j + 2) * width, drawArea.Y, width, height);

                    if (i == 0)
                    {
                        int wider = (int)(width * 0.2);
                        int taller = (int)(height * 0.2);
                        itemRect = new Rectangle(itemRect.X - wider, itemRect.Y - taller, itemRect.Width + wider * 2, itemRect.Height + taller * 2);
                    }

                    spriteBatch.Draw(inventory[itemIndex].Texture, itemRect, Color.White * (1f / (i + 1)));

                    if (i == 0)
                    {
                        Vector2 drawPoint = new Vector2(itemRect.Location.X, itemRect.Bottom - 10);
                        spriteBatch.DrawString(Utility.Fonts["quantityFont"], inventory[itemIndex].Quantity.ToString(), drawPoint, Color.Black);
                    }
                }
        }
    }
}
