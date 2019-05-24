using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame.Tiles
{
    class PushBlock : Tile, IDestroyable, IUsable
    {
        public event TileDestroyedDelegate TileDestroyed;

        public int Health { get; protected set; }
        public int InvulnFrames { get; set; }

        public PushBlock(Point gridPos) : base(new Sprite("barrel"), gridPos)
        {
            HasTransparency = true;
            SpeedModifier = 0;
            Health = 10;
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

        public void Use(Player player, GameState gameState)
        {
            Point newGridPos = GridPos;
            switch (player.Facing)
            {
                case Directions.North:
                    newGridPos = new Point(GridPos.X, GridPos.Y - 1);
                    break;
                case Directions.East:
                    newGridPos = new Point(GridPos.X + 1, GridPos.Y);
                    break;
                case Directions.West:
                    newGridPos = new Point(GridPos.X - 1, GridPos.Y);
                    break;
                case Directions.South:
                    newGridPos = new Point(GridPos.X, GridPos.Y + 1);
                    break;
            }

            if (gameState.Map.IsGridPosInMap(newGridPos))
            {
                if (!gameState.Map.IsBlockAtGridPos(newGridPos)
                    && gameState.Map.IsTileAtGridPos(newGridPos))
                {
                    gameState.Map.AddTile(new PushBlock(newGridPos));
                    Destroy(gameState);
                }
            }
        }
    }
}
