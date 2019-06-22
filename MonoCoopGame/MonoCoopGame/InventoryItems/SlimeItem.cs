using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class SlimeItem : InventoryItem
    {
        public SlimeItem(int quantity) : base
            (
            "Slime", 
            "slime_news", 
            quantity, 
            20,
            "A self-replicating slime that slows and damages players."
            ) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Slime(player.Reticle.GridPos));
        }
    }
}
