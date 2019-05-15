using Microsoft.Xna.Framework;

namespace monoCoopGame.Blocks
{
    public class Bush : Block
    {
        public Bush(Point gridPos) : base(new Sprite("bush"), gridPos)
        {
            IsSolid = true;
            Health = 2;
        }

        public override void Step(GameState gameState)
        {
        }

        public override void Use(Player player, GameState gameState)
        {
        }
    }
}
