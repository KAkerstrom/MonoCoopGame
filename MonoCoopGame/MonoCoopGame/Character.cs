using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public abstract class Character
    {
        public int Health { get; set; }
        public Directions Facing;
        public Point Pos { get { return new Point((int)x, (int)y); } }
        public Point PreviousPos { get { return new Point((int)xPrevious, (int)yPrevious); } }
        public Point PreviousGridPos
        {
            get { return new Point(PreviousHitbox.Center.X / Tile.TILE_SIZE, PreviousHitbox.Center.Y / Tile.TILE_SIZE); }
        }
        public Point GridPos
        {
            get { return new Point(Hitbox.Center.X / Tile.TILE_SIZE, Hitbox.Center.Y / Tile.TILE_SIZE); }
        }
        public Rectangle PreviousHitbox
        {
            get { return new Rectangle(PreviousPos, hitboxSize); }
        }
        public Rectangle Hitbox
        {
            get { return new Rectangle(Pos, hitboxSize); }
        }

        protected float x, y, xPrevious, yPrevious;
        protected Sprite sprite;
        protected Point hitboxSize = new Point(Tile.TILE_SIZE - 10, Tile.TILE_SIZE - 6);
        protected float moveSpeed;
        protected float currentMoveSpeed;
        protected string texturePrefix;
        protected string action;
        protected Dictionary<string, Dictionary<Directions, Sprite>> sprites
            = new Dictionary<string, Dictionary<Directions, Sprite>>();

        public Character(int x, int y, float moveSpeed)
        {
            this.moveSpeed = currentMoveSpeed = moveSpeed;
            this.x = xPrevious = x;
            this.y = yPrevious = y;
            Facing = Directions.South;
            action = "walk";
            Health = 50;
        }

        public abstract void Step(GameState gameState);

        public void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            Point drawPoint = new Point((int)x - 0, (int)y - 2);
            sprite.Draw(spriteBatch, drawPoint, (float)Pos.Y / (30 * Tile.TILE_SIZE)); //TODO: Change to map height variable
            EndDraw(spriteBatch);
        }

        protected abstract void BeginDraw(SpriteBatch spriteBatch);
        protected abstract void EndDraw(SpriteBatch spriteBatch);

        public void Damage(int damage, GameState gameState, Player attacker)
        {
            Health -= damage;
        }

        protected void Move(GameState gameState, float xDelta, float yDelta)
        {
            float speedModifier = gameState.Map.GetSpeedModifier(Hitbox.Center);
            xDelta *= speedModifier;
            yDelta *= speedModifier;

            xPrevious = x;
            yPrevious = y;
            x += xDelta;
            y += yDelta;

            Directions facingPrevious = Facing;
            if (xDelta != 0 || yDelta != 0)
            {
                if (GridPos != PreviousGridPos)
                {
                    if (!gameState.Map.IsTileAtGridPos(GridPos)
                        || (gameState.Map.GetBlockAtGridPos(GridPos) != null
                        && gameState.Map.GetBlockAtGridPos(GridPos) is Slime))
                        action = "swim";
                    else
                        action = "walk";
                    sprite = sprites[action][Facing];
                }

                sprite.Speed = 20;
                if (Math.Abs(xDelta) > Math.Abs(yDelta))
                    Facing = (xDelta > 0) ? Directions.East : Directions.West;
                else
                    Facing = (yDelta > 0) ? Directions.South : Directions.North;

                if (facingPrevious != Facing)
                {
                    sprite = sprites[action][Facing];
                    sprite.ImageIndex = 0;
                }
            }
            else if (action == "walk")
            {
                sprite.Speed = 0;
                sprite.ImageIndex = 1;
            }

            Point topRL, bottomRL, leftTB, rightTB;
            if (x > xPrevious)
            {
                topRL = new Point(Hitbox.Right, PreviousPos.Y);
                bottomRL = new Point(Hitbox.Right, PreviousHitbox.Bottom);
            }
            else if (x < xPrevious)
            {
                topRL = new Point(Pos.X, PreviousPos.Y);
                bottomRL = new Point(Pos.X, PreviousHitbox.Bottom);
            }
            else
                topRL = bottomRL = Pos;

            if (y > yPrevious)
            {
                leftTB = new Point(PreviousPos.X, PreviousHitbox.Bottom);
                rightTB = new Point(PreviousHitbox.Right, Hitbox.Bottom);
            }
            else if (y < yPrevious)
            {
                leftTB = new Point(PreviousPos.X, Pos.Y);
                rightTB = new Point(PreviousHitbox.Right, Pos.Y);
            }
            else
                leftTB = rightTB = Pos;

            if (x != xPrevious)
                if (x < Tile.TILE_SIZE || Hitbox.Right > gameState.Map.GridWidth * Tile.TILE_SIZE
                    || (gameState.Map.IsBlockAtPos(topRL) && gameState.Map.GetBlockAtPos(topRL).IsSolid)
                    || (gameState.Map.IsBlockAtPos(bottomRL) && gameState.Map.GetBlockAtPos(bottomRL).IsSolid))
                    x = (xPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;

            if (y != yPrevious)
                if (y < Tile.TILE_SIZE || Hitbox.Bottom > gameState.Map.GridHeight * Tile.TILE_SIZE
                    || (gameState.Map.IsBlockAtPos(leftTB) && gameState.Map.GetBlockAtPos(leftTB).IsSolid)
                    || (gameState.Map.IsBlockAtPos(rightTB) && gameState.Map.GetBlockAtPos(rightTB).IsSolid))
                    y = (yPrevious / Tile.TILE_SIZE) * Tile.TILE_SIZE;
        }
    }
}
