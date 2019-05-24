using Microsoft.Xna.Framework;
using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class PlaceBlockAction : Action
        {
            public PlaceBlockAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                if (gameState.Map.IsTileAtGridPos(reticle.GridPos)
                    && !gameState.Map.IsTileAtGridPos(TileMap.Layers.Blocks, reticle.GridPos))
                {
                    Tile placedTile = null;
                    switch (parent.Inventory.GetCurrentItem())
                    {
                        case "wallStone":
                            placedTile = new WallStone(reticle.GridPos);
                            break;
                        case "slime":
                            placedTile = new Slime(reticle.GridPos);
                            break;
                        case "door":
                            placedTile = new Door(reticle.GridPos);
                            break;
                        case "bush":
                            placedTile = new Bush(reticle.GridPos);
                            break;
                        case "bomb":
                            placedTile = new Bomb(reticle.GridPos, parent);
                            break;
                        case "bullet":
                            int xDelta = 0, yDelta = 0;
                            switch (parent.Facing)
                            {
                                case Directions.North: yDelta = -Tile.TILE_SIZE; break;
                                case Directions.East: xDelta = Tile.TILE_SIZE; break;
                                case Directions.West: xDelta = -Tile.TILE_SIZE; break;
                                case Directions.South: yDelta = Tile.TILE_SIZE; break;
                            }
                            Point bulletPos = new Point(parent.Hitbox.Center.X + xDelta, parent.Hitbox.Center.Y + yDelta);
                            new Bullet(bulletPos, parent, parent.Facing);
                            break;
                    }
                    if (placedTile != null)
                        gameState.Map.AddTile(placedTile);
                }
            }
        }
    }
}
