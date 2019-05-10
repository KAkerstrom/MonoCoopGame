using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoCoopGame
{
    public class Tile
    {
        public const int TILE_SIZE = 16;

        public enum TileType
        {
            None, Water, Grass, Dirt, Stone
        }

        public TileType Type { get; }
        public Sprite Sprite { get; set; }
        public Sprite BgSprite { get; set; }
        public float SpeedModifier { get; set; } = 1;
        public bool IsSolid { get { return SpeedModifier == 0; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="type">The tile type.</param>
        public Tile(TileType type)
        {
            Type = type;
            switch (type)
            {
                case TileType.Water:
                    SetProperties("water", 0.4f);
                    break;
                case TileType.Dirt:
                    SetProperties("dirt", 1.3f);
                    break;
                case TileType.Stone:
                    SetProperties("stone", 1f);
                    break;
            }
        }

        /// <summary>
        /// Sets the tile type and applies attributes for that type.
        /// </summary>
        /// <param name="type">The tile type.</param>
        private void SetProperties(string texturePrefix, float speedModifier)
        {
            Sprite = new Sprite(Sprite.GetTexture(texturePrefix + "_"));
            SpeedModifier = speedModifier;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            Sprite.Draw(spriteBatch, x, y);
        }
    }
}