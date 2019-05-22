using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class Slime : Blob, ISteppable, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;
        public int Health { get; private set; }
        public int InvulnFrames { get; set; } = 0;
        int growthTimer = 200;

        public Slime(Point gridPos) : base("slime", BlobGroups.Slime, gridPos)
        {
            SpeedModifier = 0.1f;
            Health = 10;
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            if (InvulnFrames == 0)
            {
                HasTransparency = true;
                InvulnFrames = INVULN_TIME;
                Health -= damage;
                if (Health <= 0)
                    Destroy(gameState, player);
                if (player != null)
                    player.Controller.Vibrate(0.2f, 200);
            }
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
