using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace monoCoopGame
{
    public abstract class Character
    {
        public float x, y, xPrevious, yPrevious;
        protected Sprite sprite;
        protected float moveSpeed;

        public Character (int x, int y, float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.x = xPrevious = x;
            this.y = yPrevious = y;
        }

        public abstract void Step(GameState gameState);

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)x, (int)y);
        }
    }
}
