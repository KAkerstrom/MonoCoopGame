using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public enum Directions
    {
        North, East, West, South
    }

    public struct Adjacencies
    {
        public Adjacencies(bool north, bool east, bool west, bool south)
        {
            N = north;
            E = east;
            W = west;
            S = south;
        }

        public bool N, E, W, S;
    }

    static class Utility
    {
        public static Random R = new Random();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    }
}
