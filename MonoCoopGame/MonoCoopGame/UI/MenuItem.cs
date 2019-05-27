using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame.UI
{
    public abstract class MenuItem
    {
        public delegate void MenuItemActivatedDelegate(MenuItem item);
        public event MenuItemActivatedDelegate MenuItemActivated;

        public string Name { get; }
        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                if (!enabled)
                    currentTexture = disabledTexture;
            }
        }
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (selected)
                    currentTexture = selectedTexture;
                else if (enabled)
                    currentTexture = unselectedTexture;
            }
        }

        protected Texture2D currentTexture;

        private bool enabled;
        private bool selected;
        private Texture2D unselectedTexture;
        private Texture2D selectedTexture;
        private Texture2D disabledTexture;
        private SpriteFont font;

        public MenuItem(string name, SpriteFont font, Texture2D selected, Texture2D unselected, Texture2D disabled)
        {
            Name = name;
            this.font = font;
            selectedTexture = selected;
            unselectedTexture = currentTexture =  unselected;
            disabledTexture = disabled;
            enabled = true;
            this.selected = false;
        }

        public void Activate()
        {
            ActivateDerived();
            MenuItemActivated?.Invoke(this);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawBounds)
        {
            spriteBatch.Draw(currentTexture, drawBounds, Color.White);
            Vector2 stringSize = font.MeasureString(Name);
            Vector2 drawPoint = new Vector2
                (
                drawBounds.X + (drawBounds.Width - stringSize.X) / 2,
                drawBounds.Y + (drawBounds.Height - stringSize.Y) / 2
                );
            spriteBatch.DrawString(font, Name, drawPoint, Selected ? Color.White : Color.Black);
        }

        protected abstract void ActivateDerived();
    }
}
