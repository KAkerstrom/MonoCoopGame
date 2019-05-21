using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace monoCoopGame.Tiles
{
    class Bomb : Tile, IOwnable, ISteppable, IDestroyable
    {
        private static Texture2D[] frames;
        public event TileDestroyedDelegate TileDestroyed;

        public Player Owner { get; set; }
        public int Health { get; set; }
        public int InvulnFrames { get; set; } = 0;
        private int radius;

        public Bomb(Point gridPos, Player owner) : base(new Sprite("bomb0"), gridPos)
        {
            Owner = owner;
            Health = 2;
            radius = owner.BombPower;
            PopulateTextures();
            Sprite = new Sprite(frames, 50);
            Sprite.AnimationDone += Sprite_AnimationDone;
        }

        private void Sprite_AnimationDone()
        {
            Health = -100;
        }

        private void PopulateTextures()
        {
            if (frames == null)
            {
                frames = new Texture2D[3];
                for (int i = 0; i < 3; i++)
                    frames[i] = Sprite.GetTexture("bomb" + i);
            }
        }

        public void Step(GameState gameState)
        {
            Sprite.Update();
            Damage(0, gameState, Owner); // a bit of a hack, but it works
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
            Explosion explosion = new Explosion(radius, point, Owner, gameState);
            gameState.Map.AddExplosion(explosion);
        }
    }
}
