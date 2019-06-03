using Microsoft.Xna.Framework;

namespace monoCoopGame.Tiles
{
    class Mine : Tile, IOwnable, ISteppable, IDestroyable
    {
        public event TileDestroyedDelegate TileDestroyed;

        public Player Owner { get; set; }
        public int Health { get; set; }
        public int InvulnFrames { get; set; } = 0;
        private int radius;
        private int hideTimer = 100;

        public Mine(Point gridPos, Player owner) : base(new Sprite("mine_visible"), gridPos)
        {
            Owner = owner;
            Health = 2;
            radius = 1;
        }

        public void Step(GameState gameState)
        {
            if (hideTimer > 0)
            {
                if (--hideTimer == 0)
                    Sprite = new Sprite("mine_invisible");
            }
            else
            {
                foreach (Player p in gameState.Players)
                    if (p.GridPos == GridPos)
                    {
                        Damage(100, gameState, p);
                        break;
                    }
            }
        }

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy(gameState, player);
        }

        public void Destroy(GameState gameState, Player player = null)
        {
            TileDestroyed?.Invoke(this, player);

            Point point = new Point(GridPos.X, GridPos.Y);
            Explosion explosion = new Explosion(Utility.R.Next(), radius, point, Owner, gameState);
            gameState.AddEntity(explosion);
        }
    }
}
