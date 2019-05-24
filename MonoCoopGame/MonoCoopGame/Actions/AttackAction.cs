using monoCoopGame.Tiles;

namespace monoCoopGame
{
    public partial class Player : Character
    {
        class AttackAction : Action
        {
            public AttackAction(Player parent) : base(parent) { }

            public override void Perform(GameState gameState)
            {
                if (gameState.Map.IsTileAtGridPos(reticle.GridPos)
                    && gameState.Map.GetBlockAtGridPos(reticle.GridPos) is IDestroyable)
                {
                    ((IDestroyable)gameState.Map.GetBlockAtGridPos(reticle.GridPos)).Damage(1, gameState, parent);
                    parent.Controller.Vibrate(0.2f, 200);
                }
            }
        }
    }
}
