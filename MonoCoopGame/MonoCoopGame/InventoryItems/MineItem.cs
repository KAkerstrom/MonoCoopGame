﻿using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class MineItem : InventoryItem
    {
        public MineItem(int quantity) : base("Mine", "mine_visible", quantity) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(player.Reticle.GridPos)
                && !gameState.Map.IsBlockAtGridPos(player.Reticle.GridPos))
                gameState.Map.AddTile(new Mine(player.Reticle.GridPos, player));
        }
    }
}
