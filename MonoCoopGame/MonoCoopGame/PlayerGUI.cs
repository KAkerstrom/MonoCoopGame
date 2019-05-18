using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class PlayerGUI : GUI
    {
        private Player player;
        private Rectangle drawArea;

        public PlayerGUI(Player player)
        {
            this.drawArea = new Rectangle(16 + (player.PlayerIndex * 300), 0, 250, 100);
            this.player = player;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            player.Inventory.Draw(spriteBatch, new Rectangle(drawArea.X + 16, drawArea.Y + 48, 32 * 5, 32));
            spriteBatch.DrawString(Utility.Fonts["playerGUI"], "TEST", new Vector2(drawArea.X + 16, drawArea.Y + 16), Color.Black);
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            int uiGridSize = Sprite.GetTexture("ui0_nw").Width;
            spriteBatch.Draw(Sprite.GetTexture("ui0_nw"), new Vector2(drawArea.X, drawArea.Y), Color.White);
            spriteBatch.Draw(Sprite.GetTexture("ui0_ne"), new Vector2(drawArea.Right - uiGridSize, drawArea.Y), Color.White);
            spriteBatch.Draw(Sprite.GetTexture("ui0_sw"), new Vector2(drawArea.X, drawArea.Bottom - uiGridSize), Color.White);
            spriteBatch.Draw(Sprite.GetTexture("ui0_se"), new Vector2(drawArea.Right - uiGridSize, drawArea.Bottom - uiGridSize), Color.White);

            for (int i = 0; i < (drawArea.Width / uiGridSize); i++)
            {
                spriteBatch.Draw(Sprite.GetTexture("ui0_n"), new Vector2(drawArea.X + (i * uiGridSize), drawArea.Y), Color.White);
                spriteBatch.Draw(Sprite.GetTexture("ui0_s"), new Vector2(drawArea.X + (i * uiGridSize), drawArea.Bottom - uiGridSize), Color.White);
            }

            for (int i = 0; i < (drawArea.Height / uiGridSize); i++)
            {
                spriteBatch.Draw(Sprite.GetTexture("ui0_w"), new Vector2(drawArea.X, drawArea.Y + (i * uiGridSize)), Color.White);
                spriteBatch.Draw(Sprite.GetTexture("ui0_e"), new Vector2(drawArea.Right - uiGridSize, drawArea.Y + (i * uiGridSize)), Color.White);
            }

            for (int i = 1; i < (drawArea.Width / uiGridSize); i++)
                for (int j = 1; j < (drawArea.Height / uiGridSize); j++)
                    spriteBatch.Draw(Sprite.GetTexture("ui0_"), new Vector2(drawArea.X + (i * uiGridSize), drawArea.Y + (j * uiGridSize)), Color.White);
        }
    }
}
