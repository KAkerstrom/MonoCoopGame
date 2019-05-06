using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace monoCoopGame
{
    public class Sprite
    {
        private static Dictionary<string, Texture2D> textureLib = new Dictionary<string, Texture2D>();

        private Texture2D[] frames;
        private int speed;
        private int spriteIndex = 0;
        private int animTimer = 0;

        public int Width { get { return frames[0].Width; } }
        public int Height { get { return frames[0].Height; } }

        public Sprite(Texture2D frame, int speed = 0)
        {
            this.frames = new Texture2D[1];
            frames[0] = frame;
            this.speed = speed;
        }

        public Sprite(Texture2D[] frames, int speed = 0)
        {
            this.frames = frames;
            this.speed = speed;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(frames[spriteIndex], new Rectangle(x, y, frames[spriteIndex].Bounds.Width, frames[spriteIndex].Bounds.Height), Color.White);
        }

        public void Update()
        {
            // Should probably take in a delta value to cap FPS
            if (frames.Length > 1 && animTimer++ > speed)
            {
                spriteIndex = (spriteIndex + 1) % frames.Length;
                animTimer = 0;
            }
        }

        public static Texture2D GetTexture(string name)
        {
            if (!textureLib.ContainsKey(name))
                throw new System.Exception($"Texture \"{name}\" was called without being loaded first.");
            return textureLib[name];
        }

        public static void LoadSprites(ContentManager manager, string folder)
        {
            DirectoryInfo dir = new DirectoryInfo(manager.RootDirectory + "/" + folder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string filename = file.Name.Split('.')[0];
                textureLib.Add(filename, manager.Load<Texture2D>(folder + "/" + filename));
            }
        }
    }
}
