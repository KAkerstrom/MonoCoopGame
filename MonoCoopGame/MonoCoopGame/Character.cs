using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace monoCoopGame
{
    public abstract class Character
    {
        public float x, y, xPrevious, yPrevious;
        protected Sprite sprite;
        protected float moveSpeed;

        public Character(int x, int y, float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.x = xPrevious = x;
            this.y = yPrevious = y;
        }

        public abstract void Step(GameState gameState);

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)x - 2, (int)y - 2);
        }

        protected void Move(GameState gameState, float xDelta, float yDelta) 
        {
            xPrevious = x;
            yPrevious = y;
            x += xDelta;
            y += yDelta;

            if (x > xPrevious)
            {
                if (gameState.Map.GetTileAtPoint((int)x + Tile.TILE_SIZE - 4, (int)yPrevious).IsSolid
                    || gameState.Map.GetTileAtPoint((int)x + Tile.TILE_SIZE - 4, (int)yPrevious + Tile.TILE_SIZE - 4).IsSolid)
                    x = (xPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }
            else if (x < xPrevious)
            {
                if (gameState.Map.GetTileAtPoint((int)x, (int)yPrevious).IsSolid
                    || gameState.Map.GetTileAtPoint((int)x, (int)yPrevious + Tile.TILE_SIZE - 4).IsSolid)
                    x = (xPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }

            if (y > yPrevious)
            {
                if (gameState.Map.GetTileAtPoint((int)xPrevious, (int)y + Tile.TILE_SIZE - 4).IsSolid
                    || gameState.Map.GetTileAtPoint((int)xPrevious + Tile.TILE_SIZE - 4, (int)y + Tile.TILE_SIZE - 4).IsSolid)
                    y = (yPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }
            else if (y < yPrevious)
            {
                if (gameState.Map.GetTileAtPoint((int)xPrevious, (int)y).IsSolid
                    || gameState.Map.GetTileAtPoint((int)xPrevious + Tile.TILE_SIZE - 4, (int)y).IsSolid)
                    y = (yPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }
        }
    }
}
