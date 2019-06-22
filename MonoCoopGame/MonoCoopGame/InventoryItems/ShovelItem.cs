using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;

namespace monoCoopGame.InventoryItems
{
    public class ShovelItem : InventoryItem
    {
        public ShovelItem(int quantity) : base
            (
            "Shovel", 
            "shovel", 
            quantity, 
            1,
            "Digs a hole that can also be flooded with water."
            ) { }

        public override void Use(GameState gameState, Player player)
        {
            if (gameState.Map.IsTileAtGridPos(TileMap.Layers.Grass, player.Reticle.GridPos))
                gameState.Map.RemoveTile(TileMap.Layers.Grass, player.Reticle.GridPos);
            else if (gameState.Map.IsTileAtGridPos(TileMap.Layers.Dirt, player.Reticle.GridPos))
                gameState.Map.RemoveTile(TileMap.Layers.Dirt, player.Reticle.GridPos);
        }
    }
}
