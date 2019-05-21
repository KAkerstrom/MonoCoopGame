using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class Inventory
    {
        private static Dictionary<string, Texture2D> textures;

        private int index = 0;
        private List<string> inventory = new List<string>();
        private Dictionary<string, int> items = new Dictionary<string, int>();

        public Inventory()
        {
            PopulateTextures();
            foreach (string item in textures.Keys)
            {
                inventory.Add(item);
                items.Add(item, 100);
            }
        }

        public static void PopulateTextures()
        {
            if (textures == null)
                textures = new Dictionary<string, Texture2D>
                {
                    { "wallStone", Sprite.GetTexture("wallStone") },
                    { "slime", Sprite.GetTexture("slime_news") },
                    { "door", Sprite.GetTexture("doorWood_closed") },
                    { "bush", Sprite.GetTexture("bush") },
                    { "bomb", Sprite.GetTexture("bomb0") },
                };
        }

        public void AddItem(string item, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Cannot add less than 1 item to inventory.");

            if (inventory.Contains(item))
                items[item] += quantity;
            else
            {
                inventory.Add(item);
                items.Add(item, quantity);
            }
        }

        public void DepleteItem(string item, int quantity)
        {
            if (quantity <= 0)
                throw new System.Exception("Cannot deplete less than 1 item from inventory.");

            if (inventory.Contains(item))
            {
                items[item] -= quantity;
                if (items[item] <= 0)
                    RemoveAll(item);
            }

        }

        public void RemoveAll(string item)
        {
            int itemIndex = inventory.IndexOf(item);
            if (index > itemIndex)
                index--;
            if (index >= inventory.Count)
                index = 0;

            inventory.Remove(item);
            items.Remove(item);
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

        public string GetCurrentItem()
        {
            // Probably better to either have a Place method in the items,
            // or have a seperate set of classes for inventory items?
            if (inventory.Count > 0)
                return inventory[index];
            else
                return string.Empty;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawArea)
        {
            int width = drawArea.Width / 5;
            int height = drawArea.Height;
            for (int i = 2; i >= 0; i--)
                for (int j = -1; j <= 1; j += 2) // A bit jank, but it works
                {
                    int itemIndex = (index + (i * j)) % (inventory.Count);
                    if (itemIndex < 0)
                        itemIndex = inventory.Count + itemIndex;

                    string itemName = inventory[itemIndex];
                    Rectangle itemRect = new Rectangle(drawArea.X + (i * j + 2) * width, drawArea.Y, width, height);

                    if (i == 0)
                    {
                        int wider = (int)(width * 0.2);
                        int taller = (int)(height * 0.2);
                        itemRect = new Rectangle(itemRect.X - wider, itemRect.Y - taller, itemRect.Width + wider * 2, itemRect.Height + taller * 2);
                    }

                    spriteBatch.Draw(textures[itemName], itemRect, Color.White * (1f / (i + 1)));
                }
        }
    }
}
