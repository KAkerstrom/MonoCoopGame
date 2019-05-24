using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
using System.Collections.Generic;

namespace monoCoopGame
{
    class Bullet
    {
        private static Sprite Sprite;
        public static List<Bullet> Bullets = new List<Bullet>();

        public Point Pos { get; private set; }
        public Point GridPos { get { return new Point(Pos.X / Tile.TILE_SIZE, Pos.Y / Tile.TILE_SIZE); } }
        public Player Owner;
        private Directions direction;
        private float rotation;
        private int speed = 5;
        private int damage = 2;

        public Bullet(Point position, Player owner, Directions direction)
        {
            Pos = position;
            Owner = owner;
            this.direction = direction;
            switch (direction)
            {
                case Directions.North: rotation = 0; break;
                case Directions.East: rotation = 1.5708f; break;
                case Directions.West: rotation = 4.71239f; break;
                case Directions.South: rotation = 3.14159f; break;
            }
            PopulateTextures();

            Bullets.Add(this);
        }

        private static void PopulateTextures()
        {
            if (Sprite == null)
                Sprite = new Sprite("bullet");
        }

        public void Step(GameState gameState)
        {
            int xDelta = 0, yDelta = 0;
            switch (direction)
            {
                case Directions.North: yDelta = -speed; break;
                case Directions.East: xDelta = speed; break;
                case Directions.West: xDelta = -speed; break;
                case Directions.South: yDelta = speed; break;
            }
            Pos = new Point(Pos.X + xDelta, Pos.Y + yDelta);

            if (gameState.Map.IsGridPosInMap(GridPos))
            {
                if (gameState.Map.IsBlockAtPos(Pos))
                {
                    Tile hitTile = gameState.Map.GetBlockAtGridPos(GridPos);
                    if (hitTile is IDestroyable)
                        ((IDestroyable)hitTile).Damage(damage, gameState, Owner);
                    Destroy(gameState);
                }
                else
                {
                    Point size = new Point(0, 0);
                    Rectangle hitbox = new Rectangle(Pos, size);
                    foreach (Player p in gameState.Players)
                        if (p.Hitbox.Intersects(hitbox))
                        {
                            p.Damage(damage, gameState, Owner);
                            Destroy(gameState);
                        }
                }
            }
            else
            {
                Destroy(gameState);
            }
        }

        public void Destroy(GameState gameState)
        {
            Bullets.Remove(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Point drawPoint = new Point(Pos.X - (Sprite.Width / 2), Pos.Y - (Sprite.Height / 2));
            Sprite.Draw(spriteBatch, drawPoint, 0f, rotation);
        }
    }
}
