using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;

namespace monoCoopGame.Powerups
{
    public abstract class Powerup : Tile, IEntity, ISteppable, IDestroyable
    {
        public event EntityDestroyedDelegate EntityDestroyed;
        public event TileDestroyedDelegate TileDestroyed;

        public static Sprite BgSprite;

        public int Health { get; set; } = 10;
        public int InvulnFrames { get; set; } = 0;

        public Powerup(Sprite sprite, Point gridPos) : base(sprite, gridPos)
        {
            PopulateTextures();
        }

        private void PopulateTextures()
        {
            if(BgSprite == null)
            {
                Texture2D[] frames = new Texture2D[]
                {
                    Sprite.GetTexture("powerup0"),
                    Sprite.GetTexture("powerup1"),
                    Sprite.GetTexture("powerup2"),
                    Sprite.GetTexture("powerup1"),
                };
                BgSprite = new Sprite(frames, 5);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BgSprite.Draw(spriteBatch, Pos);
            base.Draw(spriteBatch);
        }

        public void Step(GameState gameState)
        {
            foreach (Player p in gameState.Players)
                if (p.GridPos == GridPos)
                {
                    Activate(gameState, p);
                    Destroy(gameState, p);
                    break;
                }
        }

        public abstract void Activate(GameState gameState, Player player);

        public void Damage(int damage, GameState gameState, Player player = null)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy(gameState, player);
        }

        public void Destroy(GameState gameState, Player player = null)
        {
            TileDestroyed?.Invoke(this, player);
            EntityDestroyed?.Invoke(this);
        }
    }
}
