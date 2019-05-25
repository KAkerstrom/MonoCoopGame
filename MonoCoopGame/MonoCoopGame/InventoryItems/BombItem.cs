using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class BombItem : InventoryItem
    {
        public BombItem(int quantity) : base("Bomb", "bomb0", quantity) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Bomb(player.Reticle.GridPos, player));
        }
    }
}
