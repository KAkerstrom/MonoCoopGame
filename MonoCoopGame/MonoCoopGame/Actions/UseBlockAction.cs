using Microsoft.Xna.Framework;
using monoCoopGame.InventoryItems;
using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class UseBlockAction : Action
        {
            public UseBlockAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                InventoryItem item = parent.Inventory.GetCurrentItem();
                if (item != null)
                {
                    parent.Inventory.DepleteItem(item.Name, 1);
                    item.Use(gameState, parent);
                }
            }
        }
    }
}
