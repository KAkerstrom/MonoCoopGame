﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.Tiles
{
    public class Tile
    {
        public const int TILE_SIZE = 16;

        public Sprite Sprite;
        public Point GridPos { get; }
        public Point Pos { get { return new Point(GridPos.X * TILE_SIZE, GridPos.Y * TILE_SIZE); } }
        public float SpeedModifier = 1;
        public bool IsSolid { get { return SpeedModifier == 0; } }
        public bool HasTransparency = false;

        public Tile(Sprite sprite, Point gridPos)
        {
            GridPos = gridPos;
            Sprite = sprite;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Pos.X, Pos.Y);
        }
    }
}