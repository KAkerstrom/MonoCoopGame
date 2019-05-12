using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace monoCoopGame
{
    public class Tile
    {
        public const int TILE_SIZE = 16;

        public struct Adjacencies
        {
            public bool N, E, W, S;
        }

        public enum TileType
        {
            Water, Dirt, Grass, Stone
        }

        public TileType Type { get; }
        public Sprite[] Sprites { get; set; }
        public Adjacencies Adjacency { get; set; }
        public float SpeedModifier { get; set; } = 1;
        public bool IsSolid { get { return SpeedModifier == 0; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="type">The tile type.</param>
        public Tile(TileType type)
        {
            Type = type;
            Sprites = new Sprite[(int)type];
            if (type == TileType.Dirt) SpeedModifier = 1.3f;
            else if (type == TileType.Water) SpeedModifier = 0.6f;
            else if (type == TileType.Stone) SpeedModifier = 0.7f;
        }

        public void UpdateSprites(Adjacencies[] adj)
        {
            for (int i = 0; i < adj.Length; i++)
            {
                StringBuilder texture = new StringBuilder(15);
                texture.Append(((TileType)(i + 1)).ToString().ToLower() + "_");
                if (!adj[i].N)
                    texture.Append("n");
                if (!adj[i].E)
                    texture.Append("e");
                if (!adj[i].W)
                    texture.Append("w");
                if (!adj[i].S)
                    texture.Append("s");
                Sprites[i] = new Sprite(Sprite.GetTexture(texture.ToString()));
            }
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            if (Type == TileType.Water)
            {
                Sprite waterSpr = new Sprite(Sprite.GetTexture("water_"));
                waterSpr.Draw(spriteBatch, x, y);
            }
            else
                for (int i = 0; i < (int)Type; i++)
                {
                    if (Sprites[i] != null)
                        Sprites[i].Draw(spriteBatch, x, y);

                }
        }
    }
}