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

        #region Enums & Structs

        public enum TileType
        {
            None, Water, Grass, Dirt
        }

        #endregion Enums & Structs

        #region Static

        public static Tile[,] Map { get; set; }

        #endregion Static

        #region Private Fields

        private TileType type;
        private int x;
        private int y;

        #endregion Private Fields

        #region Public Properties

        public double SpeedModifier { get; set; } = 1;
        public bool IsSolid { get; private set; }

        public TileType Type
        {
            get => type;
            set { SetType(value); }
        }

        public Sprite Sprite { get; set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="type">The tile type.</param>
        public Tile(TileType type, int x, int y)
        {
            SetType(type);
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Sets the tile type and applies attributes for that type.
        /// </summary>
        /// <param name="type">The tile type.</param>
        private void SetType(TileType type)
        {
            this.type = type;
            switch (type)
            {
                case TileType.Water:
                    IsSolid = true;
                    SpeedModifier = 0.4;
                    Sprite = new Sprite(Sprite.GetTexture("water_"));
                    break;
                case TileType.Grass:
                    IsSolid = false;
                    SpeedModifier = 1;
                    Sprite = new Sprite(Sprite.GetTexture("stone_"));
                    break;
                case TileType.Dirt:
                    IsSolid = false;
                    SpeedModifier = 1.5;
                    Sprite = new Sprite(Sprite.GetTexture("stone_"));
                    break;
            }
        }

        #endregion Private Methods

        #region Public Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (Sprite != null)
            Sprite.Draw(spriteBatch, x, y);
        }

        #endregion Public Methods

    }
}