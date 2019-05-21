using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace monoCoopGame
{
    public delegate void AnimationDoneDelegate();

    public class Sprite
    {
        public event AnimationDoneDelegate AnimationDone;
        private static Dictionary<string, Texture2D> textureLib = new Dictionary<string, Texture2D>();

        public int Speed;
        public int ImageIndex = 0;

        private Texture2D[] frames;
        private int animTimer = 0;

        public int Width { get { return frames[0].Width; } }
        public int Height { get { return frames[0].Height; } }

        public Sprite()
        {
            this.frames = new Texture2D[1];
            frames[0] = GetTexture("null");
            Speed = 0;
        }

        public Sprite(Texture2D frame, int speed = 0)
        {
            this.frames = new Texture2D[1];
            frames[0] = frame;
            this.Speed = speed;
        }

        public Sprite(Texture2D[] frames, int speed = 2)
        {
            this.frames = frames;
            this.Speed = speed;
        }

        public Sprite(string textureName)
        {
            frames = new Texture2D[1];
            frames[0] = GetTexture(textureName);
            Speed = 0;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y, float depth)
        {
            //spriteBatch.Draw(frames[ImageIndex], new Rectangle(x, y, frames[ImageIndex].Bounds.Width, frames[ImageIndex].Bounds.Height), Color.White);

            Rectangle drawRect = new Rectangle(x, y, frames[ImageIndex].Bounds.Width, frames[ImageIndex].Bounds.Height);
            spriteBatch.Draw(frames[ImageIndex], drawRect, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, depth);
        }

        public void Update()
        {
            if (Speed > 0 && animTimer++ > Speed)
            {
                if(++ImageIndex == frames.Length)
                {
                    ImageIndex = 0;
                    AnimationDone?.Invoke();
                }
                animTimer = 0;
            }
        }

        public static bool TextureExists(string name)
        {
            return textureLib.ContainsKey(name);
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
