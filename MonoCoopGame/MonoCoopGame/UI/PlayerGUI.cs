using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoCoopGame.Tiles;
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
            player.Inventory.Draw(spriteBatch, new Rectangle(drawArea.X + Tile.TILE_SIZE * 2, drawArea.Y + 48, 32 * 5, 32));
            spriteBatch.DrawString(Utility.Fonts["playerGUI"], $"{player.Name} ({player.Health})", new Vector2(drawArea.X + Tile.TILE_SIZE * 3, drawArea.Y + 16), Color.Black);
            Rectangle drawRect = new Rectangle(drawArea.X + Tile.TILE_SIZE * 2, drawArea.Y + 8, 16 * 2, 16 * 2);
            spriteBatch.Draw(Sprite.GetTexture("char" + player.CharacterIndex + "_walk_s_0"), drawRect, Color.White);
            // Draw Healthbar
            drawRect = new Rectangle(drawArea.X + 16, drawArea.Y + 16, Tile.TILE_SIZE, Tile.TILE_SIZE * 2);
            spriteBatch.Draw(Sprite.GetTexture("healthbar_red"), drawRect, Color.White);
            int newHeight = drawRect.Height * (player.Health / player.MaxHealth);
            drawRect.Y -= drawRect.Height - newHeight;
            drawRect.Height = newHeight;
            spriteBatch.Draw(Sprite.GetTexture("healthbar_green"), drawRect, Color.White);
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            int uiGridSize = Sprite.GetTexture("ui0_nw").Width;
            spriteBatch.Draw(Sprite.GetTexture("ui0_nw"), new Vector2(drawArea.X, drawArea.Y), Color.White);
            spriteBatch.Draw(Sprite.GetTexture("ui0_ne"), new Vector2(drawArea.Right - uiGridSize, drawArea.Y), Color.White);
            spriteBatch.Draw(Sprite.GetTexture("ui0_sw"), new Vector2(drawArea.X, drawArea.Bottom - uiGridSize), Color.White);
            spriteBatch.Draw(Sprite.GetTexture("ui0_se"), new Vector2(drawArea.Right - uiGridSize, drawArea.Bottom - uiGridSize), Color.White);

            for (int i = 0; i < (drawArea.Width / uiGridSize); i++)
                spriteBatch.Draw(Sprite.GetTexture("ui0_s"), new Vector2(drawArea.X + (i * uiGridSize), drawArea.Bottom - uiGridSize), Color.White);

            for (int i = 0; i < (drawArea.Height / uiGridSize); i++)
            {
                spriteBatch.Draw(Sprite.GetTexture("ui0_w"), new Vector2(drawArea.X, drawArea.Y + (i * uiGridSize)), Color.White);
                spriteBatch.Draw(Sprite.GetTexture("ui0_e"), new Vector2(drawArea.Right - uiGridSize, drawArea.Y + (i * uiGridSize)), Color.White);
            }

            for (int i = 1; i < (drawArea.Width / uiGridSize); i++)
                for (int j = 0; j < (drawArea.Height / uiGridSize); j++)
                    spriteBatch.Draw(Sprite.GetTexture("ui0_"), new Vector2(drawArea.X + (i * uiGridSize), drawArea.Y + (j * uiGridSize)), Color.White);
        }
    }
}
