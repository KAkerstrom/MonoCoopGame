using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class BushItem : InventoryItem
    {
        public BushItem(int quantity) : base("Bush", "bush", quantity) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Bush(player.Reticle.GridPos));
        }
    }
}
