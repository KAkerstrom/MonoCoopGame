using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public abstract class Character
    {
        public int X { get { return (int)x; } }
        public int Y { get { return (int)y; } }
        public int XPrevious { get { return (int)xPrevious; } }
        public int YPrevious { get { return (int)yPrevious; } }
        public Directions Facing;
        public Rectangle Hitbox
        {
            get { return new Rectangle((int)x, (int)y, Tile.TILE_SIZE - 4, Tile.TILE_SIZE - 4); }
        }

        protected float x, y, xPrevious, yPrevious;
        protected Sprite sprite;
        protected float moveSpeed;
        protected float currentMoveSpeed;
        protected string texturePrefix;
        protected Dictionary<string, Dictionary<Directions, Sprite>> sprites
            = new Dictionary<string, Dictionary<Directions, Sprite>>();

        public Character(int x, int y, float moveSpeed)
        {
            this.moveSpeed = currentMoveSpeed = moveSpeed;
            this.x = xPrevious = x;
            this.y = yPrevious = y;
            Facing = Directions.South;
        }

        public abstract void Step(GameState gameState);

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)x - 2, (int)y - 2);
        }

        protected void Move(GameState gameState, float xDelta, float yDelta)
        {
            float speedModifier = 1;
            if (!gameState.Map.IsTileAtPoint((int)x + Hitbox.Width / 2, (int)y + Hitbox.Height / 2))
                speedModifier = 0.4f;
            else if (!gameState.Map.IsTileAtPoint(TileMap.Layers.Dirt, (int)x + Hitbox.Width / 2, (int)y + Hitbox.Height / 2))
                speedModifier = 1.3f;

            xDelta *= speedModifier;
            yDelta *= speedModifier;

            xPrevious = x;
            yPrevious = y;
            x += xDelta;
            y += yDelta;

            Directions facingPrevious = Facing;
            if (xDelta != 0 || yDelta != 0)
            {
                sprite.Speed = 20;
                if (Math.Abs(xDelta) > Math.Abs(yDelta))
                    Facing = (xDelta > 0) ? Directions.East : Directions.West;
                else
                    Facing = (yDelta > 0) ? Directions.South : Directions.North;

                if (facingPrevious != Facing)
                {
                    sprite = sprites["walk"][Facing];
                    sprite.SpriteIndex = 0;
                }
            }
            else
            {
                sprite.Speed = 0;
                sprite.SpriteIndex = 1;
            }

            if (x > xPrevious)
            {
                if (gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, X + Hitbox.Width, YPrevious)
                    || gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, X + Hitbox.Width, YPrevious + Hitbox.Height))
                    x = (xPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }
            else if (x < xPrevious)
            {
                if (gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, X, YPrevious)
                    || gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, X, YPrevious + Hitbox.Height))
                    x = (xPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }

            if (y > yPrevious)
            {
                if (gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, XPrevious, Y + Hitbox.Width)
                    || gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, XPrevious + Hitbox.Width, Y + Hitbox.Height))
                    y = (yPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }
            else if (y < yPrevious)
            {
                if (gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, XPrevious, Y)
                    || gameState.Map.IsTileAtPoint(TileMap.Layers.Stone, XPrevious + Hitbox.Width, Y))
                    y = (yPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
            }
        }
    }
}
