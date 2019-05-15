using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace monoCoopGame
{
    public enum Directions
    {
        North, East, West, South
    }

    static class Utility
    {
        public static Random R = new Random();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    }
}
