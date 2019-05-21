using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.Tiles
{
    public delegate void ExplosionDestroyedDelegate(Explosion explosion);

    public class Explosion : Tile, ISteppable, IOwnable
    {
        public event ExplosionDestroyedDelegate ExplosionDestroyed;
        private static Texture2D[] frames;
        public Player Owner { get; set; }

        private int TTL;
        private bool propogated = false;

        public Explosion(int ttl, Point gridPos, Player owner, GameState gameState) : base(null, gridPos)
        {
            GetSprite();
            Owner = owner;
            Sprite = new Sprite(frames, 3);
            Sprite.AnimationDone += Sprite_AnimationDone;
            TTL = ttl;

            Tile explodedTile = gameState.Map.GetBlockAtGridPos(gridPos);
            if (explodedTile != null)
            {
                if (explodedTile is IDestroyable)
                    ((IDestroyable)explodedTile).Damage(TTL + 1, gameState, owner);
                if (explodedTile.IsSolid)
                    TTL = 0;
                if (((IDestroyable)explodedTile).Health <= 0)
                {
                    if (gameState.Map.GetTileAtGridPos(TileMap.Layers.Grass, GridPos) != null)
                        gameState.Map.RemoveTile(TileMap.Layers.Grass, GridPos);
                }
            }
            else
            {
                if (gameState.Map.GetTileAtGridPos(TileMap.Layers.Grass, GridPos) != null)
                    gameState.Map.RemoveTile(TileMap.Layers.Grass, GridPos);
            }
            foreach (Player player in gameState.Players)
                player.Damage(TTL + 1, gameState, owner);
        }

        private void Sprite_AnimationDone()
        {
            ExplosionDestroyed?.Invoke(this);
        }

        public void Step(GameState gameState)
        {
            Sprite.Update();
            if (Sprite.ImageIndex == 1 && !propogated && TTL > 0)
            {
                propogated = true;
                Point[] propogations = new Point[]
                {
                    new Point(GridPos.X - 1, GridPos.Y),
                    new Point(GridPos.X + 1, GridPos.Y),
                    new Point(GridPos.X, GridPos.Y - 1),
                    new Point(GridPos.X, GridPos.Y + 1)
                };
                foreach (Point check in propogations)
                    if (!gameState.Map.IsExplosionAtGridPos(check))
                        gameState.Map.AddExplosion(new Explosion(TTL - 1, check, Owner, gameState));
            }
        }

        private static void GetSprite()
        {
            if (frames == null)
            {
                frames = new Texture2D[4];
                for (int i = 0; i < 4; i++)
                    frames[i] = Sprite.GetTexture("explosion" + i);
            }
        }
    }
}
