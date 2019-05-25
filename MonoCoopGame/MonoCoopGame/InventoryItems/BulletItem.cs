using Microsoft.Xna.Framework;
using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class BulletItem : InventoryItem
    {
        public BulletItem(int quantity) : base("Bullet", "bullet", quantity) { }

        public override void Use(GameState gameState, Player player)
        {
            int xDelta = 0, yDelta = 0;
            switch (player.Facing)
            {
                case Directions.North: yDelta = -Tile.TILE_SIZE; break;
                case Directions.East: xDelta = Tile.TILE_SIZE; break;
                case Directions.West: xDelta = -Tile.TILE_SIZE; break;
                case Directions.South: yDelta = Tile.TILE_SIZE; break;
            }
            Point bulletPos = new Point(player.Hitbox.Center.X + xDelta, player.Hitbox.Center.Y + yDelta);
            new Bullet(bulletPos, player, player.Facing);
        }
    }
}
