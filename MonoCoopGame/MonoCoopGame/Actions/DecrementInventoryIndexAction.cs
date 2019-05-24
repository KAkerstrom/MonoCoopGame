using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class DecrementInventoryIndexAction : Action
        {
            public DecrementInventoryIndexAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                parent.Inventory.DecrementIndex();
            }
        }
    }
}
