using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace monoCoopGame.Tiles
{
    public abstract class Blob : Tile
    {
        public enum BlobGroups
        {
            Dirt, Grass, Slime
        }

        private static Dictionary<Type, Dictionary<Adjacencies, Texture2D>> textures
            = new Dictionary<Type, Dictionary<Adjacencies, Texture2D>>();

        private BlobGroups BlobGroup;

        public Blob(string texturePrefix, BlobGroups blobGroup, Point gridPos) : base(new Sprite(texturePrefix + "_"), gridPos)
        {
            BlobGroup = blobGroup;
            PopulateTextures(texturePrefix);
        }

        public void UpdateAdjacency(Tile[,] map)
        {
            Adjacencies adj = GetAdjacency(map);
            HasTransparency = !adj.N || !adj.E || !adj.W || !adj.S;
            Sprite = new Sprite(textures[GetType()][adj]);
        }

        private Adjacencies GetAdjacency(Tile[,] map)
        {
            Adjacencies adj = new Adjacencies();
            adj.N = (GridPos.Y > 0)
                && map[GridPos.X, GridPos.Y - 1] is Blob
                && ((Blob)map[GridPos.X, GridPos.Y - 1])?.BlobGroup == BlobGroup;

            adj.E = (GridPos.X < map.GetUpperBound(0))
                && map[GridPos.X - 1, GridPos.Y] is Blob
                && ((Blob)map[GridPos.X - 1, GridPos.Y])?.BlobGroup == BlobGroup;

            adj.W = (GridPos.X > 0)
                && map[GridPos.X + 1, GridPos.Y] is Blob
                && ((Blob)map[GridPos.X + 1, GridPos.Y])?.BlobGroup == BlobGroup;

            adj.S = (GridPos.Y < map.GetUpperBound(1))
                && map[GridPos.X, GridPos.Y + 1] is Blob
                && ((Blob)map[GridPos.X, GridPos.Y + 1])?.BlobGroup == BlobGroup;
            return adj;
        }

        private void PopulateTextures(string texturePrefix)
        {
            if (textures.ContainsKey(GetType()))
                return;

            Dictionary<Adjacencies, Texture2D> blockTextures = new Dictionary<Adjacencies, Texture2D>();
            BitArray bits = new BitArray(4);
            for (int i = 0; i < 16; i++)
            {
                Adjacencies adj = new Adjacencies(bits[0], bits[1], bits[2], bits[3]);
                StringBuilder sb = new StringBuilder(texturePrefix + "_");
                if (!bits[0]) sb.Append('n');
                if (!bits[2]) sb.Append('e');
                if (!bits[1]) sb.Append('w');
                if (!bits[3]) sb.Append('s');

                bool carry = true;
                for (int j = 0; j < 4; j++)
                {
                    bool newbit = bits[j] ^ carry;
                    carry = bits[j] & carry;
                    bits[j] = newbit;
                }
                blockTextures.Add(adj, Sprite.GetTexture(sb.ToString()));
            }
            textures.Add(GetType(), blockTextures);
        }
    }
}
