using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class Player : Character
    {
        public int PlayerIndex { get; set; }
        private bool DEBUGBUTTONX = true;
        private bool DEBUGBUTTONY = true;

        public Player(int playerIndex, int x, int y, float moveSpeed) : base(x, y, moveSpeed)
        {
            PlayerIndex = playerIndex;
            texturePrefix = "char" + (playerIndex + 1);
            string[] actions = { "walk" };
            foreach (string act in actions)
            {
                if (!sprites.ContainsKey(act))
                    sprites.Add(act, new Dictionary<Directions, Sprite>());

                for (int dir = 0; dir < 4; dir++)
                {
                    List<Texture2D> newSprite = new List<Texture2D>();
                    string textureName = $"{texturePrefix}_{act}_" + "news"[dir] + "_";
                    int[] indexes = { 1, 0, 2, 0 };
                    foreach (int i in indexes) //Might be better to just rename them to "0, 1, 2, 3"
                        newSprite.Add(Sprite.GetTexture(textureName + i));
                    sprites[act].Add((Directions)dir, new Sprite(newSprite.ToArray(), 15));
                }
            }
            sprite = sprites["walk"][Directions.South];
        }

        /// <summary>
        /// Performs the logic for each update.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        public override void Step(GameState gameState)
        {
            sprite.Update();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);

            //some debug stuff
            currentMoveSpeed = gamePadState.IsButtonDown(Buttons.A) ? moveSpeed * 2 : moveSpeed;

            Point center = new Point(X + Hitbox.Width / 2, Y + Hitbox.Height / 2);
            Tile.TileType oldType = gameState.Map.GetTileAtPoint(center.X, center.Y).Type;

            if (gamePadState.IsButtonDown(Buttons.X))
            {
                if (DEBUGBUTTONX && gamePadState.IsButtonDown(Buttons.X) && (int)oldType - 1 >= 0)
                    gameState.Map.ChangeTile((center.X) / Tile.TILE_SIZE, (center.Y) / Tile.TILE_SIZE, new Tile((Tile.TileType)((int)oldType - 1)));
                DEBUGBUTTONX = false;
            }
            else
                DEBUGBUTTONX = true;

            if (gamePadState.IsButtonDown(Buttons.Y))
            {
                if (DEBUGBUTTONY && (int)oldType + 1 <= 3)
                    gameState.Map.ChangeTile((center.X) / Tile.TILE_SIZE, (center.Y) / Tile.TILE_SIZE, new Tile((Tile.TileType)((int)oldType + 1)));
                DEBUGBUTTONY = false;
            }
            else
                DEBUGBUTTONY = true;

            Move(gameState, gamePadState.ThumbSticks.Left.X * currentMoveSpeed, -gamePadState.ThumbSticks.Left.Y * currentMoveSpeed);
        }
    }
}
