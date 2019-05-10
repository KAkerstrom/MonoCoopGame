using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace monoCoopGame
{
    public class TileMap
    {
        public struct Adjacency
        {
            public bool N, E, W, S;
        }

        public enum Layers
        {
            Dirt, Grass, Stone
        }

        public Sprite[,,] Map { get; set; }

        /// <summary>
        /// Creates a new tile-map.
        /// </summary>
        public TileMap(int xMapSize, int yMapSize)
        {
            if (xMapSize < 1 || yMapSize < 1)
                throw new Exception("Cannot create a map with less than 1 tile.");

            Map = new Sprite[3, xMapSize, yMapSize];

            for (int layer = 0; layer < 3; layer++)
            {
                for (int i = 0; i < xMapSize; i++)
                    for (int j = 0; j < yMapSize; j++)
                    {
                        if (i < 2 || j < 2 || j > yMapSize - 2 || i > xMapSize - 2)
                            Map[(int)Layers.Stone, i, j] = new Sprite();
                        else
                        {
                            int xPara = Math.Abs(i - Map.GetUpperBound(1) / 2);
                            int yPara = Math.Abs(j - Map.GetUpperBound(2) / 2);
                            if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                            {
                                Map[(int)Layers.Dirt, i, j] = new Sprite();
                                if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                                    Map[(int)Layers.Grass, i, j] = new Sprite();
                            }
                        }
                    }
            }

            UpdateFullImage();
        }

        private void UpdateFullImage()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j <= Map.GetUpperBound(1); j++)
                    for (int k = 0; k <= Map.GetUpperBound(2); k++)
                        if (Map[i, j, k] != null)
                        {
                            Adjacency adj = GetAdjacency((Layers)i, j, k);
                            Map[i, j, k] = new Sprite(GetTileTexture((Layers)i, adj));
                        }
        }

        private void UpdateImagePart(Layers layer, int gridX, int gridY)
        {
            if (!PointIsInMap(gridX, gridY))
                return;

            UpdateSprite(layer, gridX, gridY);
            if (PointIsInMap(gridX - 1, gridY)) UpdateSprite(layer, gridX - 1, gridY);
            if (PointIsInMap(gridX + 1, gridY)) UpdateSprite(layer, gridX + 1, gridY);
            if (PointIsInMap(gridX, gridY - 1)) UpdateSprite(layer, gridX, gridY - 1);
            if (PointIsInMap(gridX, gridY + 1)) UpdateSprite(layer, gridX, gridY + 1);
        }

        private void UpdateSprite(Layers layer, int gridX, int gridY)
        {
            Adjacency adj = GetAdjacency(layer, gridX, gridY);
            Map[(int)layer, gridX, gridY] = new Sprite(GetTileTexture(layer, adj));
        }

        private Adjacency GetAdjacency(Layers layer, int gridX, int gridY)
        {
            if (!PointIsInMap(gridX, gridY))
                return new Adjacency();

            Adjacency adj = new Adjacency();
            if (Map[(int)layer, gridX, gridY] != null)
            {
                adj.N = (gridY > 0) && Map[(int)layer, gridX, gridY - 1] != null;
                adj.E = (gridX < Map.GetUpperBound(1)) && Map[(int)layer, gridX + 1, gridY] != null;
                adj.W = (gridX > 0) && Map[(int)layer, gridX - 1, gridY] != null;
                adj.S = (gridY < Map.GetUpperBound(2)) && Map[(int)layer, gridX, gridY + 1] != null;
            }
            return adj;
        }

        public bool PointIsInMap(int gridX, int gridY)
        {
            return (gridX >= 0 && gridY >= 0 && gridX <= Map.GetUpperBound(1) && gridY <= Map.GetUpperBound(2));
        }

        public void AddTile(Layers layer, int gridX, int gridY)
        {
            if (PointIsInMap(gridX, gridY))
            {
                Map[(int)layer, gridX, gridY] = new Sprite();
                UpdateImagePart(layer, gridX, gridY);
            }
        }

        public void RemoveTile(Layers layer, int gridX, int gridY)
        {
            if (PointIsInMap(gridX, gridY))
            {
                Map[(int)layer, gridX, gridY] = null;
                UpdateImagePart(layer, gridX, gridY);
            }
        }

        public static Texture2D GetTileTexture(Layers layer, Adjacency adj)
        {
            StringBuilder texture = new StringBuilder(15);
            texture.Append(layer.ToString().ToLower() + "_");

            if (!adj.N)
                texture.Append("n");
            if (!adj.E)
                texture.Append("e");
            if (!adj.W)
                texture.Append("w");
            if (!adj.S)
                texture.Append("s");

            return Sprite.GetTexture(texture.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < Map.GetUpperBound(1); j++)
                    for (int k = 0; k < Map.GetUpperBound(2); k++)
                        if (Map[i, j, k] != null)
                            Map[i, j, k].Draw(spriteBatch, j * Tile.TILE_SIZE, k * Tile.TILE_SIZE);
        }

        public bool IsTileAtPoint(int x, int y)
        {
            for (int i = 0; i < 3; i++)
                if (Map[i, x / Tile.TILE_SIZE, y / Tile.TILE_SIZE] != null)
                    return false;
            return true;
        }

        public bool IsTileAtPoint(Layers layer, int x, int y)
        {
            return Map[(int)layer, x / Tile.TILE_SIZE, y / Tile.TILE_SIZE] != null;
        }
    }
}
