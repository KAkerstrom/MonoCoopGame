using Microsoft.Xna.Framework;

namespace monoCoopGame.Blocks
{
    public class Door : Block
    {
        private bool isOpen = false;
        private bool isLiminal = false;

        public Door(Point gridPos) : base(new Sprite("doorWood"), gridPos)
        {
        }

        public override void Step(GameState gameState)
        {
            if (isOpen)
            {
                if (!isLiminal)
                {
                    foreach (Character c in gameState.Characters)
                        if (c.GridPos == GridPos)
                            isLiminal = true;
                }
                else
                {
                    foreach (Character c in gameState.Characters)
                        if (c.GridPos != GridPos)
                        {
                            isLiminal = isOpen = false;
                            Sprite = new Sprite("doorWood_closed");
                        }
                }
            }
            IsSolid = !isOpen;
        }

        public override void Use(Player player, GameState gameState)
        {
            if (!isLiminal)
                isOpen = !isOpen;
            Sprite = new Sprite(isOpen? "doorWood_open" : "doorWood_closed");
        }
    }
}
