using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace monoCoopGame
{
    public class TileMap
    {
        public enum Layers
        {
            Dirt, Grass, Stone
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
                    if (i < 1 || j < 1 || j > yMapSize - 3 || i > xMapSize - 3)
                        Map[i, j] = new Tile(Tile.TileType.Stone);
                    else
                    {
                        int xPara = Math.Abs(i - Map.GetUpperBound(0) / 2);
                        int yPara = Math.Abs(j - Map.GetUpperBound(1) / 2);
                        if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                        {
                            if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                                Map[i, j] = new Tile(Tile.TileType.Grass);
                            else
                                Map[i, j] = new Tile(Tile.TileType.Stone);
                        }
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
                    if (Map[i, j].Type != Tile.TileType.Water)
                    {
                        Tile.Adjacencies[] adj = GetAdjacencies(i, j);
                        Map[i, j].UpdateSprites(adj);
                    }
        }

        private void UpdateImagePart(int gridX, int gridY)
        {
            for (int i = gridX - 1; i <= gridX + 1; i++)
                for (int j = gridY - 1; j <= gridY + 1; j++)
                    if (GridPointIsInMap(i, j))
                    {
                        Tile.Adjacencies[] adj = GetAdjacencies(i, j);
                        Map[i, j].UpdateSprites(adj);
                    }
        }

        private Tile.Adjacencies[] GetAdjacencies(int gridX, int gridY)
        {
            Tile.TileType thisType = Map[gridX, gridY].Type;
            Tile.Adjacencies[] adj = new Tile.Adjacencies[(int)thisType];
            if (GridPointIsInMap(gridX, gridY) && thisType != Tile.TileType.Water)
                for (int i = 0; i < (int)thisType; i++)
                {
                    adj[i].N = (gridY > 0) && (int)Map[gridX, gridY - 1].Type > i;
                    adj[i].E = (gridX < Map.GetUpperBound(0)) && (int)Map[gridX + 1, gridY].Type > i;
                    adj[i].W = (gridX > 0) && (int)Map[gridX - 1, gridY].Type > i;
                    adj[i].S = (gridY < Map.GetUpperBound(1)) && (int)Map[gridX, gridY + 1].Type > i;
                }
            return adj;
        }

        public bool GridPointIsInMap(int gridX, int gridY)
        {
            return (gridX >= 0 && gridY >= 0 && gridX <= Map.GetUpperBound(0) && gridY <= Map.GetUpperBound(1));
        }

        public void ChangeTile(int gridX, int gridY, Tile newTile)
        {
            Map[gridX, gridY] = newTile;
            UpdateImagePart(gridX, gridY);
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
