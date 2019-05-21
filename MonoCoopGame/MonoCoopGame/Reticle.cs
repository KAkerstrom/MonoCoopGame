using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
using System.Linq;

namespace monoCoopGame
{
    public class Reticle
    {
        private Player player;

        public Point Pos
        {
            get
            {
                return new Point(GridPos.X * Tile.TILE_SIZE, GridPos.Y * Tile.TILE_SIZE);
            }
        }

        public Point GridPos
        {
            get
            {
                switch (player.Facing)
                {
                    case Directions.North: return new Point(player.GridPos.X, player.GridPos.Y - 1);
                    case Directions.East: return new Point(player.GridPos.X + 1, player.GridPos.Y);
                    case Directions.West: return new Point(player.GridPos.X - 1, player.GridPos.Y);
                    case Directions.South: return new Point(player.GridPos.X, player.GridPos.Y + 1);
                    default: return new Point(player.GridPos.X, player.GridPos.Y);
                }
            }
        }

        public Reticle(Player player)
        {
            this.player = player;
        }

        public void Draw(SpriteBatch spriteBatch, float alpha)
        {
            Rectangle drawRect = new Rectangle(Pos.X, Pos.Y, Tile.TILE_SIZE, Tile.TILE_SIZE);
            spriteBatch.Draw(Sprite.GetTexture("reticle"), drawRect, null, Color.White * alpha, 0, new Vector2(Tile.TILE_SIZE / 2, Tile.TILE_SIZE / 2), SpriteEffects.None, (float)Pos.Y / (24 * Tile.TILE_SIZE)); //TODO: Change to map height variable
        }
    }
}
