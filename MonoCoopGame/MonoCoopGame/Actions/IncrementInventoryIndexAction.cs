using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class IncrementInventoryIndexAction : Action
        {
            public IncrementInventoryIndexAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                parent.Inventory.IncrementIndex();
            }
        }
    }
}
