using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.Tiles
{
    public class Tile
    {
        public const int TILE_SIZE = 16;
        public const int INVULN_TIME = 15;

        public Sprite Sprite;
        public Point GridPos { get; }
        public Point Pos { get { return new Point(GridPos.X * TILE_SIZE, GridPos.Y * TILE_SIZE); } }
        public float SpeedModifier = 1;
        public bool IsSolid { get { return SpeedModifier == 0; } }
        public bool HasTransparency = true;
        public float Depth { get; protected set; }

        private float rotation = 0;

        public Tile(Sprite sprite, Point gridPos)
        {
            GridPos = gridPos;
            Sprite = sprite;
            Depth = (float)(GridPos.Y * TILE_SIZE) / (30 * TILE_SIZE); // ratio of y position to map height
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this is IDestroyable && ((IDestroyable)this).InvulnFrames > 0)
            {
                ((IDestroyable)this).InvulnFrames -= 1;
                int invuln = ((IDestroyable)this).InvulnFrames;
                rotation = MathHelper.Clamp(rotation + (invuln % 5 / 8f) - 0.3f, -0.5f, 0.5f);
                Sprite.Draw(spriteBatch, Pos.X, Pos.Y, Depth, rotation);
            }
            else
            {
                rotation = 0;
                Sprite.Draw(spriteBatch, Pos.X, Pos.Y, Depth);
            }
        }
    }
}
