using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class PushBlockItem : InventoryItem
    {
        public PushBlockItem(int quantity) : base
            (
            "PushBlock", 
            "barrel", 
            quantity, 
            5,
            "A block that can be pushed around."
            ) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new PushBlock(player.Reticle.GridPos));
        }
    }
}
