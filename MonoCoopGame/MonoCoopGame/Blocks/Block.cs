using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame
{
    public delegate void BlockDestroyedDelegate(Block block, Player player);

    public abstract class Block
    {
        public event BlockDestroyedDelegate BlockDestroyed;

        public Player Owner;
        public Sprite Sprite;
        public Point GridPos { get; }
        public Point Pos { get { return new Point(GridPos.X * Tile.TILE_SIZE, GridPos.Y * Tile.TILE_SIZE); } }
        public float SpeedModifier = 1;
        public bool IsSolid = false;
        public bool Visible = true;
        public int Health = 10;

        public Block(Sprite sprite, Point gridPos, Player owner = null)
        {
            GridPos = gridPos;
            Sprite = sprite;
            Owner = owner;
        }

        public virtual void Step(GameState gameState) { }
        public virtual void Use(Player player, GameState gameState) { }

        public virtual void Damage(Player player, GameState gameState, int damage)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy(player);
        }

        public virtual void Destroy(Player player)
        {
            BlockDestroyed?.Invoke(this, player);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                Sprite.Draw(spriteBatch, Pos.X, Pos.Y);
        }
    }
}
