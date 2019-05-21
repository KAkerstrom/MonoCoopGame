using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class Slime : Blob, ISteppable, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;
        public int Health { get; private set; }
        int growthTimer = 200;

        public Slime(Point gridPos) : base("slime", BlobGroups.Slime, gridPos)
        {
            SpeedModifier = 0.1f;
            Health = 10;
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy(gameState, player);
        }

        public void Destroy(GameState gameState, Player player = null)
        {
            TileDestroyed?.Invoke(this, player);
        }

        public void Step(GameState gameState)
        {
            if (--growthTimer == 0)
            {
                growthTimer = Utility.R.Next(100, 1000);
                Point[] checks = new Point[]
                {
                    new Point(GridPos.X - 1, GridPos.Y),
                    new Point(GridPos.X + 1, GridPos.Y),
                    new Point(GridPos.X, GridPos.Y - 1),
                    new Point(GridPos.X, GridPos.Y + 1)
                };
                foreach (Point check in checks)
                    if (Utility.R.Next(10) == 0
                        && gameState.Map.IsGridPosInMap(check)
                        && gameState.Map.IsTileAtGridPos(check)
                        && !gameState.Map.IsTileAtGridPos(TileMap.Layers.Blocks, check))
                    {
                        gameState.Map.AddTile(TileMap.Layers.Blocks, new Slime(check));
                    }
            }
        }
    }
}
