using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class WallStone : Tile, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;
        public int Health { get; private set; }

        public WallStone(Point gridPos) : base(new Sprite("wallStone"), gridPos)
        {
            HasTransparency = true;
            SpeedModifier = 0;
            Health = 10;
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy(player);
        }

        public void Destroy(Player player = null)
        {
            TileDestroyed?.Invoke(this, player);
        }
    }
}
