using Microsoft.Xna.Framework;
using monoCoopGame.InventoryItems;
using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class PlaceBlockAction : Action
        {
            public PlaceBlockAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                InventoryItem item = parent.Inventory.GetCurrentItem();
                item.Use(gameState, parent);
            }
        }
    }
}
