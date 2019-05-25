using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class SlimeItem : InventoryItem
    {
        public SlimeItem(int quantity) : base("Slime", "slime_", quantity) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Slime(player.Reticle.GridPos));
        }
    }
}
