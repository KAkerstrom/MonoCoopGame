using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class Bush : Tile, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;
        public int Health { get; private set; }
        public int InvulnFrames { get; set; } = 0;

        public Bush(Point gridPos) : base(new Sprite("bush"), gridPos)
        {
            HasTransparency = true;
            SpeedModifier = 0;
            Health = 3;
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            if (InvulnFrames == 0)
            {
                InvulnFrames = INVULN_TIME;
                Health -= damage;
                if (Health <= 0)
                    Destroy(gameState, player);
            }
        }

        public void Destroy(GameState gameState, Player player = null)
        {
            TileDestroyed?.Invoke(this, player);
        }
    }
}
