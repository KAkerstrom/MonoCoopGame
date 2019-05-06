using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoCoopGame
{
    public class TileMap
    {

        public enum TileLayer
        {
            Background, Foreground, Item
        }

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
                        Map[i, j] = new Tile(Tile.TileType.Water, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);
                    else
                    {
                        int xPara = Math.Abs(i - Map.GetUpperBound(0) / 2);
                        int yPara = Math.Abs(j - Map.GetUpperBound(1) / 2);
                        if (Utility.R.Next(xPara < yPara ? yPara : xPara) < 10 && Utility.R.Next(30) > 0)
                            Map[i, j] = new Tile(Tile.TileType.Grass, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);
                        else
                            Map[i, j] = new Tile(Tile.TileType.Water, i * Tile.TILE_SIZE, j * Tile.TILE_SIZE);
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

            if (Map[gridX, gridY].Type == Tile.TileType.Grass)
            {
                Adjacency adj = GetAdjacency(gridX, gridY, Tile.TileType.Grass);
                Adjacency dirtAdj = GetAdjacency(gridX, gridY, Tile.TileType.Dirt);
                adj.N |= dirtAdj.N;
                adj.W |= dirtAdj.W;
                adj.E |= dirtAdj.E;
                adj.S |= dirtAdj.S;
                Map[gridX, gridY].Sprite = new Sprite(GetTileTexture(Tile.TileType.Grass, adj));
                //Map[gridX, gridY].fgRect = Sprites.TileRects[TileType.Grass][adj];

                //Map[gridX, gridY].drawBg = !(adj.N && adj.E && adj.W && adj.S);
                //if (Map[gridX, gridY].drawBg)
                //{
                //    adj = GetAdjacency(gridX, gridY, TileType.Water);
                //    if (adj.N || adj.E || adj.W || adj.S)

                //        //Update image
                //        //Map[gridX, gridY].bgRect = new Rectangle(51, 17, size, size); //water
                //    else
                //        //Update image
                //        //Map[gridX, gridY].bgRect = new Rectangle(136, 170, size, size); //dirt
                //}
            }
            else if (Map[gridX, gridY].Type == Tile.TileType.Dirt)
            {
                Adjacency adj = GetAdjacency(gridX, gridY, Tile.TileType.Dirt);
                Map[gridX, gridY].Sprite = new Sprite(GetTileTexture(Tile.TileType.Dirt, adj));
                //Update image
                //Map[gridX, gridY].fgRect = Sprites.TileRects[TileType.Dirt][adj];

                //Map[gridX, gridY].drawBg = !(adj.N && adj.E && adj.W && adj.S);
                //if (Map[gridX, gridY].drawBg)
                //{
                //    adj = GetAdjacency(gridX, gridY, TileType.Water);
                //    if (adj.N || adj.E || adj.W || adj.S)
                //        //Update image
                //        //Map[gridX, gridY].bgRect = new Rectangle(51, 17, size, size); //water
                //    else
                //        //Update image
                //        //Map[gridX, gridY].bgRect = new Rectangle(136, 272, size, size); //grass
                //}
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

        /// <summary>
        /// Checks for the initial placement of a WaterFlow at a point on the map, and places it.
        /// </summary>
        /// <param name="gridX">The grid x.</param>
        /// <param name="gridY">The grid y.</param>
        private void CheckWaterFlow(int gridX, int gridY)
        {
            if (Map[gridX, gridY].Type != Tile.TileType.Dirt)
                return;

            Adjacency adj = GetAdjacency(gridX, gridY, Tile.TileType.Water);
            // IMPLEMENT WATERFLOW
            // ...or don't
            //if (adj.N || adj.W || adj.E || adj.S)
            //    new WaterFlow(gridX, gridY);
        }

        public bool PointIsInMap(int gridX, int gridY)
        {
            return (gridX >= 0 || gridY >= 0 || gridX <= Map.GetUpperBound(0) || gridY <= Map.GetUpperBound(1));
        }

        public void ChangeTile(int gridX, int gridY, Tile.TileType type)
        {
            //Don't allow tiles to be changed on the outer border - water needed for predator spawning
            if (gridX < 1 || gridY < 1 || gridX > Map.GetUpperBound(0) - 1 || gridY > Map.GetUpperBound(1) - 1 || Map[gridX, gridY].Type == type)
                return;

            Map[gridX, gridY].Type = type;

            if (type == Tile.TileType.Dirt)
                CheckWaterFlow(gridX, gridY);

            UpdateImagePart(gridX, gridY);
        }

        public static Texture2D GetTileTexture(Tile.TileType tileType, Adjacency adj)
        {
            Dictionary<Tile.TileType, string> typeStrings = new Dictionary<Tile.TileType, string>
                {
                    { Tile.TileType.Dirt, "stone_" },
                    { Tile.TileType.Grass, "stone_" },
                    { Tile.TileType.Water, "water_" },
                    { Tile.TileType.None, "stone_" }
                };
            StringBuilder texture = new StringBuilder(15);
            texture.Append(typeStrings[tileType]);

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
                    Map[i, j].Draw(spriteBatch);
        }

        public Tile GetTileAtPoint(int x, int y)
        {
            return Map[x / Tile.TILE_SIZE, y / Tile.TILE_SIZE];
        }
    }
}
