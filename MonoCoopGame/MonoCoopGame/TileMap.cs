using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class TileMap
    {
        private static Sprite waterSprite;

        public enum Layers
        {
            Dirt, Grass, Blocks
        }

        public int Width, Height;
        private Tile[][,] Tiles;

        public TileMap(int width, int height)
        {
            Width = width - 1;
            Height = height - 1;
            Tiles = new Tile[3][,];
            for (int i = 0; i < 3; i++)
                Tiles[i] = new Tile[width, height];
            CreateDebugMap(width, height);

            waterSprite = new Sprite("water_");
        }

        private void CreateDebugMap(int width, int height)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (i > 1 && j > 1 && j < height - 2 && i < width - 2)
                    {
                        int xPara = Math.Abs(i - width / 2);
                        int yPara = Math.Abs(j - height / 2);
                        if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 13 && Utility.R.Next(30) > 0)
                        {
                            Tiles[(int)Layers.Dirt][i, j] = new Dirt(new Point(i, j));
                            Tiles[(int)Layers.Grass][i, j] = new Grass(new Point(i, j));
                        }
                    }
                }

            UpdateAdjacencyFull(Layers.Dirt);
            UpdateAdjacencyFull(Layers.Grass);
        }

        public void UpdateAdjacencyFull(Layers layer)
        {
            Tile[,] map = Tiles[(int)layer];
            for (int i = 0; i <= map.GetUpperBound(0); i++)
                for (int j = 0; j <= map.GetUpperBound(1); j++)
                    if (map[i, j] != null && map[i, j] is Blob)
                        ((Blob)map[i, j]).UpdateAdjacency(map);
        }

        public void UpdateAdjacencyPart(Layers layer, Point gridPos)
        {
            Tile[,] map = Tiles[(int)layer];
            for (int i = gridPos.X - 1; i <= gridPos.X + 1; i++)
                for (int j = gridPos.Y - 1; j <= gridPos.Y + 1; j++)
                    if (map[i, j] != null && map[i, j] is Blob)
                        ((Blob)map[i, j]).UpdateAdjacency(map);
        }

        public bool IsGridPosInMap(Point gridPos)
        {
            return (gridPos.X >= 1 && gridPos.Y >= 1 && gridPos.X <= Width - 1 && gridPos.Y <= Height - 1);
        }

        public void Step(GameState gameState)
        {
            for (int i = 1; i < Width; i++)
                for (int j = 1; j < Height; j++)
                    if (Tiles[(int)Layers.Blocks][i, j] != null && Tiles[(int)Layers.Blocks][i, j] is ISteppable)
                        ((ISteppable)Tiles[(int)Layers.Blocks][i, j]).Step(gameState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i < Width; i++)
                for (int j = 1; j < Height; j++)
                {
                    // Check each layer to see if it's transparent
                    // Once a non-transparent layer is found, draw from there up
                    int layer = (int)Layers.Blocks;
                    bool goingUp = false;
                    while (layer <= (int)Layers.Blocks)
                    {
                        if (goingUp)
                        {
                            if (Tiles[layer][i, j] != null)
                                Tiles[layer][i, j].Draw(spriteBatch);
                        }
                        else
                        {
                            if (layer < 0)
                            {
                                waterSprite.Draw(spriteBatch, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);
                                goingUp = true;
                            }
                            else if (Tiles[layer][i, j] != null && !Tiles[layer][i, j].HasTransparency)
                            {
                                Tiles[layer][i, j].Draw(spriteBatch);
                                goingUp = true;
                            }
                        }
                        layer += goingUp ? 1 : -1;
                    }
                }
        }

        public void AddTile(Tile tile)
        {
            AddTile(Layers.Blocks, tile);
        }

        public void AddTile(Layers layer, Tile tile)
        {
            if (Tiles[(int)layer][tile.GridPos.X, tile.GridPos.Y] == null)
            {
                Tiles[(int)layer][tile.GridPos.X, tile.GridPos.Y] = tile;
                if (tile is Blob)
                    UpdateAdjacencyPart(layer, tile.GridPos);
                if (tile is IDestroyable)
                    ((IDestroyable)tile).TileDestroyed += TileDestroyed;
            }
            else
                throw new Exception("Attempted to overwrite block.");
        }

        private void TileDestroyed(Tile tile, Player player)
        {
            RemoveTile(Layers.Blocks, tile.GridPos);
        }

        public void RemoveBlock(Point gridPos)
        {
            RemoveTile(Layers.Blocks, gridPos);
        }

        public void RemoveTile(Layers layer, Tile tile)
        {
            RemoveTile(layer, tile.GridPos);
        }

        public void RemoveTile(Layers layer, Point gridPos)
        {
            Tile tile = Tiles[(int)layer][gridPos.X, gridPos.Y];
            Tiles[(int)layer][gridPos.X, gridPos.Y] = null;
            if (tile != null && tile is Blob)
                UpdateAdjacencyPart(layer, gridPos);
        }

        public bool IsTileAtPos(Layers layer, Point pos)
        {
            return Tiles[(int)layer][pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE] != null;
        }

        public bool IsBlockAtPos(Point pos)
        {
            return Tiles[(int)Layers.Blocks][pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE] != null;
        }

        public bool IsTileAtPos(Point pos)
        {
            for (int i = 0; i <= (int)Layers.Blocks; i++)
                if (Tiles[i][pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE] != null)
                    return true;
            return false;
        }


        public bool IsTileAtGridPos(Layers layer, Point gridPos)
        {
            return Tiles[(int)layer][gridPos.X, gridPos.Y] != null;
        }

        public bool IsTileAtGridPos(Point gridPos)
        {
            for (int i = 0; i <= (int)Layers.Blocks; i++)
                if (Tiles[i][gridPos.X, gridPos.Y] != null)
                    return true;
            return false;
        }

        public Tile GetBlockAtPos(Point pos)
        {
            return GetTileAtPos(Layers.Blocks, pos);
        }

        public Tile GetTileAtPos(Layers layer, Point pos)
        {
            return Tiles[(int)layer][pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE];
        }

        public Tile GetBlockAtGridPos(Point gridPos)
        {
            return GetTileAtGridPos(Layers.Blocks, gridPos);
        }

        public Tile GetTileAtGridPos(Layers layer, Point gridPos)
        {
            return Tiles[(int)layer][gridPos.X, gridPos.Y];
        }

        public float GetSpeedModifier(Point pos)
        {
            Point gridPos = new Point(pos.X / Tile.TILE_SIZE, pos.Y / Tile.TILE_SIZE);
            float speedMod = 0.5f;
            for (int i = 0; i <= (int)Layers.Blocks; i++)
                if (Tiles[i][gridPos.X, gridPos.Y] != null)
                    speedMod = Tiles[i][gridPos.X, gridPos.Y].SpeedModifier;
            return speedMod;
        }
    }
}
