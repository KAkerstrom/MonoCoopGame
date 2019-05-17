using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace monoCoopGame.Blocks
{
    public abstract class BlobBlock : Block
    {
        static private Dictionary<Type, Dictionary<Adjacencies, Texture2D>> textures
            = new Dictionary<Type, Dictionary<Adjacencies, Texture2D>>();

        public string BlobGroup;

        public BlobBlock(string texturePrefix, string blobGroup, Point gridPos, Player owner) : base(new Sprite(texturePrefix + "_"), gridPos, owner)
        {
            this.BlobGroup = blobGroup;
            if (!textures.ContainsKey(GetType()))
                PopulateTextures(texturePrefix);
        }

        public void UpdateSprite(Adjacencies adj)
        {
            Sprite = new Sprite(textures[GetType()][adj]);
        }

        private void PopulateTextures(string texturePrefix)
        {
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
