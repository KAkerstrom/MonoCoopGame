using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace monoCoopGame
{
    public class Player : Character
    {
        public int PlayerIndex { get; set; }

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
            currentMoveSpeed = gamePadState.IsButtonDown(Buttons.X) ? moveSpeed * 2 : moveSpeed;
            Move(gameState, gamePadState.ThumbSticks.Left.X * currentMoveSpeed, -gamePadState.ThumbSticks.Left.Y * currentMoveSpeed);
        }
    }
}
