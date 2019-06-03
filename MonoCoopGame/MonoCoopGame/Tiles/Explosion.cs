using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace monoCoopGame.Tiles
{
    public class Explosion : Tile, ISteppable, IOwnable, IEntity
    {
        public event EntityDestroyedDelegate EntityDestroyed;

        private static Texture2D[] frames;
        public Player Owner { get; set; }

        private int TTL;
        private int id;
        private bool propogated = false;

        public Explosion(int id, int ttl, Point gridPos, Player owner, GameState gameState) : base(null, gridPos)
        {
            this.id = id;
            GetSprite();
            Owner = owner;
            Sprite = new Sprite(frames, 3);
            Sprite.AnimationDone += Sprite_AnimationDone;
            TTL = ttl;
            int damage = ttl + 3;

            Tile explodedTile = gameState.Map.GetBlockAtGridPos(gridPos);
            if (explodedTile != null)
            {
                if (explodedTile is IDestroyable)
                    ((IDestroyable)explodedTile).Damage(damage, gameState, owner);
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
                if (player.GridPos == GridPos)
                    player.Damage(damage, gameState, owner);
        }

        private void Sprite_AnimationDone()
        {
            EntityDestroyed?.Invoke(this);
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
                List<IEntity> explosions = gameState.Entities.FindAll(x => x is Explosion);
                foreach (Point check in propogations)
                    if (gameState.Map.IsGridPosInMap(GridPos))
                    {
                        bool canPlace = true;
                        foreach (IEntity entity in explosions)
                            if (entity.GridPos == check && ((Explosion)entity).id == id)
                            {
                                canPlace = false;
                                break;
                            }
                        if (canPlace)
                            gameState.AddEntity(new Explosion(id, TTL - 1, check, Owner, gameState));
                    }
            }
        }

        private static void GetSprite()
        {
            if (frames == null)
            {
                frames = new Texture2D[8];
                for (int i = 0; i < 4; i++)
                    frames[i] = Sprite.GetTexture("explosion" + i);
                // Jank, but this prolongs the animation to avoid an issue with
                // explosions looping in on themselves.
                // Should eventually fix this properly, but this will do for now
                for (int i = 0; i < 4; i++)
                    frames[i + 4] = Sprite.GetTexture("null");
            }
        }
    }
}
