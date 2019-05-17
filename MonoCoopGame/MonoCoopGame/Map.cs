using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Blocks;
using System;
using System.Collections.Generic;

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
                {
                    Point checkPoint = new Point(i, j);
                    if (TileMap[i, j].Type != Tile.TileType.Water)
                    {
                        Adjacencies[] adj = GetTileAdjacencies(checkPoint);
                        TileMap[i, j].UpdateSprites(adj);
                    }

                    if (BlockMap[i, j] != null && BlockMap[i, j] is BlobBlock)
                    {
                        Adjacencies adj = GetBlockAdjacencies(checkPoint);
                        ((BlobBlock)BlockMap[i, j]).UpdateSprite(adj);
                    }
                }
        }

        private void UpdateImagePart(Point gridPos)
        {
            for (int i = gridPos.X - 1; i <= gridPos.X + 1; i++)
                for (int j = gridPos.Y - 1; j <= gridPos.Y + 1; j++)
                {
                    Point checkPoint = new Point(i, j);
                    if (GridPointIsInMap(checkPoint))
                    {
                        Adjacencies[] tileAdj = GetTileAdjacencies(checkPoint);
                        TileMap[i, j].UpdateSprites(tileAdj);

                        if (BlockMap[i, j] != null && BlockMap[i, j] is BlobBlock)
                        {
                            Adjacencies adj = GetBlockAdjacencies(checkPoint);
                            ((BlobBlock)BlockMap[i, j]).UpdateSprite(adj);
                        }
                    }
                }
        }

        private Adjacencies[] GetTileAdjacencies(Point gridPos)
        {
            Tile.TileType thisType = TileMap[gridPos.X, gridPos.Y].Type;
            Adjacencies[] adj = new Adjacencies[(int)thisType];
            if (GridPointIsInMap(gridPos.X, gridPos.Y) && thisType != Tile.TileType.Water)
                for (int i = 0; i < (int)thisType; i++)
                {
                    adj[i].N = (gridPos.Y > 0) && (int)TileMap[gridPos.X, gridPos.Y - 1].Type > i;
                    adj[i].E = (gridPos.X < TileMap.GetUpperBound(0)) && (int)TileMap[gridPos.X + 1, gridPos.Y].Type > i;
                    adj[i].W = (gridPos.X > 0) && (int)TileMap[gridPos.X - 1, gridPos.Y].Type > i;
                    adj[i].S = (gridPos.Y < TileMap.GetUpperBound(1)) && (int)TileMap[gridPos.X, gridPos.Y + 1].Type > i;
                }
            return adj;
        }

        private Adjacencies GetBlockAdjacencies(Point gridPos)
        {
            string thisClass = ((BlobBlock)BlockMap[gridPos.X, gridPos.Y]).BlobGroup;
            Adjacencies adj = new Adjacencies();
            if (GridPointIsInMap(gridPos.X, gridPos.Y))
            {
                adj.N = (gridPos.Y > 0)
                    && BlockMap[gridPos.X, gridPos.Y - 1] is BlobBlock
                    && ((BlobBlock)BlockMap[gridPos.X, gridPos.Y - 1])?.BlobGroup == thisClass;

                adj.E = (gridPos.X < TileMap.GetUpperBound(0)) 
                    && BlockMap[gridPos.X - 1, gridPos.Y] is BlobBlock
                    && ((BlobBlock)BlockMap[gridPos.X - 1, gridPos.Y])?.BlobGroup == thisClass;

                adj.W = (gridPos.X > 0) 
                    && BlockMap[gridPos.X + 1, gridPos.Y] is BlobBlock
                    && ((BlobBlock)BlockMap[gridPos.X + 1, gridPos.Y])?.BlobGroup == thisClass;

                adj.S = (gridPos.Y < TileMap.GetUpperBound(1)) 
                    && BlockMap[gridPos.X, gridPos.Y + 1] is BlobBlock
                    && ((BlobBlock)BlockMap[gridPos.X, gridPos.Y + 1])?.BlobGroup == thisClass;
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

        public void ChangeTile(Point gridPos, Tile newTile)
        {
            TileMap[gridPos.X, gridPos.Y] = newTile;
            UpdateImagePart(gridPos);
        }

        public void Step(GameState gameState)
        {
            for (int i = 1; i < TileMap.GetUpperBound(0); i++)
                for (int j = 1; j < TileMap.GetUpperBound(1); j++)
                    BlockMap[i,j]?.Step(gameState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i < TileMap.GetUpperBound(0); i++)
                for (int j = 1; j < TileMap.GetUpperBound(1); j++)
                    TileMap[i, j].Draw(spriteBatch, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);

            foreach (Block block in Blocks)
                block.Draw(spriteBatch);
        }

        public void AddBlock(Block newBlock)
        {
            if (BlockMap[newBlock.GridPos.X, newBlock.GridPos.Y] == null)
            {
                Blocks.Add(newBlock);
                BlockMap[newBlock.GridPos.X, newBlock.GridPos.Y] = newBlock;
                if (newBlock is BlobBlock)
                    UpdateImagePart(newBlock.GridPos);
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
                if (block is BlobBlock)
                    UpdateImagePart(block.GridPos);
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

        public float GetSpeedModifierAtPos(Point pos)
        {
            Point gridPos = new Point(pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE);
            float speedMod = TileMap[gridPos.X, gridPos.Y].SpeedModifier;
            if(BlockMap[gridPos.X, gridPos.Y] != null)
                speedMod = BlockMap[gridPos.X, gridPos.Y].SpeedModifier;
            return speedMod;
        }
    }
}
