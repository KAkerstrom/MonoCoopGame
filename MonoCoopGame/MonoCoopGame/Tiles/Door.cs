using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    public class Door : Tile, ISteppable, IUsable, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;
        public int Health { get; private set; }
        public int InvulnFrames { get; set; } = 0;
        private bool isOpen = false;
        private bool isLiminal = false;

        public Door(Point gridPos) : base(new Sprite("doorWood_closed"), gridPos)
        {
            HasTransparency = true;
            Health = 10;
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            if (InvulnFrames == 0)
            {
                InvulnFrames = INVULN_TIME;
                Health -= damage;
                if (Health <= 0)
                    Destroy(gameState, player);
            }
        }

        public void Destroy(GameState gameState, Player player = null)
        {
            TileDestroyed?.Invoke(this, player);
        }

        public void Step(GameState gameState)
        {
            if (isOpen)
            {
                if (!isLiminal)
                {
                    foreach (Player p in gameState.Players)
                        if (p.GridPos == GridPos)
                            isLiminal = true;
                }
                else
                {
                    bool tempLiminal = false;
                    foreach (Player p in gameState.Players)
                        if (p.GridPos == GridPos)
                            tempLiminal = true;
                    if (!tempLiminal)
                    {
                        isLiminal = isOpen = false;
                        Sprite = new Sprite("doorWood_closed");
                    }
                }
            }
            SpeedModifier = isOpen ? 1 : 0;
        }

        public void Use(Player player, GameState gameState)
        {
            if (!isLiminal)
                isOpen = !isOpen;
            Sprite = new Sprite(isOpen ? "doorWood_open" : "doorWood_closed");
        }
    }
}
