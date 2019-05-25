using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class DoorItem : InventoryItem
    {
        public DoorItem(int quantity) : base("Door", "doorWood_closed", quantity) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Door(player.Reticle.GridPos));
        }
    }
}
