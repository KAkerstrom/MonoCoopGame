using Microsoft.Xna.Framework;

namespace monoCoopGame.Blocks
{
    public class Slime : BlobBlock
    {
        int growthTimer = 200;

        public Slime(Point gridPos, Player owner) : base("slime", "slime", gridPos, owner)
        {
            SpeedModifier = 0.1f;
        }

        public override void Step(GameState gameState)
        {
            base.Step(gameState);
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
                        && gameState.Map.GridPointIsInMap(check)
                        && !gameState.Map.IsBlockAtGridPos(check)
                        && gameState.Map.GetTileAtGridPos(check).Type != Tile.TileType.Water)
                    {
                        gameState.Map.AddBlock(new Slime(check, Owner));
                    }
            }
        }
    }
}
