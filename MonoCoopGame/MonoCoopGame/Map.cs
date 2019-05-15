using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoCoopGame
{
    public class Map
    {
        public enum Layers
        {
            Dirt, Grass, Stone
        }

        public Tile[,] TileMap { get; set; }
        public Block[,] BlockMap { get; private set; }
        public List<Block> Blocks { get; private set; }

        /// <summary>
        /// Creates a new tile-map.
        /// </summary>
        public Map(int xMapSize, int yMapSize)
        {
            if (xMapSize < 1 || yMapSize < 1)
                throw new Exception("Cannot create a map with less than 1 tile.");

            TileMap = new Tile[xMapSize, yMapSize];
            BlockMap = new Block[xMapSize, yMapSize];
            Blocks = new List<Block>();

            for (int i = 0; i < xMapSize; i++)
                for (int j = 0; j < yMapSize; j++)
                {
                    if (i < 1 || j < 1 || j > yMapSize - 2 || i > xMapSize - 2)
                        TileMap[i, j] = new Tile(Tile.TileType.Water);
                    else
                    {
                        int xPara = Math.Abs(i - TileMap.GetUpperBound(0) / 2);
                        int yPara = Math.Abs(j - TileMap.GetUpperBound(1) / 2);
                        if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                        {
                            if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                                TileMap[i, j] = new Tile(Tile.TileType.Grass);
                            else
                                TileMap[i, j] = new Tile(Tile.TileType.Stone);
                        }
                        else
                            TileMap[i, j] = new Tile(Tile.TileType.Water);
                    }
                }

            UpdateFullImage();
        }

        private void UpdateFullImage()
        {
            for (int i = 0; i <= TileMap.GetUpperBound(0); i++)
                for (int j = 0; j <= TileMap.GetUpperBound(1); j++)
                    if (TileMap[i, j].Type != Tile.TileType.Water)
                    {
                        Tile.Adjacencies[] adj = GetAdjacencies(i, j);
                        TileMap[i, j].UpdateSprites(adj);
                    }
        }

        private void UpdateImagePart(int gridX, int gridY)
        {
            for (int i = gridX - 1; i <= gridX + 1; i++)
                for (int j = gridY - 1; j <= gridY + 1; j++)
                    if (GridPointIsInMap(i, j))
                    {
                        Tile.Adjacencies[] adj = GetAdjacencies(i, j);
                        TileMap[i, j].UpdateSprites(adj);
                    }
        }

        private Tile.Adjacencies[] GetAdjacencies(int gridX, int gridY)
        {
            Tile.TileType thisType = TileMap[gridX, gridY].Type;
            Tile.Adjacencies[] adj = new Tile.Adjacencies[(int)thisType];
            if (GridPointIsInMap(gridX, gridY) && thisType != Tile.TileType.Water)
                for (int i = 0; i < (int)thisType; i++)
                {
                    adj[i].N = (gridY > 0) && (int)TileMap[gridX, gridY - 1].Type > i;
                    adj[i].E = (gridX < TileMap.GetUpperBound(0)) && (int)TileMap[gridX + 1, gridY].Type > i;
                    adj[i].W = (gridX > 0) && (int)TileMap[gridX - 1, gridY].Type > i;
                    adj[i].S = (gridY < TileMap.GetUpperBound(1)) && (int)TileMap[gridX, gridY + 1].Type > i;
                }
            return adj;
        }

        public bool GridPointIsInMap(Point gridPoint)
        {
            return GridPointIsInMap(gridPoint.X, gridPoint.Y);
        }

        public bool GridPointIsInMap(int gridX, int gridY)
        {
            return (gridX >= 1 && gridY >= 1 && gridX <= TileMap.GetUpperBound(0) - 1 && gridY <= TileMap.GetUpperBound(1) - 1);
        }

        public void ChangeTile(int gridX, int gridY, Tile newTile)
        {
            TileMap[gridX, gridY] = newTile;
            UpdateImagePart(gridX, gridY);
        }

        public void Step(GameState gameState)
        {
            foreach (Block b in Blocks)
                b.Step(gameState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i < TileMap.GetUpperBound(0); i++)
                for (int j = 1; j < TileMap.GetUpperBound(1); j++)
                    TileMap[i, j].Draw(spriteBatch, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);

            foreach (Block block in Blocks)
                block.Draw(spriteBatch);
        }

        public Tile GetTileAtPoint(int x, int y)
        {
            return TileMap[x / Tile.TILE_SIZE, y / Tile.TILE_SIZE];
        }

        public void AddBlock(Block newBlock)
        {
            if (BlockMap[newBlock.GridPos.X, newBlock.GridPos.Y] == null)
            {
                Blocks.Add(newBlock);
                BlockMap[newBlock.GridPos.X, newBlock.GridPos.Y] = newBlock;
                newBlock.BlockDestroyed += BlockDestroyed;
            }
            else
                throw new Exception("Attempted to overwrite block.");
        }

        private void BlockDestroyed(Block block, Player player)
        {
            RemoveBlock(block);
        }

        private void RemoveBlock(Block block)
        {
            if (Blocks.Contains(block))
            {
                Blocks.Remove(block);
                BlockMap[block.GridPos.X, block.GridPos.Y] = null;
            }
        }

        public bool IsBlockAtGridPos(Point gridPos)
        {
            return BlockMap[gridPos.X, gridPos.Y] != null;
        }

        public bool IsBlockAtPos(Point pos)
        {
            return BlockMap[pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE] != null;
        }

        public Block GetBlockAtPos(Point pos)
        {
            return BlockMap[pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE];
        }

        public Block GetBlockAtGridPos(Point gridPos)
        {
            return BlockMap[gridPos.X, gridPos.Y];
        }

        public Tile GetTileAtPos(Point pos)
        {
            return TileMap[pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE];
        }

        public Tile GetTileAtGridPos(Point gridPos)
        {
            return TileMap[gridPos.X, gridPos.Y];
        }
    }
}
