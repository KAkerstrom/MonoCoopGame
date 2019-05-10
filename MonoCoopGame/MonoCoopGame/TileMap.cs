using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoCoopGame
{
    public class TileMap
    {
        public struct Adjacency
        {
            public bool N, E, W, S;
        }

        public Tile[,] Map { get; set; }

        /// <summary>
        /// Creates a new tile-map.
        /// </summary>
        public TileMap(int xMapSize, int yMapSize)
        {
            if (xMapSize < 1 || yMapSize < 1)
                throw new Exception("Cannot create a map with less than 1 tile.");

            Map = new Tile[xMapSize, yMapSize];

            for (int i = 0; i < xMapSize; i++)
                for (int j = 0; j < yMapSize; j++)
                {
                    if (i < 2 || j < 2 || j > yMapSize - 2 || i > xMapSize - 2)
                        Map[i, j] = new Tile(Tile.TileType.Water);
                    else
                    {
                        int xPara = Math.Abs(i - Map.GetUpperBound(0) / 2);
                        int yPara = Math.Abs(j - Map.GetUpperBound(1) / 2);
                        if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                            Map[i, j] = new Tile(Utility.R.Next(2) == 0 ? Tile.TileType.Stone : Tile.TileType.Dirt);
                        else
                            Map[i, j] = new Tile(Tile.TileType.Water);
                    }
                }

            UpdateFullImage();
        }

        private void UpdateFullImage()
        {
            for (int i = 0; i <= Map.GetUpperBound(0); i++)
                for (int j = 0; j <= Map.GetUpperBound(1); j++)
                    UpdateAdjacency(i, j);
        }

        private void UpdateImagePart(int gridX, int gridY)
        {
            if (!PointIsInMap(gridX, gridY))
                return;

            UpdateAdjacency(gridX, gridY);
            if (PointIsInMap(gridX - 1, gridY)) UpdateAdjacency(gridX - 1, gridY);
            if (PointIsInMap(gridX + 1, gridY)) UpdateAdjacency(gridX + 1, gridY);
            if (PointIsInMap(gridX, gridY - 1)) UpdateAdjacency(gridX, gridY - 1);
            if (PointIsInMap(gridX, gridY + 1)) UpdateAdjacency(gridX, gridY + 1);
        }

        private void UpdateAdjacency(int gridX, int gridY)
        {
            if (!PointIsInMap(gridX, gridY))
                return;

            if (Map[gridX, gridY].Type == Tile.TileType.Stone)
            {
                Adjacency adj = GetAdjacency(gridX, gridY, Tile.TileType.Stone);
                Adjacency dirtAdj = GetAdjacency(gridX, gridY, Tile.TileType.Dirt);
                adj.N |= dirtAdj.N;
                adj.W |= dirtAdj.W;
                adj.E |= dirtAdj.E;
                adj.S |= dirtAdj.S;
                Map[gridX, gridY].Sprite = new Sprite(GetTileTexture(Tile.TileType.Stone, adj));
            }
            else if (Map[gridX, gridY].Type == Tile.TileType.Dirt)
            {
                Adjacency adj = GetAdjacency(gridX, gridY, Tile.TileType.Dirt);
                Map[gridX, gridY].Sprite = new Sprite(GetTileTexture(Tile.TileType.Dirt, adj));
            }
        }

        public void LoadMap(Tile[,] map)
        {
            Map = map;
            UpdateFullImage();
        }

        private Adjacency GetAdjacency(int gridX, int gridY, Tile.TileType type)
        {
            Adjacency adj = new Adjacency();
            adj.N = (gridY > 0) && type == Map[gridX, gridY - 1].Type;
            adj.W = (gridX > 0) && type == Map[gridX - 1, gridY].Type;
            adj.E = (gridX < Map.GetUpperBound(0)) && type == Map[gridX + 1, gridY].Type;
            adj.S = (gridY < Map.GetUpperBound(1)) && type == Map[gridX, gridY + 1].Type;
            return adj;
        }

        public bool PointIsInMap(int gridX, int gridY)
        {
            return (gridX >= 0 && gridY >= 0 && gridX <= Map.GetUpperBound(0) && gridY <= Map.GetUpperBound(1));
        }

        public void ChangeTile(int gridX, int gridY, Tile.TileType type)
        {
            if (PointIsInMap(gridX, gridY) && Map[gridX, gridY].Type != type)
            {
                Map[gridX, gridY] = new Tile(type);
                UpdateImagePart(gridX, gridY);
            }
        }

        public static Texture2D GetTileTexture(Tile.TileType tileType, Adjacency adj)
        {
            StringBuilder texture = new StringBuilder(15);
            texture.Append(tileType.ToString().ToLower() + "_");

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
            for (int i = 0; i < Map.GetUpperBound(0); i++)
                for (int j = 0; j < Map.GetUpperBound(1); j++)
                    Map[i, j].Draw(spriteBatch, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);
        }

        public Tile GetTileAtPoint(int x, int y)
        {
            return Map[x / Tile.TILE_SIZE, y / Tile.TILE_SIZE];
        }
    }
}
