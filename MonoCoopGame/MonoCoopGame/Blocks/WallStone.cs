using Microsoft.Xna.Framework;

namespace monoCoopGame.Blocks
{
    public class WallStone : Block
    {
        public WallStone(Point gridPos) : base(new Sprite("wallStone"), gridPos)
        {
            IsSolid = true;
        }

        public override void Step(GameState gameState)
        {
        }

        public override void Use(Player player, GameState gameState)
        {
        }
    }
}
