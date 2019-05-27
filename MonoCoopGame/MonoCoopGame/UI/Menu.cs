using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace monoCoopGame.UI
{
    class Menu
    {
        public Rectangle Bounds { get; }
        public int Index { get; protected set; }

        private List<MenuItem> items;

        public Menu(Rectangle bounds, List<MenuItem> items)
        {
            if (items.Count == 0)
                throw new Exception("Cannot create menu with no items.");

            Bounds = bounds;
            this.items = items;
            items[0].Selected = true;
        }

        public void IncrementIndex(bool wrap)
        {
            items[Index].Selected = false;
            do
            {
                Index++;
                if (Index >= items.Count)
                {
                    if (wrap)
                        Index = 0;
                    else
                        Index--;
                }
            }
            while (items[Index].Enabled == false);
            items[Index].Selected = true;
        }

        public void DecrementIndex(bool wrap)
        {
            items[Index].Selected = false;
            do
            {
                Index--;
                if (Index < 0)
                {
                    if (wrap)
                        Index = items.Count - 1;
                    else
                        Index++;
                }
            }
            while (items[Index].Enabled == false);
            items[Index].Selected = true;
        }

        public void SetIndex(int index)
        {
            items[Index].Selected = false;
            if (items[Index].Enabled)
                Index = index;
            items[Index].Selected = true;
        }

        public void ActivateItem()
        {
            items[Index].Activate();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int itemHeight = Bounds.Height / items.Count;
            for (int i = 0; i < items.Count; i++)
            {
                Point location = new Point(Bounds.X, Bounds.Y + (itemHeight * i));
                Point size = new Point(Bounds.Width, itemHeight);
                Rectangle itemDrawBounds = new Rectangle(location, size);
                items[i].Draw(spriteBatch, itemDrawBounds);
            }
        }
    }
}
