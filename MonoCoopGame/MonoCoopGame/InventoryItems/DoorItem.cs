using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class DoorItem : InventoryItem
    {
        public DoorItem(int quantity) : base
            (
            "Door", 
            "doorWood_closed", 
            quantity, 
            5,
            "A fortification that can be opened and closed."
            ) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Door(player.Reticle.GridPos));
        }
    }
}
