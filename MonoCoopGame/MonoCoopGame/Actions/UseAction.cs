using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class UseAction : Action
        {
            public UseAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                if (gameState.Map.IsTileAtGridPos(reticle.GridPos)
                    && gameState.Map.GetBlockAtGridPos(reticle.GridPos) is IUsable)
                    ((IUsable)gameState.Map.GetBlockAtGridPos(reticle.GridPos)).Use(parent, gameState);
            }
        }
    }
}
