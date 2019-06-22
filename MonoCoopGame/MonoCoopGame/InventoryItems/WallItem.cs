using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class WallItem : InventoryItem
    {
        public WallItem(int quantity) : base
            (
            "Wall", 
            "wallStone", 
            quantity, 
            5,
            "A strong fortification."
            ) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new WallStone(player.Reticle.GridPos));
        }
    }
}
